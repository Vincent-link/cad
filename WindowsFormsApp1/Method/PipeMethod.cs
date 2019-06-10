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
   public class PipeMethod<T>where T:PipeModel
    {
        public void GetAllPipeInfo(T model)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            PromptSelectionResult ProSset1 = null;
            PromptSelectionResult ProSsetActuality = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName,model.PipeInfo) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;
            TypedValue[] filList1 = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName,model.PipePlanLine) };
            SelectionFilter sfilter1 = new SelectionFilter(filList1);

            TypedValue[] filListActuality = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, model.PipeActualityLine) };
            SelectionFilter sfilterActuality = new SelectionFilter(filListActuality);

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            ProSset1 = doc.Editor.SelectAll(sfilter1);
            ProSsetActuality = doc.Editor.SelectAll(sfilterActuality);
            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            List<Polyline> roadLine = new List<Polyline>();
            List<MText> roadText = new List<MText>();
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK && ProSset1.Status == PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;
                    SelectionSet sst1 = ProSset1.Value;
                    SelectionSet sstActuality = ProSsetActuality.Value;
                    ObjectId[] oids = sst.GetObjectIds();
                    ObjectId[] oids1 = sst1.GetObjectIds();
                    ObjectId[] oidsActuality = sstActuality.GetObjectIds();
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

                        if (ob is MText)
                        {
                            if ((ob as MText).BlockName.ToLower() == "*model_space")
                            {
                                roadText.Add((ob as MText));
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

                    for (int i = 0; i < oidsActuality.Length; i++)
                    {   

                        DBObject ob = tran.GetObject(oidsActuality[i], OpenMode.ForRead);


                        if (ob is Polyline)
                        {
                            if ((ob as Polyline).BlockName.ToLower() == "*model_space")
                            {
                                roadLine.Add((ob as Polyline));
                                rl++;
                            }

                        }
                    }
                    lm.Name = model.PipePlanLine;
                    lm.modelItemList = new List<object>();
                    foreach (Polyline polyline in roadLine)
                    {
                        lm.modelItemList.Add(GetPipeItemInfo(polyline, roadText));
                    }
                    if (model.allLines == null)
                    {
                        model.allLines = new List<LayerModel>();
                    }
                    model.allLines.Add(lm);
                }
            }


        }

        public PipeItemModel GetPipeItemInfo(Polyline line, List<MText> txtList)
        {
        
            PipeItemModel item = new PipeItemModel();
            try
            {
                item.PipeLength = line.Length.ToString();
                item.PipeWidth = line.ConstantWidth.ToString();
                item.ColorIndex = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    int vtnum = line.NumberOfVertices;
                    MText mText = MethodCommand.FindMTextIsInPolyineForPipe(line, txtList);
                    if (mText != null)
                    {
                        item.TxtLocation = new System.Drawing.PointF((float)mText.Location.X, (float)mText.Location.Y);
                        item.PipeType = mText.Text; //Replace(" ", "").Replace("　", "");
                        item.PipeLayer = line.Layer;
                    }


                }
                item.pipeList = PolylineMethod.GetPolyLineInfoPt(line);
            }
            catch
            { }
            return item;
        }
    }
}
