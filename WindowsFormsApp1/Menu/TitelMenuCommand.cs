

using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;

using System.Text;

[assembly: CommandClass(typeof(RegulatoryPlan.Menu.TitelMenuCommand))]
namespace RegulatoryPlan.Menu
{
   
    public class TitelMenuCommand 
    {
        [CommandMethod("CAKGM")]
        public void ShowMyMenu()
        {  //获取CAD应用程序
            AcadApplication app = (AcadApplication)Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication;
            for (int i = 0; i < app.MenuGroups.Item(0).Menus.Count; i++)
            {
                if (app.MenuGroups.Item(0).Menus.Item(i).Name == "成安控规")  //判断菜单是否已存在，如果存在则不再创建
                    return;
            }
            

           AcadPopupMenu pmParnet = app.MenuGroups.Item(0).Menus.Add("成安控规");  //添加根菜单
            

            ////多级
            //AcadPopupMenu pm = pmParnet.AddSubMenu(pmParnet.Count + 1, "打开");
            //AcadPopupMenuItem pmi0 = pm.AddMenuItem(pm.Count + 1, "文件  ", "OPEN1\n"); 
            //第一个参数是在菜单项中的位置（第几项），第二个参数是显示的名称，第三个参数是点击之后执行的命令
            //AcadPopupMenuItem pmi1 = pm.AddMenuItem(pm.Count + 1, "模版  ", "OPEN2\n");

            //单级
            AcadPopupMenuItem pmi2 = pmParnet.AddMenuItem(pmParnet.Count + 1, "数据导出 ", "ShowMainForm\n");
            //单级
            AcadPopupMenuItem pmi3 = pmParnet.AddMenuItem(pmParnet.Count + 1, "一般数据检测 ", "AutoShowMainForm\n");
            //单级
            AcadPopupMenuItem pmi4 = pmParnet.AddMenuItem(pmParnet.Count + 1, "自动删除多余图层 ", "AutoDeleteLayer\n");
            //单级
            AcadPopupMenuItem pmi5 = pmParnet.AddMenuItem(pmParnet.Count + 1, "分图则批量发送", "SendPointPlans\n");
            //单级
            AcadPopupMenuItem pmi6 = pmParnet.AddMenuItem(pmParnet.Count + 1, "单元图则批量发送", "SendUnitPlans\n");
            //将创建的菜单加入到CAD的菜单中
            pmParnet.InsertInMenuBar(app.MenuBar.Count + 1);
        }
    }
}
