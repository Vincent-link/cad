using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;

namespace RegulatoryPlan.Method
{
    public class CenterCityUseLandMethod<T> : AttributeBaseMethod<T> where T : CenterCityUseLandPlanModel
    {
        private CenterCityUseLandMethod() { }
        public CenterCityUseLandMethod(T model):base(model)
        {
            
        }
        public override void GetAllAttributeInfo(T model)
        {
            foreach (AttributeModel am in model.attributes)
            {
                foreach (string layerName in GetRealLayer(am.LayerName))
                {
                    LayerModel lyModel = new LayerModel();
                    List<BlockInfoModel> list = new List<BlockInfoModel>();
                    lyModel.Name = layerName;
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
                    List<BlockReference> blockTableRecords = new List<BlockReference>();
                    if (ProSset.Status == PromptStatus.OK)
                    {
                        lyModel.pointFs = new Dictionary<int, List<object>>();
                        using (Transaction tran = db.TransactionManager.StartTransaction())
                        {
                            SelectionSet sst = ProSset.Value;

                            ObjectId[] oids = sst.GetObjectIds();

                            int ad = 0;
                            List<string> aa = new List<string>();

                            LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                            foreach (ObjectId layerId in lt)
                            {
                                LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                                if (ltr.Name == layerName)
                                {
                                    lyModel.Color = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                                }
                            }

                            int i = 0;
                            foreach (ObjectId lengGemo in oids)
                            {

                                DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                                BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob,am);
                                if (plModel != null)
                                {
                                    List<object> obj = new List<object>() { plModel };
                                    lyModel.pointFs.Add(i, obj);
                                    i++;
                                }

                            }
                        }
                        if (model.allLines == null)
                        {
                            model.allLines = new List<LayerModel>();
                        }
                        model.allLines.Add(lyModel);
                    }

                }
            }
        }
    }
}
