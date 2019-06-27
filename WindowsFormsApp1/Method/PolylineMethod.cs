using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryPlan.Method
{
    public static  class PolylineMethod
    {

        public static Dictionary<int, List<PointF>> GetPolylinesPoints(string layerName)
        {
            Dictionary<int, List<System.Drawing.PointF>> pts = new Dictionary<int, List<System.Drawing.PointF>>();
            List<Polyline> list = GetPolyliness(layerName);
            for (int i = 0; i < list.Count; i++)
            {
                pts.Add(i, PolylineMethod.GetPolyLineInfoPt(list[i]));
            }

            return pts;
        }

        public static LayerModel GetLayerModel(string layerName)
        {
            LayerModel model = new LayerModel();
            model.Name = layerName;
            Dictionary<int, List<object>> pts = new Dictionary<int, List<object>>();
            List<Polyline> list = GetPolyliness(layerName, model);
            for (int i = 0; i < list.Count; i++)
            {
                pts.Add(i, PolylineMethod.GetPolyLineInfo(list[i]));
            }

            model.pointFs = pts;
            return model;
        }


        public static  List<Polyline> GetPolyliness(string layerName)
        {
            List<Polyline> list = new List<Polyline>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            //  List<ObjectId> idss=  GetEntitiesInModelSpace();
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;
                    ObjectId[] oids = sst.GetObjectIds();
                    int ad = 0;
                  //  List<string> aa = new List<string>();
                    for (int i = 0; i < oids.Length; i++)
                    {
                        DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);
                        //if (!aa.Contains((ob as Polyline).BlockName)) { aa.Add((ob as Polyline).BlockName); }
                        if (ob is Polyline && (ob as Polyline).BlockName.ToLower() == "*model_space")
                        {
                            list.Add(ob as Polyline);
                        }
                    }
                }
            }
            return list;
            
        }

        public static List<Polyline> GetPolyliness(string layerName,LayerModel model)
        {
            List<Polyline> list = new List<Polyline>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;
         
            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            //  List<ObjectId> idss=  GetEntitiesInModelSpace();
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                    foreach (ObjectId layerId in lt)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                        if (ltr.Name == layerName)
                        {
                            model.Color =  System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                        }
                    }
                    SelectionSet sst = ProSset.Value;
                    ObjectId[] oids = sst.GetObjectIds();
                    int ad = 0;
                    List<string> aa = new List<string>();
                    for (int i = 0; i < oids.Length; i++)
                    {
                     
                        DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);
                        
                        if (!aa.Contains((ob as Polyline).BlockName)) { aa.Add((ob as Polyline).BlockName); }
                        if (ob is Polyline && (ob as Polyline).BlockName.ToLower() == "*model_space")
                        {
                            list.Add(ob as Polyline);
                        }
                    }
                }
            }
            return list;

        }
        public static List<object> GetPolyLineInfo(Polyline line)
        {
            List<object> pfList = new List<object>();
            int vtnum = line.NumberOfVertices;

            for (int i = 0; i < vtnum; i++)
            {
                Point3d p1=line.GetPoint3dAt(i);
                PointF pf = new PointF((float)p1.X, (float)p1.Y);
                pfList.Add(pf);
            }
            return pfList;
        }

        public static List<PointF> GetPolyLineInfoPt(Polyline line)
        {
            List<PointF> pfList = new List<PointF>();
            int vtnum = line.NumberOfVertices;

            for (int i = 0; i < vtnum; i++)
            {
                Point3d p1 = line.GetPoint3dAt(i);
                PointF pf = new PointF((float)p1.X, (float)p1.Y);
                pfList.Add(pf);
            }
            return pfList;
        }

        /// <summary>
        /// 判断给定点 pt 是否在折线上: 1:在折线上;0:不在折线上
        /// </summary>
        /// <param name="pt">待判断的点</param>
        /// <param name="polyline">线坐标列表</param>
        /// <param name="allowError">容差值(该值应包括线的宽度,如为零则表示精确匹配)</param>
        /// <returns>1:在折线上;0:不在折线上</returns>
        public static int PtInPolyLine(Point2d pt, Point2d l1, Point2d l2, double allowError)
        {
            //如果选择的点与当前点重合
            if (Math.Abs(l2.X - pt.X) <= allowError && Math.Abs(l2.Y - pt.Y) <= allowError)
                return 1;
            if (Math.Min(l1.X, l2.X) <= pt.X && Math.Min(l1.Y, l2.Y) <= pt.Y &&
            Math.Max(l1.X, l2.X) >= pt.X && Math.Max(l1.Y, l2.Y) >= pt.Y)
            {
                //精确匹配判断的话
                if (Math.Abs(allowError - 0.0) <= 0.0001)
                {
          
                    Point2d tp1 = new Point2d(l2.X - pt.X, l2.Y - pt.Y);  //矢量减法
                    Point2d tp2 = new Point2d(pt.X - l1.X, pt.Y - l1.Y);  //矢量减法
                    if (Math.Abs(Math.Abs(tp1.X * tp2.Y - tp2.X * tp1.Y) - 0.0) <= 0.00000001)         //矢量叉乘,平行四边形的面积
                        return 1;
                    
                }
                else
                {
                    if (Math.Abs(l2.X - l1.X) <= allowError && Math.Abs(l2.X - pt.X) <= allowError)
                        return 1;
                    if (Math.Abs(l2.Y - l1.Y) <= allowError && Math.Abs(l2.Y - pt.Y) <= allowError)
                        return 1;
                    if (MethodCommand.DistancePointToSegment(pt, l1, l2) <= allowError)
                        return 1;
                    //如果点到线段的距离在容差范围内,则选取成功
                    if (MethodCommand.DistancePointToSegment(pt, l1, l2) <= allowError)
                        return 1;
                }
            }

            return 0;
        }
     
    }
}
