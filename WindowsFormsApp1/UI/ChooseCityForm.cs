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

    public partial class ChooseCityForm : Form
    {
        void richTextBox1_GotFocus(object sender, EventArgs e)
        {
            HideCaret((sender as RichTextBox).Handle);
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret((sender as RichTextBox).Handle);
        }

        public string openFile = "";
        public string openCity = "";
        public DerivedTypeEnum derivedType = DerivedTypeEnum.None;

        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);
        List<Dictionary<string, string>> contentList = new List<Dictionary<string, string>>();

        public ChooseCityForm()
        {
            // 获取定义好的项目
            string cityId = Method.SaveProjectIdToXData.GetDefinedProject();

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
            string city = "";
            foreach (Dictionary<string, string> name in contentList)
            {
                names.Add(name["name"]);
                if (name["oid"] == cityId)
                {
                    city = name["name"];
                }
            }

            InitializeComponent(names, city);
            UIMethod.SetFormRoundRectRgn(this, 5);  //设置圆角
            this.textBox1.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择文件夹";
            openFileDialog.Filter = "CAD文件|*.dwg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fi = openFileDialog.FileName;
                label1.Text = Path.GetFileNameWithoutExtension(fi);
                //lb_FileTime.Text = File.GetLastWriteTime(fi).ToLongDateString();
                //this.lb_FileLocation.Text = Path.GetDirectoryName(fi);
                this.textBox1.Text = fi;
                this.button4.Visible = true;
                this.button4.Location = new System.Drawing.Point(this.label1.Right, 246);

                this.button5.Visible = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void deleteFile_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            //lb_FileTime.Text = File.GetLastWriteTime(fi).ToLongDateString();
            //this.lb_FileLocation.Text = Path.GetDirectoryName(fi);
            this.textBox1.Text = "";
            this.button4.Visible = false;

            this.button5.Visible = false;
            HideCaret(this.richTextBox1.Handle);

        }

        private string GetChooseCity()
        {
            string crtCity = "";
            switch (comboBoxCity.SelectedIndex)
            {
                case 0:
                    crtCity = contentList[comboBoxCity.SelectedIndex]["oid"];
                    break;
                case 1:
                    crtCity = contentList[0]["oid"];
                    break;
            }
            return crtCity;
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
                    crtType = DerivedTypeEnum.WaterSupply;
                    break;
                case 4:
                    crtType = DerivedTypeEnum.HeatSupply;
                    break;
                case 5:
                    crtType = DerivedTypeEnum.FuelGas;
                    break;
                case 6:
                    crtType = DerivedTypeEnum.Communication;
                    break;
                case 7:
                    crtType = DerivedTypeEnum.BuildingIntegrated;
                    break;
                case 8:
                    crtType = DerivedTypeEnum.TheRoadSection;
                    break;
                case 9:
                    crtType = DerivedTypeEnum.PipeLine;
                    break;
                case 10:
                    crtType = DerivedTypeEnum.Sewage;
                    break;
                case 11:
                    crtType = DerivedTypeEnum.FiveLine;
                    break;
                case 12:
                    crtType = DerivedTypeEnum.LimitFactor;
                    break;
                case 13:
                    crtType = DerivedTypeEnum.RainWater;
                    break;
                case 14:
                    crtType = DerivedTypeEnum.ReuseWater;
                    break;
                case 15:
                    crtType = DerivedTypeEnum.Road;
                    break;
                case 16:
                    crtType = DerivedTypeEnum.CenterCityUseLandPlan;
                    break;
                case 17:
                    crtType = DerivedTypeEnum.UseLandNumber;
                    break;
                case 18:
                    crtType = DerivedTypeEnum.CenterCityLifeUseLandPlan;
                    break;
                case 19:
                    crtType = DerivedTypeEnum.RoadSituation;
                    break;
                case 20:
                    crtType = DerivedTypeEnum.FacilityControl;
                    break;
                case 21:
                    crtType = DerivedTypeEnum.FiveLineControl;
                    break;
                case 22:
                    crtType = DerivedTypeEnum.None;
                    break;
            }
            return crtType;
        }

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
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("图纸不能为空！");
                return;
            }

            this.DialogResult = DialogResult.OK;
            openFile = this.textBox1.Text;
            derivedType = GetChooseType();
            openCity = contentList[comboBoxCity.SelectedIndex]["oid"];

            this.Close();

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


        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);
        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_STYLE = -16;
        public const int WS_DISABLED = 0x8000000;


        public static void SetControlEnabled(Control c, bool enabled)
        {
            if (enabled)
            { SetWindowLong(c.Handle, GWL_STYLE, (~WS_DISABLED) & GetWindowLong(c.Handle, GWL_STYLE)); }
            else
            { SetWindowLong(c.Handle, GWL_STYLE, WS_DISABLED + GetWindowLong(c.Handle, GWL_STYLE)); }
        }

    }
}
