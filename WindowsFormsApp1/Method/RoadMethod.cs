using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Method
{
    public class RoadMethod<T> where T : RoadModel
    {
        public void GetAllRoadInfo(T model)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            PromptSelectionResult ProSset1 = null;
            PromptSelectionResult ProSsetSection = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, model.RoadNameLayer) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;
            TypedValue[] filList1 = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, model.RoadLineLayer) };
            SelectionFilter sfilter1 = new SelectionFilter(filList1);
            TypedValue[] filListSec = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, model.RoadSectionLayer) };
            SelectionFilter sfilterSec = new SelectionFilter(filListSec);


            string ss = layoutMgr.CurrentLayout;

            ProSset = doc.Editor.SelectAll(sfilter);
            ProSset1 = doc.Editor.SelectAll(sfilter1);
            ProSsetSection = doc.Editor.SelectAll(sfilterSec);
            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            List<Polyline> roadLine = new List<Polyline>();
            List<DBText> roadText = new List<DBText>();
            List<Polyline> sectionLine = new List<Polyline>();
            List<DBText> sectionText = new List<DBText>();
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK && ProSset1.Status == PromptStatus.OK)
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
                            lm.Color =  System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                        }
                    }
                    for (int i = 0; i < oids.Length; i++)
                    {

                        DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);

                            if ((ob as DBText).BlockName.ToLower() == "*model_space")
                            {

                                roadText.Add((ob as DBText));
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

                    if(ProSsetSection.Status== PromptStatus.OK)
                    {
                        SelectionSet sstSec = ProSsetSection.Value;
                        ObjectId[] oidsSec = sstSec.GetObjectIds();
                        for (int i = 0; i < oidsSec.Length; i++)
                        {

                            DBObject ob = tran.GetObject(oidsSec[i], OpenMode.ForRead);


                            if (ob is Polyline)
                            {
                                if ((ob as Polyline).BlockName.ToLower() == "*model_space")
                                {
                                    sectionLine.Add((ob as Polyline));

                                }


                            }
                            else if (ob is DBText)
                            {

                                if ((ob as DBText).BlockName.ToLower() == "*model_space")
                                {

                                    sectionText.Add((ob as DBText));

                                }
                            }
                        }
                    }
                    lm.Name = model.RoadLineLayer;
                    lm.modelItemList = new List<object>();
                    foreach (Polyline polyline in roadLine)
                    {
                        if (sectionLine.Count > 0)
                        {
                            lm.modelItemList.Add(GetRoadItemInfo(polyline, roadText,sectionLine,sectionText));
                        }
                        else
                        {
                            lm.modelItemList.Add(GetRoadItemInfo(polyline, roadText));
                        }
                    }
                    if (model.allLines == null)
                    {
                        model.allLines = new List<LayerModel>();
                    }
                    model.allLines.Add(lm);
                }
            }


        }

        public RoadInfoItemModel GetRoadItemInfo(Polyline line, List<DBText> txtList)
        {
            RoadInfoItemModel item = new RoadInfoItemModel();
            item.RoadLength = line.Length.ToString();
            item.RoadWidth = line.ConstantWidth.ToString();
            item.RoadType = "polyline";
            item.ColorIndex = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
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

                    for (int i = 0; i < vtnum - 1; i++)
                    {
                        if (PolylineMethod.PtInPolyLine(pt, line.GetPoint2dAt(i), line.GetPoint2dAt(i + 1), 20) == 1)
                        {
                            item.RoadName = mText.TextString.Replace(" ", "").Replace("　", "");
                            item.RoadNameLocaiton = new List<System.Drawing.PointF>();
                            double middleLen = MethodCommand.DistancePointToPoint(mText.Position, mText.AlignmentPoint);
                            double textLen = MethodCommand.GetEndLengthByTheorem(middleLen, mText.Height / 2) * 2;
                            double partLength = textLen / item.RoadName.Length;

                            for (int j = 1; j < item.RoadName.Length + 1; j++)
                            {
                                item.RoadNameLocaiton.Add(MethodCommand.GetEndPointByTrigonometricHu(mText.Rotation, AutoCad2ModelTools.Point3d2Pointf(mText.Position), partLength * j));
                            }

                            item.RoadNameLayer = mText.Layer;
                            item.RoadNameType = "text";

                            break;
                        }
                    }

                }
                item.roadList = PolylineMethod.GetPolyLineInfoPt(line);
            }
            return item;
        }
        public RoadInfoItemModel GetRoadItemInfo(Polyline line, List<DBText> txtList, List<Polyline> sectionLineList, List<DBText> sectionTextList)
        {
            RoadInfoItemModel item = new RoadInfoItemModel();
            item.RoadLength = line.Length.ToString();
            item.RoadWidth = line.ConstantWidth.ToString();
            item.RoadType = "polyline";
            item.ColorIndex = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            item.roadList = PolylineMethod.GetPolyLineInfoPt(line);
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                int vtnum = line.NumberOfVertices;
                for (int i = 0; i < vtnum - 1; i++)
                {
                    foreach (DBText mText in txtList)
                    {
                        if (item.RoadName != null)
                        {
                            break;
                        }
                        Point2d pt = mText.AlignmentPoint.Convert2d(new Plane());

                     

                        if (PolylineMethod.PtInPolyLine(pt, line.GetPoint2dAt(i), line.GetPoint2dAt(i + 1), 20) == 1)
                        {
                            item.RoadName = mText.TextString.Replace(" ", "").Replace("　", "");
                            item.RoadNameLocaiton = new List<System.Drawing.PointF>();
                            item.sectionList = new List<RoadSectionItemModel>();
                       
                            double middleLen = MethodCommand.DistancePointToPoint(mText.Position, mText.AlignmentPoint);
                            double textLen = MethodCommand.GetEndLengthByTheorem(middleLen, mText.Height / 2) * 2;
                            double partLength = textLen / item.RoadName.Length;

                            for (int j = 1; j < item.RoadName.Length + 1; j++)
                            {
                                item.RoadNameLocaiton.Add(MethodCommand.GetEndPointByTrigonometricHu(mText.Rotation, AutoCad2ModelTools.Point3d2Pointf(mText.Position), partLength * j));
                            }

                            item.RoadNameLayer = mText.Layer;
                            item.RoadNameType = "text";

                            break;
                        }

                    }
                    //获取横截面
                  

                    foreach (Polyline pl in sectionLineList)
                    {
                        for (int j = 0; j < pl.NumberOfVertices; j++)
                        {
                            double dic = MethodCommand.DistancePointToSegment(pl.GetPoint2dAt(j), line.GetPoint2dAt(i), line.GetPoint2dAt(i + 1));
                            if (dic < 60)
                            {

                                RoadSectionItemModel modelsc = new RoadSectionItemModel();
                                modelsc.Line = AutoCad2ModelTools.Polyline2Model(pl);
                                DBText secMtext = MethodCommand.FindMTextIsInPolyineForPipe(pl, sectionTextList);
                                if (secMtext != null)
                                {
                                    modelsc.SectionName = AutoCad2ModelTools.DbText2Model(secMtext);
                                }
                                item.sectionList.Add(modelsc);

                            }
                        }
                    }
                }
                    return item;
                }

            


        }
    }
}
