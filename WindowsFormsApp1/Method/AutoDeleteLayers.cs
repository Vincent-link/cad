using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryPlan.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Method
{
    class AutoDeleteLayers
    {
        private AutoDeleteLayers()
        {

        }

        public static AutoDeleteLayers Instance
        {
            get { return Nest.instance; }
            
        }
        class Nest
        {
            internal readonly static AutoDeleteLayers instance = new AutoDeleteLayers();
        }

        internal void AutoDeleteLayer(string file)
        {
            try
            {
                // 获取当前文档和数据库  
                Document acDoc = Application.DocumentManager.Open(file, false);
                Database acCurDb = acDoc.Database;
                Editor ed = acDoc.Editor;
                List<ObjectId> layers = new List<ObjectId>();
                LayerTableRecord layer;

                using (DocumentLock acLckDoc = acDoc.LockDocument())
                {
                    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    {

                        foreach (ObjectId layerId in MethodCommand.LayersToList(acCurDb))
                        {
                            try
                            {
                                layer = acTrans.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;

                                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layer.Name) };
                                SelectionFilter sfilter = new SelectionFilter(filList);

                                PromptSelectionResult result = acDoc.Editor.SelectAll(sfilter);
                                if (result.Status == PromptStatus.OK)
                                {
                                    SelectionSet acSSet = result.Value;
                                    foreach (ObjectId id in acSSet.GetObjectIds())
                                    {
                                        try
                                        {
                                            Entity hatchobj = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                                            hatchobj.Erase(true);//删除  

                                        }
                                        catch (Exception e)
                                        {
                                            System.Windows.Forms.MessageBox.Show(e.Message);
                                        }
                                    }

                                }
                                // 删除图层
                                if (layer.Name != "0")
                                {
                                    layer.Erase();
                                }
                            }
                            catch (Exception e)
                            {
                                System.Windows.Forms.MessageBox.Show(e.Message);
                            }
                        }

                        acTrans.Commit();

                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }
    }
}
