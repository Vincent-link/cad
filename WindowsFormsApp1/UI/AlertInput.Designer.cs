﻿using DataGridViewTreeComboxColumn;
using RegulatoryPlan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace RegulatoryPlan.UI
{
    partial class AlertInput
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
        private void InitializeComponent(FactorJsonData factors, StageJsonData stages, System.Data.DataTable table)
        {

            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();

            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.polylineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.polylineNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.individualName = new DataGridViewTextBoxColumn();
            this.entitycount = new DataGridViewTextBoxColumn();
            this.individualStage = new DataGridViewTextBoxColumn();
            this.treeComboBox1 = new DataGridViewTreeComboxColumn.TreeComboBox();
            this.factor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGet = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnFind = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.treeComboBox2 = new DataGridViewTreeComboxColumn.TreeComboBox();


            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(503, 419);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.cancel_Click);
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.polylineId,
            this.polylineNum,
            this.factor,
            this.individualName,
            this.entitycount,
            this.individualStage,
            this.btnGet,
            this.btnFind,
            this.btnDelete});
            this.dataGridView1.Location = new System.Drawing.Point(31, 26);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(547, 362);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            // 
            // polylineId
            // 
            this.polylineId.HeaderText = "多段线Id";
            this.polylineId.Name = "polylineId";
            this.polylineId.Visible = false;
            // 
            // polylineNum
            // 
            this.polylineNum.HeaderText = "多段线编码";
            this.polylineNum.Name = "polylineNum";

            // factor
            Dictionary<string,string> dic = new Dictionary<string, string>();
            List<string> result = new List<string>();
            TreeNode node = new TreeNode();
            if (factors.result != null)
            {
                GetFactorNode(factors.result, ref node);
            }

            ////foreach (var item in dic)
            ////{
            ////    result.Add(item.Value);
            ////}
            ////node.Text = "节点";

            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode item in node.Nodes)
                {

                    this.treeComboBox1.Nodes.Add(item);
                }
            }
            else
            {
                this.treeComboBox1.Nodes.Add(node);
            }

            TreeNode node2 = new TreeNode();
            if (stages.result != null)
            {
                GetStageNode(stages.result, ref node2);
            }

            if (node2.Nodes.Count > 0)
            {
                foreach (TreeNode item in node2.Nodes)
                {
                    this.treeComboBox2.Nodes.Add(item);
                }
            }
            else
            {
                this.treeComboBox2.Nodes.Add(node2);
            }

            ////为下拉列表添加节点
            //for (int i = 0; i < 4; i++)
            //{
            //    this.treeComboBox1.Nodes.Add("key" + i, "Depart" + i, 0, 0);
            //    for (int j = 0; j < i + 1; j++)
            //    {
            //        this.treeComboBox1.Nodes[i].Nodes.Add("key_child" + i + j, "User" + i + j, 2, 2);
            //        for (int k = 0; k < j + 1; k++)
            //        {
            //            this.treeComboBox1.Nodes[i].Nodes[j].Nodes.Add("key_child" + i + j + k, "User" + i + j + k, 2, 2);
            //        }
            //    }
            //}

            //this.factor._root = this.treeComboBox1;
            //Items.AddRange(result.ToArray());
            this.factor.HeaderText = "个体要素";
            this.factor.Name = "factor";

            dataGridView1.ColumnWidthChanged += dgv_User_ColumnWidthChanged;
            dataGridView1.CurrentCellChanged += dgv_User_CurrentCellChanged;
            dataGridView1.Scroll += dgv_User_Scroll;
            dataGridView1.DataBindingComplete += dgv_User_DataBindingComplete;
          
            this.treeComboBox1.AfterExpand += new TreeViewEventHandler(treeComboBox1_AfterExpand);
            this.treeComboBox1.AfterCollapse += new TreeViewEventHandler(treeComboBox1_AfterCollapse);


            this.treeComboBox1.Visible = false;

            this.treeComboBox1.SelectedIndexChanged += new EventHandler(cmb_Temp_SelectedIndexChanged);
            dataGridView1.Controls.Add(this.treeComboBox1);


            // 阶段
            dataGridView1.ColumnWidthChanged += stage_ColumnWidthChanged;
            dataGridView1.CurrentCellChanged += stage_CurrentCellChanged;
            dataGridView1.Scroll += stage_Scroll;

            this.treeComboBox2.AfterExpand += new TreeViewEventHandler(treeComboBox2_AfterExpand);
            this.treeComboBox2.AfterCollapse += new TreeViewEventHandler(treeComboBox2_AfterCollapse);

            this.treeComboBox2.Visible = false;

            this.treeComboBox2.SelectedIndexChanged += new EventHandler(cmb_Temp_SelectedIndexChanged);
            dataGridView1.Controls.Add(this.treeComboBox2);

            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    int index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = row[0];
                    this.dataGridView1.Rows[index].Cells[1].Value = row[1];
                    this.dataGridView1.Rows[index].Cells[3].Value = row["个体名称"];
                    this.dataGridView1.Rows[index].Cells[4].Value = row["数量"];

                    string result1 = "";
                    GetFactorName(factors.result, (string)row["个体要素"], ref result1);

                    this.dataGridView1.Rows[index].Cells[2].Value = result1;
                    this.dataGridView1.Rows[index].Cells[2].Tag = (string)row["个体要素"];

                    string result2 = "";
                    GetStageName(stages.result, (string)row["个体阶段"], ref result2);

                    this.dataGridView1.Rows[index].Cells[5].Value = result2;
                    this.dataGridView1.Rows[index].Cells[5].Tag = (string)row["个体阶段"];

                }
            }

            // 
            // individualCode
            // 
            this.individualName.HeaderText = "个体名称";
            this.individualName.Name = "individualName";
            //
            //
            //
            this.entitycount.HeaderText = "数量";
            this.entitycount.Name = "entitycount";
            this.entitycount.ReadOnly = true;
            this.entitycount.FillWeight = 30;
            //
            //
            //
            this.individualStage.HeaderText = "个体阶段";
            this.individualStage.Name = "individualStage";
            // 
            // btnGet
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "拾取";
            this.btnGet.DefaultCellStyle = dataGridViewCellStyle2;
            this.btnGet.HeaderText = "拾取";
            this.btnGet.Name = "btnGet";
            // 
            // btnFind
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "定位";
            this.btnFind.DefaultCellStyle = dataGridViewCellStyle3;
            this.btnFind.HeaderText = "定位";
            this.btnFind.Name = "btnFind";
            // 
            // btnDelete
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "删除";
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle4;
            this.btnDelete.HeaderText = "删除";
            this.btnDelete.Name = "btnDelete";
            // 
            // AlertInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(656, 467);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AlertInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入编码";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.Button button1;
        private DataGridViewButtonColumn btnModify;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn polylineId;
        private DataGridViewTextBoxColumn polylineNum;
        private DataGridViewButtonColumn btnGet;
        private DataGridViewButtonColumn btnFind;
        private DataGridViewButtonColumn btnDelete;
        private DataGridViewTextBoxColumn factor;
        private DataGridViewTreeComboxColumn.TreeComboBox treeComboBox1;
        private DataGridViewTextBoxColumn individualName;
        private DataGridViewTextBoxColumn entitycount;
        private DataGridViewTextBoxColumn individualStage;
        private DataGridViewTreeComboxColumn.TreeComboBox treeComboBox2;

    }
}