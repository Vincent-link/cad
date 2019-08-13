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

    //public class TestRichTextBox : System.Windows.Forms.RichTextBox
    //{
    //    private const int WM_SETFOCUS = 0x7;
    //    private const int WM_LBUTTONDOWN = 0x201;
    //    private const int WM_LBUTTONUP = 0x202;
    //    private const int WM_LBUTTONDBLCLK = 0x203;
    //    private const int WM_RBUTTONDOWN = 0x204;
    //    private const int WM_RBUTTONUP = 0x205;
    //    private const int WM_RBUTTONDBLCLK = 0x206;
    //    private const int WM_KEYDOWN = 0x0100;
    //    private const int WM_KEYUP = 0x0101;

    //    public TestRichTextBox()
    //    {
    //        this.Cursor = Cursors.Arrow;//设置鼠标样式
    //    }

    //    protected override void WndProc(ref Message m)
    //    {
    //        if (m.Msg == WM_SETFOCUS || m.Msg == WM_KEYDOWN || m.Msg == WM_KEYUP || m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONUP || m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_RBUTTONUP || m.Msg == WM_RBUTTONDBLCLK)
    //        {
    //            return;
    //        }
    //        base.WndProc(ref m);
    //    }
    //}
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

        public ChooseCityForm()
        {
            InitializeComponent();
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
                    crtCity = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                    break;
                case 1:
                    crtCity = "";
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
            openCity = GetChooseCity();
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
