using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Method
{
    public static class SaveProjectIdToXData
    {
        public static string GetDefinedProject()
        {
            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

            ObjectId LayerObjectId = GetLayer0();
            ResultBuffer resBuf = ReadX(LayerObjectId, "Layer0");
            string city = "";
            if (resBuf != null)
            {
                foreach (TypedValue res in resBuf)
                {
                    city = res.Value.ToString();
                }
            }
            m_DocumentLock.Dispose();

            return city;
        }

        public static ResultBuffer ReadX(ObjectId objId, string xRecordSearchKey)
        {
            ResultBuffer resBuf = new ResultBuffer();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBObject obj = objId.GetObject(OpenMode.ForRead);//以读的方式打开对象

                ObjectId dictId = obj.ExtensionDictionary;//获取对象的扩展字典的id

                if (!dictId.IsNull)
                {
                    DBDictionary dict = dictId.GetObject(OpenMode.ForRead) as DBDictionary;//获取对象的扩展字典
                    if (!dict.Contains(xRecordSearchKey))
                    {
                        tr.Commit();
                        m_DocumentLock.Dispose();
                        return null;//如果扩展字典中没有包含指定关键 字的扩展记录，则返回null；
                    }
                    //先要获取对象的扩展字典或图形中的有名对象字典，然后才能在字典中获取要查询的扩展记录
                    ObjectId xrecordId = dict.GetAt(xRecordSearchKey);//获取扩展记录对象的id
                    Xrecord xrecord = xrecordId.GetObject(OpenMode.ForRead) as Xrecord;//根据id获取扩展记录对象
                    resBuf = xrecord.Data;
                }
                tr.Commit();
            }
            m_DocumentLock.Dispose();
            return resBuf;
        }

        public static void SaveSelectedProjectIdToXData(string city)
        {
            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();
            ResultBuffer result = new ResultBuffer();
            result.Add(new TypedValue((int)DxfCode.Text, city));

            ObjectId LayerObjectId = GetLayer0();
            DelObjXrecord(LayerObjectId, "Layer0");
            AddXRecordToObj(LayerObjectId, "Layer0", result);
            m_DocumentLock.Dispose();

        }

        /// <summary>
        ///  添加扩展记录，如果没有扩展字典，那就创建扩展字典
        /// </summary>
        /// <param name="objId">对象的objectid</param>
        /// <param name="xRecordSearchKey">扩展记录名称</param>
        /// <param name="values">扩展记录的内容</param>
        /// <returns></returns>
        public static void AddXRecordToObj(ObjectId objId, string xRecordSearchKey, ResultBuffer values, Transaction mytransaction = null)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            if (mytransaction == null)
            {
                DocumentLock m_DocumentLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    //添加扩展记录之前，先创建对象的扩展字典
                    DBObject obj = objId.GetObject(OpenMode.ForRead);//以读的方式打开
                    if (obj.ExtensionDictionary.IsNull)//如果对象无扩展字典，那就给创建
                    {
                        obj.UpgradeOpen();//切换对象为写的状态
                        obj.CreateExtensionDictionary();//为对象创建扩展字典，一个对象只能拥有一个扩展字典
                        obj.DowngradeOpen();//将对象切换为读的状态
                    }
                    //打开对象的扩展字典
                    DBDictionary dict = obj.ExtensionDictionary.GetObject(OpenMode.ForRead) as DBDictionary;
                    //如果扩展字典中已包含指定的扩展记录对象  
                    if (dict.Contains("Layer0"))
                    {
                        m_DocumentLock.Dispose();
                    }
                    else //若没有包含扩展记录，则创建一个
                    {
                        Xrecord xrec = new Xrecord();//为对象创建一个扩展记录 
                        xrec.Data = values;//指定扩展记录的内容，这里用到了自定义类型转换，TypedValueList-->ResultBuffer
                        dict.UpgradeOpen();//将扩展字典切换为写的状态，以便添加一个扩展记录
                        ObjectId xrecId = dict.SetAt(xRecordSearchKey, xrec);//在扩展字典中加入新建的扩展记录，并指定它的搜索关键字
                        //objId.Database.TransactionManager.AddNewlyCreatedDBObject(xrec, true);
                        tr.AddNewlyCreatedDBObject(xrec, true);
                        dict.DowngradeOpen();//将扩展字典切换为读的状态

                        m_DocumentLock.Dispose();
                    }
                    tr.Commit();
                }
            }

        }

        public static bool DelObjXrecord(ObjectId objId, string xRecordSearchKey, Transaction transaction = null)
        {
            if (transaction == null)
            {
                DocumentLock m_DocumentLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBObject obj = objId.GetObject(OpenMode.ForRead);//以读的方式打开对象
                    ObjectId dictId = obj.ExtensionDictionary;//获取对象的扩展字典id
                    if (dictId.IsNull)
                    {
                        return false;//若对象没有扩展字典，则返回 
                    }
                    //如果对象有扩展字典，则以读的方式打开
                    DBDictionary dict = dictId.GetObject(OpenMode.ForRead) as DBDictionary;
                    if (dict.Contains(xRecordSearchKey))//如果扩展字典中包含指定关键字的扩展记录，则删除；
                    {
                        dict.UpgradeOpen();//切换为写的状态
                        dict.Remove(xRecordSearchKey);//删除扩展记录
                        dict.DowngradeOpen();//切换为读的状态
                    }
                    tr.Commit();
                }

                m_DocumentLock.Dispose();
            }

            return true;
        }

        public static ObjectId GetLayer0()
        {
            ObjectId id = new ObjectId();
            List<LayerTableRecord> layerList = GetLayerName();
            foreach (LayerTableRecord layer in layerList)
            {
                if (layer.Name == "0")
                {
                    id = layer.ObjectId;
                    break;
                }
            }
            return id;
        }

        /// <summary>
        /// 获取所有图层
        /// </summary>
        /// <returns></returns>
        public static List<LayerTableRecord> GetLayerName(Transaction transaction = null)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            List<LayerTableRecord> list = new List<LayerTableRecord>();
            if (transaction == null)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                    foreach (ObjectId layerId in lt)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(layerId, OpenMode.ForRead);
                        //ed.WriteMessage(ltr.Name);
                        list.Add(ltr);
                    }
                    trans.Commit();
                }
            }
            else
            {
                LayerTable lt = (LayerTable)transaction.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId layerId in lt)
                {
                    LayerTableRecord ltr = (LayerTableRecord)transaction.GetObject(layerId, OpenMode.ForRead);
                    list.Add(ltr);
                }
            }
            return list;
        }

    }
}
