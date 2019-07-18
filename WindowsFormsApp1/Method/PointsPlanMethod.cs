using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace RegulatoryPlan.Method
{
    public class PointsPlanMethod<T> where T : PointsPlanModel
    {
        public void GetAllPointsPlaneInfo(T model)
        {
            LayerModel lm = new LayerModel();
            //string attributeLists = JsonConvert.SerializeObject(attributeList);
            //MessageBox.Show(attributeLists);

            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            model.allLines.Add(lm);

            if (lm.modelItemList == null)
            {
                lm.modelItemList = new List<object>();
            }

            model.attributeList = AttributeList();
            model.kgGuide = KgGuide();

            //地块图层
            GetAllYDBMGemo(model, "地块界限");
            GetAllDimensioning(model, "尺寸标注");
        }

        public void GetAllYDBMGemo(T model, string layerName)
        {
            if (model != null)
            {
                LayerModel lm = new LayerModel();
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                ObjectIdCollection ids = new ObjectIdCollection();
                lm.Name = layerName;

                PromptSelectionResult ProSset = null;
                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName) };
                SelectionFilter sfilter = new SelectionFilter(filList);

                ProSset = doc.Editor.SelectAll(sfilter);
                if (ProSset.Status == PromptStatus.OK)
                {
                    using (Transaction tran = db.TransactionManager.StartTransaction())
                    {
                        SelectionSet sst = ProSset.Value;
                        ObjectId[] oids = sst.GetObjectIds();

                        LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                        foreach (ObjectId layerId in lt)
                        {
                            LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                            if (ltr.Name == layerName)
                            {
                                lm.Color = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                            }
                        }

                        foreach (ObjectId lengGemo in oids)
                        {
                            
                            DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                            PointsPlanItemModel pointsPlanItem = new PointsPlanItemModel();
                            if (ob is Polyline)
                            {

                                Polyline aPl = ob as Polyline;
                                // 读取分图则闭合多段线内的用地代码
                                List<PointF> pfs = new List<PointF>();
                                if (aPl.Closed is true)
                                {
                                    int vn2 = aPl.NumberOfVertices;  //lwp已知的多段线
                                    for (int J = 0; J < vn2; J++)
                                    {
                                        Point2d pt = aPl.GetPoint2dAt(J);
                                        PointF pf = new PointF((float)pt.X, (float)pt.Y);
                                        pfs.Add(pf);
                                    }
                                }
                                pointsPlanItem.Num = MethodCommand.GetAttrIndex(pfs);
                            }
                         
                            BlockInfoModel plModel =MethodCommand.AnalysisBlcokInfo(ob);
                            pointsPlanItem.Geom = plModel;
                        
                            if (lm.modelItemList == null)
                            {
                                lm.modelItemList = new List<object>();
                            }
                            
                            lm.modelItemList.Add(pointsPlanItem);

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

        public void GetAllDimensioning(T model, string layerName)
        {
            if (model != null)
            {
                LayerModel lm = new LayerModel();
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                ObjectIdCollection ids = new ObjectIdCollection();
                lm.Name = layerName;

                PromptSelectionResult ProSset = null;
                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName) };
                SelectionFilter sfilter = new SelectionFilter(filList);

                ProSset = doc.Editor.SelectAll(sfilter);
                if (ProSset.Status == PromptStatus.OK)
                {
                    // lyModel.pointFs = new Dictionary<int, List<object>>();
                    using (Transaction tran = db.TransactionManager.StartTransaction())
                    {
                        SelectionSet sst = ProSset.Value;
                        ObjectId[] oids = sst.GetObjectIds();

                        LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                        foreach (ObjectId layerId in lt)
                        {
                            LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                            if (ltr.Name == layerName)
                            {
                                lm.Color = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                            }
                        }

                        foreach (ObjectId lengGemo in oids)
                        {

                            DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                            PointsPlanItemModel pointsPlanItem = new PointsPlanItemModel();

                            Entity ety = ob as Entity;
                            DBObjectCollection objs = new DBObjectCollection();
                            ety.Explode(objs);

                            foreach (DBObject obj in objs)
                            {
                                if (obj is DBText)
                                {
                                    pointsPlanItem.RoadWidth = (obj as DBText).TextString;
                                }
                                if (obj is MText)
                                {
                                    pointsPlanItem.RoadWidth = (obj as MText).Text;
                                }
                            }

                            //BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob);
                            //pointsPlanItem.Geom = plModel;

                            if (lm.modelItemList == null)
                            {
                                lm.modelItemList = new List<object>();
                            }

                            lm.modelItemList.Add(pointsPlanItem);

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

        public System.Data.DataTable AttributeList()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 识别个体指标图表 开始

            // 增加一个个体指标dataTable，个体指标必须在同一图层上，且固定一个图层名，比如“个体指标”
            System.Data.DataTable table = new System.Data.DataTable("控制指标");

            TypedValue[] tvs =
                new TypedValue[1] {
                new TypedValue(
                    (int)DxfCode.LayerName,
                    "控制指标"
                )
                };

            SelectionFilter sf = new SelectionFilter(tvs);
            PromptSelectionResult psr = ed.SelectAll(sf);
            SelectionSet SS = psr.Value;

            if (psr.Status == PromptStatus.OK)
            {
                ObjectId[] idArray = SS.GetObjectIds();
                using (Transaction acTrans = db.TransactionManager.StartTransaction())
                {
                    // 声明DataColumn、DataRow变量
                    System.Data.DataColumn column;
                    System.Data.DataRow row;

                    // 个体指标地块编号索引
                    ArrayList index = new ArrayList();
                    // 个体指标图标表头
                    ArrayList biaotou = new ArrayList();

                    // 增加一个排序列表，把实体对应的距离和文本内容放进去
                    SortedList eSListRes = new SortedList();

                    // 循环所有实体 获取表头、地块编号索引
                    for (int j = 0; j < idArray.Length; j++)
                    {
                        Entity ent1 = (Entity)idArray[j].GetObject(OpenMode.ForRead);
                        if (ent1 is MText)
                        {
                            // 循环所有实体
                            for (int c = 0; c < idArray.Length; c++)
                            {
                                // 读取数组里的实体
                                Entity ent2 = (Entity)idArray[c].GetObject(OpenMode.ForRead);
                                if (ent2 is MText && ((MText)ent2).Text == "地块编号")
                                {
                                    // 添加表头
                                    if (((MText)ent2).Location.X <= ((MText)ent1).Location.X && ((MText)ent2).Location.Y - 14 < ((MText)ent1).Location.Y && ((MText)ent1).Location.Y < ((MText)ent2).Location.Y + 14)
                                    {
                                        //MessageBox.Show((((MText)ent1).Text).ToString());
                                        biaotou.Add(ent1);
                                    }

                                    // 添加索引
                                    if (((MText)ent2).Location.Y > ((MText)ent1).Location.Y && ((MText)ent2).Location.X - 14 < ((MText)ent1).Location.X && ((MText)ent1).Location.X < ((MText)ent2).Location.X + 14)
                                    {
                                        //MessageBox.Show((((MText)ent1).Text).ToString());
                                        index.Add(ent1);
                                    }
                                }
                            }
                        }

                    } // 循环整个实体群组结束

                    // 给表头排序 从小到大
                    List<int> distances = new List<int>();
                    List<string> texts = new List<string>();
                    string temp;
                    int tempDis;
                    for (int i = 0; i < idArray.Length; i++)
                    {
                        Entity ent3 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
                        if (ent3 is MText && ((MText)ent3).Text == "地块编号")
                        {
                            for (int s = 0; s < biaotou.Count; s++)
                            {
                                Entity ent4 = (Entity)biaotou[s];
                                int eDistance = (int)MethodCommand.DistancePointToPoint(((MText)ent4).Location, ((MText)ent3).Location);
                                //MessageBox.Show(eDistance.ToString());

                                distances.Add(eDistance);
                                texts.Add(((MText)ent4).Text);
                            }

                            // 把获取的属性值按照距离大小排序，距离最近的放在第一位，以此类推
                            for (int m = 0; m < distances.Count; m++)
                            {
                                for (int q = 0; q < distances.Count - m - 1; q++)
                                {
                                    if (distances[q] > distances[q + 1])
                                    {
                                        temp = texts[q];
                                        texts[q] = texts[q + 1];
                                        texts[q + 1] = temp;

                                        tempDis = distances[q];
                                        distances[q] = distances[q + 1];
                                        distances[q + 1] = tempDis;
                                    }
                                }
                            }

                        }
                    }

                    // 添加表头
                    foreach (string text in texts)
                    {
                        table.Columns.Add(new System.Data.DataColumn(text, typeof(string)));
                    }

                    // 循环所有实体 给每一行排序 
                    for (int s = 0; s < index.Count; s++)
                    {
                        distances.Clear();
                        texts.Clear();
                        Entity ent3 = (Entity)index[s];
                        // 循环所有实体，如果实体的x值大于table索引的x值，加进table的每一行
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            Entity ent4 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
                            if (ent4 is MText && ((MText)ent3).Location.X <= ((MText)ent4).Location.X && ((MText)ent3).Location.Y - 3.5 < ((MText)ent4).Location.Y && ((MText)ent4).Location.Y < ((MText)ent3).Location.Y + 3.5)
                            {
                                int eDistance = (int)MethodCommand.DistancePointToPoint(((MText)ent4).Location, ((MText)ent3).Location);

                                //MessageBox.Show(eDistance.ToString());
                                distances.Add(eDistance);
                                texts.Add(((MText)ent4).Text);
                            }
                        }

                        // 把获取的属性值按照距离大小排序，距离最近的放在第一位，以此类推
                        for (int m = 0; m < distances.Count; m++)
                        {
                            for (int q = 0; q < distances.Count - m - 1; q++)
                            {
                                if (distances[q] > distances[q + 1])
                                {
                                    temp = texts[q];
                                    texts[q] = texts[q + 1];
                                    texts[q + 1] = temp;

                                    tempDis = distances[q];
                                    distances[q] = distances[q + 1];
                                    distances[q + 1] = tempDis;
                                }
                            }
                        }

                        row = table.NewRow();
                        for (int f = 0; f < texts.Count; f++)
                        {
                            //MessageBox.Show(table.Columns[f].ToString());
                            row[table.Columns[f]] = texts[f];
                        }

                        table.Rows.Add(row);
                    }
                    // 识别个体指标图表结束

                    //SetupLayout(songsDataGridView);

                } // 事务结束
            }

            // 用地构成表
            //InitializeComponent(songsDataGridView);

            //MessageBox.Show(table.ToString());

            return table;

        } // form 结束

        public ArrayList KgGuide()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 识别控规引导 开始
            // 增加一个控规引导，必须在同一图层上，且固定一个图层名，比如“控规引导”
            ArrayList kgGuide = new ArrayList();

            TypedValue[] tvs2 =
                new TypedValue[1] {
                    new TypedValue(
                        (int)DxfCode.LayerName,
                        "控制引导"
                    )
                };

            SelectionFilter sf2 = new SelectionFilter(tvs2);
            PromptSelectionResult psr2 = ed.SelectAll(sf2);
            SelectionSet SS2 = psr2.Value;

            if (psr2.Status == PromptStatus.OK)
            {
                ObjectId[] idArray = SS2.GetObjectIds();
                using (Transaction acTrans = db.TransactionManager.StartTransaction())
                {
                    Entity ent1 = (Entity)idArray[0].GetObject(OpenMode.ForRead);
                    Entity ent2 = (Entity)idArray[0].GetObject(OpenMode.ForRead);
                    for (int j = 0; j < idArray.Length; j++)
                    {
                        ent1 = (Entity)idArray[0].GetObject(OpenMode.ForRead);
                        ent2 = (Entity)idArray[1].GetObject(OpenMode.ForRead);
                        Entity ent3;

                        // 如果ent2在ent1左边
                        if (ent1 is MText && ent2 is MText && ((MText)ent2).Location.X < ((MText)ent1).Location.X)
                        {
                            ent3 = ent2;
                            ent2 = ent1;
                            ent1 = ent3;
                        }
                    }

                    kgGuide.Add(((MText)ent1).Text);
                    kgGuide.Add(((MText)ent2).Text);
                }
            }
            // 识别控规引导 结束

            //MessageBox.Show(kgGuide.ToString());
            return kgGuide;
            // 分图则发送到GIS
            //FenTuZe fen = new FenTuZe();

            //fen.SendData(table, kgGuide);
        } // form 结束
    }
}