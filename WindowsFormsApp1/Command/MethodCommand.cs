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

        /// <summary>
        /// 管线文本查询
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="lineList"></param>
        /// <returns></returns>
        public static DBText FindMTextIsInPolyineForPipe(Polyline line, List<DBText> txtList)
        {
            double min = 20;
            DBText resText = null;

            double mToPtMin = double.NaN;
            int num = line.NumberOfVertices;
            List<Point2d> point = new List<Point2d>();
            for (int i = 0; i < num; i++)
            {
                point.Add(line.GetPoint2dAt(i));


            }
            double mToPt = double.NaN;
            foreach (DBText txt in txtList)
            {
                Point2d mPt = txt.Position.Convert2d(new Plane());

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
                if (txt.Location.X>maxX&& txt.Location.X < thres + maxX && txt.Location.Y > minY && txt.Location.Y < maxY)
                {
                    if (line.LayerName != null && line.LayerName == "公厕")
                    { }
                    line.LayerName = txt.Text;
                    res = true;
                    break;
                }
            }
            return res;
        }

        internal static void OpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            try
            {
                fileName = file;
                cityName = city;
                dp = derivedType;
                num = 0;
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
                Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
                Application.DocumentManager.DocumentActivated -= docChange;
            }
            catch { }
        }
        internal static void AutoOpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            try
            {
                fileName = file;
                cityName = city;
                dp = derivedType;
                num = 0;
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docAutoChange);
                Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
                Application.DocumentManager.DocumentActivated -= docChange;
                Application.DocumentManager.MdiActiveDocument.CloseAndDiscard();
            }
            catch { }
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
            try
            {
                if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
                {
                    num++;
                    MainForm mf = new MainForm(cityName, dp,true);
                    mf.Show();
                   // mf.Close();
                    
                }
            }
            catch { }
        }

        /// <summary>
        /// 文档发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void docAutoChange(object sender, DocumentCollectionEventArgs e)
        {
            try
            {
                if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
                {
                    num++;
                    MainForm mf = new MainForm(cityName, dp);
                    mf.ShowDialog();
                    // mf.Close();

                }
            }
            catch { }
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


        public static List<PointF> GetArcPoints(PointF centerPoint,double startAngel,double endAngel,double r)
        {
            List<PointF> list = new List<PointF>();
            double de2g = Math.PI / 180;
            double deg2 = 180 / Math.PI;
            int n = 360;
            startAngel = deg2 * startAngel;
            endAngel = deg2 * endAngel;
            for (int i = 0; i < n + 1; i++)
            {
                PointF pt = new PointF();
                double ang = (startAngel + (endAngel - startAngel) * i / n) * de2g;
                pt.X =(float)( centerPoint.X + Math.Cos(ang));
                pt.Y = (float)(centerPoint.Y + Math.Cos(ang));
                list.Add(pt);
            }
            return list;
        }

        public static List<PointF> GetArcPointsByPoint2d(Point2d[] ptList)
        {
            List<PointF> list = new List<PointF>();
            foreach (Point2d pt in ptList)
            { 
                list.Add(new PointF(((float)pt.X),(float)pt.Y));
            }
            return list;
        }

        public static Point2dCollection GetPoint2DByPointF(List<PointF> ptList)
        {
            Point2dCollection list = new Point2dCollection();
            foreach (PointF pt in ptList)
            {
                list.Add(new Point2d(((float)pt.X), (float)pt.Y));
            }
            return list;
        }

        /// <summary>
        /// 结构：表示一个点
        /// </summary>
        public struct MyPoint
        {
            //横、纵坐标
            public double x, y;
            //构造函数
            public MyPoint(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            //该点到指定点pTarget的距离
            public double DistanceTo(MyPoint p)
            {
                return Math.Sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y));
            }
            //重写ToString方法
            public override string ToString()
            {
                return string.Concat("Point (",
                 this.x.ToString("#0.000"), ',',
                 this.y.ToString("#0.000"), ')');
            }
        }
        /// <summary>
        /// 计算点P(x,y)与X轴正方向的夹角
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>夹角弧度</returns>
        private static double radPOX(double x, double y)
        {
            //P在(0,0)的情况
            if (x == 0 && y == 0) return 0;
            //P在四个坐标轴上的情况：x正、x负、y正、y负
            if (y == 0 && x > 0) return 0;
            if (y == 0 && x < 0) return Math.PI;
            if (x == 0 && y > 0) return Math.PI / 2;
            if (x == 0 && y < 0) return Math.PI / 2 * 3;
            //点在第一、二、三、四象限时的情况
            if (x > 0 && y > 0) return Math.Atan(y / x);
            if (x < 0 && y > 0) return Math.PI - Math.Atan(y / -x);
            if (x < 0 && y < 0) return Math.PI + Math.Atan(-y / -x);
            if (x > 0 && y < 0) return Math.PI * 2 - Math.Atan(-y / x);
            return 0;
        }
        /// <summary>
        /// 返回点P围绕点A旋转弧度rad后的坐标
        /// </summary>
        /// <param name="P">待旋转点坐标</param>
        /// <param name="A">旋转中心坐标</param>
        /// <param name="rad">旋转弧度</param>
        /// <param name="isClockwise">true:顺时针/false:逆时针</param>
        /// <returns>旋转后坐标</returns>
      public static MyPoint RotatePoint(MyPoint P, MyPoint A,
         double rad, bool isClockwise = true)
        {
            //点Temp1
            MyPoint Temp1 = new MyPoint(P.x - A.x, P.y - A.y);
            //点Temp1到原点的长度
            double lenO2Temp1 = Temp1.DistanceTo(new MyPoint(0, 0));
            //∠T1OX弧度
            double angT1OX = radPOX(Temp1.x, Temp1.y);
            //∠T2OX弧度（T2为T1以O为圆心旋转弧度rad）
            double angT2OX = angT1OX - (isClockwise ? 1 : -1) * rad;
            //点Temp2
            MyPoint Temp2 = new MyPoint(
             lenO2Temp1 * Math.Cos(angT2OX),
             lenO2Temp1 * Math.Sin(angT2OX));
            //点Q
            return new MyPoint(Temp2.x + A.x, Temp2.y + A.y);
        }

        public static List<PointF> GetRoationPoint(MyPoint startpt, MyPoint endpt,MyPoint center,
    double startAngel, double endAngel,bool isClockWise)
        {
            List<PointF> points = new List<PointF>();
            int angelCout =Math.Abs( (int)(( endAngel - startAngel)/0.1));
          
            for (int i=0;i<angelCout;i++)
            {
                MyPoint pt = RotatePoint(startpt, center,((double)i)/10.0, isClockWise);
                points.Add(new PointF(( float)pt.x, (float)pt.y));
            }
            return points;
        }

        public static List<PointF> GetArcPoints(Curve cre,double length)
        {
            List<PointF> points = new List<PointF>();
          //  length = cre.EndParam;

            for (int i = 0; i < 20; i++)
            {
                double dis=cre.GetParameterAtDistance(length * (i / 20.0));
                Point3d pt = cre.GetPointAtParameter(dis);
                points.Add(new PointF((float)pt.X, (float)pt.Y));
            }
            points.Add(points[0]);
            return points;
        }


        public static List<PointF> GetArcPointsByBulge(Point2d startPoint,Point2d endPoint,double bulge)
        {
            List<PointF> list = new List<PointF>();
       
            CircularArc2d circularArc2 = new CircularArc2d(startPoint,endPoint,bulge,false);
           
            list= GetArcPointsByPoint2d(circularArc2.GetSamplePoints(20));
            return list;
        }

        public static bool PointsAllInPoints(List<PointF> ptSamll, List<PointF> ptBig)
        {
           // PolylineCurve2d circularArc2 = new PolylineCurve2d(GetPoint2DByPointF(ptBig));
            foreach (PointF item in ptSamll)
            {
                if (PtinPolygon(item, ptBig)==1)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 判断点是否在闭合多线段内
        /// </summary>
        /// <param name="pt">待判断点</param>
        /// <param name="poly">目标多边形</param>
        /// <returns>在多边形内则返回1</returns>

        private static double min(double x, double y)
        {
            if (x > y)
                return y;
            else
                return x;
        }
        private static double max(double x, double y)
        {
            if (x > y)
                return x;
            else
                return y;
        }
        /// <summary>
        /// 判断点是否在图形内
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="ptPolygon"></param>
        /// <returns></returns>
        public static int PtinPolygon(Point2d pt, Point2d[] ptPolygon)
        {
            int nCount = ptPolygon.Length;
            // 记录是否在多边形的边上
            bool isBeside = false;
            // 多边形外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;
                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }
                if ((pt.X > maxx) || (pt.X < minx) || (pt.Y > maxy) || (pt.Y < miny))
                    return -1;
            }
            // 射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount; i++)
            {
                Point2d p1 = ptPolygon[i];
                Point2d p2 = ptPolygon[(i + 1) % nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && pt.X <= max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                // 交点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                // 求交点的X坐标
                double x = (double)(pt.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
                if (x > pt.X)
                    nCross++;  // 只统计单边交点
                else if (x == pt.X)
                    isBeside = true;
            }
            if (isBeside)
                return 0;  //多边形上
            else if (nCross % 2 == 1)
                return 1;  // 多边形内
            return -1;     // 多边形外
        }

        public static int PtinPolygon(Point2d pt, Point2dCollection ptPolygon)
        {
            int nCount = ptPolygon.Count;
            // 记录是否在多边形的边上
            bool isBeside = false;
            // 多边形外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;
                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }
                if ((pt.X > maxx) || (pt.X < minx) || (pt.Y > maxy) || (pt.Y < miny))
                    return -1;
            }
            // 射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount; i++)
            {
                Point2d p1 = ptPolygon[i];
                Point2d p2 = ptPolygon[(i + 1) % nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && pt.X <= max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                // 交点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                // 求交点的X坐标
                double x = (double)(pt.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
                if (x > pt.X)
                    nCross++;  // 只统计单边交点
                else if (x == pt.X)
                    isBeside = true;
            }
            if (isBeside)
                return 0;  //多边形上
            else if (nCross % 2 == 1)
                return 1;  // 多边形内
            return -1;     // 多边形外
        }
        public static int PtinPolygon(PointF pt, List<PointF> ptPolygon)
        {
            int nCount = ptPolygon.Count;
            // 记录是否在多边形的边上
            bool isBeside = false;
            // 多边形外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;
                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }
                if ((pt.X > maxx) || (pt.X < minx) || (pt.Y > maxy) || (pt.Y < miny))
                    return -1;
            }
            // 射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount; i++)
            {
                PointF p1 = ptPolygon[i];
                PointF p2 = ptPolygon[(i + 1) % nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && pt.X <= max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                // 交点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                // 求交点的X坐标
                double x = (double)(pt.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
                if (x > pt.X)
                    nCross++;  // 只统计单边交点
                else if (x == pt.X)
                    isBeside = true;
            }
            if (isBeside)
                return 0;  //多边形上
            else if (nCross % 2 == 1)
                return 1;  // 多边形内
            return -1;     // 多边形外
        }
        //定数等分单条曲线
        public static DBObjectCollection AveragesCurve(Curve cv, int n)
        {
            double ep = cv.EndParam;
            double len = cv.GetDistanceAtParameter(ep);
            double split = len / n;

            DoubleCollection pas = new DoubleCollection();
            if (cv.Closed)                //闭合曲线要先断开
                pas.Add(cv.StartParam);

            for (int i = 1; i < n; i++)
            {
                pas.Add(cv.GetParameterAtDistance(i * split));
            }
            cv.Erase();
            return cv.GetSplitCurves(pas);
        }
        //等距等分单条曲线(dist<0反向等分)
        public static DBObjectCollection AveragesCurve(Curve cv, Double dist)
        {
            double ep = cv.EndParam;
            double len = cv.GetDistanceAtParameter(ep);
            double dst = 0;
            //从最后开始等分
            if (dist < 0)
            {
                dist = -dist;
                dst = (cv.GetDistanceAtParameter(cv.EndParam) % dist) - dist;
            }

            DoubleCollection pas = new DoubleCollection();
            if (cv.Closed) pas.Add(cv.StartParam);//闭合曲线要先断开

            while (true)
            {
                dst += dist;
                if (dst >= len) break;
                pas.Add(cv.GetParameterAtDistance(dst));
            }

            cv.Erase();
            return cv.GetSplitCurves(pas);
        }

        /// <summary>
        /// 多段线转curve2d
        /// </summary>
        /// <param name="pPoly"></param>
        /// <returns></returns>
     public   static Curve2d convertPolylineToGeCurve(Polyline pPoly)

        {
            List<Curve2d> geCurves = new List<Curve2d>();
            Curve2d out_pGeCurve;
            //  PointerArray geCurves;
            Vector3d normal = pPoly.Normal;
            // Is the polyline closed or open
            int nSegs = -1;
            if (pPoly.Closed)
                nSegs = pPoly.NumberOfVertices;
            else
                nSegs = pPoly.NumberOfVertices - 1;

            for (int i = 0; i < nSegs; i++)
            {
                if (pPoly.GetSegmentType(i) == SegmentType.Line)
                {
                    LineSegment2d line;
                    line = pPoly.GetLineSegment2dAt(i);

                    geCurves.Add(line);
                }
                else if (pPoly.GetSegmentType(i) == SegmentType.Arc)
                {
                    CircularArc2d arc;
                    arc = pPoly.GetArcSegment2dAt(i);

                    geCurves.Add(arc);
                }
            }// for
            if (geCurves.Count == 1)
                out_pGeCurve = (Curve2d)(geCurves[0]);
            else
                out_pGeCurve = new CompositeCurve2d(geCurves.ToArray());
            return out_pGeCurve;
        }

   
    }
}
        
    
