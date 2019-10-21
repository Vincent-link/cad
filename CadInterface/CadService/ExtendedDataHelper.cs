using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadInterface.CadService
{
    public class ExtendedDataHelper
    {
        /// <summary>
        /// 获取对象的扩展字典中的扩展记录
        /// </summary>
        /// <param name="objId">对象的id</param>
        /// <param name="xRecordSearchKey">扩展记录名称</param>
        /// <returns></returns>
        public static ResultBuffer GetObjXrecord(ObjectId objId, string xRecordSearchKey)
        {
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                DocumentLock m_DocumentLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBObject obj = objId.GetObject(OpenMode.ForRead);//以读的方式打开对象
                    ObjectId dictId = obj.ExtensionDictionary;//获取对象的扩展字典的id
                    if (dictId.IsNull)
                    {
                        tr.Commit();
                        m_DocumentLock.Dispose();
                        return null;//若对象没有扩展字典，则返回null
                    }
                    DBDictionary dict = dictId.GetObject(OpenMode.ForRead) as DBDictionary;//获取对象的扩展字典
                    if (!dict.Contains(xRecordSearchKey))
                    {
                        tr.Commit();
                        m_DocumentLock.Dispose();
                        return null;//如果扩展字典中没有包含指定关键字的扩展记录，则返回null；
                    }
                    //先要获取对象的扩展字典或图形中的有名对象字典，然后才能在字典中获取要查询的扩展记录
                    ObjectId xrecordId = dict.GetAt(xRecordSearchKey);//获取扩展记录对象的id
                    Xrecord xrecord = xrecordId.GetObject(OpenMode.ForRead) as Xrecord;//根据id获取扩展记录对象
                    ResultBuffer values = xrecord.Data;
                    tr.Commit();
                    m_DocumentLock.Dispose();
                    return values;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("扩展属性读" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 用于替换扩展字典中的整个一条扩展记录
        /// </summary>
        /// <param name="objId">对象id</param>
        /// <param name="xRecordSearchKey">扩展记录的名称</param>
        /// <param name="values">扩展记录的内容</param>
        public static void ModObjXrecord(ObjectId objId, string xRecordSearchKey, ResultBuffer values)
        {
            
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;
                DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                try
                {
                    DBObject obj = tr.GetObject(objId, OpenMode.ForRead);//以读的方式打开
                    if (obj.ExtensionDictionary.IsNull)//如果对象无扩展字典，那就给创建
                    {
                        obj.UpgradeOpen();//切换对象为写的状态
                        obj.CreateExtensionDictionary();//为对象创建扩展字典，一个对象只能拥有一个扩展字典
                        obj.DowngradeOpen();//将对象切换为读的状态
                    }
                    DBDictionary dict = obj.ExtensionDictionary.GetObject(OpenMode.ForRead) as DBDictionary;
                    if (!dict.Contains(xRecordSearchKey))
                    {
                        Xrecord xrec = new Xrecord();//为对象创建一个扩展记录 
                        xrec.Data = values;//指定扩展记录的内容，这里用到了自定义类型转换，TypedValueList-->ResultBuffer
                        dict.UpgradeOpen();//将扩展字典切换为写的状态，以便添加一个扩展记录
                        ObjectId xrecId = dict.SetAt(xRecordSearchKey, xrec);//在扩展字典中加入新建的扩展记录，并指定它的搜索关键字
                        objId.Database.TransactionManager.AddNewlyCreatedDBObject(xrec, true);
                        dict.DowngradeOpen();//将扩展字典切换为读的状态
                    }
                    else
                    {
                        ObjectId xrecordId = dict.GetAt(xRecordSearchKey);//获取扩展记录的id
                        Xrecord xrecord = xrecordId.GetObject(OpenMode.ForWrite) as Xrecord;
                        xrecord.Data = values;//覆盖原来的数据，因为values有了新的指向
                        xrecord.DowngradeOpen();//将扩展记录切换为读的状态
                    }
                    
                }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("扩展属性存：" + ex.Message);
            }
            finally
            {
                tr.Commit();
                m_DocumentLock.Dispose();
            }
        }
           
        }


        /// <summary>
        ///用于删除对象扩展字典中的指定的扩展记录
        /// </summary>
        /// <param name="objId">对象id</param>
        /// <param name="xRecordSearchKey"> 扩展记录名称</param>
        public static bool DelObjXrecord(ObjectId objId, string xRecordSearchKey)
        {
            try
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
                        tr.Commit();
                        m_DocumentLock.Dispose();
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
                return true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("扩展属性删：" + ex.Message);
                return false;
            }
        }
    }
}
