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
    public class CenterCityLifeUseLandMethod<T> : AttributeBaseMethod<T> where T : CenterCityLifeUseLandPlanModel
    {
        // 中心城区居住用地规划
        private CenterCityLifeUseLandMethod() { }
        public CenterCityLifeUseLandMethod(T model) : base(model)
        {

        }
        public override void GetAllAttributeInfo(T model)
        {
            foreach (AttributeModel am in model.attributes)
            {
                foreach (string layerName in GetRealLayer(am.LayerName))
                {
                    Document doc = Application.DocumentManager.MdiActiveDocument;
                    Database db = doc.Database;

                    LayerModel lyModel = new LayerModel();
                    lyModel.IsHaveAttribute = true;
                    List<BlockInfoModel> list = new List<BlockInfoModel>();
                    lyModel.Name = layerName;
                    // Document doc = Application.DocumentManager.MdiActiveDocument;
                    ObjectIdCollection ids = new ObjectIdCollection();

                    PromptSelectionResult ProSset = null;
                    TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName) };
                    SelectionFilter sfilter = new SelectionFilter(filList);
                    LayoutManager layoutMgr = LayoutManager.Current;

                    string ss = layoutMgr.CurrentLayout;
                    ProSset = doc.Editor.SelectAll(sfilter);
                    //  List<ObjectId> idss=  GetEntitiesInModelSpace();
                    //    Database db = doc.Database;
                    List<BlockReference> blockTableRecords = new List<BlockReference>();
                    if (ProSset.Status == PromptStatus.OK)
                    {
                        lyModel.pointFs = new Dictionary<int, List<object>>();
                        using (Transaction tran = doc.Database.TransactionManager.StartTransaction())
                        {
                            SelectionSet sst = ProSset.Value;

                            ObjectId[] oids = sst.GetObjectIds();

                            int ad = 0;
                            List<string> aa = new List<string>();

                            LayerTable lt = (LayerTable)doc.Database.LayerTableId.GetObject(OpenMode.ForRead);
                            foreach (ObjectId layerId in lt)
                            {
                                LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                                if (ltr.Name == layerName)
                                {
                                    lyModel.Color = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                                }
                            }
                            double totalFamily = 0;
                            double totalPeople = 0;
                            int i = 0;

                            foreach (ObjectId lengGemo in oids)
                            {
                                DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                                BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob, am);

                                foreach (DbTextModel dbTextModel in plModel.DbText)
                                {
                                    if (dbTextModel.attItemList != null && dbTextModel.attItemList.Count > 0)
                                    {
                                        foreach (AttributeItemModel attribute in dbTextModel.attItemList)
                                        {
                                            if (attribute.AtValue.Contains(":") || attribute.AtValue.Contains("："))
                                            {
                                                string[] fmList = attribute.AtValue.Split(new char[2] { '：', ':' });

                                                if (attribute.TargetName == "总户数")
                                                {

                                                    totalFamily += double.Parse(fmList[3]);
                                                }
                                                else if (attribute.TargetName == "总人数")
                                                {
                                                    
                                                    totalPeople += double.Parse(System.Text.RegularExpressions.Regex.Replace(fmList[2].Replace("(", "").Replace(")", ""), @"[\u4e00-\u9fa5]", ""));

                                                }

                                            }

                                        }
                                    }
                                }


                                if (plModel != null)
                                {
                                    List<object> obj = new List<object>() { plModel };
                                    lyModel.pointFs.Add(i, obj);
                                    i++;
                                }

                            }
                            foreach (List<object> ptssItem in lyModel.pointFs.Values)
                            {

                                foreach (object ptsItem in ptssItem)
                                {
                                    BlockInfoModel tempBlock = new BlockInfoModel();
                                    if (ptsItem is BlockInfoModel)
                                    {
                                        for (int j = 0; j < (ptsItem as BlockInfoModel).DbText.Count; j++)
                                        {

                                            DbTextModel dbTextModel = (ptsItem as BlockInfoModel).DbText[j];
                                            List<AttributeItemModel> kk = new List<AttributeItemModel>();
                                            if (dbTextModel.attItemList != null && dbTextModel.attItemList.Count > 0)
                                            {
                                                foreach (AttributeItemModel attribute in dbTextModel.attItemList)
                                                {
                                                    AttributeItemModel tempModel = attribute;
                                                    if (attribute.TargetName == "总户数")
                                                    {
                                                        tempModel = attribute;
                                                        tempModel.AtValue = totalFamily.ToString();
                                                        kk.Add(tempModel);// dbTextModel.attItemList[attribute] = totalFamily.ToString();
                                                    }
                                                    else if (attribute.TargetName == "总人数")
                                                    {
                                                        tempModel = attribute;
                                                        tempModel.AtValue = totalPeople.ToString();
                                                        kk.Add(tempModel);
                                                    }
                                                    else
                                                    {
                                                        kk.Add(attribute);
                                                    }
                                                }
                                            }
                                            dbTextModel.attItemList = kk;
                                        }
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
}
