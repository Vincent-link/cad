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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitPage();
        }

        private void InitPage()
        {
         this.lb_DrawingName.Text=   DrawingMethod.GetDrawingName();
            foreach (string item in comboBox1.Items)
            {
                if (item.Contains(lb_DrawingName.Text))
                {
                    comboBox1.SelectedItem = item;
                    break;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
