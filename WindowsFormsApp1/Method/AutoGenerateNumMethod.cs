using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.Interop;
using RegulatoryPlan.Command;
using RegulatoryPlan.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Method
{
    class AutoGenerateNumMethod
    {
        private AutoGenerateNumMethod()
        {
        }

        public static AutoGenerateNumMethod Instance
        {
            get { return Nest.instance; }
        }
        class Nest
        {
            internal readonly static AutoGenerateNumMethod instance = new AutoGenerateNumMethod();
        }

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="Label">标记名</param>
        /// <param name="Prompt">提示</param>
        /// <param name="Value">属性值</param>
        /// <param name="pt">属性插入点位置</param>
        /// <returns></returns>
        public static AttributeDefinition AttributeDefinition(string Label, string Prompt, string Value, Point3d pt)
        {
            AttributeDefinition ad = new AttributeDefinition();
            ad.Constant = false;
            ad.Tag = Label;
            ad.Prompt = Prompt;
            ad.TextString = Value;
            ad.Position = pt;
            return ad;
        }

        /// <summary>
        /// 将块表记录加入到块表中
        /// </summary>
        /// <returns></returns>
        public static ObjectId AddToBlockTable(BlockTableRecord Record)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId id = new ObjectId();
            using (Transaction transaction = db.TransactionManager.StartTransaction())
            {
                BlockTable table = transaction.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                id = table.Add(Record);
                transaction.AddNewlyCreatedDBObject(Record, true);
                transaction.Commit();
            }
            return id;
        }

        public static ObjectId ToModelSpace(Entity ent)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId entId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                entId = btr.AppendEntity(ent);
                trans.AddNewlyCreatedDBObject(ent, true);
                trans.Commit();
            }
            return entId;
        }

        internal static String GetPolyline(string num, string factor, string individualName)
        {
            string polylineId = "";
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;

                DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    PromptSelectionResult acSSPrompt = ed.GetSelection();

                    if (acSSPrompt.Status == PromptStatus.OK)
                    {
                        SelectionSet acSSet = acSSPrompt.Value;
                        foreach (SelectedObject acSSObj in acSSet)
                        {
                            if (acSSObj != null)
                            {
                                Entity ent1 = tr.GetObject(acSSObj.ObjectId, OpenMode.ForRead) as Entity;

                                if (ent1 is Polyline)
                                {
                                    //添加扩展记录之前，先创建对象的扩展字典
                                    DBObject obj = tr.GetObject(acSSObj.ObjectId, OpenMode.ForRead);//以读的方式打开

                                    //AddXdata(acSSObj.ObjectId, "num", num);
                                    if (obj.ExtensionDictionary.IsNull)//如果对象无扩展字典，那就给创建
                                    {
                                        obj.UpgradeOpen();//切换对象为写的状态
                                        obj.CreateExtensionDictionary();//为对象创建扩展字典，一个对象只能拥有一个扩展字典
                                        obj.DowngradeOpen();//将对象切换为读的状态
                                    }
                                    //打开对象的扩展字典
                                    DBDictionary dict = obj.ExtensionDictionary.GetObject(OpenMode.ForRead) as DBDictionary;
                                    //如果扩展字典中已包含指定的扩展记录对象  
                                    if (dict.Contains("polylineNumber"))
                                    {
                                        polylineId = acSSObj.ObjectId.Handle.Value.ToString();

                                        m_DocumentLock.Dispose();
                                    }
                                    else //若没有包含扩展记录，则创建一个
                                    {
                                        ResultBuffer valueBuffer = new ResultBuffer();
                                        valueBuffer.Add(new TypedValue(5005, num));
                                        valueBuffer.Add(new TypedValue(5005, factor));
                                        valueBuffer.Add(new TypedValue(5005, individualName));

                                        Xrecord xrec = new Xrecord();//为对象创建一个扩展记录 
                                        xrec.Data = valueBuffer;//指定扩展记录的内容，这里用到了自定义类型转换，TypedValueList-->ResultBuffer

                                        dict.UpgradeOpen();//将扩展字典切换为写的状态，以便添加一个扩展记录
                                        ObjectId xrecId = dict.SetAt("polylineNumber", xrec);//在扩展字典中加入新建的扩展记录，并指定它的搜索关键字
                                        tr.AddNewlyCreatedDBObject(xrec, true);
                                        dict.DowngradeOpen();//将扩展字典切换为读的状态

                                        m_DocumentLock.Dispose();

                                        polylineId = acSSObj.ObjectId.Handle.Value.ToString();
                                        System.Windows.Forms.MessageBox.Show("添加成功");

                                    }

                                }
                            }
                        }
                    }
                    tr.Commit();

                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }

            return polylineId;
        }

        internal static void FindPolyline(string polylineId)
        {
            try
            {
                Autodesk.AutoCAD.Interop.AcadApplication acadApplication = (Autodesk.AutoCAD.Interop.AcadApplication)Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication;
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;

                DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectId newObjectId = db.GetObjectId(false, new Handle(Convert.ToInt64(polylineId)), 0);
                    Entity entity = tr.GetObject(newObjectId, OpenMode.ForWrite) as Entity;
                    var sdfasd = entity.GeometricExtents;
                    if (entity == null)
                    {
                        return;
                    }
                    //根据实体的范围对视图进行缩放
                    entity.TransformBy(ed.CurrentUserCoordinateSystem.Inverse());

                    double[] doubles1 = new double[3] { sdfasd.MinPoint.X, sdfasd.MinPoint.Y, sdfasd.MinPoint.Z };
                    double[] doubles2 = new double[3] { sdfasd.MaxPoint.X, sdfasd.MaxPoint.Y, sdfasd.MaxPoint.Z };
                    //参数要求是双精度的数组
                    if (acadApplication != null)
                        acadApplication.ZoomWindow(doubles1, doubles2);
                    //acadApplication.ZoomWindow(sdfasd.MinPoint, sdfasd.MaxPoint);
                    entity.Highlight();
                    tr.Commit();
                }
                m_DocumentLock.Dispose();

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }

        }

        internal static void DeletePolyline(string polylineId)
        {
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;

                DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectId newObjectId = db.GetObjectId(false, new Handle(Convert.ToInt64(polylineId)), 0);
                    Entity entity = tr.GetObject(newObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null)
                    {
                        return;
                    }

                    ObjectId dictId = entity.ExtensionDictionary;
                    if (dictId.IsNull)
                    {
                        return;//若对象没有扩展字典，则返回
                    }
                    entity.UpgradeOpen();
                    DBDictionary dict = dictId.GetObject(OpenMode.ForWrite) as DBDictionary;
                    dict.Remove("polylineNumber");
                    entity.ReleaseExtensionDictionary();
                    entity.DowngradeOpen();

                    System.Windows.Forms.MessageBox.Show("删除成功");
                    tr.Commit();
                }
                m_DocumentLock.Dispose();

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }

        }

        public static System.Data.DataTable GetAllPolylineNums()
        {
            System.Data.DataTable table = new System.Data.DataTable("编码");
            table.Columns.Add(new System.Data.DataColumn(("多段线id"), typeof(string)));
            table.Columns.Add(new System.Data.DataColumn(("个体编码"), typeof(string)));
            table.Columns.Add(new System.Data.DataColumn(("个体要素"), typeof(string)));
            table.Columns.Add(new System.Data.DataColumn(("个体名称"), typeof(string)));

            System.Data.DataColumn column;
            System.Data.DataRow row;

            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;
                DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId layerId in LayersToList(db))
                    {
                        LayerTableRecord layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;

                        TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layer.Name) };
                        SelectionFilter sfilter = new SelectionFilter(filList);

                        PromptSelectionResult result = ed.SelectAll(sfilter);
                        if (result.Status == PromptStatus.OK)
                        {
                            SelectionSet acSSet = result.Value;
                            foreach (ObjectId id in acSSet.GetObjectIds())
                            {
                                DBObject obj = id.GetObject(OpenMode.ForRead);//以读的方式打开对象

                                ObjectId dictId = obj.ExtensionDictionary;//获取对象的扩展字典的id

                                Entity ent1 = tr.GetObject(id, OpenMode.ForRead) as Entity;

                                if (ent1 is Polyline && !dictId.IsNull)
                                {

                                    DBDictionary dict = dictId.GetObject(OpenMode.ForRead) as DBDictionary;//获取对象的扩展字典
                                    if (!dict.Contains("polylineNumber"))
                                    {
                                        continue;//如果扩展字典中没有包含指定关键 字的扩展记录，则返回null；
                                    }
                                    //先要获取对象的扩展字典或图形中的有名对象字典，然后才能在字典中获取要查询的扩展记录
                                    ObjectId xrecordId = dict.GetAt("polylineNumber");//获取扩展记录对象的id
                                    Xrecord xrecord = xrecordId.GetObject(OpenMode.ForRead) as Xrecord;//根据id获取扩展记录对象
                                    ResultBuffer resBuf = xrecord.Data;

                                    ResultBufferEnumerator rator = resBuf.GetEnumerator();
                                    int i = 0;
                                    row = table.NewRow();
                                    row["多段线id"] = id.Handle.Value.ToString();

                                    while (rator.MoveNext())
                                    {
                                        TypedValue re = rator.Current;
                                        if (i==0)
                                        {
                                            row["个体编码"] = re.Value;
                                        }
                                        if(i == 1)
                                        {
                                            row["个体要素"] = re.Value;
                                        }
                                        if (i == 2)
                                        {
                                            row["个体名称"] = re.Value;
                                         }
                                        i++;
                                    }

                                    table.Rows.Add(row);
                                }
                            }
                        }
                    }

                }


            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
            return table;

        }

        //public static GetPolyInfo(Polyline polyline)
        //{
        //    System.Data.DataTable table = new System.Data.DataTable("编码");
        //    table.Columns.Add(new System.Data.DataColumn(("多段线id"), typeof(string)));
        //    table.Columns.Add(new System.Data.DataColumn(("多段线编码"), typeof(string)));
        //    table.Columns.Add(new System.Data.DataColumn(("个体要素"), typeof(string)));
        //    table.Columns.Add(new System.Data.DataColumn(("个体名称"), typeof(string)));

        //    System.Data.DataColumn column;
        //    System.Data.DataRow row;

        //    try
        //    {
        //        Document doc = Application.DocumentManager.MdiActiveDocument;
        //        Editor ed = doc.Editor;
        //        Database db = doc.Database;
        //        DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();

        //        using (Transaction tr = db.TransactionManager.StartTransaction())
        //        {
        //            foreach (ObjectId layerId in LayersToList(db))
        //            {
        //                LayerTableRecord layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;

        //                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layer.Name) };
        //                SelectionFilter sfilter = new SelectionFilter(filList);

        //                PromptSelectionResult result = ed.SelectAll(sfilter);
        //                if (result.Status == PromptStatus.OK)
        //                {
        //                    SelectionSet acSSet = result.Value;
        //                    foreach (ObjectId id in acSSet.GetObjectIds())
        //                    {
        //                        DBObject obj = id.GetObject(OpenMode.ForRead);//以读的方式打开对象

        //                        ObjectId dictId = obj.ExtensionDictionary;//获取对象的扩展字典的id

        //                        Entity ent1 = tr.GetObject(id, OpenMode.ForRead) as Entity;

        //                        if (ent1 is Polyline && !dictId.IsNull)
        //                        {

        //                            DBDictionary dict = dictId.GetObject(OpenMode.ForRead) as DBDictionary;//获取对象的扩展字典
        //                            if (!dict.Contains("polylineNumber"))
        //                            {
        //                                return null;//如果扩展字典中没有包含指定关键 字的扩展记录，则返回null；
        //                            }
        //                            //先要获取对象的扩展字典或图形中的有名对象字典，然后才能在字典中获取要查询的扩展记录
        //                            ObjectId xrecordId = dict.GetAt("polylineNumber");//获取扩展记录对象的id
        //                            Xrecord xrecord = xrecordId.GetObject(OpenMode.ForRead) as Xrecord;//根据id获取扩展记录对象
        //                            ResultBuffer resBuf = xrecord.Data;

        //                            ResultBufferEnumerator rator = resBuf.GetEnumerator();
        //                            int i = 0;
        //                            row = table.NewRow();
        //                            row["多段线id"] = id.Handle.Value.ToString();

        //                            while (rator.MoveNext())
        //                            {
        //                                TypedValue re = rator.Current;
        //                                if (i == 0)
        //                                {
        //                                    row["多段线编码"] = re.Value;
        //                                }
        //                                if (i == 1)
        //                                {
        //                                    row["个体要素"] = re.Value;
        //                                }
        //                                if (i == 2)
        //                                {
        //                                    row["个体名称"] = re.Value;
        //                                }
        //                                i++;
        //                            }

        //                            table.Rows.Add(row);
        //                        }
        //                    }
        //                }
        //            }

        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        System.Windows.Forms.MessageBox.Show(e.ToString());
        //    }

        //    return table;
        //}



        public static List<ObjectId> LayersToList(Database db)
        {
            List<ObjectId> lstlay = new List<ObjectId>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in lt)
                {
                    try
                    {
                        LayerTableRecord layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;

                        lstlay.Add(layerId);
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }
                tr.Commit();
            }
            return lstlay;
        }


        /// <summary>
                /// 对实体进行写属性
                /// </summary>
                /// <param name="objId">实体id</param>
                /// <param name="appName">外部数据名</param>
                /// <param name="proStr">属性</param>
                /// <returns>true: 成功 false: 失败</returns>
        public static bool AddXdata(ObjectId objId, string appName, string proStr)
        {

            bool retureValue = false;

            try
            {

                using (Database db = HostApplicationServices.WorkingDatabase)
                {
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        RegAppTable rAt = (RegAppTable)trans.GetObject(db.RegAppTableId, OpenMode.ForWrite);
                        RegAppTableRecord rAtr;
                        ObjectId rAtrId = ObjectId.Null;
                        TypedValue tvName = new TypedValue

                        (DxfCode.ExtendedDataRegAppName.GetHashCode(), appName);
                        TypedValue tvPro = new TypedValue
                        (DxfCode.ExtendedDataAsciiString.GetHashCode(), proStr);
                        ResultBuffer rb = new ResultBuffer(tvName, tvPro);

                        if (rAt.Has(appName))
                        {
                            rAtrId = rAt[appName];
                        }
                        else
                        {
                            rAtr = new RegAppTableRecord();
                            rAtr.Name = appName;
                            rAtrId = rAt.Add(rAtr);
                            trans.AddNewlyCreatedDBObject(rAtr, true);
                        }

                        Entity en;
                        en = (Entity)trans.GetObject(objId, OpenMode.ForWrite);
                        en.XData = rb;
                        trans.Commit();
                        retureValue = true;
                    }
                }
            }
            catch
            {
                retureValue = false;
            }
            return retureValue;
        }


        /// <summary>
        ///  添加扩展记录，如果没有扩展字典，那就创建扩展字典
        /// </summary>
        /// <param name="objId">对象的objectid</param>
        /// <param name="xRecordSearchKey">扩展记录名称</param>
        /// <param name="values">扩展记录的内容</param>
        /// <returns></returns>
        public static void AddNumberToPolyline(ObjectId objId, string values)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                //添加扩展记录之前，先创建对象的扩展字典
                DBObject obj = tr.GetObject(objId, OpenMode.ForRead);//以读的方式打开
                if (obj.ExtensionDictionary.IsNull)//如果对象无扩展字典，那就给创建
                {
                    obj.UpgradeOpen();//切换对象为写的状态
                    obj.CreateExtensionDictionary();//为对象创建扩展字典，一个对象只能拥有一个扩展字典
                    obj.DowngradeOpen();//将对象切换为读的状态
                }
                //打开对象的扩展字典
                DBDictionary dict = obj.ExtensionDictionary.GetObject(OpenMode.ForRead) as DBDictionary;
                //如果扩展字典中已包含指定的扩展记录对象  
                if (dict.Contains("polylineNumber"))
                {
                    m_DocumentLock.Dispose();
                }
                else //若没有包含扩展记录，则创建一个
                {
                    ResultBuffer valueBuffer = new ResultBuffer();
                    valueBuffer.Add(new TypedValue(5005, values));
                    Xrecord xrec = new Xrecord();//为对象创建一个扩展记录 
                    xrec.Data = valueBuffer;//指定扩展记录的内容，这里用到了自定义类型转换，TypedValueList-->ResultBuffer

                    dict.UpgradeOpen();//将扩展字典切换为写的状态，以便添加一个扩展记录
                    ObjectId xrecId = dict.SetAt("polylineNumber", xrec);//在扩展字典中加入新建的扩展记录，并指定它的搜索关键字
                    tr.AddNewlyCreatedDBObject(xrec, true);
                    dict.DowngradeOpen();//将扩展字典切换为读的状态

                    m_DocumentLock.Dispose();

                }
                tr.Commit();
            }
        }
    }
}
