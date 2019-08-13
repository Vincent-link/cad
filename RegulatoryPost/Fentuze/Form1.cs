using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPost.Fentuze
{
    public partial class Form1 : Form
    {
        public Form1(string chartName)
        {
            InitializeComponent();
            this.chartName.Text = chartName;
        }

        private void chartName_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
