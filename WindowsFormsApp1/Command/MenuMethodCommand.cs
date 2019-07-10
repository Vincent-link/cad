using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;
using RegulatoryModel.Model;
using RegulatoryPlan.Method;
using RegulatoryPlan.UI;
using RegulatoryPost.FenTuZe;

[assembly: CommandClass(typeof(RegulatoryPlan.Command.MenuMethodCommand))]
namespace RegulatoryPlan.Command
{
   public class MenuMethodCommand
    {
        [CommandMethod("ShowMainForm")]
        public void ShowMain()
        {
            ChooseCityForm cf = new ChooseCityForm();
            if (cf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CadHelper.Instance.OpenFile(cf.openFile,cf.openCity,cf.derivedType);
               
            }
            //MainForm form = new MainForm();
            //form.ShowDialog();
        }

        [CommandMethod("AutoShowMainForm")]
        public void AutoShowMain()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//等于true表示可以选择多个文件

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string item in dialog.FileNames)
                {
                    if (Path.GetExtension(item).ToLower() == ".dwg")

                    {CadHelper.Instance.AutoOpenFile(item, "成安县", DerivedTypeEnum.None); }
                }
            }



            //MainForm form = new MainForm();
            //form.ShowDialog();
        }
        [CommandMethod("AutoDeleteLayer", CommandFlags.Session)]
        public void AutoDeleteLayer()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//等于true表示可以选择多个文件

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in dialog.FileNames)
                {
                    if (Path.GetExtension(item).ToLower() == ".dwg")
                    { AutoDeleteLayers.AutoDeleteLayer(item); }

                    //{ MethodCommand.AutoOpenFile(item, "成安县", DerivedTypeEnum.None); }

                }
            }
        }

        [CommandMethod("SendPointPlans", CommandFlags.Session)]
        public void SendPointPlans()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//等于true表示可以选择多个文件

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in dialog.FileNames)
                {
                    if (Path.GetExtension(item).ToLower() == ".dwg")
                    { AutoSendFiles.Instance.AutoOpenPointPlanFile(item, DerivedTypeEnum.PointsPlan); }
                }
            }
        }

        [CommandMethod("SendUnitPlans", CommandFlags.Session)]
        public void SendUnitPlans()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//等于true表示可以选择多个文件

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in dialog.FileNames)
                {
                    if (Path.GetExtension(item).ToLower() == ".dwg")
                    { AutoSendFiles.Instance.AutoOpenPointPlanFile(item, DerivedTypeEnum.UnitPlan); }
                }
            }
        }

        [CommandMethod("测试发送")]
        public void SendTestInfo()
        {

        }

        // 手动选择实体
        [CommandMethod("手动选择")]
        static public void fen()
        {
            FenTuZeMethod r = new FenTuZeMethod();
            r.ManualSelect();
        }

        // 按图层读取
        [CommandMethod("图层选择")]
        static public void Layer()
        {
            FenTuZeMethod r = new FenTuZeMethod();
            r.LayerSelect();
        }


    }
}
