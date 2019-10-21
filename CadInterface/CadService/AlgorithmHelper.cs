using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadInterface.CadService
{
    public class AlgorithmHelper
    {
        /// <summary>
        /// 两点距离
        /// </summary>
        public static double lineSpace(double x1, double y1, double x2, double y2)
        {
            double lineLength = 0;
            lineLength = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            return lineLength;
        }
        /// <summary>
        /// 计算误差
        /// </summary>
        internal static double pointToLine(double x1, double y1, double x2, double y2, double x0, double y0)
        {
            double space = 0;
            double a, b, c;
            a = lineSpace(x1, y1, x2, y2);// 线段的长度    
            b = lineSpace(x1, y1, x0, y0);// (x1,y1)到点的距离    
            c = lineSpace(x2, y2, x0, y0);// (x2,y2)到点的距离    
            if (c <= 0.000001 || b <= 0.000001)
            {
                space = 0;
                return space;
            }
            if (a <= 0.000001)
            {
                space = b;
                return space;
            }
            if (c * c >= a * a + b * b)
            {
                space = b;
                return space;
            }
            if (b * b >= a * a + c * c)
            {
                space = c;
                return space;
            }
            double p = (a + b + c) / 2;// 半周长    
            double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));// 海伦公式求面积    
            space = 2 * s / a;// 返回点到线的距离（利用三角形面积公式求高）    
            return space;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pf"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool GetPointIsInLine(Point3d pf, Point3d p1, Point3d p2, double range)
        {
            double distance = pointToLine(p1.X, p1.Y, p2.X, p2.Y, pf.X, pf.Y);
            if (distance <= range)
                return true;
            return false;
            /*
            //range 判断的的误差，不需要误差则赋值0
            //点在线段首尾两端之外则return false
            double cross = (p2.X - p1.X) * (pf.X - p1.X) + (p2.Y - p1.Y) * (pf.Y - p1.Y);
            if (cross <= 0) return false;
            double d2 = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            if (cross >= d2) return false;
            double r = cross / d2;
            double px = p1.X + (p2.X - p1.X) * r;
            double py = p1.Y + (p2.Y - p1.Y) * r;
            //判断距离是否小于误差
            return Math.Sqrt((pf.X - px) * (pf.X - px) + (py - pf.Y) * (py - pf.Y)) <= range;
            */
        }

        /// <summary>
        /// 点到直线的垂足
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Point3d GetFootOfPerpendicular(Point3d pt, Point3d begin, Point3d end)
        {
            double dx = begin.X - end.X;
            double dy = begin.Y - end.Y;
            if (Math.Abs(dx) < 0.00000001 && Math.Abs(dy) < 0.00000001)
                return begin;
            double u = (pt.X - begin.X) * (begin.X - end.X) + (pt.Y - begin.Y) * (begin.Y - end.Y);
            u = u / ((dx * dx) + (dy * dy));
            return new Point3d(begin.X + u * dx, begin.Y + u * dy, 0);
        }
        /// <summary>
        /// 获取夹角
        /// </summary>
        public static double GetAngle(Point2d startPoint, Point2d endPoint, double radValue)
        {
            double rotation = Math.Atan((startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X));
            double loadAngle = (180 / Math.PI) * rotation;
            //水平面角度
            double angle = Math.Abs((180 / Math.PI) * rotation);
            //指北针角度
            double compassAngle = Math.Abs((180 / Math.PI) * radValue);
            if (loadAngle > 0 && loadAngle < 90)
                return Math.Abs(180 - (angle + (180 - compassAngle)));
            else if (loadAngle < 0)
                return Math.Abs(180 - (angle + compassAngle));
            else if (loadAngle == 0)
                return Math.Abs(180 - compassAngle);
            else if (loadAngle == 90)
                return Math.Abs(90 - compassAngle);
            else
                return 0D;
        }
        /// <summary>
        /// 判断点到直线的垂足是否在延长线上
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="endLine"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Get(Point3d startLine, Point3d endLine, Point3d point)
        {
            double distance1 = (endLine.X - startLine.X) * (point.X - startLine.X) + (endLine.Y - startLine.Y) * (point.Y - startLine.Y);
            double distance2 = (endLine.X - startLine.X) * (point.X - endLine.X) + (endLine.Y - startLine.Y) * (point.Y - endLine.Y);
            if (distance1 < 0 || distance2 < 0)//不在
                return false;
            else//在
                return true;
        }
    }
}
