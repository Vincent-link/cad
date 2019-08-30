using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace RegulatoryPlan.Method
{
    public class UnitPlanMethod<T> where T : UnitPlanModel
    {
        public void GetAllUnitPlaneInfo(T model)
        {
            // 坐标点图层 特殊处理
            ModelBaseMethod<ModelBase> mbm = new ModelBaseMethod<ModelBase>();

            LayerModel lm = new LayerModel();
            lm = mbm.GetAllLayerGemo(model, UnitPlanModel.unitPlanLineLayer);

            LayerModel lm2 = new LayerModel();
            lm2 = mbm.GetAllLayerGemo(model, "控制单元范围");

            if (lm.modelItemList == null)
            {
                lm.modelItemList = new List<object>();
            }

            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            model.allLines.Add(lm);
            model.allLines.Add(lm2);

            model.attributeList = AttributeList();
            model.kgGuide = ControlList();

            //mbm.GetExportLayers(model);
            //foreach (string layer in model.LayerList)
            //{
            //    GetAttributeIndex(model, layer);
            //}
            if (model.attributeList.Columns.Count != 0)
            {
                GetAttributeIndex(model, "用地代码");
            }

        }

        public void GetAttributeIndex(T model, string layerName)
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

                        // 去重
                        List<string> numsAll = new List<string>();

                        // 增加用地大类
                        List<string> nums = new List<string>();

                        LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                        foreach (ObjectId layerId in lt)
                        {
                            LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                            if (ltr.Name == layerName)
                            {
                                lm.Color = ColorTranslator.ToHtml(ltr.Color.ColorValue);
                            }
                        }

                        foreach (ObjectId lengGemo in oids)
                        {

                            DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                            PointsPlanItemModel pointsPlanItem = new PointsPlanItemModel();
                            List<PointF> pfs = new List<PointF>();

                            //if (ob is Hatch)
                            //{
                            //    Hatch h = ob as Hatch;
                            //    int count = h.NumberOfLoops;

                            //    for (int i = 0; i < count; i++)
                            //    {
                            //        HatchLoop loop = h.GetLoopAt(i);

                            //        if (loop.IsPolyline)
                            //        {
                            //            foreach (BulgeVertex pt in loop.Polyline)
                            //            {
                            //                pfs.Add(new PointF((float)pt.Vertex.X, (float)pt.Vertex.Y));
                            //            }
                            //        }
                            //        else
                            //        {
                            //            foreach (Curve2d item in loop.Curves)
                            //            {
                            //                Point2d[] M_point2d = item.GetSamplePoints(20);
                            //                foreach (Point2d pt in M_point2d)
                            //                {
                            //                    pfs.Add(new PointF((float)pt.X, (float)pt.Y));
                            //                }
                            //            }
                            //        }
                            //    }

                            //    pointsPlanItem.Num = MethodCommand.GetAttrIndex(pfs);

                            //    if (pointsPlanItem.Num == null)
                            //    {

                            //    }
                            //}

                            if (ob is MText)
                            {
                                MText h = ob as MText;
                                pointsPlanItem.Num = h.Text;
                            }

                            if (ob is DBText)
                            {
                                DBText h = ob as DBText;
                                pointsPlanItem.Num = h.TextString;
                            }

                            if (pointsPlanItem.Num != null && !numsAll.Contains(pointsPlanItem.Num))
                            {
                                numsAll.Add(pointsPlanItem.Num);

                                BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob);
                                pointsPlanItem.Geom = plModel;

                                if (lm.modelItemList == null)
                                {
                                    lm.modelItemList = new List<object>();
                                }

                                PointsPlanItemModel pointsPlanItem2 = new PointsPlanItemModel();
                                string firstLetter = pointsPlanItem.Num.Substring(0, 1);

                                if (!nums.Contains(firstLetter))
                                {
                                    nums.Add(firstLetter);

                                    pointsPlanItem2.Num = firstLetter;
                                    pointsPlanItem2.Geom = pointsPlanItem.Geom;
                                    lm.modelItemList.Add(pointsPlanItem2);
                                }

                                lm.modelItemList.Add(pointsPlanItem);
                            }

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

            // 声明DataColumn、DataRow变量
            System.Data.DataColumn column;
            System.Data.DataRow row;

            // 增加一个用地构成表（用地代码）dataTable
            System.Data.DataTable table = new System.Data.DataTable("用地构成");

            TypedValue[] tvs = new TypedValue[1] { new TypedValue( (int)DxfCode.LayerName, "用地构成" ) };

            SelectionFilter sf = new SelectionFilter(tvs);
            PromptSelectionResult psr = ed.SelectAll(sf);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet SS = psr.Value;
                ObjectId[] idArray = SS.GetObjectIds();

                using (Transaction acTrans = db.TransactionManager.StartTransaction())
                {

                    //List<string> compositionTableIndex = new List<string>() { "R", "R2", "A", "A1", "A33", "A7", "A9", "B", "B1", "B41", "S", "S1", "S42", "U", "U15", "G", "G1", "G2", "G3", "H11", "E", "E1" };

                    List<string> compositionTableIndex = new List<string>();
                    List<Entity> mtexts = new List<Entity>();

                    for (int j = 0; j < idArray.Length; j++)
                    {
                        Entity ent2 = (Entity)idArray[j].GetObject(OpenMode.ForRead);
                        if (ent2 is MText)
                        {
                            mtexts.Add(ent2);
                        }
                    }
                    ///找出索引
                    for (int j = 0; j < mtexts.Count; j++)
                    {
                        Entity ent2 = (Entity)mtexts[j];
                        string indexText = ((MText)mtexts[j]).Text.Replace("\n", "").Replace("\r", "").Replace(" ", "");
                        // 找一个参照物——主导功能
                        if (indexText == "用地代码")
                        {
                            for (int z = 0; z < mtexts.Count; z++)
                            {
                                // 读取数组里的实体
                                Entity ent3 = (Entity)mtexts[z];

                                if (((MText)ent2).Location.X - 300 < ((MText)ent3).Location.X && ((MText)ent3).Location.X < ((MText)ent2).Location.X + 500 && ((MText)ent2).Location.Y > ((MText)ent3).Location.Y)
                                {
                                    compositionTableIndex.Add(((MText)ent3).Text);
                                }
                            }
                        }
                    }

                    // 增加表格表头名称   
                    table.Columns.Add(new System.Data.DataColumn(("用地代码"), typeof(string)));
                    table.Columns.Add(new System.Data.DataColumn(("用地名称"), typeof(string)));
                    table.Columns.Add(new System.Data.DataColumn(("面积（ha）"), typeof(string)));
                    table.Columns.Add(new System.Data.DataColumn(("占建设用地比例（%）"), typeof(string)));

                    //table.Columns.Add(new System.Data.DataColumn(("col"),typeof(string)));
                    //table.NewRow().ItemArray = new string[] { "",""};

                    // 用地代码循环
                    for (int w = 0; w < compositionTableIndex.Count; w++)
                    {
                        for (int j = 0; j < mtexts.Count; j++)
                        {
                            Entity ent1 = mtexts[j];

                            // 找出所有用地代码的关联属性
                            if (ent1 is MText && ((MText)ent1).Text == compositionTableIndex[w])
                            {

                                //ed.WriteMessage("\nFound X：{0} \n Y：{1} of {2}", ((MText)ent1).Location.X, ((MText)ent1).Location.Y, ((MText)ent1).Text);

                                // 增加一个排序列表，把实体对应的距离和文本内容放进去                                
                                List<int> distances = new List<int>();
                                List<string> texts = new List<string>();

                                for (int c = 0; c < mtexts.Count; c++)
                                {
                                    Entity ent2 = mtexts[c];

                                    // 如果为多行文本，以ent2为参考点，在y轴方向，在+400~-400范围内的，x轴方向，大于x轴的实体
                                    if (ent2 is MText)
                                    {
                                        if (((MText)ent1).Location.Y - 400 < ((MText)ent2).Location.Y && ((MText)ent2).Location.Y < ((MText)ent1).Location.Y + 400 && ((MText)ent1).Location.X <= ((MText)ent2).Location.X)
                                        {
                                            int distance = (int)MethodCommand.DistancePointToPoint(((MText)ent1).Location, ((MText)ent2).Location);

                                            distances.Add(distance);
                                            texts.Add(((MText)ent2).Text);
                                        }
                                    }
                                }

                                // 把获取的属性值按照距离大小排序，距离最近的放在第一位，以此类推
                                string temp;
                                int tempDis;
                                for (int m = 0; m < distances.Count; m++)
                                {
                                    for (int q = 0; q < distances.Count - m - 1; q++)
                                    {
                                        try
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
                                        catch (Exception e)
                                        {
                                            System.Windows.Forms.MessageBox.Show(e.Message);
                                        }
                                    }
                                }

                                row = table.NewRow();
                                row["用地代码"] = texts[0];
                                row["用地名称"] = texts[1];
                                row["面积（ha）"] = texts[2];
                                if (texts.Count < 4)
                                    row["占建设用地比例（%）"] = "无";
                                else
                                    row["占建设用地比例（%）"] = texts[3];
                                table.Rows.Add(row);

                            } // 找出所有用地代码的关联属性


                        } // 循环整个实体群组结束

                    } // 用地代码循环结束

                }

            }
            return table;

        } // form 结束

        public ArrayList ControlList()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 增加一个规划控制要求
            ArrayList controlList = new ArrayList();

            TypedValue[] tvs =
                new TypedValue[1] {
                new TypedValue(
                    (int)DxfCode.LayerName,
                    "控制要求"
                )
                };

            SelectionFilter sf = new SelectionFilter(tvs);
            PromptSelectionResult psr = ed.SelectAll(sf);

            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet SS = psr.Value;
                ObjectId[] idArray = SS.GetObjectIds();

                using (Transaction acTrans = db.TransactionManager.StartTransaction())
                {
                    //string[] requirementsIndex = { "主导功能", "规    模", "居    住", "工    业", "绿    地", "商业设施", "交通设施", "市政设施", "公共服务设施", "公共安全设施", "开发用地控制", "城市设计", "地下空间" };
                    List<string> requirementsIndex = new List<string>();
                    List<Entity> entities = new List<Entity>();

                    // 找出索引
                    for (int j = 0; j < idArray.Length; j++)
                    {
                        // 读取数组里的实体
                        Entity ent2 = (Entity)idArray[j].GetObject(OpenMode.ForRead);

                        // 找一个参照物——主导功能
                        if (ent2 is MText && (((MText)ent2).Text == "主导功能" || ((MText)ent2).Text == "城市红线"))
                        {

                            for (int z = 0; z < idArray.Length; z++)
                            {
                                // 读取数组里的实体
                                Entity ent3 = (Entity)idArray[z].GetObject(OpenMode.ForRead);

                                if (ent3 is MText && (((MText)ent2).Location.X - 1000 < ((MText)ent3).Location.X && ((MText)ent3).Location.X < ((MText)ent2).Location.Y + 1000 && ((MText)ent2).Location.Y >= ((MText)ent3).Location.Y))
                                {
                                    requirementsIndex.Add(((MText)ent3).Text);
                                    entities.Add(ent3);
                                }
                            }

                            // 如果是城市红线，重新排序
                            if (ent2 is MText && ((MText)ent2).Text == "城市红线")
                            {
                                for (int b = 0; b < entities.Count; b++)
                                {
                                    for (int c = 0; c < entities.Count - 1; c++)
                                    {
                                        // 找出多Row文本
                                        double entbMin = ((MText)entities[b]).Location.Y - ((MText)entities[b]).ActualHeight;
                                        double entcMin = ((MText)entities[c]).Location.Y - ((MText)entities[c]).ActualHeight;
                                        if (((MText)entities[b]).Location.Y > ((MText)entities[c]).Location.Y && entbMin < entcMin && ((MText)entities[b]).Location.X < ((MText)entities[c]).Location.X)
                                        {
                                            for (int d = 0; d < requirementsIndex.Count; d++)
                                            {
                                                // 把城市黄线从索引中去掉
                                                if (requirementsIndex[d] == ((MText)entities[b]).Text)
                                                {
                                                    requirementsIndex.RemoveAt(d);
                                                }
                                                // 把多行里的每一行文字加上“城市黄线”
                                                if (requirementsIndex[d] == ((MText)entities[c]).Text)
                                                {
                                                    requirementsIndex[d] = "城市黄线" + ((MText)entities[c]).Text;
                                                }
                                            }
                                        }
                                    }
                                }
                            } // 索引重新排序结束
                        } // 找一个参照物结束
                    } // 找出索引结束


                    // 输出最终结果的字符串
                    Dictionary<string, string> controlListOne = new Dictionary<string, string>();

                    //table.Columns.Add(new System.Data.DataColumn(("col"),typeof(string)));
                    //table.NewRow().ItemArray = new string[] { "",""};

                    // 规划要求表 循环开始
                    for (int r = 0; r < requirementsIndex.Count; r++)
                    {
                        // 循环所有实体
                        for (int j = 0; j < entities.Count; j++)
                        {
                            // 如果为多行文本，以ent2为参考点，在y轴方向，在+400~-400范围内的，x轴方向，大于x轴的实体
                            if (entities[j] is MText && requirementsIndex[r].Contains(((MText)entities[j]).Text))
                            {
                                for (int z = 0; z < idArray.Length; z++)
                                {
                                    // 读取数组里的实体
                                    Entity ent3 = (Entity)idArray[z].GetObject(OpenMode.ForRead);

                                    if (ent3 is MText && ((MText)entities[j]).Location.Y - 700 < ((MText)ent3).Location.Y && ((MText)ent3).Location.Y < ((MText)entities[j]).Location.Y + 700 && ((MText)entities[j]).Location.X < ((MText)ent3).Location.X)
                                    {
                                        controlListOne.Add(requirementsIndex[r], ((MText)ent3).Text);
                                    }
                                }

                            } // 查找每个索引对应的实体结束

                            // 如果为单行文本，以ent2为参考点，在y轴方向，在+400~-400范围内的，x轴方向，大于x轴的实体
                            //if (ent2 is DBText && ((DBText)ent2).Text == requirementsIndex[r])
                            //{
                            //    if (((MText)ent1).Location.Y - 400 < ((DBText)ent2).Position.Y && ((DBText)ent2).Position.Y < ((MText)ent1).Location.Y + 400 && ((MText)ent1).Location.X < ((DBText)ent2).Position.X)
                            //    {
                            //        row = table2.NewRow();
                            //        table2.Rows.Add(row);
                            //    }
                            //}
                            //dResString = dResString + "\n" + GetDistance(((MText)ent1).Location.X, ((MText)ent1).Location.Y, ((MText)ent2).Location.X, ((MText)ent2).Location.Y);

                        } // 循环所有实体结束

                    } // 规划要求索引循环结束
                    controlList.Add("规划控制要求");
                    controlList.Add(controlListOne);

                } // 事务结束
            }
            return controlList;
        } // form 结束

        /// <summary>
        /// 对实体进行写属性
        /// </summary>
        /// <param name="objId">实体id</param>
        /// <param name="appName">外部数据名</param>
        /// <param name="proStr">属性</param>
        /// <returns>true: 成功 false: 失败</returns>
        public bool AddXdata(ObjectId objId, string appName, string proStr)
        {
            bool retureValue = false;
            try
            {
                using (Database db = HostApplicationServices.WorkingDatabase)
                {
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        RegAppTable rAt = (RegAppTable)trans.GetObject(db.RegAppTableId, OpenMode.ForWrite);

                        RegAppTableRecord rAtr;
                        ObjectId rAtrId = ObjectId.Null;

                        TypedValue tvName = new TypedValue
                        (DxfCode.ExtendedDataRegAppName.GetHashCode(), appName);
                        TypedValue tvPro = new TypedValue
                        (DxfCode.ExtendedDataAsciiString.GetHashCode(), proStr);

                        ResultBuffer rb = new ResultBuffer(tvName, tvPro);
                        if (rAt.Has(appName))
                        {
                            rAtrId = rAt[appName];
                        }
                        else
                        {
                            rAtr = new RegAppTableRecord();
                            rAtr.Name = appName;
                            rAtrId = rAt.Add(rAtr);
                            trans.AddNewlyCreatedDBObject(rAtr, true);
                        }

                        Entity en = (Entity)trans.GetObject(objId, OpenMode.ForWrite);
                        en.XData = rb;
                        trans.Commit();
                        retureValue = true;
                    }
                }
            }
            catch
            {
                retureValue = false;
            }
            return retureValue;
        }
    }
}