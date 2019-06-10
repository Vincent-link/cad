using RegulatoryModel.Command;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Method;
using RegulatoryPlan.Model;
using RegulatoryPost.FenTuZe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPlan.UI
{
    public partial class MainForm : Form
    {
        ModelBase model;
   
        public MainForm()
        {
            InitializeComponent();
            InitPage();
            InitData();
        }
        public MainForm(string cityName,DerivedTypeEnum derivedType)
        {
           crtType = derivedType;
            InitializeComponent();
            InitPage();
            InitData();
            this.lb_City.Text = cityName;
        }

        private void InitPage()
        {
         this.lb_DrawingName.Text=   DrawingMethod.GetDrawingName();
            foreach (string item in comboBox1.Items)
            {
                if (item.Contains(lb_DrawingName.Text))
                {
                    comboBox1.SelectedItem = item;
                    break;
                }

            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MethodCommand.num--;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (model.DerivedType)
            {
                case DerivedTypeEnum.BuildingIntegrated:
                    PostModel.PostModelBase(model as BuildingIntegratedModel);
                    break;

                case DerivedTypeEnum.UnitPlan:
                    PostModel.PostModelBase(model);
                    break;
                case DerivedTypeEnum.PointsPlan:
                    PostModel.PostModelBase(model);
                    break;
                case DerivedTypeEnum.Power10Kv:
                    PostModel.PostModelBase(model as Power10kvModel);
                    break;
                case DerivedTypeEnum.Power35kv:
                    PostModel.PostModelBase(model as Power35kvModel );
                    break;
                case DerivedTypeEnum.WaterSupply:
                    PostModel.PostModelBase(model as WaterSupplyvModel);
                    break;
                case DerivedTypeEnum.HeatSupply:
                    PostModel.PostModelBase(model as HeatSupplyModel);
                    break;
                case DerivedTypeEnum.FuelGas:
                    PostModel.PostModelBase(model as FuelGasModel);
                    break;
                case DerivedTypeEnum.Communication:
                    PostModel.PostModelBase(model as CommunicationModel);
                    break;
                case DerivedTypeEnum.TheRoadSection:
                    PostModel.PostModelBase(model as RoadSectionModel);
                    break;
                case DerivedTypeEnum.PipeLine:
                    PostModel.PostModelBase(model as PipeLineModel);
                    break;
                case DerivedTypeEnum.Sewage:
                    PostModel.PostModelBase(model as SewageModel);
                    break;
                case DerivedTypeEnum.FiveLine:
                    PostModel.PostModelBase(model as FiveLineModel);
                    break;
                case DerivedTypeEnum.LimitFactor:
                    PostModel.PostModelBase(model as LimitFactorModel );
                    break;
                case DerivedTypeEnum.RainWater:
                    PostModel.PostModelBase(model as RainWaterModel);
                    break;
                case DerivedTypeEnum.ReuseWater:
                    PostModel.PostModelBase(model as ReuseWaterModel);
                    break;

            }
        }
        DerivedTypeEnum crtType;

        private ModelBase ChangeToModel(ModelBase mb)
        {
            switch (crtType)
            {
                case DerivedTypeEnum.TheRoadSection:
                    return new RoadSectionModel();
                case DerivedTypeEnum.Sewage:
                    return new SewageModel();
            }
            return new ModelBase();

        }
        private void InitData()
        {
            ModelBase mb = new ModelBase();
            mb= ChangeToModel(mb);
            ModelBaseMethod<ModelBase> mbm = new ModelBaseMethod<ModelBase>();
            // mbm.GetLengedPoints(mb);
            mbm.GetAllLengedGemo(mb);
            mbm.GetExportLayers(mb);
            LayerSpecialCommand<ModelBase> layerSpecial = new LayerSpecialCommand<ModelBase>();
            layerSpecial.AddSpecialLayerModel(mb);
          //  mb.AddSpecialLayerModel();
            DataTable tb = new DataTable();
            tb.Columns.Add("所在图层");
            tb.Columns.Add("数据图层");
            tb.Columns.Add("图例框顶点");
            //tb.Columns.Add("图例文本");
            //tb.Columns.Add("图例线段");

            int colNum = 1;
            if (mb.LegendList != null)
            {
                for (int i = 0; i < mb.LegendList.Count; i++)
                {
                    DataRow dr = tb.NewRow();
                    dr[0] =MethodCommand.LegendLayer;
            
                   dr["数据图层"]=mb.LegendList[i].LayerName;
                    string peakPoint = "";

                    foreach (PointF PT in mb.LegendList[i].BoxPointList)
                    {
                        peakPoint += "Point:" + PT.X + "," + PT.Y + ";";
                    }
                    dr["图例框顶点"] = peakPoint ;
                    int bimIndex = 0;
                    int colIndex = 0;
                    foreach (BlockInfoModel item in mb.LegendList[i].GemoModels)
                    {
                        if (item.DbText != null) {
                            if (colNum <= bimIndex + 1)
                            {
                                colNum++;
                                DataColumn dc = new DataColumn("图例图形" + (bimIndex + 1));
                                tb.Columns.Add(dc);
                            }
                            dr["图例图形" + (bimIndex + 1)] = ReflectionClass.GetAllPropertyInfo<DbTextModel>(item.DbText, "文本");
                            bimIndex++;
                    }
                       
                        if (item.PolyLine!= null)
                        {
                            if (colNum <= bimIndex + 1)
                            {
                                colNum++;
                                DataColumn dc = new DataColumn("图例图形" + (bimIndex + 1));
                                tb.Columns.Add(dc);
                            }
                            dr["图例图形" + (bimIndex + 1)] = ReflectionClass.GetAllPropertyInfo<PolyLineModel>(item.PolyLine, "多段线");
                           
                            bimIndex++;
                        }

                        if (item.Line != null&&item.Line.Count>0)
                        {
                            foreach (LineModel lineMode in item.Line)
                            {
                                if (colNum <= bimIndex + 1)
                                {
                                    colNum++;
                                    DataColumn dc = new DataColumn("图例图形" + (bimIndex + 1));
                                    tb.Columns.Add(dc);
                                }
                                dr["图例图形" + (bimIndex + 1)] = ReflectionClass.GetAllPropertyInfo<LineModel>(lineMode, "直线");

                                bimIndex++;
                            }
                        }
                        if (item.Hatch!= null )
                        {
                            if (colNum <= bimIndex + 1)
                            {
                                colNum++;
                                DataColumn dc = new DataColumn("图例图形" + (bimIndex + 1));
                                tb.Columns.Add(dc);
                            }
                            dr["图例图形" + (bimIndex + 1)] = ReflectionClass.GetAllPropertyInfo<HatchModel>(item.Hatch, "填充");

                            bimIndex++;
                        }
                        if (item.Circle!= null && item.Circle.Count > 0)
                        {
                            foreach (CircleModel lineMode in item.Circle)
                            {
                                if (colNum <= bimIndex + 1)
                                {
                                    colNum++;
                                    DataColumn dc = new DataColumn("图例图形" + (bimIndex + 1));
                                    tb.Columns.Add(dc);
                                }
                                dr["图例图形" + (bimIndex + 1)] = ReflectionClass.GetAllPropertyInfo<CircleModel>(lineMode, "圆");

                                bimIndex++;
                            }
                        }


                    }
                    //for (int j = 0; j < mb.LegendList.Count; j++)
                    //{
                    //    if (colNum <= j + 1)
                    //    {
                    //        colNum++;
                    //        DataColumn dc = new DataColumn("Point" + (j + 1));
                    //        tb.Columns.Add(dc);
                    //    }
                    //    dr[j + 1] = "X:" + mb.LegendList[i][j].X + ",Y:" + mb.LegendList[i][j].Y;
                    //}
                    tb.Rows.Add(dr);
                }
            }
            if (mb.LayerList != null)
            {
                TreeNode rotNode = new TreeNode("图层筛选");
                this.treeView1.Nodes.Add(rotNode);
                foreach (string layer in mb.LayerList)
                {
                    TreeNode node = new TreeNode(layer);
                    rotNode.Nodes.Add(node);
                }
                this.treeView1.ExpandAll();
            }
            if (mb is RoadSectionModel)
            {
                LayerModel spModel = (mb as RoadSectionModel).allLines[(mb as RoadSectionModel).allLines.Count - 1];
                GetSpecialDataRowInfo(spModel.modelItemList, tb, spModel.Name);
            }
            else if (mb is PipeModel)
            {
                LayerModel spModel = (mb as PipeModel).allLines[(mb as PipeModel).allLines.Count - 1];
                GetSpecialDataRowInfo(spModel.modelItemList, tb, spModel.Name);

            }

            this.dataGridView1.DataSource = tb;
            model = mb;
        }

        private void GetLineDataRowInfo(Dictionary<int, List<object>> list,DataTable tb,string layerName)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int colNum = 1;
                DataRow dr = tb.NewRow();
                dr[0] = layerName;
                for (int j = 0; j < list[i].Count; j++)
                {
                    if (colNum <= j + 1)
                    {
                        colNum++;
                        DataColumn dc = new DataColumn("Point" + (j + 1));
                        if (!tb.Columns.Contains("Point" + (j + 1)))
                        {
                            tb.Columns.Add(dc);
                        }

                    }
                    if (list[i][j] is PointF)
                    {
                        dr[j + 1] = "X:" + ((PointF)list[i][j]).X + ",Y:" + ((PointF)list[i][j]).Y;
                    }

                }
                tb.Rows.Add(dr);
            }
           
        }


        private void GetSpecialDataRowInfo(List<object> list, DataTable tb, string layerName)
        {
            if (list.Count > 0)
            {
                if (list[0] is RoadSectionItemModel)
                {
                    DataColumn dc = new DataColumn("道路名称");
                    DataColumn dc1 = new DataColumn("道路长度");
                    DataColumn dc2 = new DataColumn("道路宽度");
                    DataColumn dc3 = new DataColumn("线颜色");

                    if (!tb.Columns.Contains("道路名称"))
                    {
                        tb.Columns.Add(dc);
                    }
                    if (!tb.Columns.Contains("道路长度"))
                    {
                        tb.Columns.Add(dc1);
                    }
                    //if (!tb.Columns.Contains("道路宽度"))
                    //{
                    //    tb.Columns.Add(dc2);
                    //}
                    if (!tb.Columns.Contains("线颜色"))
                    {
                        tb.Columns.Add(dc3);
                    }



                    for (int i = 0; i < list.Count; i++)
                    {
                        int colNum = 1;
                        DataRow dr = tb.NewRow();
                        dr[0] = layerName;

                        if (list[i] is RoadSectionItemModel)
                        {
                            RoadSectionItemModel road = list[i] as RoadSectionItemModel;
                            dr["道路名称"] = road.RoadName == null ? "" : road.RoadName;
                            dr["道路长度"] = road.RoadLength == null ? "" : road.RoadLength;
                            dr["线颜色"] = road.ColorIndex;
                            for (int j = 0; j < road.roadList.Count; j++)
                            {
                                DataColumn dc5 = new DataColumn("Point" + (j + 1));
                                if (!tb.Columns.Contains("Point" + (j + 1)))
                                {
                                    tb.Columns.Add(dc5);
                                }
                                dr[("Point" + (j + 1))] = "X:" + road.roadList[j].X + ",Y:" + road.roadList[j].Y;
                            }

                        }
                        tb.Rows.Add(dr);
                    }
                }

                if (list[0] is PipeItemModel)
                {
                    DataColumn dc = new DataColumn("管线类型");
                    DataColumn dc1 = new DataColumn("管线长度");
                    DataColumn dc2 = new DataColumn("管线宽度");
                    DataColumn dc3 = new DataColumn("管线颜色");
                    DataColumn dc6 = new DataColumn("管道信息位置");
                 

                    if (!tb.Columns.Contains("管线类型"))
                    {
                        tb.Columns.Add(dc);
                    }
                    if (!tb.Columns.Contains("管道信息位置"))
                    {
                        tb.Columns.Add(dc6);
                    }
                    if (!tb.Columns.Contains("管线长度"))
                    {
                        tb.Columns.Add(dc1);
                    }
                    if (!tb.Columns.Contains("管线宽度"))
                    {
                        tb.Columns.Add(dc2);
                    }
                    if (!tb.Columns.Contains("管线颜色"))
                    {
                        tb.Columns.Add(dc3);
                    }



                    for (int i = 0; i < list.Count; i++)
                    {
                        int colNum = 1;
                        DataRow dr = tb.NewRow();
                       

                        if (list[i] is PipeItemModel)
                        {
                          
                            PipeItemModel road = list[i] as PipeItemModel;
                            dr["所在图层"] = road.PipeLayer == null ? "" : road.PipeLayer;
                            dr["管线类型"] = road.PipeType == null ? "" : road.PipeType;
                            dr["管道信息位置"] = "X:" + road.TxtLocation.X + ",Y:" + road.TxtLocation.Y;
                            dr["管线长度"] = road.PipeLength== null ? "" : road.PipeLength;
                            dr["管线宽度"] = road.PipeWidth== null ? "" : road.PipeWidth;
                            dr["管线颜色"] = road.ColorIndex;
                            for (int j = 0; j < road.pipeList.Count; j++)
                            {
                                DataColumn dc5 = new DataColumn("Point" + (j + 1));
                                if (!tb.Columns.Contains("Point" + (j + 1)))
                                {
                                    tb.Columns.Add(dc5);
                                }
                                dr[("Point" + (j + 1))] = "X:" + road.pipeList[j].X + ",Y:" + road.pipeList[j].Y;
                            }

                        }
                        tb.Rows.Add(dr);
                    }
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

            DataTable tb = null;

            if (this.dataGridView1.DataSource != null)
            {
                tb = this.dataGridView1.DataSource as DataTable;
            }
            else
            {
                tb = new DataTable();
                tb.Columns.Add("图层");

            }
            if (!e.Node.Checked)
            {
                List<DataRow> removeList = new List<DataRow>();
                foreach(DataRow dr in tb.Rows)
                {
                    if (dr[0].ToString() == e.Node.Text)
                    {
                      removeList.Add(dr);
                    }
                }

                foreach (DataRow dr in removeList)
                {
                    tb.Rows.Remove(dr);
                }
                LayerModel mm = new LayerModel();
                foreach (LayerModel item in model.allLines)
                {
                    if (item.Name==e.Node.Text)
                    {
                        mm = item;
                    }
                }
                model.allLines.Remove(mm);
            }
            else
            {
                LayerModel model = PolylineMethod.GetLayerModel(e.Node.Text);

               
                GetLineDataRowInfo(model.pointFs,tb,e.Node.Text);
                if (this.model.allLines == null)
                {
                    this.model.allLines = new List<LayerModel>();
                }
                this.model.allLines.Add(model);
            }
            this.dataGridView1.DataSource = tb;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        { 
            string layerName = dataGridView1.SelectedRows[0].Cells["数据图层"].Value.ToString();
            string outStr = "";
            foreach (LengedModel item in model.LegendList)
            {
                if (item.LayerName == layerName)
                {
                    int part = 0;
                    outStr += "{";
                    foreach(BlockInfoModel item1 in item.GemoModels)
                    {
                        try
                        {
                            outStr += "\"line" + part + "\":[" + JsonCommand.ToJson(item1.PolyLine.Vertices) + "],";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    outStr = outStr.TrimEnd(',');
                    outStr += "}";
                }
            }
        }
    }
}
