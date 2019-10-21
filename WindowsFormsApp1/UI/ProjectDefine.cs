using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using RegulatoryPlan.Models;
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

    public partial class ProjectDefine : Form
    {
        void richTextBox1_GotFocus(object sender, EventArgs e)
        {
            HideCaret((sender as RichTextBox).Handle);
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret((sender as RichTextBox).Handle);
        }

        public string openCity = "";

        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);
        JsonCityData contentList = new JsonCityData();


        public ProjectDefine()
        {
            List<string> names = new List<string>();
            try
            {
                string projectIdBaseAddress = "http://172.18.84.155:8080/PDD/pdd/cim-interface!findAllProject";
                var projectIdHttp = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri(projectIdBaseAddress));

                var response = projectIdHttp.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream, Encoding.UTF8);
                var content = sr.ReadToEnd();
                contentList = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonCityData>(content);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(content);
            if (contentList.result!=null)
            {
                foreach (City city in contentList.result)
                {
                    names.Add(city.name);
                }
            }

            InitializeComponent(names);
            UIMethod.SetFormRoundRectRgn(this, 5);  //设置圆角
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBoxCity.SelectedItem == null)
            {
                MessageBox.Show("城市不能为空！");
                return;
            }

            this.DialogResult = DialogResult.OK;            
            openCity = contentList.result[comboBoxCity.SelectedIndex].oid;

            //点击button按钮事件
            Method.SaveProjectIdToXData.SaveSelectedProjectIdToXData(openCity);

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
