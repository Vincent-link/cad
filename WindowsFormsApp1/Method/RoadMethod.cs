using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Method
{
   public class RoadMethod<T> where T : RoadSectionModel
    {
        public void GetAllRoadInfo(T model)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            PromptSelectionResult ProSset1 = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, RoadSectionModel.roadNameLayer) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;
            TypedValue[] filList1 = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, RoadSectionModel.roadLineLayer) };
            SelectionFilter sfilter1 = new SelectionFilter(filList1);
    

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            ProSset1 = doc.Editor.SelectAll(sfilter1);
            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            List<Polyline> roadLine = new List<Polyline>();
            List<DBText> roadText = new List<DBText>();
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK&&ProSset1.Status==PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;
                    SelectionSet sst1 = ProSset1.Value;
                    ObjectId[] oids = sst.GetObjectIds();
                    ObjectId[] oids1 = sst1.GetObjectIds();
               
                    int rt = 0;
                    int rl = 0;
                    //List<string> aa = new List<string>();
                    LayerModel lm = new LayerModel();
                    LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                    foreach (ObjectId layerId in lt)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                        if (ltr.Name == MethodCommand.LegendLayer)
                        {
                            lm.Color = ltr.Color.PenIndex;
                        }
                    }
                    for (int i = 0; i < oids.Length; i++)
                    {
                       
                        DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);
                   
                            if (ob is DBText)
                            {

                            if ((ob as DBText).BlockName.ToLower() == "*model_space")
                            {

                                roadText.Add((ob as DBText));
                               rt++;
                            }
                        }
                    }
                    for (int i = 0; i < oids1.Length; i++)
                    {

                        DBObject ob = tran.GetObject(oids1[i], OpenMode.ForRead);

                        
                            if (ob is Polyline)
                            {
                            if ((ob as Polyline).BlockName.ToLower() == "*model_space")
                            {
                                roadLine.Add((ob as Polyline));
                                rl++;
                            }

                         
                        }
                    }
                    lm.Name = RoadSectionModel.roadLineLayer;
                    lm.modelItemList = new List<object>();
                    foreach (Polyline polyline in roadLine)
                    {
                       lm.modelItemList.Add(GetRoadItemInfo(polyline,roadText));   
                    }
                    if (model.allLines==null)
                    {
                        model.allLines = new List<LayerModel>();
                    }
                    model.allLines.Add(lm);
                }
            }


        }

        public RoadSectionItemModel GetRoadItemInfo(Polyline line,List<DBText> txtList)
        {
            RoadSectionItemModel item = new RoadSectionItemModel();
            item.RoadLength = line.Length.ToString();
            item.RoadWidth = line.ConstantWidth.ToString();
            item.ColorIndex = line.ColorIndex.ToString();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                int vtnum = line.NumberOfVertices;

                foreach (DBText mText in txtList)
                {
                    if (item.RoadName != null)
                    {
                        break;
                    }
                        Point2d pt = mText.AlignmentPoint.Convert2d(new Plane());

                        for (int i = 0; i < vtnum-1; i++)
                        {
                            if (PolylineMethod.PtInPolyLine(pt,line.GetPoint2dAt(i),line.GetPoint2dAt(i+1),20)==1)
                            {
                                item.RoadName =  mText.TextString.Replace(" ", "").Replace("　","");
                                break;
                            }
                        }
                    
                }
                 item.roadList= PolylineMethod.GetPolyLineInfoPt(line);
            }
            return item;
        }

        
    }
}
