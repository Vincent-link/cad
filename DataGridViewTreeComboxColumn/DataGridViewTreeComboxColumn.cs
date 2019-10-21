using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DataGridViewTreeComboxColumn
{
     /// <summary>
     /// Host TreeView In DataGridView Cell
     /// </summary>
    public class TreeViewColumn : DataGridViewColumn
     {
         public TreeViewColumn()
             : base(new TreeViewCell())
         {
         }
         public TreeComboBox _root
         {
             get{return Roots.tree;}
             set{Roots.tree=value;}
         }
         public override DataGridViewCell CellTemplate
         {
             get
             {
                 return base.CellTemplate;
             }
             set
             {
                 // Ensure that the cell used for the template is a TreeViewCell.
                 if (value != null &&
                     !value.GetType().IsAssignableFrom(typeof(TreeViewCell)))
                 {
                     throw new InvalidCastException("Must be a TreeViewCell");
                 }
                 base.CellTemplate = value;
             }
         }
    }

   public class TreeViewCell : DataGridViewTextBoxCell
     {
 
         public TreeViewCell()
             : base()
         {
         
             //初始设置
         }
        
         public override void InitializeEditingControl(int rowIndex, object
             initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
         {
             // Set the value of the editing control to the current cell value.
             base.InitializeEditingControl(rowIndex, initialFormattedValue,
                 dataGridViewCellStyle);
             TreeViewEditingControl ctl =
                 DataGridView.EditingControl as TreeViewEditingControl;
             // Use the default row value when Value property is null.
             if (this.Value == null)
             {

                 ctl.SelectedNode = new TreeNode(this.DefaultNewRowValue.ToString());
             }
             else if(this.Value.ToString()!="")
             {
                 ctl.SelectedNode = new TreeNode(this.Value.ToString());
             }
         }
 
         public override Type EditType
         {
             get
             {
                 // Return the type of the editing control that CalendarCell uses.
                 return typeof(TreeViewEditingControl);
             }
         }
 
         public override Type ValueType
         {
             get
             {
                 // Return the type of the value that CalendarCell contains.
                 return typeof(TreeNode);
             }
         }
 
         public override object DefaultNewRowValue
         {
             get
             {
                 // Use the current date and time as the default value.
                 return this.Value;
             }
         }
    }
     /// <summary>
     /// 静态类的静态属性，用于在不同class间传递参数
     /// </summary>
     public static class Roots
     {
         //从前台绑定树
        public static TreeComboBox tree = null;
     }

    public class TreeViewEditingControl : TreeComboBox, IDataGridViewEditingControl
      {
          DataGridView dataGridView;
          private bool valueChanged = false;
          int rowIndex;
          public TreeViewEditingControl()
          {
              try
              {
                  //必须加Roots.tree.Nodes[0].Clone() 否则报错 不能在多处增添或插入项，必须首先将其从当前位置移除或将其克隆
                  this.Nodes.Add(Roots.tree.Nodes[0].Clone() as TreeNode);
                  this.SelectedNode = this.Nodes[0];

                //this.AfterExpand += new TreeViewEventHandler(treeComboBox1_AfterExpand);
                //this.AfterCollapse += new TreeViewEventHandler(treeComboBox1_AfterCollapse);
            }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
             
           
          }
     
          // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
          // property.
          public object EditingControlFormattedValue
          {
              get
              {
                  return this.SelectedNode.Text;
              }
              set
              {
                  if (value is String)
                  {
                      try
                      {
                          // This will throw an exception of the string is 
                          // null, empty, or not in the format of a date.
                          this.SelectedNode = new TreeNode((String)value);
                          
                      }
                      catch
                      {
                          // In the case of an exception, just use the 
                          // default value so we're not left with a null
                          // value.
                          this.SelectedNode = new TreeNode("");
                      }
                  }
              }
          }
  
          // Implements the 
          // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
          public object GetEditingControlFormattedValue(
              DataGridViewDataErrorContexts context)
          {
              return EditingControlFormattedValue;
          }
  
          // Implements the 
          // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
          public void ApplyCellStyleToEditingControl(
              DataGridViewCellStyle dataGridViewCellStyle)
          {
              this.Font = dataGridViewCellStyle.Font;
              this.ForeColor = dataGridViewCellStyle.ForeColor;
              this.BackColor = dataGridViewCellStyle.BackColor;
          }
  
          // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
          // property.
          public int EditingControlRowIndex
          {
              get
              {
                  return rowIndex;
              }
              set
              {
                  rowIndex = value;
              }
          }
  
          // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
          // method.
          public bool EditingControlWantsInputKey(
              Keys key, bool dataGridViewWantsInputKey)
          {
              // Let the TreeViewPicker handle the keys listed.
              switch (key & Keys.KeyCode)
              {
                  case Keys.Left:
                  case Keys.Up:
                  case Keys.Down:
                  case Keys.Right:
                  case Keys.Home:
                  case Keys.End:
                  case Keys.PageDown:
                  case Keys.PageUp:
                      return true;
                  default:
                      return !dataGridViewWantsInputKey;
              }
          }
  
          // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
          // method.
          public void PrepareEditingControlForEdit(bool selectAll)
          {
              // No preparation needs to be done.
          }
  
          // Implements the IDataGridViewEditingControl
          // .RepositionEditingControlOnValueChange property.
          public bool RepositionEditingControlOnValueChange
          {
              get
              {
                  return false;
              }
          }
 
  
          // Implements the IDataGridViewEditingControl
          // .EditingControlDataGridView property.
          public DataGridView EditingControlDataGridView
          {
              get
              {
                  return dataGridView;
              }
              set
              {
                  dataGridView = value;
              }
          }
  
          // Implements the IDataGridViewEditingControl
          // .EditingControlValueChanged property.
          public bool EditingControlValueChanged
          {
              get
              {
                  return valueChanged;
              }
              set
              {
                  valueChanged = value;
              }
          }
  
          // Implements the IDataGridViewEditingControl
          // .EditingPanelCursor property.
          public Cursor EditingPanelCursor
          {
              get
              {
                  return base.Cursor;
              }
        }

        //protected override void OnAfterExpand(TreeViewEventArgs e)
        //  {
        //      base.OnAfterExpand(e);
        //      this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].Width = this.Width + 10;
        //      this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex].Height = this.Height + 20;

        //  }
        protected override void Select(bool directed, bool forward)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.Select(directed, forward);
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnSelectedIndexChanged(e);
        }
        //protected override void OnAfterSelect(TreeViewEventArgs e)
        //{
        //    // Notify the DataGridView that the contents of the cell
        //    // have changed.
        //    valueChanged = true;
        //    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        //    base.OnAfterSelect(e);

        //}
        //void treeComboBox1_AfterCollapse(object sender, TreeViewEventArgs e)
        //{
        //    if (e.Node.ImageIndex == 1)
        //        e.Node.ImageIndex = e.Node.SelectedImageIndex = 0;

        //}

        //void treeComboBox1_AfterExpand(object sender, TreeViewEventArgs e)
        //{
        //    if (e.Node.ImageIndex == 0)
        //        e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
        //    this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].Width = this.Width + 10;
        //    this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex].Height = this.Height + 20;
        //}
    }
}
