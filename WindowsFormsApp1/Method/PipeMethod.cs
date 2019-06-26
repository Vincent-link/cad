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

            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName,model.PipeInfo) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;


            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);

            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            List<Polyline> roadLine = new List<Polyline>();
            foreach (string layer in model.LayerList)
            {
                roadLine.AddRange(PolylineMethod.GetPolyliness(layer));
            }
            List<MText> roadText = new List<MText>();
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK )
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;

                    ObjectId[] oids = sst.GetObjectIds();

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

                        if (ob is MText)
                        {
                            if ((ob as MText).BlockName.ToLower() == "*model_space")
                            {
                                roadText.Add((ob as MText));
                                rt++;
                            }
                        }
                    }
                
                    lm.Name = model.LayerList[0];
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
                item.Style.LineWidth= line.ConstantWidth.ToString();
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
                        item.PipeLayer =mText.Layer;
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
