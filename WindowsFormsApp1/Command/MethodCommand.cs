using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Method;
using RegulatoryPlan.Model;
using RegulatoryPlan.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryPlan.Command
{
    public static class MethodCommand
    {
        public static string LegendLayer = "图例";

     public   static string fileName = "";
    public    static string cityName = "";
        static DerivedTypeEnum dp = DerivedTypeEnum.None;
        public static bool FindDBTextIsInPolyine(MText txt, List<Polyline> lineList)
        {
            bool res = false;
            foreach (Polyline line in lineList)
            {
                int vtnum = line.NumberOfVertices;
                double minX = double.NaN;
                double maxX = double.NaN;
                double maxY = double.NaN;
                double minY = double.NaN;
                double thres = double.NaN;
                for (int i = 0; i < vtnum; i++)
                {
                    Point3d p1 = line.GetPoint3dAt(i);
                    if (p1.X > maxX || double.IsNaN(maxX))
                    {
                        maxX = p1.X;
                    }
                    if (p1.Y < minY || double.IsNaN(minY))
                    {
                        minY = p1.Y;
                    }
                    if (p1.Y > maxY || double.IsNaN(maxY))
                    {
                        maxY = p1.Y;
                    }
                    if (p1.X < minX || double.IsNaN(minX))
                    {
                        minX = p1.X;
                    }
                }

                thres = maxX - minX;
                if (txt.Location.X < thres + maxX && txt.Location.Y > minY && txt.Location.Y < maxY)
                {
                    return true;
                }
            }
            return res;
        }
        /// <summary>
        /// 管线文本查询
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="lineList"></param>
        /// <returns></returns>
        public static MText FindMTextIsInPolyineForPipe(Polyline line, List<MText> txtList)
        {
            double min = 20;
            MText resText = null;

            double mToPtMin = double.NaN;
            int num = line.NumberOfVertices;
            List<Point2d> point = new List<Point2d>();
            for (int i = 0; i < num; i++)
            {
                point.Add(line.GetPoint2dAt(i));


            }
            double mToPt = double.NaN;
            foreach (MText txt in txtList)
            {
                Point2d mPt = txt.Location.Convert2d(new Plane());

                for (int i = 0; i < point.Count - 1; i++)
                {

                    Point2d st = point[i];
                    Point2d et = point[i + 1];
                    double lLen = MethodCommand.DistancePointToSegment(mPt, st, et);
                    if (double.IsNaN(mToPt) || lLen < mToPt)
                    {
                        mToPt = lLen;
                    }
                }


                if (double.IsNaN(mToPtMin) || (mToPtMin > mToPt))
                {

                    mToPtMin = mToPt;
                    resText = txt;
                }

            }

            return resText;
        }

        public static bool FindDBTextIsInPolyine(MText txt, List<LengedModel> lineList)
        {
            bool res = false;

            foreach (LengedModel line in lineList)
            {

                double minX = double.NaN;
                double maxX = double.NaN;
                double maxY = double.NaN;
                double minY = double.NaN;
                double thres = double.NaN;
                for (int i = 0; i < line.BoxPointList.Count; i++)
                {
                    PointF p1 = line.BoxPointList[i];
                    if (p1.X > maxX || double.IsNaN(maxX))
                    {
                        maxX = p1.X;
                    }
                    if (p1.Y < minY || double.IsNaN(minY))
                    {
                        minY = p1.Y;
                    }
                    if (p1.Y > maxY || double.IsNaN(maxY))
                    {
                        maxY = p1.Y;
                    }
                    if (p1.X < minX || double.IsNaN(minX))
                    {
                        minX = p1.X;
                    }
                }

                thres = maxX - minX;
                if (txt.Location.X < thres + maxX && txt.Location.Y > minY && txt.Location.Y < maxY)
                {
                    line.LayerName = txt.Text;
                    return true;
                }
            }
            return res;
        }

        internal static void OpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            fileName = file;
            cityName = city;
            dp = derivedType;
            num = 0;
            Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
            Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
            Application.DocumentManager.DocumentActivated -= docChange;
        }

        public static void OpenFile(string file, string city)
        {
            fileName = file;
            cityName = city;
            num = 0;
            Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
            Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
            Application.DocumentManager.DocumentActivated -= docChange;
        }
        public static int num = 0;
        /// <summary>
        /// 文档发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void docChange(object sender, DocumentCollectionEventArgs e)
        {
            if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
            {
                num++;
                MainForm mf = new MainForm(cityName, dp);
                mf.Show();
            }
        }
        /// <summary>
        /// 查询面积最大的多段线,key=0 图例框，key=1 其他多段线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static Dictionary<int, List<Polyline>> FindMaxAreaPoline(List<Polyline> polylines)
        {
            Dictionary<int, List<Polyline>> allPolines = new Dictionary<int, List<Polyline>>();
            allPolines.Add(0, new List<Polyline>());
            allPolines.Add(1, new List<Polyline>());
            double maxArea = double.NaN;
            foreach (Polyline line in polylines)
            {
                if (double.IsNaN(maxArea) || line.Area > maxArea)
                {
                    maxArea = line.Area;
                }
            }

            foreach (Polyline line in polylines)
            {
                if (line.Area.ToString("F0") == maxArea.ToString("F0"))
                {
                    allPolines[0].Add(line);
                }
                else
                {
                    allPolines[1].Add(line);
                }
            }
            return allPolines;
        }


        /// <summary>
        /// 根据图层名获取图层颜色
        /// </summary>
        /// <returns></returns>
        public static string GetLayerColorByName(string layerName)
        {
            string colorStr = "";
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction Trans = db.TransactionManager.StartTransaction())
            {
                //以读模式打开图层表
                LayerTable layerTable;
                layerTable = Trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                string sLayerName = "test1";
                if (layerTable.Has(layerName))
                {
                    LayerTableRecord ltr = Trans.GetObject(layerTable[layerName], OpenMode.ForNotify) as LayerTableRecord;
                   
                    colorStr = System.Drawing.ColorTranslator.ToHtml( ltr.Color.ColorValue);

                }

                return colorStr;
            }
        }

    
        /// <summary>
        /// 根据图层名获取图层颜色
        /// </summary>
        /// <returns></returns>
        public static string GetLayerColorByID(ObjectId layerId)
        {
            string colorStr = "";
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction Trans = db.TransactionManager.StartTransaction())
            {

                LayerTableRecord ltr = Trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

                if (ltr != null)
                {

                    colorStr = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);

                }
            }
            return colorStr;
        }
        /// <summary>
        /// 点到线段的距离公式(利用平行四边形的面积算法，非常牛叉)
        /// </summary>
        /// <param name="P">目标点</param>
        /// <param name="A">线段端点A</param>
        /// <param name="B">线段端点B</param>
        /// <returns></returns>
        public static double DistancePointToSegment(Point2d P, Point2d A, Point2d B)
        {
            double atp = A.GetDistanceTo(P);
            double btp = B.GetDistanceTo(P);
            double atb = A.GetDistanceTo(B);

            if (atp * atp >= atb * atb + btp * btp)
            {
                return atp;
            }
            if (btp * btp >= atb * atb + atp * atp)
            {
                return btp;
            }
            //计算点到线段(a,b)的距离  
            double l = 0.0;
            double s = 0.0;
            l = DistancePointToPoint(A, B);
            s = ((A.Y - P.Y) * (B.X - A.X) - (A.X - P.X) * (B.Y - A.Y)) / (l * l);
            return (Math.Abs(s * l));
        }

        public static double GetMinDistance(PointF pt1, PointF pt2, PointF pt3)
        {
            double dis = 0;
            if (pt1.X == pt2.X)
            {
                dis = Math.Abs(pt3.X - pt1.X);
                return dis;
            }
            double lineK = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
            double lineC = (pt2.X * pt1.Y - pt1.X * pt2.Y) / (pt2.X - pt1.X);
            dis = Math.Abs(lineK * pt3.X - pt3.Y + lineC) / (Math.Sqrt(lineK * lineK + 1));
            return dis;

        }

        public static double GetMinDistance(Point2d pt1, Point2d pt2, Point2d pt3)
        {
            double dis = 0;
            if (pt1.X == pt2.X)
            {
                dis = Math.Abs(pt3.X - pt1.X);
                return dis;
            }
            double lineK = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
            double lineC = (pt2.X * pt1.Y - pt1.X * pt2.Y) / (pt2.X - pt1.X);
            dis = Math.Abs(lineK * pt3.X - pt3.Y + lineC) / (Math.Sqrt(lineK * lineK + 1));
            return dis;

        }

        /// <summary>
        /// 点到点的距离
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double DistancePointToPoint(Point2d ptA, Point2d ptB)
        {
            return Math.Sqrt(Math.Pow(ptA.X - ptB.X, 2) + Math.Pow(ptA.Y - ptB.Y, 2));
        }

        /// <summary>
        /// 点到点的距离
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double DistancePointToPoint(Point3d ptA, Point3d ptB)
        {
            return Math.Sqrt(Math.Pow(ptA.X - ptB.X, 2) + Math.Pow(ptA.Y - ptB.Y, 2));
        }



        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="StartPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static PointF GetEndPointByTrigonometric(double angle, PointF StartPoint, double distance)
        {
           PointF EndPoint = new PointF();

            //角度转弧度
            var radian = (angle * Math.PI) / 180;

            //计算新坐标 r 就是两者的距离
            EndPoint.X =(float) (StartPoint.X +distance * Math.Cos(radian));
            EndPoint.Y= (float)(StartPoint.Y + distance * Math.Sin(radian));
        
            return EndPoint;
        }

        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">弧度</param>
        /// <param name="StartPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static PointF GetEndPointByTrigonometricHu(double angle, PointF StartPoint, double distance)
        {
            PointF EndPoint = new PointF();

            //角度转弧度
         //   var radian = (angle * Math.PI) / 180;

            //计算新坐标 r 就是两者的距离
            EndPoint.X = (float)(StartPoint.X + distance * Math.Cos(angle));
            EndPoint.Y = (float)(StartPoint.Y + distance * Math.Sin(angle));

            return EndPoint;
        }

        /// <summary>
        /// 勾股定理求长度
        /// </summary>
        /// <param name="最长边长度">length1</param>
        /// <param name="边长度">length2</param>
        /// <returns>另一边长度</returns>
        public static double GetEndLengthByTheorem(double length1,double length2)
        {
            return Math.Sqrt(Math.Pow(length1,2)-Math.Pow(length2,2));
        }


    }
}
        
    
