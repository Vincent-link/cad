using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.IO;

namespace RegulatoryPlan.Method
{
   public static class DrawingMethod
    {
        public static string GetDrawingName()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
           string name= Path.GetFileNameWithoutExtension(doc.Name);
            //Editor ed = doc.Editor;
           // ed.
            return name;
        }
    }
}
