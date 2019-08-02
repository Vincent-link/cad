namespace RegulatoryPlan.UI
{
    partial class ChooseCityForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_FileLocation = new System.Windows.Forms.Label();
            this.lb_FileTime = new System.Windows.Forms.Label();
            this.lb_PageName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 21;
            this.listBox1.Items.AddRange(new object[] {
            "成安县"});
            this.listBox1.Location = new System.Drawing.Point(5, 27);
            this.listBox1.Margin = new System.Windows.Forms.Padding(5);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(213, 454);
            this.listBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 497);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(673, 497);
            this.button2.Margin = new System.Windows.Forms.Padding(5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 46);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(467, 29);
            this.textBox1.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(495, 40);
            this.button3.Margin = new System.Windows.Forms.Padding(5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(67, 40);
            this.button3.TabIndex = 4;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lb_FileLocation);
            this.groupBox1.Controls.Add(this.lb_FileTime);
            this.groupBox1.Controls.Add(this.lb_PageName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(237, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(562, 481);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "导出图纸";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "地块控制总图则",
            "分图图则",
            "供电工程规划图",
            " ",
            "给水工程规划图",
            "供热工程规划图",
            "燃气工程规划图",
            "通信工程规划图",
            "城市设计图",
            "道路横断面规划图",
            "管线综合规划图",
            "污水工程规划图",
            "五线规划图",
            "限制性要素分析图",
            "雨水工程规划图",
            "再生水工程规划图",
            "道路名称图",
            "用地规划图",
            "用地编码图",
            "居住用地规划图",
            "道路现状图"});
            this.comboBox1.Location = new System.Drawing.Point(98, 116);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(451, 29);
            this.comboBox1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(10, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 21);
            this.label4.TabIndex = 11;
            this.label4.Text = "导出类型:";
            // 
            // lb_FileLocation
            // 
            this.lb_FileLocation.AutoSize = true;
            this.lb_FileLocation.Location = new System.Drawing.Point(62, 364);
            this.lb_FileLocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_FileLocation.Name = "lb_FileLocation";
            this.lb_FileLocation.Size = new System.Drawing.Size(15, 21);
            this.lb_FileLocation.TabIndex = 10;
            this.lb_FileLocation.Text = " ";
            // 
            // lb_FileTime
            // 
            this.lb_FileTime.AutoSize = true;
            this.lb_FileTime.Location = new System.Drawing.Point(105, 282);
            this.lb_FileTime.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_FileTime.Name = "lb_FileTime";
            this.lb_FileTime.Size = new System.Drawing.Size(15, 21);
            this.lb_FileTime.TabIndex = 9;
            this.lb_FileTime.Text = " ";
            // 
            // lb_PageName
            // 
            this.lb_PageName.AutoSize = true;
            this.lb_PageName.Location = new System.Drawing.Point(105, 203);
            this.lb_PageName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_PageName.Name = "lb_PageName";
            this.lb_PageName.Size = new System.Drawing.Size(15, 21);
            this.lb_PageName.TabIndex = 8;
            this.lb_PageName.Text = " ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(10, 282);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "修改时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(10, 362);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "位置：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 203);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "图纸名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(3, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(223, 486);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "城市选择";
            // 
            // ChooseCityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 551);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ChooseCityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出图纸";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_FileLocation;
        private System.Windows.Forms.Label lb_FileTime;
        private System.Windows.Forms.Label lb_PageName;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
    }
}