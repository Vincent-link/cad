using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using RegulatoryPlan.UI;
[assembly: CommandClass(typeof(RegulatoryPlan.Command.MenuMethodCommand))]
namespace RegulatoryPlan.Command
{
   public class MenuMethodCommand
    {
        [CommandMethod("ShowMainForm")]
        public void ShowMain()
        {
            MainForm form = new MainForm();
            form.ShowDialog();
        }

    }
}
