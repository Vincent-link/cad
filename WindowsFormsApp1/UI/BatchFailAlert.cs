using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RegulatoryModel.Command;

namespace RegulatoryPlan.UI
{
    public partial class BatchFailAlert : Form
    {
        public List<string> filenames = new List<string>();
        public BatchFailAlert(List<string> fileNames)
        {
            filenames.Clear();
            filenames = fileNames;

            InitializeComponent();
            this.Load += new System.EventHandler(this.LoadFileName);

        }

        public void LoadFileName(object sender, EventArgs e)
        {
            this.BatchFailLists.Controls.Clear();
            int i = 0;
            foreach (string fileName in filenames)
            {
                PictureBox pictureBox3 = new PictureBox();
                ((System.ComponentModel.ISupportInitialize)(pictureBox3)).BeginInit();

                pictureBox3.BackgroundImage = global::RegulatoryPlan.Properties.Resources.Shape002;
                pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                pictureBox3.Location = new System.Drawing.Point(15, 20 + 26 * i);
                pictureBox3.Name = "pictureBox3" + i;
                pictureBox3.Size = new System.Drawing.Size(15, 26);
                pictureBox3.TabIndex = 26;
                pictureBox3.TabStop = false;
                ((System.ComponentModel.ISupportInitialize)(pictureBox3)).EndInit();

                this.BatchFailLists.Controls.Add(pictureBox3);
                // 
                // label1
                // 
                Label label1 = new Label();
                label1.AutoSize = true;
                label1.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                label1.Location = new System.Drawing.Point(30, 15 + 26 * i);
                label1.Name = "label1" + i;
                label1.Size = new System.Drawing.Size(41, 26);
                label1.TabIndex = 27;
                label1.Text = fileName;
                this.BatchFailLists.Controls.Add(label1);
                i++;
            }
        }
        // 取消
        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
    }
}
