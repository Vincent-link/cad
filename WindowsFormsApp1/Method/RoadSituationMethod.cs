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
    public class RoadSituationMethod<T>:ModelBaseMethod<T> where T:RoadModel
    {
        public void GetAllRoadInfo(T model)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, model.RoadNameLayer) };
            SelectionFilter sfilter = new SelectionFilter(filList);

            ProSset = doc.Editor.SelectAll(sfilter);
            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            List<Polyline> roadLine = new List<Polyline>();
            List<MText> roadText = new List<MText>();
            List<Polyline> sectionLine = new List<Polyline>();
            List<DBText> sectionText = new List<DBText>();
            Database db = doc.Database;

            model.attributeList = new System.Data.DataTable("道路");
            model.attributeList.Columns.Add(new System.Data.DataColumn(("道路名称"), typeof(string)));

            if (ProSset.Status == PromptStatus.OK)
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
                            lm.Color = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                        }
                    }
                    for (int i = 0; i < oids.Length; i++)
                    {

                        DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);

                        if ((ob as MText).BlockName.ToLower() == "*model_space")
                        {
                            roadText.Add((ob as MText));
                        }
                    }

                    lm.Name = model.RoadLineLayer;
                    lm.modelItemList = new List<object>();

                    foreach (MText text in roadText)
                    {
                        // 获取道路的几何数据
                        lm.modelItemList.Add(GetRoadItemInfo(text));

                        // 获取道路的属性数据
                        System.Data.DataRow row;
                        row = model.attributeList.NewRow();
                        row["道路名称"] = text.Text.Replace(" ", "").Replace("　", "").Replace("\r", "").Replace("\n", ""); ;
                        model.attributeList.Rows.Add(row);
                    }

                    if (model.allLines == null)
                    {
                        model.allLines = new List<LayerModel>();
                    }
                    model.allLines.Add(lm);
                }
            }

        }


        public RoadInfoItemModel GetRoadItemInfo(MText text)
        {
            RoadInfoItemModel item = new RoadInfoItemModel();
            item.RoadLength = "";
            item.RoadWidth = "";
            item.RoadType = "";
            item.ColorIndex = "";

            using (Transaction tran = CadHelper.Instance.Database.TransactionManager.StartTransaction())
            {
                item.RoadName = text.Text.Replace(" ", "").Replace("　", "").Replace("\r", "").Replace("\n", "");
                item.RoadNameLocaiton = new List<System.Drawing.PointF>();
                double textLen = text.TextHeight * item.RoadName.Length + text.LineSpaceDistance * (item.RoadName.Length - 1);
                double partLength = textLen / item.RoadName.Length;

                for (int j = 1; j < item.RoadName.Length + 1; j++)
                {
                    item.RoadNameLocaiton.Add(MethodCommand.GetEndPointByTrigonometricHu(text.Rotation,MethodCommand.Point3d2Pointf(text.Location), partLength * j));
                }

                item.RoadNameLayer = text.Layer;
                item.RoadNameType = "text";


            }
            return item;
        }
    }
}
