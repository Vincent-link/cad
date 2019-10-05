using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryPlan.Method;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPlan.UI
{
    public partial class AlertInput : Form
    {
        ObjectId objId;
        System.Data.DataTable ta;
        List<Dictionary<string, string>> contentList = new List<Dictionary<string, string>>();

        public AlertInput(System.Data.DataTable table)
        {
            string cityId = Method.SaveProjectIdToXData.GetDefinedProject();

            List<string> factors = new List<string>();
            try
            {
                string projectIdBaseAddress = "http://172.18.84.70:8080/PDD/pdd/cim-interface!findElementByProjectId?projectId="+ cityId;
                var projectIdHttp = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri(projectIdBaseAddress));

                var response = projectIdHttp.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new System.IO.StreamReader(stream, Encoding.UTF8);
                var content = sr.ReadToEnd();
                contentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(content);
            foreach (Dictionary<string, string> name in contentList)
            {
                factors.Add(name["name"]);
            }
            InitializeComponent(factors, table);
            Load(table);
        }

        // 清除编码
        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        // 项目定义确定
        private void ok_Click(object sender, EventArgs e)
        {


            this.Close();
            return;
        }

        private void Load(System.Data.DataTable table)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击button按钮事件
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnGet" && e.RowIndex >= 0)
            {
                //说明点击的列是DataGridViewButtonColumn列
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

                if(dataGridView1.Rows[e.RowIndex].Cells[1].Value is null)
                {
                    MessageBox.Show("请输入编码");
                    return;
                }

                if (dataGridView1.Rows[e.RowIndex].Cells["factor"].Value is null)
                {
                    MessageBox.Show("请选择个体要素");
                    return;
                }

                if (dataGridView1.Rows[e.RowIndex].Cells["individualName"].Value is null)
                {
                    MessageBox.Show("请选择个体名称");
                    return;
                }
                string nameId = "";
                foreach (Dictionary<string, string> name in contentList)
                {
                    if (name["name"] == dataGridView1.Rows[e.RowIndex].Cells["factor"].Value.ToString())
                    {
                        nameId = name["id"];
                    }
                }

                string value = AutoGenerateNumMethod.GetPolyline(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), nameId, dataGridView1.Rows[e.RowIndex].Cells["individualName"].Value.ToString());

                try
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[0].Value = value;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            //点击button按钮事件
            if (dataGridView1.Rows.Count > 0 && dataGridView1.Columns[e.ColumnIndex].Name == "btnFind" && e.RowIndex >= 0)
            {

                if (dataGridView1.Rows[e.RowIndex].Cells[0].Value is "" || dataGridView1.Rows[e.RowIndex].Cells[0].Value == null || dataGridView1.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    MessageBox.Show("请先拾取多端线");
                    return;
                }

                AutoGenerateNumMethod.FindPolyline(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                //dataGridView1.CurrentRow.Cells[2].Value = polylineNumber;
            }

            //点击button按钮事件
            if (dataGridView1.Rows.Count > 0 && dataGridView1.Columns[e.ColumnIndex].Name == "btnDelete" && e.RowIndex >= 0)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    try
                    {
                        if (this.dataGridView1.CurrentRow.Cells[0].Value != null)
                        {
                            AutoGenerateNumMethod.DeletePolyline(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            dataGridView1.Rows.RemoveAt(e.RowIndex);
                        }
                        // 如果多段线和编码为空
                        else if (this.dataGridView1.CurrentRow.Cells[1].Value != null)
                        {
                            dataGridView1.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            MessageBox.Show("无法删除新行！");
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }

                //dataGridView1.CurrentRow.Cells[2].Value = polylineNumber;
            }

        }

        // 增加拾取编码
        //private void add_PickNumber(object sender, EventArgs e)
        //{
        //    Panel gbox = new Panel();

        //    if (panel1.Controls.Count > 0)
        //    {
        //        gbox.Location = new Point(this.panel1.Controls[panel1.Controls.Count - 1].Location.X, this.panel1.Controls[panel1.Controls.Count - 1].Location.Y + 10);
        //    }
        //    else
        //    {
        //        gbox.Location = new Point(this.panel1.Location.X, this.panel1.Location.Y + 10);
        //    }

        //    Label txt = new Label();
        //    txt.Text = "输入编码";
        //    txt.Location = new Point(gbox.Location.X, gbox.Location.Y);
        //    txt.AutoSize = true;
        //    gbox.Controls.Add(txt);

        //    TextBox textBox = new TextBox();
        //    textBox.Location = new Point(txt.Right, 0);
        //    gbox.Controls.Add(textBox);

        //    Button txt2 = new Button();
        //    txt2.Text = "拾取";
        //    txt2.Location = new Point(textBox.Right, 0);
        //    gbox.Controls.Add(txt2);

        //    Button txt3 = new Button();
        //    txt3.Text = "保存";
        //    txt3.Location = new Point(textBox.Right, 0);
        //    gbox.Controls.Add(txt3);

        //    Button txt4 = new Button();
        //    txt4.Text = "清除";
        //    txt4.Location = new Point(textBox.Right, 0);
        //    gbox.Controls.Add(txt4);

        //    this.panel1.Controls.Add(gbox);

        //    //// 
        //    //// button2
        //    //// 
        //    //this.button2.Location = new System.Drawing.Point(202, 42);
        //    //this.button2.Name = "button2";
        //    //this.button2.Size = new System.Drawing.Size(75, 23);
        //    //this.button2.TabIndex = 3;
        //    //this.button2.Text = "清除编码";
        //    //this.button2.UseVisualStyleBackColor = true;
        //    //this.button2.Click += new System.EventHandler(this.cancel_Click);
        //}

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);


        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern bool ReleaseCapture();
        private const long WM_GETMINMAXINFO = 0x24;

        private void MainForm_Load(object sender, MouseEventArgs e)
        {
            const int WM_NCLBUTTONDOWN = 0x00A1;
            const int HTCAPTION = 2;
            if (e.Button == MouseButtons.Left) // 按下的是鼠标左键 
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero); // 拖动窗体 
            }
        }
    }
}
