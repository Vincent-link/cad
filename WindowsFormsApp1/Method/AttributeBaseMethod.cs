using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Method
{
    public abstract class AttributeBaseMethod<T> : ModelBaseMethod<T> where T : AttributeBaseModel
    {
       public AttributeBaseMethod() { }

         public AttributeBaseMethod(T model)
        {
            GetAllAttributeInfo(model);
        }
        public abstract void GetAllAttributeInfo(T model);

        protected List<string> GetRealLayer(string lay)
        {
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            //PromptSelectionResult ProSset = null;
            List<string> resultList = new List<string>();
            List<string> layerList = MethodCommand.GetAllLayer();
            
                string layerName = "";
                if (lay.Contains("*"))
                {
                    string temp = "";
                    temp = lay.Split('*')[1];
                    foreach (string laySpc in layerList)
                    {
                        if (laySpc.EndsWith(temp))
                        {
                            resultList.Add(laySpc);
                        }
                    }
                }
                else
                {
                    resultList.Add(lay);
                }
            
            return resultList;

        }

        private LayerModel GetRealLayer(Document doc, PromptSelectionResult ProSset,string layerName)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
        LayerModel lyModel = new LayerModel();
        List<BlockInfoModel> list = new List<BlockInfoModel>();


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
                    BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob);
                    if (plModel != null)
                    {
                        List<object> obj = new List<object>() { plModel };
                        lyModel.pointFs.Add(i, obj);
                        i++;
                    }

                }
            }}
        return lyModel;
        }
        

        
    }
}
