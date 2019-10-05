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

using RegulatoryPlan.Method;

namespace RegulatoryPlan.Method
{
    public sealed class AutoSendFiles
    {
        private AutoSendFiles()
        {

        }
        private static readonly AutoSendFiles instance = new AutoSendFiles();

        public static AutoSendFiles Instance
        {
            get { return instance; }
        }

        internal void AutoOpenPointPlanFile(string file, string cityName, DerivedTypeEnum derivedType, List<string> failedFiles)
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
                if (derivedType is DerivedTypeEnum.None)
                {
                    mb = new ModelBase();
                }

                // 判断是否读取布局空间（papermodel）
                MainForm.isOnlyModel = mb.IsOnlyModel;
                mb.DocName = Path.GetFileNameWithoutExtension(file);
                mb.projectId = cityName;

                // 获取图例
                ModelBaseMethod<ModelBase> mbm = new ModelBaseMethod<ModelBase>();
                mbm.GetAllLengedGemo(mb);
                mbm.GetExportLayers(mb);

                // 获取特殊图层
                LayerSpecialCommand<ModelBase> layerSpecial = new LayerSpecialCommand<ModelBase>();
                layerSpecial.AddSpecialLayerModel(mb);

                //mb.LayerList = new List<string>{"道路中线"};
                if (mb.LayerList != null)
                {
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
                        PostModel.AutoPostModelBase(mb as ModelBase, failedFiles);
                    }
                    if (derivedType is DerivedTypeEnum.UnitPlan)
                    {
                        PostModel.AutoPostModelBase(mb as ModelBase, failedFiles);
                    }
                    if (derivedType is DerivedTypeEnum.None)
                    {
                        PostModel.AutoPostModelBase(mb as ModelBase, failedFiles);
                    }
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

                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            // 发送类型

            // 发送图层数据
        }
    }
}
