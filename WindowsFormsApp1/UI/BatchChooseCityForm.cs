using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPlan.UI
{

    public partial class BatchChooseCityForm : Form
    {
        public List<string> openFile = new List<string>();
        public string openCity = "";
        public DerivedTypeEnum derivedType = DerivedTypeEnum.None;
        List<Dictionary<string, string>> contentList = new List<Dictionary<string, string>>();

        public BatchChooseCityForm()
        {
            List<string> names = new List<string>();
            try
            {
                string projectIdBaseAddress = "http://172.18.84.114:8080/PDD/pdd/individual-manage!findAllProject.action";
                var projectIdHttp = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri(projectIdBaseAddress));

                var response = projectIdHttp.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream, Encoding.UTF8);
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
                names.Add(name["name"]);
            }

            InitializeComponent(names);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//等于true表示可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "CAD文件|*.dwg";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                for (int j = 0; j < dialog.FileNames.Length; j++)
                {
                    if (!openFile.Contains(dialog.FileNames[j]))
                    {
                        openFile.Add(dialog.FileNames[j]);
                    }
                }
            }
            for (int i = 0; i < openFile.Count; i++)
            {
                Panel gbox = new Panel();
                gbox.Name = openFile[i] + i;
                gbox.Text = openFile[i];
                gbox.Width = 400;
                gbox.Height = 33;
                gbox.Location = new Point(15, 15 + i * 33);
                //调用添加文本控件的方法
                AddTxt(gbox, Path.GetFileNameWithoutExtension(openFile[i]));

                this.fileGroup.Controls.Add(gbox);
            }


        }
        //添加文本控件
        public void AddTxt(Panel gb, string fileName)
        {
            Button file_icon_button = new Button();

            file_icon_button.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            file_icon_button.BackgroundImage = global::RegulatoryPlan.Properties.Resources.biaoqian;
            file_icon_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            file_icon_button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            file_icon_button.FlatAppearance.BorderSize = 0;
            file_icon_button.FlatAppearance.MouseDownBackColor = SystemColors.GradientInactiveCaption;
            file_icon_button.FlatAppearance.MouseOverBackColor = SystemColors.GradientInactiveCaption;
            file_icon_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            file_icon_button.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            file_icon_button.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            file_icon_button.Location = new System.Drawing.Point(0, 5);
            file_icon_button.Width = 15;
            file_icon_button.Name = "file_icon_button" + gb.Name;
            file_icon_button.UseVisualStyleBackColor = false;
            file_icon_button.Visible = true;
            gb.Controls.Add(file_icon_button);

            Label txt = new Label();
            txt.Name = gb.Name;
            txt.Text = fileName;
            txt.Location = new Point(file_icon_button.Right, 0);
            txt.AutoSize = true;
            gb.Controls.Add(txt);

            Button file_icon_button_close = new Button();
            file_icon_button_close.BackColor = SystemColors.GradientInactiveCaption;
            file_icon_button_close.BackgroundImage = Properties.Resources.shanchu;
            file_icon_button_close.BackgroundImageLayout = ImageLayout.None;
            file_icon_button_close.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            file_icon_button_close.FlatAppearance.BorderSize = 0;

            file_icon_button_close.FlatAppearance.MouseDownBackColor = SystemColors.GradientInactiveCaption;
            file_icon_button_close.FlatAppearance.MouseOverBackColor = SystemColors.GradientInactiveCaption;
            file_icon_button_close.FlatStyle = FlatStyle.Flat;
            file_icon_button_close.Font = new Font("微软雅黑", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            file_icon_button_close.ForeColor = SystemColors.ButtonHighlight;
            int l = file_icon_button.Location.X + file_icon_button.Width + txt.Width;
            //MessageBox.Show(file_icon_button.Right.ToString());
            file_icon_button_close.Location = new Point(240, 7);
            file_icon_button_close.Name = "file_icon_button_close" + gb.Name;
            file_icon_button_close.UseVisualStyleBackColor = false;
            file_icon_button_close.Visible = true;

            gb.Controls.Add(file_icon_button_close);
            file_icon_button_close.Click += new System.EventHandler(this.deleteFile_Click);
        }

        // 删除指定的选择图纸
        private void deleteFile_Click(object sender, EventArgs e)
        {
            //(sender as Label)file_name.Text = "";
            //lb_FileTime.Text = File.GetLastWriteTime(fi).ToLongDateString();
            //this.lb_FileLocation.Text = Path.GetDirectoryName(fi);
            this.fileGroup.Controls.Remove((sender as Button).Parent);
            
        }


        private string GetChooseCity()
        {
            string crtCity = "";
            switch (comboBoxCity.SelectedIndex)
            {
                case 0:
                    crtCity = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                    break;
                case 1:
                    crtCity = "";
                    break;
            }
            return crtCity;
        }

        // 取消
        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // 打开并发送
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBoxCity.SelectedItem == null)
            {
                MessageBox.Show("城市不能为空！");
                return;
            }
            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("导出类型不能为空！");
                return;
            }
            if (this.fileGroup.Controls.Count == 0)
            {
                MessageBox.Show("图纸不能为空！");
                return;
            }

            this.DialogResult = DialogResult.OK;
            foreach (Control con in this.fileGroup.Controls)
            {
                try
                {
                    if (con is Panel)
                        openFile.Add((con as Panel).Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            derivedType = GetChooseType();
            openCity = contentList[comboBoxCity.SelectedIndex]["oid"];
            this.Close();

        }


        private DerivedTypeEnum GetChooseType()
        {
            DerivedTypeEnum crtType = DerivedTypeEnum.None;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    crtType = DerivedTypeEnum.UnitPlan;
                    break;
                case 1:
                    crtType = DerivedTypeEnum.PointsPlan;
                    break;
                case 2:
                    crtType = DerivedTypeEnum.Power10Kv;
                    break;
                case 3:
                    crtType = DerivedTypeEnum.Power35kv;
                    break;
                case 4:
                    crtType = DerivedTypeEnum.WaterSupply;
                    break;
                case 5:
                    crtType = DerivedTypeEnum.HeatSupply;
                    break;
                case 6:
                    crtType = DerivedTypeEnum.FuelGas;
                    break;
                case 7:
                    crtType = DerivedTypeEnum.Communication;
                    break;
                case 8:
                    crtType = DerivedTypeEnum.BuildingIntegrated;
                    break;
                case 9:
                    crtType = DerivedTypeEnum.TheRoadSection;
                    break;
                case 10:
                    crtType = DerivedTypeEnum.PipeLine;
                    break;
                case 11:
                    crtType = DerivedTypeEnum.Sewage;
                    break;
                case 12:
                    crtType = DerivedTypeEnum.FiveLine;
                    break;
                case 13:
                    crtType = DerivedTypeEnum.LimitFactor;
                    break;
                case 14:
                    crtType = DerivedTypeEnum.RainWater;
                    break;
                case 15:
                    crtType = DerivedTypeEnum.ReuseWater;
                    break;
                case 16:
                    crtType = DerivedTypeEnum.Road;
                    break;
                case 17:
                    crtType = DerivedTypeEnum.CenterCityUseLandPlan;
                    break;
                case 18:
                    crtType = DerivedTypeEnum.UseLandNumber;
                    break;
                case 19:
                    crtType = DerivedTypeEnum.CenterCityLifeUseLandPlan;
                    break;
                case 20:
                    crtType = DerivedTypeEnum.RoadSituation;
                    break;
                case 21:
                    crtType = DerivedTypeEnum.FacilityControl;
                    break;
                case 22:
                    crtType = DerivedTypeEnum.FiveLineControl;
                    break;
                case 23:
                    crtType = DerivedTypeEnum.None;
                    break;
            }
            return crtType;
        }

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
