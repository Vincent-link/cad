using Autodesk.AutoCAD.Runtime;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace EntitySelection

{

    public class Commands

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

    }

}