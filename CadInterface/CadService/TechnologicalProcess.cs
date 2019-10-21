using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadInterface.CadService
{
    public class TechnologicalProcess
    {
        /// <summary>
        /// 获取DWG的ID
        /// </summary>
        /// <returns></returns>
        public static ObjectId GetDWGId()
        {
            Database ZcDB = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            ObjectId drawingId = SymbolUtilityServices.GetBlockModelSpaceId(ZcDB);
            return drawingId;
        }
        /// <summary>
        /// 选择实体
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static Entity GetEntity(string word)
        {
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    PromptEntityResult ent = doc.Editor.GetEntity(word);
                    if (ent.Status == PromptStatus.OK)
                    {
                        Entity entity = tr.GetObject(ent.ObjectId, OpenMode.ForWrite) as Entity;
                        tr.Commit();
                        return entity;
                    }
                    else
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// handle字符串转id
        /// </summary>
        /// <param name="handleString"></param>
        /// <returns></returns>
        public static ObjectId GetObjectId(string handleString,int fromBase=16)
        {
            try
            {
                Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
                Database db = editor.Document.Database;
                long handleInt = Convert.ToInt64(handleString, fromBase);
                Handle handle = new Handle(handleInt);
                return db.GetObjectId(false, handle, 0);
            }
            catch (Exception) { return new ObjectId(); }
        }
        /// <summary>
        /// 获取多段线的所有端点
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static List<Point3d> GetPolylinePoint(Polyline polyline)
        {
            try
            {
                List<Point3d> list = new List<Point3d>();
                int pcount = polyline.NumberOfVertices;
                for (int i = 0; i < pcount; i++)
                {
                    Point3d point3d = polyline.GetPoint3dAt(i);
                    list.Add(point3d);
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取多段线的直线集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isClosed"></param>
        /// <returns></returns>
        public static List<List<Point3d>> GetPolylineLine(List<Point3d> list, bool isClosed)
        {
            List<List<Point3d>> lineLine = new List<List<Point3d>>();
            if (list.Count == 2)
            {
                List<Point3d> point3Ds = new List<Point3d>();
                point3Ds.Add(list[0]);
                point3Ds.Add(list[1]);
                lineLine.Add(point3Ds);
                return lineLine;
            }
            else
            {
                for (int i = 0; i < list.Count + 1; i++)
                {
                    if (i != 0)
                    {
                        List<Point3d> point3Ds = new List<Point3d>();
                        if (isClosed)
                        {
                            point3Ds.Add(list[i - 1]);
                            if (i == list.Count)
                                point3Ds.Add(list[0]);
                            else
                                point3Ds.Add(list[i]);
                            lineLine.Add(point3Ds);
                        }
                        else
                        {
                            if (i != list.Count)
                            {
                                point3Ds.Add(list[i - 1]);
                                point3Ds.Add(list[i]);
                                lineLine.Add(point3Ds);
                            }
                        }
                    }
                }
            }
            return lineLine;
        }
        /// <summary>
        /// 取消Cad操作
        /// </summary>
        public static void CancelOperation()
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            acDoc.SendStringToExecute("(Command 0x1e)\n", true, false, true);
        }
        /// <summary>
        /// 获取全部实体
        /// </summary>
        /// <returns></returns>
        public static List<ObjectId> GetEntitiesInModelSpace()
        {
            List<ObjectId> objects = new List<ObjectId>();
            using (Transaction transaction = Application.DocumentManager.
                MdiActiveDocument.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead);
                BlockTableRecord blockTableRecord = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                foreach (ObjectId objId in blockTableRecord)
                    objects.Add(objId);
                transaction.Commit();
            }
            return objects;
        }
        /// <summary>
        /// 获取图层中的实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectIdCollection GetObjectIdsAtLayer(string name)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            PromptSelectionResult ProSset = null;
            //LayerName (int)DxfCode.LayerName
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, name) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ProSset = ed.SelectAll(sfilter);
            if (ProSset.Status == PromptStatus.OK)
            {
                SelectionSet sst = ProSset.Value;
                ObjectId[] oids = sst.GetObjectIds();
                for (int i = 0; i < oids.Length; i++)
                {
                    ids.Add(oids[i]);
                }
            }
            return ids;
        }
        /// <summary>
        /// 获取制定块的对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectIdCollection GetObjectIdsAtBlock(string name)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.BlockName, name) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ProSset = ed.SelectAll(sfilter);
            if (ProSset.Status == PromptStatus.OK)
            {
                SelectionSet sst = ProSset.Value;
                ObjectId[] oids = sst.GetObjectIds();
                for (int i = 0; i < oids.Length; i++)
                {
                    ids.Add(oids[i]);
                }
            }
            return ids;
        }
        /// <summary>
        /// 实体变换图层
        /// </summary>
        /// <param name="id"></param>
        /// <param name="layerId"></param>
        public static void SetLayer(ObjectId id, ObjectId layerId)
        {
            DocumentLock m_DocumentLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
            Database db = HostApplicationServices.WorkingDatabase;
            Editor acDocEd = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity entity = (Entity)trans.GetObject(id, OpenMode.ForWrite, true);
                if (layerId != ObjectId.Null)
                    entity.SetLayerId(layerId, false);
                trans.Commit();
            }
            acDocEd.UpdateScreen();
            m_DocumentLock.Dispose();
        }
        /// <summary>
        /// 获取所有图层
        /// </summary>
        /// <returns></returns>
        public static List<LayerTableRecord> GetLayerName()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            List<LayerTableRecord> list = new List<LayerTableRecord>();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId layerId in lt)
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(layerId, OpenMode.ForRead);
                    list.Add(ltr);
                }
                trans.Commit();
            }
            return list;
        }
        /// <summary>
        /// 获取图层ID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static ObjectId GetLayerID(string name, Transaction transaction = null)
        {
            ObjectId id = new ObjectId();
            List<LayerTableRecord> layerList = GetLayerName();
            foreach (LayerTableRecord layer in layerList)
            {
                if (layer.Name == name)
                {
                    id = layer.ObjectId;
                    break;
                }
            }
            return id;
        }
    }
}
