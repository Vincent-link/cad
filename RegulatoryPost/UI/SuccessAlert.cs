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
            FenTuZe.UIMethod.SetFormRoundRectRgn(this, 5);	//设置圆角

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
