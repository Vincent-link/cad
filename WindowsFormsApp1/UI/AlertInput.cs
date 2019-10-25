using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using CadInterface.CadService;
using RegulatoryPlan.Method;
using RegulatoryPlan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPlan.UI
{
    public partial class AlertInput : Form
    {
        ObjectId objId;
        System.Data.DataTable ta;
        FactorJsonData _contentList = new FactorJsonData();
        StageJsonData _stages = new StageJsonData();

        public AlertInput(System.Data.DataTable table, FactorJsonData contentList, StageJsonData stages)
        {
            _contentList = contentList;
            _stages = stages;

            //List<string> factors = new List<string>();

            ////MessageBox.Show(content);
            //foreach (Dictionary<string, string> name in _contentList)
            //{
            //    factors.Add(name["name"]);
            //}
            InitializeComponent(_contentList, _stages, table);
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
        private void GetFactorId(List<Factor> factors,string key,ref string nameId)
        {            
            foreach (Factor factor in factors)
            {
                if (factor.name == key)
                {
                    nameId = factor.id;                    
                    break;
                }
                else
                {
                    if (factor.child != null)
                    {
                        GetFactorId(factor.child, key, ref nameId);
                    }
                }
            }
        }
        private void GetFactorName(List<Factor> factors, string tag,ref string name)
        {
            foreach (Factor factor in factors)
            {
                if (factor.id==tag)
                {
                    name = factor.name;
                    break;
                }
               
                if (factor.child!=null)
                {
                    GetFactorName(factor.child, tag, ref name);                
                }
            }
        }

        private void GetStageName(List<Stage> stages, string tag, ref string name)
        {
            foreach (Stage stage in stages)
            {
                if (stage.id == tag)
                {
                    name = stage.name;
                    break;
                }

                if (stage.child != null)
                {
                    GetStageName(stage.child, tag, ref name);
                }
            }
        }

        private void GetFactorNode(List<Factor> factors,ref TreeNode node)
        {
            foreach (Factor factor in factors)
            {
                TreeNode n1 = node.Nodes.Add(factor.name);
                n1.Tag = factor.id;
                if (factor.child != null)
                {
                    GetFactorNode(factor.child,ref n1);
                }
            }
        }

        private void GetStageNode(List<Stage> stages, ref TreeNode node)
        {
            foreach (Stage stage in stages)
            {
                TreeNode n1 = node.Nodes.Add(stage.name);
                n1.Tag = stage.id;
                if (stage.child != null)
                {
                    GetStageNode(stage.child, ref n1);
                }
            }
        }

        private void dgv_User_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, false);
                    if(dataGridView1.CurrentCell.Value!=null)
                    {
                        string name = dataGridView1.CurrentCell.Value.ToString();
                        string tag = dataGridView1.CurrentCell.Tag.ToString();
                        this.treeComboBox1.Text = name;
                        this.treeComboBox1.Tag = tag;
                    }
                    else
                    {
                        this.treeComboBox1.Text = "";
                    }
                    //if (sexValue == "1")
                    //{
                    //    box.Text = "男";
                    //}
                    //else
                    //{
                    //    box.Text = "女";
                    //}
                    this.treeComboBox1.Left = rect.Left;
                    this.treeComboBox1.Top = rect.Top;
                    this.treeComboBox1.Width = rect.Width;
                    this.treeComboBox1.Height = rect.Height;
                    this.treeComboBox1.Visible = true;
                }
                else
                {
                    this.treeComboBox1.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void stage_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentCell.ColumnIndex == 5)
                {
                    Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, false);
                    if (dataGridView1.CurrentCell.Value != null)
                    {
                        string name = dataGridView1.CurrentCell.Value.ToString();
                        string tag = dataGridView1.CurrentCell.Tag.ToString();
                        this.treeComboBox2.Text = name;
                        this.treeComboBox2.Tag = tag;
                    }
                    else
                    {
                        this.treeComboBox2.Text = "";
                    }
                    //if (sexValue == "1")
                    //{
                    //    box.Text = "男";
                    //}
                    //else
                    //{
                    //    box.Text = "女";
                    //}
                    this.treeComboBox2.Left = rect.Left;
                    this.treeComboBox2.Top = rect.Top;
                    this.treeComboBox2.Width = rect.Width;
                    this.treeComboBox2.Height = rect.Height;
                    this.treeComboBox2.Visible = true;
                }
                else
                {
                    this.treeComboBox2.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void cmb_Temp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (((ComboBox)sender).Text == "男")
            //{
            dataGridView1.CurrentCell.Value = ((DataGridViewTreeComboxColumn.TreeComboBox)sender).Text;
            dataGridView1.CurrentCell.Tag = ((DataGridViewTreeComboxColumn.TreeComboBox)sender).SelectedNode.Tag;
            //}
            //else
            //{
            //    dataGridView1.CurrentCell.Value = "女";
            //    dataGridView1.CurrentCell.Tag = "0";
            //}
        }
        private void HideAllControl(Control control)
        {
            if (control.Controls != null)
            {
                foreach (Control item in control.Controls)
                {
                    item.Visible = false;
                    HideAllControl(item);
                }
            }
        }

        private void dgv_User_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.treeComboBox1.Visible)
            {
                this.treeComboBox1.Visible = false;
            }
        }

        private void stage_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.treeComboBox2.Visible)
            {
                this.treeComboBox2.Visible = false;
            }
        }

        private void dgv_User_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.treeComboBox1.Visible)
            {
                this.treeComboBox1.Visible = false;
            }
        }

        private void stage_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.treeComboBox2.Visible)
            {
                this.treeComboBox2.Visible = false;
            }
        }

        private void dgv_User_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells[2].Value != null && dataGridView1.Rows[i].Cells[2].ColumnIndex == 2)
            //    {
            //        dataGridView1.Rows[i].Cells[2].Tag = dataGridView1.Rows[i].Cells[2].Value.ToString();
            //        if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "1")
            //        {
            //            dataGridView1.Rows[i].Cells[2].Value = "男";
            //        }
            //        else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "0")
            //        {
            //            dataGridView1.Rows[i].Cells[2].Value = "女";
            //        }
            //    }
            //}
        }


        private void DataGridView1_SelectionChanged(object sender, System.EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        void treeComboBox1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 1)
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 0;

        }

        void treeComboBox1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 0)
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
        }

        void treeComboBox2_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 1)
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 0;

        }
        void treeComboBox2_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 0)
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
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
                if (dataGridView1.Rows[e.RowIndex].Cells["individualStage"].Value is null)
                {
                    MessageBox.Show("请选择阶段");
                    return;
                }
                //string nameId = "";

                //foreach (Dictionary<string, string> name in _contentList)
                //{
                //    if (name["name"] == dataGridView1.Rows[e.RowIndex].Cells["factor"].Value.ToString())
                //    {
                //        nameId = name["id"];
                //        break;
                //    }
                //}
                //int dlCount = 0;
                string result = dataGridView1.Rows[e.RowIndex].Cells["factor"].Tag.ToString();
                string stage = dataGridView1.Rows[e.RowIndex].Cells["individualStage"].Tag.ToString();

                //foreach (var item in result)
                //{
                //    if (item=='-')
                //    {
                //        dlCount++;
                //    }
                //}

                //GetFactorId(_contentList.result, result, ref nameId);

                AutoGenerateNumMethod.GetPolyline(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), result, dataGridView1.Rows[e.RowIndex].Cells["individualName"].Value.ToString(), stage, e.RowIndex,ref dataGridView1);
            }

            //点击button按钮事件
            if (dataGridView1.Rows.Count > 0 && dataGridView1.Columns[e.ColumnIndex].Name == "btnFind" && e.RowIndex >= 0)
            {

                if (dataGridView1.Rows[e.RowIndex].Cells[0].Value is "" || dataGridView1.Rows[e.RowIndex].Cells[0].Value == null || dataGridView1.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    MessageBox.Show("请先拾取多段线");
                    return;
                }
                string selects = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (selects.EndsWith(","))
                {
                    selects = selects.Remove(selects.Length-1, 1);
                }
                List<String> list = new List<string>(selects.Split(','));
                List<String> list1 = new List<string>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if(row.Index!=e.RowIndex)
                        {
                            string select = row.Cells[0].Value.ToString();
                            if (select.EndsWith(","))
                            {
                                select = select.Remove(select.Length - 1, 1);
                            }
                            list1.AddRange(select.Split(','));
                        }
                    }
                }

                LocationService.FindPolyline(list, list1);
                //LocationService.FindPolylines(list);
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
                            string selects = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                            if (selects.EndsWith(","))
                            {
                                selects = selects.Remove(selects.Length - 1, 1);
                            }
                            List<String> list = new List<string>(selects.Split(','));

                            AutoGenerateNumMethod.DeletePolyline(list);
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
