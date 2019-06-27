using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Method;
using RegulatoryPlan.Model;
using RegulatoryPlan.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryPlan.Command
{
    public static class MethodCommand
    {
        public static string LegendLayer = "图例";

        public   static string fileName = "";
        public    static string cityName = "";
        static DerivedTypeEnum dp = DerivedTypeEnum.None;


        public static BlockInfoModel AnalysisBlcokInfo(DBObject ob)
        {


            if (ob is Entity && (((ob as Entity).BlockName.ToLower() == "*model_space" && UI.MainForm.isOnlyModel) || (!UI.MainForm.isOnlyModel)))
            {
                BlockInfoModel plModel = new BlockInfoModel();
                try
                {
                    if (ob is Polyline)
                    {

                        plModel.PolyLine.Add(AutoCad2ModelTools.Polyline2Model(ob as Polyline));

                    }
                    else if (ob is Arc)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Arc2Model(ob as Arc));

                    }
                    else if (ob is BlockReference)
                    {
                        plModel = BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference);
                    }
                    else if (ob is DBText)
                    {
                        plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as DBText));
                    }
                    else if (ob is MText)
                    {
                        plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as MText));
                    }
                    else if (ob is Hatch)
                    {
                        plModel.Hatch.Add(AutoCad2ModelTools.Hatch2Model(ob as Hatch));

                    }
                    else if (ob is Circle)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle));

                    }
                    else if (ob is Ellipse)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Ellipse2Model(ob as Ellipse));

                    }
                    else if (ob is Line)
                    {

                        plModel.Line.Add(AutoCad2ModelTools.Line2Model(ob as Line));

                    }
                    else if (ob is Polyline2d)
                    {
                        plModel.Circle.Add(AutoCad2ModelTools.Polyline2DModel(ob as Polyline2d));
                    }
                    else if (ob is Entity)
                    {
                        Entity ety = ob as Entity;
                        DBObjectCollection objs = new DBObjectCollection();
                        ety.Explode(objs);
                        foreach (DBObject obj in objs)
                        {
                            AnalysisBlcokInfo(plModel, obj);
                        }
                    }

                }
                catch
                {

                }
                return plModel;
            }
            return null;
        }

        public static BlockInfoModel AnalysisBlcokInfo(DBObject ob,AttributeModel attModel)
        {


            if (ob is Entity && (((ob as Entity).BlockName.ToLower() == "*model_space" && UI.MainForm.isOnlyModel) || (!UI.MainForm.isOnlyModel)))
            {
                BlockInfoModel plModel = new BlockInfoModel();
                try
                {
                    if (ob is Polyline)
                    {

                        plModel.PolyLine.Add(AutoCad2ModelTools.Polyline2Model(ob as Polyline,attModel));

                    }
                    else if (ob is Arc)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Arc2Model(ob as Arc, attModel));

                    }
                    else if (ob is BlockReference)
                    {
                        plModel = BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference, attModel);
                    }
                    else if (ob is DBText)
                    {
                        plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as DBText, attModel));
                    }
                    else if (ob is MText)
                    {
                        plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as MText,attModel));
                    }
                    else if (ob is Hatch)
                    {
                        plModel.Hatch.Add(AutoCad2ModelTools.Hatch2Model(ob as Hatch,attModel));

                    }
                    else if (ob is Circle)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle,attModel));

                    }
                    else if (ob is Ellipse)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Ellipse2Model(ob as Ellipse,attModel));

                    }
                    else if (ob is Line)
                    {

                        plModel.Line.Add(AutoCad2ModelTools.Line2Model(ob as Line,attModel));

                    }
                    else if (ob is Polyline2d)
                    {
                        plModel.Circle.Add(AutoCad2ModelTools.Polyline2DModel(ob as Polyline2d,attModel));
                    }
                    else if (ob is Entity)
                    {
                        Entity ety = ob as Entity;
                        DBObjectCollection objs = new DBObjectCollection();
                        ety.Explode(objs);
                        foreach (DBObject obj in objs)
                        {
                            AnalysisBlcokInfo(plModel, obj, attModel);
                        }
                    }

                }
                catch
                {

                }
                return plModel;
            }
            return null;
        }
        public static void AnalysisBlcokInfo(BlockInfoModel plModel, DBObject ob)
        {



            if (ob is Polyline)
            {

                plModel.PolyLine.Add(AutoCad2ModelTools.Polyline2Model(ob as Polyline));

            }
            else if (ob is BlockReference)
            {
                plModel = BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference);
            }
            else if (ob is DBText)
            {
                plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as DBText));
            }
            else if (ob is MText)
            {
                plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as MText));
            }
            else if (ob is Hatch)
            {
                plModel.Hatch.Add(AutoCad2ModelTools.Hatch2Model(ob as Hatch));

            }
            else if (ob is Circle)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle));

            }
            else if (ob is Ellipse)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Ellipse2Model(ob as Ellipse));

            }
            else if (ob is Line)
            {

                plModel.Line.Add(AutoCad2ModelTools.Line2Model(ob as Line));

            }
            else if (ob is Arc)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Arc2Model(ob as Arc));

            }
            else if (ob is Polyline2d)
            {
                plModel.Circle.Add(AutoCad2ModelTools.Polyline2DModel(ob as Polyline2d));
            }
            else if (ob is Entity)
            {
                Entity ety = ob as Entity;
                DBObjectCollection objs = new DBObjectCollection();
                ety.Explode(objs);
                foreach (DBObject obj in objs)
                {
                    AnalysisBlcokInfo(plModel, obj);
                }
            }



        }

        public static void AnalysisBlcokInfo(BlockInfoModel plModel, DBObject ob,AttributeModel attModel)
        {



            if (ob is Polyline)
            {

                plModel.PolyLine.Add(AutoCad2ModelTools.Polyline2Model(ob as Polyline, attModel));

            }
            else if (ob is BlockReference)
            {
                plModel = BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference, attModel);
            }
            else if (ob is DBText)
            {
                plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as DBText, attModel));
            }
            else if (ob is MText)
            {
                plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as MText, attModel));
            }
            else if (ob is Hatch)
            {
                plModel.Hatch.Add(AutoCad2ModelTools.Hatch2Model(ob as Hatch, attModel));

            }
            else if (ob is Circle)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle, attModel));

            }
            else if (ob is Ellipse)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Ellipse2Model(ob as Ellipse, attModel));

            }
            else if (ob is Line)
            {

                plModel.Line.Add(AutoCad2ModelTools.Line2Model(ob as Line, attModel));

            }
            else if (ob is Arc)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Arc2Model(ob as Arc, attModel));

            }
            else if (ob is Polyline2d)
            {
                plModel.Circle.Add(AutoCad2ModelTools.Polyline2DModel(ob as Polyline2d, attModel));
            }
            else if (ob is Entity)
            {
                Entity ety = ob as Entity;
                DBObjectCollection objs = new DBObjectCollection();
                ety.Explode(objs);
                foreach (DBObject obj in objs)
                {
                    AnalysisBlcokInfo(plModel, obj, attModel);
                }
            }



        }
        /// <summary>
        /// 获取图层中的实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectIdCollection GetObjectIdsAtLayer(string name)
        {
            //string names = System.Text.RegularExpressions.Regex.Unescape(name);
            //System.Text.RegularExpressions.Regex.Escape(name);
            ObjectIdCollection ids = new ObjectIdCollection();
            PromptSelectionResult ProSset = null;
            //LayerName (int)DxfCode.LayerName
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, name) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ProSset = ed.SelectAll(sfilter);
            if (ProSset.Status == PromptStatus.OK)
            {
                SelectionSet sst = ProSset.Value;
                ObjectId[] oids = sst.GetObjectIds();
                for (int i = 0; i < oids.Length; i++)
                {
                    ids.Add(oids[i]);
                }
            }
            return ids;
        }

        public static bool FindDBTextIsInPolyine(MText txt, List<Polyline> lineList)
        {
            bool res = false;
            foreach (Polyline line in lineList)
            {
                int vtnum = line.NumberOfVertices;
                double minX = double.NaN;
                double maxX = double.NaN;
                double maxY = double.NaN;
                double minY = double.NaN;
                double thres = double.NaN;
                for (int i = 0; i < vtnum; i++)
                {
                    Point3d p1 = line.GetPoint3dAt(i);
                    if (p1.X > maxX || double.IsNaN(maxX))
                    {
                        maxX = p1.X;
                    }
                    if (p1.Y < minY || double.IsNaN(minY))
                    {
                        minY = p1.Y;
                    }
                    if (p1.Y > maxY || double.IsNaN(maxY))
                    {
                        maxY = p1.Y;
                    }
                    if (p1.X < minX || double.IsNaN(minX))
                    {
                        minX = p1.X;
                    }
                }

                thres = maxX - minX;
                if (txt.Location.X < thres + maxX && txt.Location.Y > minY && txt.Location.Y < maxY)
                {
                    return true;
                }
            }
            return res;
        }
        /// <summary>
        /// 管线文本查询
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="lineList"></param>
        /// <returns></returns>
        public static MText FindMTextIsInPolyineForPipe(Polyline line, List<MText> txtList)
        {
            double min = 20;
            MText resText = null;

            double mToPtMin = double.NaN;
            int num = line.NumberOfVertices;
            List<Point2d> point = new List<Point2d>();
            for (int i = 0; i < num; i++)
            {
                point.Add(line.GetPoint2dAt(i));


            }
            double mToPt = double.NaN;
            foreach (MText txt in txtList)
            {
                Point2d mPt = txt.Location.Convert2d(new Plane());

                for (int i = 0; i < point.Count - 1; i++)
                {

                    Point2d st = point[i];
                    Point2d et = point[i + 1];
                    double lLen = MethodCommand.DistancePointToSegment(mPt, st, et);
                    if (double.IsNaN(mToPt) || lLen < mToPt)
                    {
                        mToPt = lLen;
                    }
                }


                if (double.IsNaN(mToPtMin) || (mToPtMin > mToPt))
                {

                    mToPtMin = mToPt;
                    resText = txt;
                }

            }

            return resText;
        }

        /// <summary>
        /// 管线文本查询
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="lineList"></param>
        /// <returns></returns>
        public static DBText FindMTextIsInPolyineForPipe(Polyline line, List<DBText> txtList)
        {
            double min = 20;
            DBText resText = null;

            double mToPtMin = double.NaN;
            int num = line.NumberOfVertices;
            List<Point2d> point = new List<Point2d>();
            for (int i = 0; i < num; i++)
            {
                point.Add(line.GetPoint2dAt(i));


            }
            double mToPt = double.NaN;
            foreach (DBText txt in txtList)
            {
                Point2d mPt = txt.Position.Convert2d(new Plane());

                for (int i = 0; i < point.Count - 1; i++)
                {

                    Point2d st = point[i];
                    Point2d et = point[i + 1];
                    double lLen = MethodCommand.DistancePointToSegment(mPt, st, et);
                    if (double.IsNaN(mToPt) || lLen < mToPt)
                    {
                        mToPt = lLen;
                    }
                }


                if (double.IsNaN(mToPtMin) || (mToPtMin > mToPt))
                {

                    mToPtMin = mToPt;
                    resText = txt;
                }

            }

            return resText;
        }

        public static bool FindDBTextIsInPolyine(MText txt, List<LengedModel> lineList)
        {
            bool res = false;

            foreach (LengedModel line in lineList)
            {

                double minX = double.NaN;
                double maxX = double.NaN;
                double maxY = double.NaN;
                double minY = double.NaN;
                double thres = double.NaN;
                for (int i = 0; i < line.BoxPointList.Count; i++)
                {
                    PointF p1 = line.BoxPointList[i];
                    if (p1.X > maxX || double.IsNaN(maxX))
                    {
                        maxX = p1.X;
                    }
                    if (p1.Y < minY || double.IsNaN(minY))
                    {
                        minY = p1.Y;
                    }
                    if (p1.Y > maxY || double.IsNaN(maxY))
                    {
                        maxY = p1.Y;
                    }
                    if (p1.X < minX || double.IsNaN(minX))
                    {
                        minX = p1.X;
                    }
                }

                thres = maxX - minX;
                if (txt.Location.X>maxX&& txt.Location.X < thres + maxX && txt.Location.Y > minY && txt.Location.Y < maxY)
                {
                    if (line.LayerName != null && line.LayerName == "公厕")
                    { }
                    line.LayerName = txt.Text;
                    res = true;
                    break;
                }
            }
            return res;
        }

        internal static void OpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            try
            {
                fileName = file;
                cityName = city;
                dp = derivedType;
                num = 0;
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
                Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
                Application.DocumentManager.DocumentActivated -= docChange;
            }
            catch { }
        }
        internal static void AutoOpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            try
            {
                fileName = file;
                cityName = city;
                dp = derivedType;
                num = 0;
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docAutoChange);
                Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file,false);
                Application.DocumentManager.DocumentActivated -= docChange;
                Application.DocumentManager.MdiActiveDocument.CloseAndDiscard();
            }
            catch { }
        }



        public static void OpenFile(string file, string city)
        {
            fileName = file;
            cityName = city;
            num = 0;
            Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
            Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
            Application.DocumentManager.DocumentActivated -= docChange;
        }
        public static int num = 0;
        /// <summary>
        /// 文档发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void docChange(object sender, DocumentCollectionEventArgs e)
        {
            try
            {
                if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
                {
                    num++;
                    MainForm mf = new MainForm(cityName, dp);
                    mf.Show();
                   // mf.Close();
                    
                }
            }
            catch { }
        }

        /// <summary>
        /// 文档发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void docAutoChange(object sender, DocumentCollectionEventArgs e)
        {
            try
            {
                if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
                {
                    num++;
                    MainForm mf = new MainForm(cityName, dp, true);
                    mf.ShowDialog();
                    // mf.Close();

                }
            }
            catch { }
        }

        /// <summary>
                /// 根据图形边界显示视图
                /// </summary>
                /// <param name="ed"></param>
        public static void ZoomExtens( Editor ed)
        {
            Database db = ed.Document.Database;
            //更新当前模型空间的范围
            db.UpdateExt(true);
            //根据当前图形的界限范围对视图进行缩放
            if (db.Extmax.X < db.Extmin.X)
            {
                Plane plane = new Plane();
                Point3d pt1 = new Point3d(plane, db.Limmin);
                Point3d pt2 = new Point3d(plane, db.Limmax);
                ZoomWindow(ed,pt1, pt2);
            }
            else
            {
                ZoomWindow(ed,db.Extmin, db.Extmax);
            }
        }    /// <summary>
                     /// 实现视图的窗口缩放
                     /// </summary>
                     /// <param name="ed"></param>
                     /// <param name="pt1">窗口角点</param>
                     /// <param name="pt2">窗口角点</param>
        public static void ZoomWindow( Editor ed, Point3d pt1, Point3d pt2)
        {
            //创建一临时的直线用于获取两点表示的范围
            using (Line line = new Line(pt1, pt2))
            {
                //获取两点表示的范围
                Extents3d extents = new Extents3d(line.GeometricExtents.MinPoint, line.GeometricExtents.MaxPoint);
                //获取范围内的最小值点及最大值点
                Point2d minPt = new Point2d(extents.MinPoint.X, extents.MinPoint.Y);
                Point2d maxPt = new Point2d(extents.MaxPoint.X, extents.MaxPoint.Y);
                //得到当前视图
                ViewTableRecord view = ed.GetCurrentView();
                //设置视图的中心点、高度和宽度
                view.CenterPoint = minPt + (maxPt - minPt) / 2;
                view.Height = maxPt.Y - minPt.Y;
                view.Width = maxPt.X - minPt.X;
                //更新当前视图
                ed.SetCurrentView(view);
            }
        }


        /// <summary>
        /// 查询面积最大的多段线,key=0 图例框，key=1 其他多段线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static Dictionary<int, List<Polyline>> FindMaxAreaPoline(List<Polyline> polylines)
        {
            Dictionary<int, List<Polyline>> allPolines = new Dictionary<int, List<Polyline>>();
            allPolines.Add(0, new List<Polyline>());
            allPolines.Add(1, new List<Polyline>());
            double maxArea = double.NaN;
            foreach (Polyline line in polylines)
            {
                if (double.IsNaN(maxArea) || line.Area > maxArea)
                {
                    maxArea = line.Area;
                }
            }

            foreach (Polyline line in polylines)
            {
                if (line.Area.ToString("F0") == maxArea.ToString("F0"))
                {
                    allPolines[0].Add(line);
                }
                else
                {
                    allPolines[1].Add(line);
                }
            }
            return allPolines;
        }


        /// <summary>
        /// 根据图层名获取图层颜色
        /// </summary>
        /// <returns></returns>
        public static string GetLayerColorByName(string layerName)
        {
            string colorStr = "";
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction Trans = db.TransactionManager.StartTransaction())
            {
                //以读模式打开图层表
                LayerTable layerTable;
                layerTable = Trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                string sLayerName = "test1";
                if (layerTable.Has(layerName))
                {
                    LayerTableRecord ltr = Trans.GetObject(layerTable[layerName], OpenMode.ForNotify) as LayerTableRecord;
                   
                    colorStr = System.Drawing.ColorTranslator.ToHtml( ltr.Color.ColorValue);

                }

                return colorStr;
            }
        }
        /// <summary>
        /// 获取所有图层
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllLayer()
        {
            List<string> allLayers = new List<string>();
     
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //以读模式打开图层表
                LayerTable layerTable;
                layerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                foreach (ObjectId id in layerTable)
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(id, OpenMode.ForRead);
        
                    allLayers.Add(ltr.Name);
                }
                

            }
            return allLayers;
        }

        /// <summary>
        /// 根据图层名获取图层颜色
        /// </summary>
        /// <returns></returns>
        public static string GetLayerColorByID(ObjectId layerId)
        {
            string colorStr = "";
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction Trans = db.TransactionManager.StartTransaction())
            {

                LayerTableRecord ltr = Trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

                if (ltr != null)
                {

                    colorStr = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);

                }
            }
            return colorStr;
        }

        /// <summary>
        /// 根据图层名获取图层线型
        /// </summary>
        /// <returns></returns>
        public static string GetLayerLineTypeByID(ObjectId layerId)
        {
            string colorStr = "";
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction Trans = db.TransactionManager.StartTransaction())
            {
                LayerTableRecord ltr = Trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;
                if (ltr != null)
                {
                    LinetypeTableRecord re= Trans.GetObject(ltr.LinetypeObjectId, OpenMode.ForRead) as LinetypeTableRecord;
                    colorStr =re.Name;
                }
            }
            return colorStr;
        }

        /// <summary>
        /// 根据图层名获取图层线型
        /// </summary>
        /// <returns></returns>
        public static string GetLayerLineTypeByID(Entity entity)
        {
            
            string colorStr = "";
            if (entity.Linetype.ToString() == "BYLAYER")
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                using (Transaction Trans = db.TransactionManager.StartTransaction())
                {
                    LayerTableRecord ltr = Trans.GetObject(entity.LayerId, OpenMode.ForRead) as LayerTableRecord;
                    if (ltr != null)
                    {
                        LinetypeTableRecord re = Trans.GetObject(ltr.LinetypeObjectId, OpenMode.ForRead) as LinetypeTableRecord;
                        colorStr = re.Name;
                    }
                }
            }
            else
            {
                colorStr = entity.Linetype.ToString();   
            }
            return colorStr;
        }
        /// <summary>
        /// 点到线段的距离公式(利用平行四边形的面积算法，非常牛叉)
        /// </summary>
        /// <param name="P">目标点</param>
        /// <param name="A">线段端点A</param>
        /// <param name="B">线段端点B</param>
        /// <returns></returns>
        public static double DistancePointToSegment(Point2d P, Point2d A, Point2d B)
        {
            double atp = A.GetDistanceTo(P);
            double btp = B.GetDistanceTo(P);
            double atb = A.GetDistanceTo(B);

            if (atp * atp >= atb * atb + btp * btp)
            {
                return atp;
            }
            if (btp * btp >= atb * atb + atp * atp)
            {
                return btp;
            }
            //计算点到线段(a,b)的距离  
            double l = 0.0;
            double s = 0.0;
            l = DistancePointToPoint(A, B);
            s = ((A.Y - P.Y) * (B.X - A.X) - (A.X - P.X) * (B.Y - A.Y)) / (l * l);
            return (Math.Abs(s * l));
        }

        public static double GetMinDistance(PointF pt1, PointF pt2, PointF pt3)
        {
            double dis = 0;
            if (pt1.X == pt2.X)
            {
                dis = Math.Abs(pt3.X - pt1.X);
                return dis;
            }
            double lineK = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
            double lineC = (pt2.X * pt1.Y - pt1.X * pt2.Y) / (pt2.X - pt1.X);
            dis = Math.Abs(lineK * pt3.X - pt3.Y + lineC) / (Math.Sqrt(lineK * lineK + 1));
            return dis;

        }

        public static double GetMinDistance(Point2d pt1, Point2d pt2, Point2d pt3)
        {
            double dis = 0;
            if (pt1.X == pt2.X)
            {
                dis = Math.Abs(pt3.X - pt1.X);
                return dis;
            }
            double lineK = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
            double lineC = (pt2.X * pt1.Y - pt1.X * pt2.Y) / (pt2.X - pt1.X);
            dis = Math.Abs(lineK * pt3.X - pt3.Y + lineC) / (Math.Sqrt(lineK * lineK + 1));
            return dis;

        }

        /// <summary>
        /// 点到点的距离
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double DistancePointToPoint(Point2d ptA, Point2d ptB)
        {
            return Math.Sqrt(Math.Pow(ptA.X - ptB.X, 2) + Math.Pow(ptA.Y - ptB.Y, 2));
        }

        /// <summary>
        /// 点到点的距离
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double DistancePointToPoint(Point3d ptA, Point3d ptB)
        {
            return Math.Sqrt(Math.Pow(ptA.X - ptB.X, 2) + Math.Pow(ptA.Y - ptB.Y, 2));
        }



        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="StartPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static PointF GetEndPointByTrigonometric(double angle, PointF StartPoint, double distance)
        {
           PointF EndPoint = new PointF();

            //角度转弧度
            var radian = (angle * Math.PI) / 180;

            //计算新坐标 r 就是两者的距离
            EndPoint.X =(float) (StartPoint.X +distance * Math.Cos(radian));
            EndPoint.Y= (float)(StartPoint.Y + distance * Math.Sin(radian));
        
            return EndPoint;
        }

        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">弧度</param>
        /// <param name="StartPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static PointF GetEndPointByTrigonometricHu(double angle, PointF StartPoint, double distance)
        {
            PointF EndPoint = new PointF();

            //角度转弧度
         //   var radian = (angle * Math.PI) / 180;

            //计算新坐标 r 就是两者的距离
            EndPoint.X = (float)(StartPoint.X + distance * Math.Cos(angle));
            EndPoint.Y = (float)(StartPoint.Y + distance * Math.Sin(angle));

            return EndPoint;
        }

        /// <summary>
        /// 勾股定理求长度
        /// </summary>
        /// <param name="最长边长度">length1</param>
        /// <param name="边长度">length2</param>
        /// <returns>另一边长度</returns>
        public static double GetEndLengthByTheorem(double length1,double length2)
        {
            return Math.Sqrt(Math.Pow(length1,2)-Math.Pow(length2,2));
        }


        public static List<PointF> GetArcPoints(PointF centerPoint,double startAngel,double endAngel,double r)
        {
            List<PointF> list = new List<PointF>();
            double de2g = Math.PI / 180;
            double deg2 = 180 / Math.PI;
            int n = 360;
            startAngel = deg2 * startAngel;
            endAngel = deg2 * endAngel;
            for (int i = 0; i < n + 1; i++)
            {
                PointF pt = new PointF();
                double ang = (startAngel + (endAngel - startAngel) * i / n) * de2g;
                pt.X =(float)( centerPoint.X + Math.Cos(ang));
                pt.Y = (float)(centerPoint.Y + Math.Cos(ang));
                list.Add(pt);
            }
            return list;
        }

        public static List<PointF> GetArcPointsByPoint2d(Point2d[] ptList)
        {
            List<PointF> list = new List<PointF>();
            foreach (Point2d pt in ptList)
            { 
                list.Add(new PointF(((float)pt.X),(float)pt.Y));
            }
            return list;
        }

        public static Point2dCollection GetPoint2DByPointF(List<PointF> ptList)
        {
            Point2dCollection list = new Point2dCollection();
            foreach (PointF pt in ptList)
            {
                list.Add(new Point2d(((float)pt.X), (float)pt.Y));
            }
            return list;
        }

        /// <summary>
        /// 结构：表示一个点
        /// </summary>
        public struct MyPoint
        {
            //横、纵坐标
            public double x, y;
            //构造函数
            public MyPoint(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            //该点到指定点pTarget的距离
            public double DistanceTo(MyPoint p)
            {
                return Math.Sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y));
            }
            //重写ToString方法
            public override string ToString()
            {
                return string.Concat("Point (",
                 this.x.ToString("#0.000"), ',',
                 this.y.ToString("#0.000"), ')');
            }
        }
        /// <summary>
        /// 计算点P(x,y)与X轴正方向的夹角
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>夹角弧度</returns>
        private static double radPOX(double x, double y)
        {
            //P在(0,0)的情况
            if (x == 0 && y == 0) return 0;
            //P在四个坐标轴上的情况：x正、x负、y正、y负
            if (y == 0 && x > 0) return 0;
            if (y == 0 && x < 0) return Math.PI;
            if (x == 0 && y > 0) return Math.PI / 2;
            if (x == 0 && y < 0) return Math.PI / 2 * 3;
            //点在第一、二、三、四象限时的情况
            if (x > 0 && y > 0) return Math.Atan(y / x);
            if (x < 0 && y > 0) return Math.PI - Math.Atan(y / -x);
            if (x < 0 && y < 0) return Math.PI + Math.Atan(-y / -x);
            if (x > 0 && y < 0) return Math.PI * 2 - Math.Atan(-y / x);
            return 0;
        }
        /// <summary>
        /// 返回点P围绕点A旋转弧度rad后的坐标
        /// </summary>
        /// <param name="P">待旋转点坐标</param>
        /// <param name="A">旋转中心坐标</param>
        /// <param name="rad">旋转弧度</param>
        /// <param name="isClockwise">true:顺时针/false:逆时针</param>
        /// <returns>旋转后坐标</returns>
      public static MyPoint RotatePoint(MyPoint P, MyPoint A,
         double rad, bool isClockwise = true)
        {
            //点Temp1
            MyPoint Temp1 = new MyPoint(P.x - A.x, P.y - A.y);
            //点Temp1到原点的长度
            double lenO2Temp1 = Temp1.DistanceTo(new MyPoint(0, 0));
            //∠T1OX弧度
            double angT1OX = radPOX(Temp1.x, Temp1.y);
            //∠T2OX弧度（T2为T1以O为圆心旋转弧度rad）
            double angT2OX = angT1OX - (isClockwise ? 1 : -1) * rad;
            //点Temp2
            MyPoint Temp2 = new MyPoint(
             lenO2Temp1 * Math.Cos(angT2OX),
             lenO2Temp1 * Math.Sin(angT2OX));
            //点Q
            return new MyPoint(Temp2.x + A.x, Temp2.y + A.y);
        }

        public static List<PointF> GetRoationPoint(MyPoint startpt, MyPoint endpt,MyPoint center,
    double startAngel, double endAngel,bool isClockWise)
        {
            List<PointF> points = new List<PointF>();
            int angelCout =Math.Abs( (int)(( endAngel - startAngel)/0.1));
          
            for (int i=0;i<angelCout;i++)
            {
                MyPoint pt = RotatePoint(startpt, center,((double)i)/10.0, isClockWise);
                points.Add(new PointF(( float)pt.x, (float)pt.y));
            }
            return points;
        }

        public static List<PointF> GetArcPoints(Curve cre,double length)
        {
            List<PointF> points = new List<PointF>();
          //  length = cre.EndParam;

            for (int i = 0; i < 20; i++)
            {
                double dis=cre.GetParameterAtDistance(length * (i / 20.0));
                Point3d pt = cre.GetPointAtParameter(dis);
                points.Add(new PointF((float)pt.X, (float)pt.Y));
            }
            points.Add(points[0]);
            return points;
        }


        public static List<PointF> GetArcPointsByBulge(Point2d startPoint,Point2d endPoint,double bulge)
        {
            List<PointF> list = new List<PointF>();
       
            CircularArc2d circularArc2 = new CircularArc2d(startPoint,endPoint,bulge,false);
           
            list= GetArcPointsByPoint2d(circularArc2.GetSamplePoints(20));
            return list;
        }

        public static bool PointsAllInPoints(List<PointF> ptSamll, List<PointF> ptBig)
        {
           // PolylineCurve2d circularArc2 = new PolylineCurve2d(GetPoint2DByPointF(ptBig));
            foreach (PointF item in ptSamll)
            {
                if (PtinPolygon(item, ptBig)==1)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 判断点是否在闭合多线段内
        /// </summary>
        /// <param name="pt">待判断点</param>
        /// <param name="poly">目标多边形</param>
        /// <returns>在多边形内则返回1</returns>

        private static double min(double x, double y)
        {
            if (x > y)
                return y;
            else
                return x;
        }
        private static double max(double x, double y)
        {
            if (x > y)
                return x;
            else
                return y;
        }
        /// <summary>
        /// 判断点是否在图形内
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="ptPolygon"></param>
        /// <returns></returns>
        public static int PtinPolygon(Point2d pt, Point2d[] ptPolygon)
        {
            int nCount = ptPolygon.Length;
            // 记录是否在多边形的边上
            bool isBeside = false;
            // 多边形外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;
                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }
                if ((pt.X > maxx) || (pt.X < minx) || (pt.Y > maxy) || (pt.Y < miny))
                    return -1;
            }
            // 射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount; i++)
            {
                Point2d p1 = ptPolygon[i];
                Point2d p2 = ptPolygon[(i + 1) % nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && pt.X <= max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                // 交点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                // 求交点的X坐标
                double x = (double)(pt.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
                if (x > pt.X)
                    nCross++;  // 只统计单边交点
                else if (x == pt.X)
                    isBeside = true;
            }
            if (isBeside)
                return 0;  //多边形上
            else if (nCross % 2 == 1)
                return 1;  // 多边形内
            return -1;     // 多边形外
        }

        public static int PtinPolygon(Point2d pt, Point2dCollection ptPolygon)
        {
            int nCount = ptPolygon.Count;
            // 记录是否在多边形的边上
            bool isBeside = false;
            // 多边形外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;
                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }
                if ((pt.X > maxx) || (pt.X < minx) || (pt.Y > maxy) || (pt.Y < miny))
                    return -1;
            }
            // 射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount; i++)
            {
                Point2d p1 = ptPolygon[i];
                Point2d p2 = ptPolygon[(i + 1) % nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && pt.X <= max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                // 交点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                // 求交点的X坐标
                double x = (double)(pt.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
                if (x > pt.X)
                    nCross++;  // 只统计单边交点
                else if (x == pt.X)
                    isBeside = true;
            }
            if (isBeside)
                return 0;  //多边形上
            else if (nCross % 2 == 1)
                return 1;  // 多边形内
            return -1;     // 多边形外
        }
        public static int PtinPolygon(PointF pt, List<PointF> ptPolygon)
        {
            int nCount = ptPolygon.Count;
            // 记录是否在多边形的边上
            bool isBeside = false;
            // 多边形外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;
                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }
                if ((pt.X > maxx) || (pt.X < minx) || (pt.Y > maxy) || (pt.Y < miny))
                    return -1;
            }
            // 射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount; i++)
            {
                PointF p1 = ptPolygon[i];
                PointF p2 = ptPolygon[(i + 1) % nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && pt.X <= max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                // 交点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                // 求交点的X坐标
                double x = (double)(pt.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
                if (x > pt.X)
                    nCross++;  // 只统计单边交点
                else if (x == pt.X)
                    isBeside = true;
            }
            if (isBeside)
                return 0;  //多边形上
            else if (nCross % 2 == 1)
                return 1;  // 多边形内
            return -1;     // 多边形外
        }
        //定数等分单条曲线
        public static DBObjectCollection AveragesCurve(Curve cv, int n)
        {
            double ep = cv.EndParam;
            double len = cv.GetDistanceAtParameter(ep);
            double split = len / n;

            DoubleCollection pas = new DoubleCollection();
            if (cv.Closed)                //闭合曲线要先断开
                pas.Add(cv.StartParam);

            for (int i = 1; i < n; i++)
            {
                pas.Add(cv.GetParameterAtDistance(i * split));
            }
            cv.Erase();
            return cv.GetSplitCurves(pas);
        }
        //等距等分单条曲线(dist<0反向等分)
        public static DBObjectCollection AveragesCurve(Curve cv, Double dist)
        {
            double ep = cv.EndParam;
            double len = cv.GetDistanceAtParameter(ep);
            double dst = 0;
            //从最后开始等分
            if (dist < 0)
            {
                dist = -dist;
                dst = (cv.GetDistanceAtParameter(cv.EndParam) % dist) - dist;
            }

            DoubleCollection pas = new DoubleCollection();
            if (cv.Closed) pas.Add(cv.StartParam);//闭合曲线要先断开

            while (true)
            {
                dst += dist;
                if (dst >= len) break;
                pas.Add(cv.GetParameterAtDistance(dst));
            }

            cv.Erase();
            return cv.GetSplitCurves(pas);
        }

        /// <summary>
        /// 多段线转curve2d
        /// </summary>
        /// <param name="pPoly"></param>
        /// <returns></returns>
        public   static Curve2d convertPolylineToGeCurve(Polyline pPoly)

        {
            List<Curve2d> geCurves = new List<Curve2d>();
            Curve2d out_pGeCurve;
            //  PointerArray geCurves;
            Vector3d normal = pPoly.Normal;
            // Is the polyline closed or open
            int nSegs = -1;
            if (pPoly.Closed)
                nSegs = pPoly.NumberOfVertices;
            else
                nSegs = pPoly.NumberOfVertices - 1;

            for (int i = 0; i < nSegs; i++)
            {
                if (pPoly.GetSegmentType(i) == SegmentType.Line)
                {
                    LineSegment2d line;
                    line = pPoly.GetLineSegment2dAt(i);

                    geCurves.Add(line);
                }
                else if (pPoly.GetSegmentType(i) == SegmentType.Arc)
                {
                    CircularArc2d arc;
                    arc = pPoly.GetArcSegment2dAt(i);

                    geCurves.Add(arc);
                }
            }// for
            if (geCurves.Count == 1)
                out_pGeCurve = (Curve2d)(geCurves[0]);
            else
                out_pGeCurve = new CompositeCurve2d(geCurves.ToArray());
            return out_pGeCurve;
        }


        public static List<ObjectId> GetCrossObjectIds(Point3dCollection p3c, Editor ed)
        {
            List<ObjectId> ooids = new List<ObjectId>();

            PromptSelectionResult psr = ed.SelectCrossingPolygon(p3c);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet sst = psr.Value;
                ObjectId[] oids = sst.GetObjectIds();

                foreach (ObjectId item in oids)
                {
                    ooids.Add(item);
                }

            }

            return ooids;
        }
        /// <summary>
        /// 获取用地编码-分图则
        /// </summary>
        /// <param name="pointFs"></param>
        /// <returns></returns>
        public static string GetAttrIndex(List<PointF> pointFs)
        {
            // 属性索引
            // 获取每个hatch对应的用地代码
            Document doc = Application.DocumentManager.MdiActiveDocument;
            string word = "";
            List<ObjectId> idArray2 = new List<ObjectId>();

             Point3dCollection col_point3d = new Point3dCollection();
            foreach (PointF pf in pointFs)
            {
                col_point3d.Add(new Point3d(pf.X, pf.Y, 0));
            }

            List<string> layers = new List<string>() { "地块编码", "用地代码"};
            foreach (string layerName in layers)
            { 
                TypedValue[] tvs2 =
                    new TypedValue[1] {
                        new TypedValue(
                            (int)DxfCode.LayerName,
                            layerName
                        )
                    };

                SelectionFilter sf2 = new SelectionFilter(tvs2);
                PromptSelectionResult psr2 = doc.Editor.SelectAll(sf2);
                SelectionSet SS2 = psr2.Value;

                if (psr2.Status == PromptStatus.OK)
                {
                    ObjectId[] idArray3 = SS2.GetObjectIds();
                    for (int j = 0; j < idArray3.Length; j++)
                    {
                        idArray2.Add(idArray3[j]);
                    }
                }
            }
            if (idArray2 != null)
            {
                using (Transaction trans = doc.Database.TransactionManager.StartTransaction())
                {
                    for (int j = 0; j < idArray2.Count; j++)
                    {
                        Entity ent1 = (Entity)idArray2[j].GetObject(OpenMode.ForRead);

                        if (ent1 is BlockReference)
                        {
                            List<string> tagList = new List<string>();

                            if (IsInPolygon((ent1 as BlockReference).Position, col_point3d))
                            {
                                foreach (ObjectId rt in ((BlockReference)ent1).AttributeCollection)
                                {
                                    DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
                                    AttributeReference acAttRef = dbObj as AttributeReference;

                                    tagList.Add(acAttRef.TextString);

                                    //MessageBox.Show("Tag: " + acAttRef.Tag + "\n" +
                                    //                "Value: " + acAttRef.TextString + "\n");
                                }
                            }
                            // 如果地块编码属性只有两个属性值，attributeIndexList，如果少于2个或者多于2个都视为异常，添加空。
                            if (tagList.Count == 2)
                            {
                                word = tagList[0] + "_" + tagList[1];
                            }
                        }

                        if (ent1 is DBText)
                        {
                            if (IsInPolygon((ent1 as DBText).Position, col_point3d))
                            {
                                word = ((DBText)ent1).TextString;
                            }
                        }
                        if (ent1 is MText)
                        {
                            if (IsInPolygon((ent1 as MText).Location, col_point3d))
                            {
                                word = ((MText)ent1).Text;
                            }
                        }
                    }
                }
            }

            //string word = "";

            //List<ObjectId> idArray = GetCrossObjectIds(col_point3d, doc.Editor);

            //for (int i = 0; i < idArray.Count; i++)
            //{
            //    Entity ent1 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
            //    if (ent1 is DBText)
            //    {
            //        word = ((DBText)ent1).TextString;

            //    }
            //    if (ent1 is MText)
            //    {
            //        word = ((MText)ent1).Text;
            //    }
            //    if (ent1 is BlockReference)
            //    {
            //        List<string> tagList = new List<string>();
            //        using (Transaction trans = doc.Database.TransactionManager.StartTransaction())
            //        {
            //            BlockReference ent2 = (BlockReference)ent1;
            //            if (IsInPolygon(ent2.Position, col_point3d))
            //            {
            //                foreach (ObjectId rt in ((BlockReference)ent1).AttributeCollection)
            //                {
            //                    DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
            //                    AttributeReference acAttRef = dbObj as AttributeReference;

            //                    tagList.Add(acAttRef.TextString);

            //                    //MessageBox.Show("Tag: " + acAttRef.Tag + "\n" +
            //                    //                "Value: " + acAttRef.TextString + "\n");
            //                }
            //            }
            //        }

            //        // 如果地块编码属性只有两个属性值，attributeIndexList，如果少于2个或者多于2个都视为异常，添加空。
            //        if (tagList.Count == 2)
            //        {
            //            word = tagList[0] + "_" + tagList[1];
            //        }

            //    }
            //}

            return word;

        }

        // 判断点是否在闭合多段线内
        public static bool IsInPolygon(Point3d checkPoint, Point3dCollection col_point2d)
        {
            bool inside = false;
            int pointCount = col_point2d.Count;
            Point3d p1, p2;
            for (int i = 0, j = pointCount - 1; i < pointCount; j = i, i++)//第一个点和最后一个点作为第一条线，之后是第一个点和第二个点作为第二条线，之后是第二个点与第三个点，第三个点与第四个点...
            {
                p1 = col_point2d[i];
                p2 = col_point2d[j];
                if (checkPoint.Y < p2.Y)
                {//p2在射线之上
                    if (p1.Y <= checkPoint.Y)
                    {//p1正好在射线中或者射线下方
                        if ((checkPoint.Y - p1.Y) * (p2.X - p1.X) > (checkPoint.X - p1.X) * (p2.Y - p1.Y))//斜率判断,在P1和P2之间且在P1P2右侧
                        {
                            //射线与多边形交点为奇数时则在多边形之内，若为偶数个交点时则在多边形之外。
                            //由于inside初始值为false，即交点数为零。所以当有第一个交点时，则必为奇数，则在内部，此时为inside=(!inside)
                            //所以当有第二个交点时，则必为偶数，则在外部，此时为inside=(!inside)
                            inside = (!inside);
                        }
                    }
                }
                else if (checkPoint.Y < p1.Y)
                {
                    //p2正好在射线中或者在射线下方，p1在射线上
                    if ((checkPoint.Y - p1.Y) * (p2.X - p1.X) < (checkPoint.X - p1.X) * (p2.Y - p1.Y))//斜率判断,在P1和P2之间且在P1P2右侧
                    {
                        inside = (!inside);
                    }
                }
            }
            return inside;
        }
        public static System.Drawing.PointF Point2d2Pointf(Point2d point2d)
        {
            return new System.Drawing.PointF((float)point2d.X, (float)point2d.Y);
        }

        public static System.Drawing.PointF Point3d2Pointf(Point3d point2d)
        {
            return new System.Drawing.PointF((float)point2d.X, (float)point2d.Y);
        }
    }
}
        
    
