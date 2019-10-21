using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadInterface.CadService
{
    public class LocationService
    {
        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="polylineHandle"></param>
        public static void FindPolyline(string polylineHandle)
        {
            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ObjectId newObjectId = TechnologicalProcess.GetObjectId(polylineHandle,10);
                    Entity entity = tr.GetObject(newObjectId, OpenMode.ForWrite) as Entity;
                    var range = entity.GeometricExtents;
                    if (entity == null)
                        return;
                    Autodesk.AutoCAD.Interop.AcadApplication acadApplication = (Autodesk.AutoCAD.Interop.AcadApplication)Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication;
                    //参数要求是双精度的数组
                    if (acadApplication != null)
                    {
                        double[] doubles1 = new double[3] { range.MinPoint.X, range.MinPoint.Y, range.MinPoint.Z };
                        double[] doubles2 = new double[3] { range.MaxPoint.X, range.MaxPoint.Y, range.MaxPoint.Z };
                        acadApplication.ZoomWindow(doubles1, doubles2);
                        entity.Highlight();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("定位：" + e.ToString());
                }
                finally
                {
                    tr.Commit();
                    m_DocumentLock.Dispose();
                }
            }
        }
        /// <summary>
        /// 多实体定位
        /// </summary>
        /// <param name="polylineHandleList"></param>
        public static void FindPolyline(List<string> polylineHandleList)
        {
            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    List<Point3d> maxList = new List<Point3d>();
                    List<Point3d> minList = new List<Point3d>();
                    foreach (var polylineHandle in polylineHandleList)
                    {
                        ObjectId newObjectId = TechnologicalProcess.GetObjectId(polylineHandle,10);
                        Entity entity = tr.GetObject(newObjectId, OpenMode.ForWrite) as Entity;
                        if (entity == null) continue;
                        var range = entity.GeometricExtents;
                        entity.Highlight();
                        maxList.Add(range.MaxPoint);
                        minList.Add(range.MinPoint);
                    }
                    double maxX = GetMaximumValue(maxList, true);
                    double maxY = GetMaximumValue(maxList, false);
                    double minX = GetMinimumBValue(minList, true);
                    double minY = GetMinimumBValue(minList, false);
                    Autodesk.AutoCAD.Interop.AcadApplication acadApplication = (Autodesk.AutoCAD.Interop.AcadApplication)Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication;
                    //参数要求是双精度的数组
                    if (acadApplication != null)
                    {
                        double[] doubles1 = new double[3] { minX, minY, 0 };
                        double[] doubles2 = new double[3] { maxX, maxY, 0 };
                        acadApplication.ZoomWindow(doubles1, doubles2);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("定位：" + e.ToString());
                }
                finally
                {
                    tr.Commit();
                    m_DocumentLock.Dispose();
                }
            }
        }
        /// <summary>
        /// 多实体定位,并移除其他实体已高亮的状态
        /// </summary>
        /// <param name="polylineHandleList"></param>
        public static void FindPolyline(List<string> polylineHandleList, List<string> polylineHandleListOther)
        {
            DocumentLock m_DocumentLock = Application.DocumentManager.MdiActiveDocument.LockDocument();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    List<Point3d> maxList = new List<Point3d>();
                    List<Point3d> minList = new List<Point3d>();

                    foreach (var polylineHandle in polylineHandleListOther)
                    {
                        ObjectId newObjectId = TechnologicalProcess.GetObjectId(polylineHandle, 10);
                        Entity entity = tr.GetObject(newObjectId, OpenMode.ForWrite) as Entity;
                        if (entity == null) continue;
                        entity.Unhighlight();
                    }

                    foreach (var polylineHandle in polylineHandleList)
                    {
                        ObjectId newObjectId = TechnologicalProcess.GetObjectId(polylineHandle, 10);
                        Entity entity = tr.GetObject(newObjectId, OpenMode.ForWrite) as Entity;
                        if (entity == null) continue;
                        var range = entity.GeometricExtents;
                        entity.Highlight();
                        maxList.Add(range.MaxPoint);
                        minList.Add(range.MinPoint);
                    }
                    double maxX = GetMaximumValue(maxList, true);
                    double maxY = GetMaximumValue(maxList, false);
                    double minX = GetMinimumBValue(minList, true);
                    double minY = GetMinimumBValue(minList, false);
                    Autodesk.AutoCAD.Interop.AcadApplication acadApplication = (Autodesk.AutoCAD.Interop.AcadApplication)Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication;
                    //参数要求是双精度的数组
                    if (acadApplication != null)
                    {
                        double[] doubles1 = new double[3] { minX, minY, 0 };
                        double[] doubles2 = new double[3] { maxX, maxY, 0 };
                        acadApplication.ZoomWindow(doubles1, doubles2);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("定位：" + e.ToString());
                }
                finally
                {
                    tr.Commit();
                    m_DocumentLock.Dispose();
                }
            }
        }
        /// <summary>
        /// .net2.0集合取最大x，y
        /// </summary>
        /// <returns></returns>
        public static double GetMaximumValue(List<Point3d> point3Ds, bool isXOrY)
        {
            if (isXOrY)
            {
                double maxX = point3Ds[0].X;
                foreach (Point3d point in point3Ds)
                {
                    if (point.X > maxX)
                        maxX = point.X;
                }
                return maxX;
            }
            else
            {
                double maxY = point3Ds[0].Y;
                foreach (var point in point3Ds)
                {
                    if (point.Y > maxY)
                        maxY = point.Y;
                }
                return maxY;
            }
        }
        /// <summary>
        /// .net2.0集合取最小x，y
        /// </summary>
        /// <returns></returns>
        public static double GetMinimumBValue(List<Point3d> point3Ds, bool isXOrY)
        {
            if (isXOrY)
            {
                double minX = point3Ds[0].X;
                foreach (Point3d point in point3Ds)
                {
                    if (point.X < minX)
                        minX = point.X;
                }
                return minX;
            }
            else
            {
                double minY = point3Ds[0].Y;
                foreach (var point in point3Ds)
                {
                    if (point.Y < minY)
                        minY = point.Y;
                }
                return minY;
            }
        }
    }
}
