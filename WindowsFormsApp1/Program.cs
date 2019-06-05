using Autodesk.AutoCAD.Runtime;
using System.Windows.Forms;
using WindowsFormsApp1;
using RegulatoryPlan.Menu;
[assembly: ExtensionApplication(typeof(RegulatoryPlan.Commands))]
namespace RegulatoryPlan

{
   
    public class Commands:IExtensionApplication

    {
        
        [CommandMethod("send")]

        static public void main()

        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());

        }

        [CommandMethod("分图则")]

        static public void fen()

        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());

        }

        [CommandMethod("read")]

        static public void read()

        {
            System.Windows.Forms.Application.Run(new Form2());
        }

        //在程序集初始化时
        public void Initialize()
        {
            //在程序集被初始化时，执行创建菜单操作，再配合注册表设置可以实现菜单的自动加载
            TitelMenuCommand main = new TitelMenuCommand();
            main.ShowMyMenu();
        }
        //在程序集被卸载时（也可以理解为CAD关闭时）
        public void Terminate()
        {

        }

    }

}