using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPost.Fentuze
{
    public partial class SuccessAlert : Form
    {
        public SuccessAlert(string chartName)
        {
            InitializeComponent();
        }

        private void Close(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            this.Close();
        }
        private void Timer(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Interval = 3 * 1000;

            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Close);

        }
    }
}
