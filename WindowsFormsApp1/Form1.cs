using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
    
        public Form1()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptResult pr = ed.GetString("输入图层名字");

            if (pr.Status == PromptStatus.OK)
            {
                TypedValue[] tvs =
                  new TypedValue[1] {
                    new TypedValue(
                      (int)DxfCode.LayerName,
                      pr.StringResult
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

                    string[] compositionTableIndex = new string[22] {"R", "R2", "A", "A1", "A33", "A7", "A9", "B", "B1", "B41",

                                "S", "S1", "S42", "U", "U15", "G", "G1", "G2", "G3", "H11", "E", "E1"};

                    string[] requirementsIndex = {"主导功能", "规    模", "居    住", "工    业", "绿    地", "商业设施", "交通设施", "市政设施", "公共服务设施", "公共安全设施",

                                "开发用地控制", "城市设计", "地下空间"};

                    // 输出最终结果的字符串
                    string rString = "";

                    // 声明DataColumn、DataRow变量
                    System.Data.DataColumn column;
                    System.Data.DataRow row;
                    
                    // 增加一个用地构成表（用地代码）dataTable
                    System.Data.DataTable table = new System.Data.DataTable("用地代码");
                    DataGridView songsDataGridView = new DataGridView();
                    
                    // 增加表格表头名称   
                    table.Columns.Add(new System.Data.DataColumn(("用地代码"), typeof(string)));
                    table.Columns.Add(new System.Data.DataColumn(("用地名称"), typeof(string)));
                    table.Columns.Add(new System.Data.DataColumn(("面积"), typeof(string)));
                    table.Columns.Add(new System.Data.DataColumn(("占建设用地比例"), typeof(string)));

                    // 增加一个规划控制要求dataTable
                    System.Data.DataTable table2 = new System.Data.DataTable("规划要求");
                    DataGridView songsDataGridView2 = new DataGridView();

                    // 增加表格表头名称   
                    table2.Columns.Add(new System.Data.DataColumn(("标题"), typeof(string)));
                    table2.Columns.Add(new System.Data.DataColumn(("内容"), typeof(string)));

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
                                            row = table2.NewRow();
                                            row["标题"] = requirementsIndex[r];
                                            row["内容"] = ((MText)ent3).Text;
                                            table2.Rows.Add(row);
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

                            songsDataGridView2.DataSource = table2;

                        } // 循环所有实体结束

                    } // 规划要求索引循环结束

                    //SetupLayout(songsDataGridView);
                    InitializeComponent(songsDataGridView);
                    InitializeComponent2(songsDataGridView2);
                    Send sendTo = new Send();
                    sendTo.SendData(table2);

                } // 事务结束


            } // 有指定的图层

        } // form 结束

        // 求两点间距离函数
        private double GetDistance(double point01X, double point01Y, double point02X, double point02Y)
        {
            double distance;
            distance = Math.Sqrt(Math.Pow((point01X - point02X), 2) + Math.Pow((point01Y - point02Y), 2));
            return distance;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
