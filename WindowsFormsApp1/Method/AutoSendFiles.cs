using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.UI;
using RegulatoryPost.FenTuZe;

namespace RegulatoryPlan.Method
{
    class AutoSendFiles
    {
        public static void AutoOpenPointPlanFile(string file, DerivedTypeEnum derivedType)
        {
            try
            {
                Document acDoc = Application.DocumentManager.Open(file, false);
                Database acCurDb = acDoc.Database;
                Editor ed = acDoc.Editor;

                ModelBase mb = new ModelBase();
                if (derivedType is DerivedTypeEnum.PointsPlan)
                {
                    mb = new PointsPlanModel();
                }
                if (derivedType is DerivedTypeEnum.UnitPlan)
                {
                    mb = new UnitPlanModel();
                }

                // 判断是否读取布局空间（papermodel）
                MainForm.isOnlyModel = mb.IsOnlyModel;
                mb.DocName = Path.GetFileNameWithoutExtension(file);

                // 获取图例
                ModelBaseMethod<ModelBase> mbm = new ModelBaseMethod<ModelBase>();
                mbm.GetAllLengedGemo(mb);
                mbm.GetExportLayers(mb);

                // 获取特殊图层
                LayerSpecialCommand<ModelBase> layerSpecial = new LayerSpecialCommand<ModelBase>();
                layerSpecial.AddSpecialLayerModel(mb);

                //mb.LayerList = new List<string>{"道路中线"};
                foreach (string layer in mb.LayerList)
                {
                    ModelBaseMethod<ModelBase> modelMe = new ModelBaseMethod<ModelBase>();
                    LayerModel lyModel = modelMe.GetAllLayerGemo(mb, layer);

                    if (mb.allLines == null)
                    {
                        mb.allLines = new List<LayerModel>();
                    }
                    mb.allLines.Add(lyModel);
                }
                if (derivedType is DerivedTypeEnum.PointsPlan)
                {
                    PostModel.PostModelBase(mb as PointsPlanModel);
                }
                if (derivedType is DerivedTypeEnum.UnitPlan)
                {
                    PostModel.PostModelBase(mb as UnitPlanModel);
                }

            }
            catch (Exception e)
            {
                //string logFileName = @"C: \Users\Public\Documents\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                //using (TextWriter logFile = TextWriter.Synchronized(File.AppendText(logFileName)))
                //{
                //    logFile.WriteLine(DateTime.Now);
                //    logFile.WriteLine(e.Message);
                //    logFile.WriteLine("\r\n");
                //    logFile.Flush();
                //    logFile.Close();
                //}

                //System.Windows.Forms.MessageBox.Show(e.Message);
            }
            // 发送类型

            // 发送图层数据
        }
    }
}
