using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RegulatoryPlan.Method
{
    public class ReadDanAttributeList<T> where T : ModelBase
    {
        List<Polyline> lengedList = new List<Polyline>();
        List<ObjectId> allModelId = new List<ObjectId>();

        public System.Data.DataTable AttributeList()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 声明DataColumn、DataRow变量
            System.Data.DataColumn column;
            System.Data.DataRow row;

            // 增加一个用地构成表（用地代码）dataTable
            System.Data.DataTable table = new System.Data.DataTable("用地构成");
            DataGridView songsDataGridView = new DataGridView();

            List<string> info = LayersToList(db);
            foreach (string lname in info)
            {

                if (lname == "用地构成")
                {
                    TypedValue[] tvs =
                      new TypedValue[1] {
                        new TypedValue(
                          (int)DxfCode.LayerName,
                          lname
                        )
                       };

                    SelectionFilter sf = new SelectionFilter(tvs);
                    PromptSelectionResult psr = ed.SelectAll(sf);
                    SelectionSet SS = psr.Value;
                    ObjectId[] idArray = SS.GetObjectIds();

                    using (Transaction acTrans = db.TransactionManager.StartTransaction())
                    {
                        // 初始化一个字符串数组
                        string[] words = new string[idArray.Length];

                        string[] compositionTableIndex = new string[22] { "R", "R2", "A", "A1", "A33", "A7", "A9", "B", "B1", "B41", "S", "S1", "S42", "U", "U15", "G", "G1", "G2", "G3", "H11", "E", "E1" };

                        // 输出最终结果的字符串
                        string rString = "";

                        // 增加表格表头名称   
                        table.Columns.Add(new System.Data.DataColumn(("用地代码"), typeof(string)));
                        table.Columns.Add(new System.Data.DataColumn(("用地名称"), typeof(string)));
                        table.Columns.Add(new System.Data.DataColumn(("面积"), typeof(string)));
                        table.Columns.Add(new System.Data.DataColumn(("占建设用地比例"), typeof(string)));

                        //table.Columns.Add(new System.Data.DataColumn(("col"),typeof(string)));
                        //table.NewRow().ItemArray = new string[] { "",""};

                        // 用地代码循环
                        for (int w = 0; w < compositionTableIndex.Length; w++)
                        {

                            // 循环所有实体
                            for (int j = 0; j < idArray.Length; j++)
                            {
                                Entity ent1 = (Entity)idArray[j].GetObject(OpenMode.ForRead);

                                int[] keysArr = new int[30];
                                string[] valuesArr = new string[30];

                                // 找出所有用地代码的关联属性
                                if (ent1 is MText && ((MText)ent1).Text == compositionTableIndex[w])
                                {

                                    //ed.WriteMessage("\nFound X：{0} \n Y：{1} of {2}", ((MText)ent1).Location.X, ((MText)ent1).Location.Y, ((MText)ent1).Text);

                                    // 增加一个排序列表，把实体对应的距离和文本内容放进去
                                    SortedList eSListRes = new SortedList();

                                    // 循环所有实体
                                    for (int c = 0; c < idArray.Length; c++)
                                    {
                                        // 读取数组里的实体
                                        Entity ent2 = (Entity)idArray[c].GetObject(OpenMode.ForRead);

                                        // 如果为多行文本，以ent2为参考点，在y轴方向，在+400~-400范围内的，x轴方向，大于x轴的实体
                                        if (ent2 is MText)
                                        {
                                            if (((MText)ent1).Location.Y - 400 < ((MText)ent2).Location.Y && ((MText)ent2).Location.Y < ((MText)ent1).Location.Y + 400 && ((MText)ent1).Location.X <= ((MText)ent2).Location.X)
                                            {
                                                int eDistance = (int)GetDistance(((MText)ent1).Location.X, ((MText)ent1).Location.Y, ((MText)ent2).Location.X, ((MText)ent2).Location.Y);

                                                eSListRes.Add(eDistance, ((MText)ent2).Text);
                                            }
                                        }

                                        // 如果为单行文本，以ent2为参考点，在y轴方向，在+400~-400范围内的，x轴方向，大于x轴的实体
                                        if (ent2 is DBText)
                                        {
                                            if (((MText)ent1).Location.Y - 400 < ((DBText)ent2).Position.Y && ((DBText)ent2).Position.Y < ((MText)ent1).Location.Y + 400 && ((MText)ent1).Location.X <= ((DBText)ent2).Position.X)
                                            {
                                                int eDistance = (int)GetDistance(((MText)ent1).Location.X, ((MText)ent1).Location.Y, ((DBText)ent2).Position.X, ((DBText)ent2).Position.Y);

                                                eSListRes.Add(eDistance, ((DBText)ent2).TextString);
                                            }
                                        }
                                        //dResString = dResString + "\n" + GetDistance(((MText)ent1).Location.X, ((MText)ent1).Location.Y, ((MText)ent2).Location.X, ((MText)ent2).Location.Y);
                                    }

                                    // 获取与用地代码相关的距离和属性
                                    int b = 0;
                                    int a = 0;

                                    // 距离
                                    ICollection key = eSListRes.Keys;
                                    foreach (int k in key)
                                    {
                                        keysArr[b] = k;
                                        b++;
                                    }
                                    // 属性值
                                    ICollection value = eSListRes.Values;
                                    foreach (string v in value)
                                    {
                                        valuesArr[a] = v;
                                        a++;
                                    }

                                    // 把获取的属性值按照距离大小排序，距离最近的放在第一位，以此类推
                                    string temp = "";
                                    for (int m = 0; m < keysArr.Length; m++)
                                    {
                                        for (int q = 0; q < keysArr.Length - m - 1; q++)
                                        {
                                            if (keysArr[q] > keysArr[q + 1])
                                            {
                                                temp = valuesArr[q];
                                                valuesArr[q] = valuesArr[q + 1];
                                                valuesArr[q + 1] = temp;
                                            }
                                        }
                                    }

                                    row = table.NewRow();
                                    row["用地代码"] = valuesArr[0];
                                    row["用地名称"] = valuesArr[1];
                                    row["面积"] = valuesArr[2];
                                    row["占建设用地比例"] = valuesArr[3];
                                    table.Rows.Add(row);

                                } // 找出所有用地代码的关联属性

                            } // 循环整个实体群组结束

                        } // 用地代码循环结束
                        songsDataGridView.DataSource = table;


                    } // 事务结束

                } // 有指定的图层
            }
            return table;

        } // form 结束

        public ArrayList ControlList()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 声明DataColumn、DataRow变量


            // 增加一个规划控制要求dataTable
            ArrayList controlList = new ArrayList();

            List<string> info = LayersToList(db);
            foreach (string lname in info)
            {
                if (lname == "控制要求")
                {
                    TypedValue[] tvs =
                      new TypedValue[1] {
                        new TypedValue(
                          (int)DxfCode.LayerName,
                          lname
                        )
                       };

                    SelectionFilter sf = new SelectionFilter(tvs);
                    PromptSelectionResult psr = ed.SelectAll(sf);
                    SelectionSet SS = psr.Value;
                    ObjectId[] idArray = SS.GetObjectIds();

                    using (Transaction acTrans = db.TransactionManager.StartTransaction())
                    {
                        // 初始化一个字符串数组
                        string[] words = new string[idArray.Length];

                        string[] requirementsIndex = { "主导功能", "规    模", "居    住", "工    业", "绿    地", "商业设施", "交通设施", "市政设施", "公共服务设施", "公共安全设施", "开发用地控制", "城市设计", "地下空间" };

                        // 输出最终结果的字符串
                        Dictionary<string ,string> controlListOne = new Dictionary<string, string>();                            

                        //table.Columns.Add(new System.Data.DataColumn(("col"),typeof(string)));
                        //table.NewRow().ItemArray = new string[] { "",""};

                        // 规划要求表 循环开始
                        for (int r = 0; r < requirementsIndex.Length; r++)
                        {
                            // 循环所有实体
                            for (int j = 0; j < idArray.Length; j++)
                            {
                                // 读取数组里的实体
                                Entity ent2 = (Entity)idArray[j].GetObject(OpenMode.ForRead);

                                // 如果为多行文本，以ent2为参考点，在y轴方向，在+400~-400范围内的，x轴方向，大于x轴的实体
                                if (ent2 is MText && ((MText)ent2).Text == requirementsIndex[r])
                                {
                                    for (int z = 0; z < idArray.Length; z++)
                                    {
                                        // 读取数组里的实体
                                        Entity ent3 = (Entity)idArray[z].GetObject(OpenMode.ForRead);

                                        if (ent3 is MText && ((MText)ent2).Location.Y - 700 < ((MText)ent3).Location.Y && ((MText)ent3).Location.Y < ((MText)ent2).Location.Y + 700 && ((MText)ent2).Location.X < ((MText)ent3).Location.X)
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

                } // 有指定的图层
            }
            return controlList;
        } // form 结束

        public Hashtable TuliList()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 识别图例 开始
            Hashtable tuliListData = new Hashtable();

            List<Polyline> allLengenPolyine = new List<Polyline>();
            List<BlockReference> blockTableRecords = new List<BlockReference>();
            List<DBText> dbtextList = new List<DBText>();
            List<MText> mtextList = new List<MText>();

            TypedValue[] tvs3 =
                new TypedValue[1] {
                    new TypedValue(
                        (int)DxfCode.LayerName,
                        "图例"
                    )
                };

            SelectionFilter sf3 = new SelectionFilter(tvs3);
            PromptSelectionResult psr3 = ed.SelectAll(sf3);
            SelectionSet SS3 = psr3.Value;

            if (psr3.Status == PromptStatus.OK)
            {
                ObjectId[] idArray = SS3.GetObjectIds();
                using (Transaction acTrans = db.TransactionManager.StartTransaction())
                {
                    for (int j = 0; j < idArray.Length; j++)
                    {
                        DBObject ob = acTrans.GetObject(idArray[j], OpenMode.ForRead);

                        if (ob is Polyline && (ob as Polyline).Closed)
                        {
                            allLengenPolyine.Add((ob as Polyline));
                        }
                        else if (ob is BlockReference)
                        {
                            blockTableRecords.Add(ob as BlockReference);
                        }
                        else if (ob is DBText)
                        {
                            dbtextList.Add(ob as DBText);
                        }
                        else if (ob is MText)
                        {
                            mtextList.Add(ob as MText);
                        }
                    }

                    List<string> title = new List<string>();
                    Hashtable tuxing = new Hashtable();

                    ArrayList backGroud = new ArrayList();
                    Dictionary<string, Hashtable> canvas = new Dictionary<string, Hashtable>();

                    // 找到面积最大的多段线
                    Dictionary<int, List<Polyline>> plList = MethodCommand.FindMaxAreaPoline(allLengenPolyine);
                    if (plList[0].Count > 0)
                    {
                        foreach (MText dBText in mtextList)
                        {
                            if (MethodCommand.FindDBTextIsInPolyine(dBText, plList[0]))
                            {
                                title.Add(dBText.Text);
                            }
                        }
                    }

                    ArrayList geom = new ArrayList();
                    ArrayList type = new ArrayList();
                    ArrayList color = new ArrayList();

                    tuxing.Add("geom", GetAllLengedGemo());
                    tuxing.Add("type", type);
                    tuxing.Add("color", color);

                    tuliListData.Add("title", title);
                    tuliListData.Add("tuxing", tuxing);
                }
            }

            MessageBox.Show(tuliListData.ToString());
            return tuliListData;

        }  // 识别图例 结束


        public ArrayList GetAllLengedGemo()
        {
            List<BlockInfoModel> list = new List<BlockInfoModel>();
            List<Polyline> allLengenPolyine = new List<Polyline>();
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, "图例") };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;

            ArrayList geom = new ArrayList();

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            //  List<ObjectId> idss=  GetEntitiesInModelSpace();
            Database db = doc.Database;
            List<BlockReference> blockTableRecords = new List<BlockReference>();
            if (ProSset.Status == PromptStatus.OK)
            {

                using (Transaction tran = db.TransactionManager.StartTransaction())
                {

                    SelectionSet sst = ProSset.Value;

                    ObjectId[] oids = sst.GetObjectIds();

                    int ad = 0;
                    List<string> aa = new List<string>();
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
                        if (ob is Polyline)
                        {
                            allLengenPolyine.Add((ob as Polyline));
                        }
                        else if (ob is BlockReference)
                        {
                            blockTableRecords.Add(ob as BlockReference);
                        }
                    }
                    Dictionary<int, List<Polyline>> plList = MethodCommand.FindMaxAreaPoline(allLengenPolyine);
                    this.lengedList = plList[0];
                    foreach (Polyline polyline in plList[0])
                    {
                        List<PointF> pfList = new List<PointF>();
                        pfList = PolylineMethod.GetPolyLineInfoPt(polyline);
                        List<ObjectId> ois = GetCrossObjectIds(doc.Editor, polyline, sfilter, tran);

                        if (ois != null)
                        {
                            foreach (ObjectId lengGemo in ois)
                            {
                                DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);

                                if (ob is Polyline)
                                {
                                    BlockInfoModel plModel = new BlockInfoModel();
                                    plModel.PolyLine = AutoCad2ModelTools.Polyline2Model(ob as Polyline);
                                    geom.Add(plModel);
                                }
                                else if (ob is BlockReference)
                                {
                                    geom.Add(BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference));
                                }
                                else if (ob is DBText)
                                {

                                    BlockInfoModel plModel = new BlockInfoModel();
                                    plModel.DbText = AutoCad2ModelTools.DbText2Model(ob as DBText);
                                    geom.Add(plModel);
                                }
                                else if (ob is MText)
                                {

                                    BlockInfoModel plModel = new BlockInfoModel();
                                    plModel.DbText = AutoCad2ModelTools.DbText2Model(ob as MText);
                                    geom.Add(plModel);
                                }
                                else if (ob is Hatch)
                                {
                                    BlockInfoModel plModel = new BlockInfoModel();
                                    plModel.Hatch = AutoCad2ModelTools.Hatch2Model(ob as Hatch);
                                    geom.Add(plModel);
                                }
                                else if (ob is Circle)
                                {
                                    BlockInfoModel plModel = new BlockInfoModel();
                                    plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle));
                                    geom.Add(plModel);
                                }
                            }
                        }

                    }


                }
            }

            return geom;
        }

        public List<ObjectId> GetCrossObjectIds(Editor ed, Polyline pl, SelectionFilter sf, Transaction tran)
        {
            Point3dCollection point3DCollection = new Point3dCollection();
            ObjectId[] list = null; List<ObjectId> ooids = new List<ObjectId>();
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                point3DCollection.Add(pl.GetPoint3dAt(i));
            }
            PromptSelectionResult psr = ed.SelectCrossingPolygon(point3DCollection, sf);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet sst = psr.Value;
                ObjectId[] oids = sst.GetObjectIds();

                foreach (ObjectId item in oids)
                {
                    if (item.OldId != pl.ObjectId.OldId)
                    {
                        ooids.Add(item);
                    }
                }

            }

            return ooids;
        }

        // 求两点间距离函数
        private double GetDistance(double point01X, double point01Y, double point02X, double point02Y)
        {
            double distance;
            distance = Math.Sqrt(Math.Pow((point01X - point02X), 2) + Math.Pow((point01Y - point02Y), 2));
            return distance;
        }

        // 获取所有图层
        List<string> LayersToList(Database db)
        {
            List<string> lstlay = new List<string>();

            LayerTableRecord layer;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in lt)
                {
                    layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    if (!layer.IsLocked)
                    {
                        lstlay.Add(layer.Name);
                    }
                }

            }
            return lstlay;
        }

    }
}
