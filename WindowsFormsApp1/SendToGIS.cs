using System;
using System.Windows.Forms;

/// <summary>
/// Summary description for Class1
/// </summary>
public class SendToGis
{
	public SendToGis()
	{
        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        Editor ed = doc.Editor;
        Database db = doc.Database;

        // 获取指定图层上的所有实体
        PromptResult pr = ed.GetString("\nEnter name of layer: ");

    }
}
