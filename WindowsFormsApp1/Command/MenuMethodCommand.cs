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
            ChooseCityForm cf = new ChooseCityForm();
            if (cf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MethodCommand.OpenFile(cf.openFile,cf.openCity,cf.derivedType);
               
            }
            //MainForm form = new MainForm();
            //form.ShowDialog();
        }

    }
}
