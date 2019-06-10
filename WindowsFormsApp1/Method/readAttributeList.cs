using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryPlan.Method;
using RegulatoryPlan.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RegulatoryPlan.Method
{
    public class readAttributeList
    {
        public System.Data.DataTable AttributeList()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 识别个体指标图表 开始

            // 增加一个个体指标dataTable，个体指标必须在同一图层上，且固定一个图层名，比如“个体指标”
            System.Data.DataTable table = new System.Data.DataTable("个体指标");
            DataGridView songsDataGridView = new DataGridView();

            TypedValue[] tvs =
                new TypedValue[1] {
                new TypedValue(
                    (int)DxfCode.LayerName,
                    "字"
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

                    // 以地块编号为参照物，向右、向下读取
                    table.Columns.Add(new System.Data.DataColumn(("地块编号"), typeof(string)));

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
                                    if (((MText)ent2).Location.X < ((MText)ent1).Location.X && ((MText)ent2).Location.Y - 14 < ((MText)ent1).Location.Y && ((MText)ent1).Location.Y < ((MText)ent2).Location.Y + 14)
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

                    int[] keysArr = new int[biaotou.Count];
                    string[] valuesArr = new string[biaotou.Count];

                    // 给表头排序 从小到大
                    for (int i = 0; i < idArray.Length; i++)
                    {
                        Entity ent3 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
                        if (ent3 is MText && ((MText)ent3).Text == "地块编号")
                        {
                            for (int s = 0; s < biaotou.Count; s++)
                            {
                                Entity ent4 = (Entity)biaotou[s];

                                int eDistance = (int)GetDistance(((MText)ent4).Location.X, ((MText)ent4).Location.Y, ((MText)ent3).Location.X, ((MText)ent3).Location.Y);
                                //MessageBox.Show(eDistance.ToString());

                                eSListRes.Add(eDistance, ((MText)ent4).Text);

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

                        }
                    }

                    // 添加表头
                    for (int s = 0; s < valuesArr.Length; s++)
                    {
                        table.Columns.Add(new System.Data.DataColumn((valuesArr[s]), typeof(string)));
                    }

                    // 循环所有实体 给每一行的实体排序 
                    for (int s = 0; s < index.Count; s++)
                    {
                        eSListRes.Clear();
                        Entity ent3 = (Entity)index[s];
                        // 循环所有实体，如果实体的x值大于table索引的x值，加进table的每一行
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            Entity ent4 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
                            if (ent4 is MText && ((MText)ent3).Location.X < ((MText)ent4).Location.X && ((MText)ent3).Location.Y - 3.5 < ((MText)ent4).Location.Y && ((MText)ent4).Location.Y < ((MText)ent3).Location.Y + 3.5)
                            {
                                int eDistance = (int)GetDistance(((MText)ent4).Location.X, ((MText)ent4).Location.Y, ((MText)ent3).Location.X, ((MText)ent3).Location.Y);

                                //MessageBox.Show(eDistance.ToString());
                                eSListRes.Add(eDistance, ((MText)ent4).Text);
                            }
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
                        row["地块编号"] = ((MText)index[s]).Text;
                        for (int f = 0; f < valuesArr.Length; f++)
                        {
                            //MessageBox.Show(table.Columns[f].ToString());
                            row[table.Columns[f + 1]] = valuesArr[f];
                        }

                        table.Rows.Add(row);
                    }
                    // 识别个体指标图表结束

                    //SetupLayout(songsDataGridView);

                } // 事务结束
            }

            songsDataGridView.DataSource = table;
            // 用地构成表
            //InitializeComponent(songsDataGridView);

            //MessageBox.Show(table.ToString());

            return table;

        } // form 结束

        public ArrayList KgGuide()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 识别控规引导 开始
            // 增加一个控规引导，必须在同一图层上，且固定一个图层名，比如“控规引导”
            ArrayList kgGuide = new ArrayList();

            TypedValue[] tvs2 =
                new TypedValue[1] {
                    new TypedValue(
                        (int)DxfCode.LayerName,
                        "控规引导"
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

        public ArrayList TuliList()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 识别图例 开始
            ArrayList tuliListData = new ArrayList();

            List<Polyline> allLengenPolyine = new List<Polyline>();
            List<BlockReference> blockTableRecords = new List<BlockReference>();
            List<DBText> dbtextList = new List<DBText>();
            List<MText> mtextList = new List<MText>();

            TypedValue[] tvs3 =
                new TypedValue[1] {
                    new TypedValue(
                        (int)DxfCode.LayerName,
                        "0-图框"
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

                    // 一个文本对应一个图例框，找出所有文本以及对应的图例框，并把对应的图例框内的图形识别出来

                    // 文本左边最近的闭合多段线

                    for (int i = 0; i < mtextList.Count; i++)
                    {
                        //FindLastPolyine(mtextList[i], allLengenPolyine);


                    }

                    string[] name = new string[]{ "幼儿园", "公厕"};
                    string[] nameGeom = new string[] { "[114.66905719484419, 36.458559633057178]", "[114.66905719484419, 36.458559633057178]" };
                    string[] nameType = new string[] { "MText", "MText" };


                    // 单个图例名称
                    tuliListData.Add(name);
                    tuliListData.Add(nameGeom);
                    tuliListData.Add(nameType);


                    // 单个图例所对应的图例框
                    //tuliListData.Add("tukuang", polyline);
                    //tuliListData.Add("tukuangGeom", PolylineGeom);
                    //tuliListData.Add("tukuangType", tukuangType);

                    //// 单个图例所对应的图例框内的图形
                    //tuliListData.Add("tuxing", tuxing);
                    //tuliListData.Add("tuxingGeom", tuxingGeom);
                    //tuliListData.Add("tuxingType", tuxingType);


                }
            }
            // 识别图例 结束

            MessageBox.Show(tuliListData.ToString());
            return tuliListData;

        } // form 结束

        //public static Polyline FindLastPolyine(MText txt, List<Polyline> Polylines)
        //{
        //    foreach (Polyline polyline in Polylines)
        //    {
        //        int vn = polyline.NumberOfVertices;
        //        for (int w = 0; w < vn; w++)
        //        {
        //            Point2d pt = polyline.GetPoint2dAt(w);

        //            col_point3d.Add(new Point3d(pt.X, pt.Y, 0));
        //        }
        //    }
        //}

        public static Dictionary<int, List<Polyline>> FindMaxAreaPoline(List<Polyline> polylines)
        {
            Dictionary<int, List<Polyline>> allPolines = new Dictionary<int, List<Polyline>>();
            allPolines.Add(0, new List<Polyline>());
            allPolines.Add(1, new List<Polyline>());
            double maxArea = double.NaN;
            foreach (Polyline line in polylines)
            {
                if (double.IsNaN(maxArea) || line.Area > maxArea)
                {
                    maxArea = line.Area;
                }
            }

            foreach (Polyline line in polylines)
            {
                if (line.Area.ToString("F5") == maxArea.ToString("F5"))
                {
                    allPolines[0].Add(line);
                }
                else
                {
                    allPolines[1].Add(line);
                }
            }
            return allPolines;
        }

        // 求两点间距离函数
        private double GetDistance(double point01X, double point01Y, double point02X, double point02Y)
        {
            double distance;
            distance = Math.Sqrt(Math.Pow((point01X - point02X), 2) + Math.Pow((point01Y - point02Y), 2));
            return distance;
        }
    }
}
