﻿using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
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
                MethodCommand.OpenFile(cf.openFile,cf.openCity,cf.derivedType);
               
            }
            //MainForm form = new MainForm();
            //form.ShowDialog();
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