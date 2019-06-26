using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPlan.UI
{
    public partial class ChooseCityForm : Form
    {
       public string openFile = "";
        public string openCity = "";
        public DerivedTypeEnum derivedType = DerivedTypeEnum.None;
        public ChooseCityForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择文件夹";
            openFileDialog.Filter = "CAD文件|*.dwg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fi = openFileDialog.FileName;
                lb_PageName.Text= Path.GetFileNameWithoutExtension(fi);
                lb_FileTime.Text = File.GetLastWriteTime(fi).ToLongDateString();
                this.lb_FileLocation.Text = Path.GetDirectoryName(fi);
                this.textBox1.Text = fi;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

     
        
        private DerivedTypeEnum GetChooseType()
        {
            DerivedTypeEnum crtType= DerivedTypeEnum.None;
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

            }    
            return crtType;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem == null||string.IsNullOrEmpty(this.textBox1.Text))
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            openFile = this.textBox1.Text;
            derivedType = GetChooseType();
            openCity = this.listBox1.SelectedItem.ToString();
            this.Close();
           
        }
    }
}
