using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void songsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells. 
            if (e.RowIndex < 0 || e.ColumnIndex < 0) MessageBox.Show("不能修改");
        }

        #region Windows 窗体设计器生成的代码
        private void InitializeComponent(DataGridView songsDataGridView)
        {
            this.Controls.Add(songsDataGridView);

            songsDataGridView.Name = "songsDataGridView";
            songsDataGridView.Size = new Size(500, 650);

            DataTable tCxC = (DataTable)songsDataGridView.DataSource;

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(songsDataGridView.DataSource);

            MessageBox.Show(JSONString);

            ////Delete link

            //DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
            //Deletelink.UseColumnTextForLinkValue = true;
            //Deletelink.HeaderText = "delete";
            //Deletelink.DataPropertyName = "lnkColumn";
            //Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
            //Deletelink.Text = "删除";
            //songsDataGridView.Columns.Add(Deletelink);

            //songsDataGridView.Sort(songsDataGridView.Columns["用地代码"], System.ComponentModel.ListSortDirection.Ascending);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(684, 661);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitializeComponent2(DataGridView songsDataGridView)
        {
            this.Controls.Add(songsDataGridView);

            songsDataGridView.Name = "songsDataGridView";
            songsDataGridView.Size = new Size(500, 650);

            DataTable tCxC = (DataTable)songsDataGridView.DataSource;

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(songsDataGridView.DataSource);

            MessageBox.Show(JSONString);

            ////Delete link

            //DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
            //Deletelink.UseColumnTextForLinkValue = true;
            //Deletelink.HeaderText = "delete";
            //Deletelink.DataPropertyName = "lnkColumn";
            //Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
            //Deletelink.Text = "删除";
            //songsDataGridView.Columns.Add(Deletelink);

            //songsDataGridView.Sort(songsDataGridView.Columns["用地代码"], System.ComponentModel.ListSortDirection.Ascending);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(684, 661);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

