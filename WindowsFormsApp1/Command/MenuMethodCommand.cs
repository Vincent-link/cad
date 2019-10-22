using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;
using RegulatoryModel.Model;
using RegulatoryPlan.Method;
using RegulatoryPlan.Models;
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

        [CommandMethod("SendPointPlans", CommandFlags.Session)]
        public void SendPointPlans()
        {
            BatchChooseCityForm cf = new BatchChooseCityForm();
            if (cf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 List<string> failedFiles = new List<string>();

                foreach (string item in cf.openFile)
                {
                    if (Path.GetExtension(item).ToLower() == ".dwg")
                    { AutoSendFiles.Instance.AutoOpenPointPlanFile(item, cf.openCity, cf.derivedType, failedFiles); }
                }
                if (failedFiles.Count > 0)
                {
                    BatchFailAlert f = new BatchFailAlert(failedFiles);
                    f.Show();
                }
                else
                {
                    RegulatoryPost.Fentuze.SuccessAlert f = new RegulatoryPost.Fentuze.SuccessAlert("图纸");
                    f.Show();
                }
            }
        }

        //[CommandMethod("SendUnitPlans", CommandFlags.Session)]
        //public void SendUnitPlans()
        //{
        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.Multiselect = true;//等于true表示可以选择多个文件

        //    if (dialog.ShowDialog() == DialogResult.OK)
        //    {
        //        List<string> failedFiles = new List<string>();

        //        foreach (string item in dialog.FileNames)
        //        {
        //            if (Path.GetExtension(item).ToLower() == ".dwg")
        //            { AutoSendFiles.Instance.AutoOpenPointPlanFile(item, DerivedTypeEnum.UnitPlan, failedFiles); }
        //        }
        //    }
        //}

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
                    { AutoDeleteLayers.Instance.AutoDeleteLayer(item); }

                    //{ MethodCommand.AutoOpenFile(item, "成安县", DerivedTypeEnum.None); }

                }
            }
        }

        [CommandMethod("ProjectDefine", CommandFlags.Session)]
        public void ProjectDefine()
        {
            ProjectDefine a = new ProjectDefine();
            a.Show();
        }

        [CommandMethod("AutoGenerateNumber", CommandFlags.Session)]
        public void AutoGenerateNumber()
        {
            //DataGridView polylineList = new DataGridView();
            //polylineList.TabIndex = 0;

            if (alertInput == null||alertInput.IsDisposed)
            {
                FactorJsonData contentList = new FactorJsonData();

                string cityId = Method.SaveProjectIdToXData.GetDefinedProject();

                try
                {
                    string projectIdBaseAddress = "http://172.18.84.114:8080/PDD/pdd/cim-interface!findElementByProjectId?projectId=" + cityId;
                    var projectIdHttp = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri(projectIdBaseAddress));

                    var response = projectIdHttp.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new System.IO.StreamReader(stream, Encoding.UTF8);
                    var content = sr.ReadToEnd();
                    contentList = Newtonsoft.Json.JsonConvert.DeserializeObject<FactorJsonData>(content);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                if (contentList.result.Count <= 0)
                {
                    MessageBox.Show("请先定义项目");
                    return;
                }

                System.Data.DataTable table = Method.AutoGenerateNumMethod.GetAllPolylineNums();

                alertInput = new AlertInput(table, contentList);
                Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(alertInput);
            }
        }

        private static AlertInput alertInput = null;

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
