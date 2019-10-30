using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DataGridViewTreeComboxColumn.Properties;

namespace DataGridViewTreeComboxColumn
{
    public class TreeComboBox : UserControl
    {
        #region �Զ����Ա����

        private TextBox m_TextBox;
        private TreeView m_TreeView;
        private Button m_DropdownButton;
        private int m_MaxDropDownItems = 8;
        //�Ƿ�������ʾ�����б�
        private bool b_Dropdown = false;
        //ʹ��
        private bool b_Enabled = true;
        
        //�¼�
        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;
        public event EventHandler EnableChanged;
        public event EventHandler SelectedIndexChanged;
        public event TreeViewCancelEventHandler BeforeExpand;
        public event TreeViewCancelEventHandler BeforeCollapse;
        public event TreeViewEventHandler AfterExpand;
        public event TreeViewEventHandler AfterCollapse;
        #endregion

        /// <summary>
        /// ���캯��
        /// </summary>
        public TreeComboBox()
        {
            try
            {
                this.InitControls();
                this.LayoutControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region �ڲ���������
        /// <summary>
        /// ��������ʼ�����пؼ������������¼���������
        /// </summary>
        private void InitControls()
        {

            //TextBox
            this.m_TextBox = new TextBox();
            this.m_TextBox.KeyDown += new KeyEventHandler(m_TextBox_KeyDown);
            this.m_TextBox.Parent = this;

            //Button
            this.m_DropdownButton = new Button();
            //Assembly asm = Assembly.GetExecutingAssembly();
            //System.IO.Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".BM_dropdown.bmp");
            Bitmap bm = new Bitmap(Resources.BM_dropdown);
            this.m_DropdownButton.Image = bm;
            this.m_DropdownButton.Width = 16;
            this.m_DropdownButton.Height = this.m_TextBox.Height - 2;
            this.m_DropdownButton.Location = new Point(this.m_TextBox.Right - 18, 1);
            this.m_DropdownButton.Click += new EventHandler(m_DropdownButton_Click);
            this.m_DropdownButton.FlatStyle = FlatStyle.Flat;
            this.m_DropdownButton.Parent = this;
            this.m_DropdownButton.BringToFront();

            //TreeView
            this.m_TreeView = new TreeView();
            this.m_TreeView.Visible = false;
            this.m_TreeView.DoubleClick += new EventHandler(m_TreeView_DoubleClick);
            this.m_TreeView.KeyDown += new KeyEventHandler(m_TreeView_KeyDown);
            this.m_TreeView.LostFocus += new EventHandler(m_TreeView_LostFocus);
            this.m_TreeView.BeforeExpand += new TreeViewCancelEventHandler(m_TreeView_BeforeExpand);
            this.m_TreeView.BeforeCollapse += new TreeViewCancelEventHandler(TreeComboBox_BeforeCollapse);
            this.m_TreeView.AfterExpand += new TreeViewEventHandler(m_TreeView_AfterExpand);
            this.m_TreeView.AfterCollapse += new TreeViewEventHandler(TreeComboBox_AfterCollapse);
            this.m_TreeView.Location = new Point(0, 0);
            this.m_TreeView.Parent = null;

            this.LostFocus += new EventHandler(TreeComboBox_LostFocus);
        }

        /// <summary>
        /// �������пؼ�����TextBox�ߴ���Ӧ�����ߴ�
        /// </summary>
        private void LayoutControls()
        {
            this.m_TextBox.Width = this.Width;
            this.Height = this.m_TextBox.Height;
            this.m_DropdownButton.Left = this.m_TextBox.Right - 18;
            this.m_DropdownButton.Height = this.m_TextBox.Height - 2;
            this.m_TreeView.Width = this.Width;
            this.m_TreeView.Height = (int)((this.Font.Height + 3) * this.m_MaxDropDownItems);
        }
        /// <summary>
        /// ����TreeViewѡ�нڵ�
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="selectStr">ѡ�нڵ��ı�</param>
        private void SelectTreeView(TreeView treeView, string tag)
        {
            treeView.Focus();
            for (int i = 0; i < treeView.Nodes.Count; i++)
            {
                for (int j = 0; j < treeView.Nodes[i].Nodes.Count; j++)
                {
                    if ((string)treeView.Nodes[i].Nodes[j].Tag == tag)
                    {
                        treeView.SelectedNode = treeView.Nodes[i].Nodes[j];//ѡ��
                                                                            //treeView.Nodes[i].Nodes[j].Checked = true;
                        treeView.Nodes[i].Expand();//չ������
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// ��ʾ�����б�
        /// </summary>
        private void ShowDropDown()
        {
            try
            {


                if (this.Parent == null)
                    return;

                // ���ܼ�����ʾ��λ�ã��������·���ʾ�����û���㹻�ռ䣬�����Ϸ���ʾ
                // �������·���λ�ã�����ֻ����Ը����ڵ����λ�ã�
                Point pos = new Point(this.Left, this.Bottom - 1);

                // ��λ��ӳ�䵽���㴰��,��ȡ�����ڵ���Ļ����
                Point parentPos = this.Parent.PointToScreen(this.Parent.Location);
                // ��ȡ���㴰�ڵ���Ļ����
                Point topParentPos = this.TopLevelControl.PointToScreen(this.Parent.Location);
                // ����Ը����ڵ�λ�ñ任Ϊ��Զ������ڵ�λ�ã���Ϊpopup�ĸ��Ƕ�������
                pos.Offset(parentPos.X - topParentPos.X, parentPos.Y - topParentPos.Y);

                // ����Ƿ����㹻�ռ�������label�·���ʾday picker
                if ((pos.Y + this.m_TreeView.Height) > this.TopLevelControl.ClientRectangle.Height)
                {
                    // û���㹻�Ŀռ䣨�����˶������ڿͻ��������������Ϸ���ʾ��Y��������ƽ��
                    pos.Y -= (this.Height + this.m_TreeView.Height);
                    if (pos.Y < 0)
                    {
                        // ����Ϸ���Ȼû�пռ���ʾ������ʾ�ڶ������ڵĵײ�
                        pos.Y = (this.TopLevelControl.ClientRectangle.Height - this.m_TreeView.Height);
                    }
                }

                // ����ͣ��,����ұ߳����������ڵĿͻ������򽫿ؼ������ƶ����������ڶ��������Ҳ�
                if ((pos.X + this.m_TreeView.Width) > this.TopLevelControl.ClientRectangle.Width)
                    pos.X = (this.TopLevelControl.ClientRectangle.Width - this.m_TreeView.Width);

                this.m_TreeView.Location = pos;// this.Parent.PointToScreen(pt);
                this.m_TreeView.Visible = true;
                this.m_TreeView.Parent = this.TopLevelControl;
                this.m_TreeView.BringToFront();
                this.b_Dropdown = true;
                //raise event
                if (this.DropDown != null)
                    this.DropDown(this, EventArgs.Empty);
                this.m_TreeView.Focus();
                if (this.Tag!=null)
                {
                    SelectTreeView(this.m_TreeView, (string)this.Tag);
                }
                else
                {
                    this.m_TreeView.SelectedNode=null;
                    this.m_TreeView.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// ���������б�
        /// </summary>
        private void HideDropDown()
        {
            if (this.DropDownClosed != null)
                this.DropDownClosed(this, EventArgs.Empty);
            this.m_TreeView.Parent = null;
            this.m_TreeView.Visible = false;
            this.b_Dropdown = false;
        }
        #endregion

        #region �¼����� - TextBox
        /// <summary>
        /// �ڱ༭���а��°���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.b_Dropdown)
                    this.HideDropDown();
                else
                    this.ShowDropDown();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ShowDropDown();
                this.m_TreeView.Focus();
            }

            if (this.SelectedIndexChanged != null)
                this.SelectedIndexChanged(this, EventArgs.Empty);
        }

        #endregion

        #region �¼����� - TreeView
        /// <summary>
        /// �������б���ѡ����һ���ڵ���¼�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            //������»س���ʾҪѡ�е�ǰ�ڵ�
            if (e.KeyCode == Keys.Enter)
            {
                this.m_TreeView_DoubleClick(sender, EventArgs.Empty);
                if (this.SelectedIndexChanged != null)
                    this.SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// ʧȥ����ʱ��������Ǳ�������ť��ȡ�Ľ��㣬����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_TreeView_LostFocus(object sender, EventArgs e)
        {
            if (!this.m_DropdownButton.Focused)
                this.HideDropDown();
        }

        /// <summary>
        /// �������б���˫��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_TreeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = this.m_TreeView.SelectedNode;
            if (node != null)
                this.m_TextBox.Text = node.Text;

            if (this.b_Dropdown)
            {
                this.HideDropDown();
            }

            if (this.SelectedIndexChanged != null)
                this.SelectedIndexChanged(this, EventArgs.Empty);
        }

        void TreeComboBox_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (this.AfterCollapse != null)
                this.AfterCollapse(this, e);
        }

        void TreeComboBox_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (this.BeforeCollapse != null)
                this.BeforeCollapse(this, e);
        }

        void m_TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (this.AfterExpand != null)
                this.AfterExpand(this, e);
        }

        void m_TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (this.BeforeExpand != null)
                this.BeforeExpand(this, e);
        }


        #endregion

        #region �¼����� - Button
        private void m_DropdownButton_Click(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (this.b_Dropdown)
                this.HideDropDown();
            else
                this.ShowDropDown();
        }
        #endregion

        #region �¼����� - ����
        /// <summary>
        /// ʧȥ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeComboBox_LostFocus(object sender, EventArgs e)
        {
            if (this.b_Dropdown)
                this.HideDropDown();
        }

        /// <summary>
        /// ����ߴ�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.LayoutControls();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.LayoutControls();
        }

        #endregion

        #region �ⲿ���Է�װ

        /// <summary>
        /// ��ȡ�ڵ㼯��
        /// </summary>
        public TreeNodeCollection Nodes
        {
            get { return this.m_TreeView.Nodes; }
        }

        /// <summary>
        /// ���û��߻�ȡ�ڵ��ͼƬ�б�
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return this.m_TreeView.ImageList;
            }
            set
            {
                this.m_TreeView.ImageList = value;
            }
        }

        /// <summary>
        /// ��дenabled����
        /// </summary>
        public new bool Enabled
        {
            get { return this.b_Enabled; }
            set
            {
                if (this.b_Enabled != value)
                {
                    this.b_Enabled = value;
                    this.m_DropdownButton.Enabled = value;
                    this.b_Enabled = value;
                    //���ҵ�ʱ�����������б�
                    if (!this.b_Enabled && this.b_Dropdown)
                        this.HideDropDown();
                    if (this.EnableChanged != null)
                        this.EnableChanged(this, EventArgs.Empty);
                }
            }
        }
     
        /// <summary>
        /// ����
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.m_TextBox.Font = value;
                this.m_TreeView.Font = value;
                //��������
                this.LayoutControls();
            }
        }

        /// <summary>
        /// ��дText
        /// </summary>
        public override string Text
        {
            get
            {
                return this.m_TextBox.Text;
            }
            set
            {
                if (this.Text != value)
                {
                    this.m_TextBox.Text = value;
                }
            }
        }

        /// <summary>
        /// �Ƿ���ʾlines
        /// </summary>
        public bool ShowLines
        {
            get { return this.m_TreeView.ShowLines; }
            set { this.m_TreeView.ShowLines = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ+ -��ť
        /// </summary>
        public bool ShowPlusMinus
        {
            get { return this.m_TreeView.ShowPlusMinus; }
            set { this.m_TreeView.ShowPlusMinus = value; }
        }

        /// <summary>
        /// �Ƿ���ʾroot lines
        /// </summary>
        public bool ShowRootLines
        {
            get { return this.m_TreeView.ShowRootLines; }
            set { this.m_TreeView.ShowRootLines = value; }
        }

        /// <summary>
        /// �Ƿ���ʾroot lines
        /// </summary>
        public bool ShowNodeToolTips
        {
            get { return this.m_TreeView.ShowNodeToolTips; }
            set { this.m_TreeView.ShowNodeToolTips = value; }
        }

        /// <summary>
        /// ��ȡ��������ѡ�еĽڵ㣡
        /// </summary>
        public TreeNode SelectedNode
        {
            get
            {
                return this.m_TreeView.SelectedNode;
            }
            set
            {
                this.m_TreeView.SelectedNode = value;
                if (value != null)
                    this.Text = value.Text;
            }
        }

        public int MaxDropDownItems
        {
            get
            {
                return this.m_MaxDropDownItems;
            }
            set
            {
                if (this.m_MaxDropDownItems != value)
                {
                    this.m_MaxDropDownItems = value;
                    this.m_TreeView.Height = this.m_TextBox.Height * value;
                }
            }
        }

        #endregion

    }
}