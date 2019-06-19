using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using System;
using System.Collections.Generic;
using System.Text;
using static RegulatoryPlan.Command.MethodCommand;

namespace RegulatoryPlan.Model
{
    public class AutoCad2ModelTools
    {
        public static DbTextModel DbText2Model(DBText dbText)
        {
            DbTextModel dbModel = new DbTextModel();
            dbModel.Height = dbText.Height;
            //dbModel.Position = new System.Drawing.PointF((float)dbText.Position.X, (float)dbText.Position.Y);
            dbModel.Position = Point3d2Pointf(dbText.Position);
            dbModel.Rotation = dbText.Rotation;
            dbModel.ThickNess = dbText.Thickness;
            dbModel.Text = dbText.TextString;
            dbModel.Color =dbText.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(dbText.LayerId) : System.Drawing.ColorTranslator.ToHtml(dbText.Color.ColorValue);
            return dbModel;
        }

        public static DbTextModel DbText2Model(MText dbText)
        {
            DbTextModel dbModel = new DbTextModel();
            dbModel.Height = dbText.ActualHeight;
            //dbModel.Position = new System.Drawing.PointF((float)dbText.Position.X, (float)dbText.Position.Y);
            dbModel.Position = Point3d2Pointf(dbText.Location);
            dbModel.Rotation = dbText.Rotation;
      //      dbModel.ThickNess = dbText.TextHeight;
            dbModel.Text = dbText.Text;
            dbModel.Color = dbText.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(dbText.LayerId) : System.Drawing.ColorTranslator.ToHtml(dbText.Color.ColorValue);
            return dbModel;
        }

        public static HatchModel Hatch2Model(Hatch dbText)
        {
            HatchModel dbModel = new HatchModel();
        
            int cont = dbText.NumberOfLoops;

            for (int i = 0; i < cont; i++)
            {
                dbModel.loopPoints.Add(i, new ColorAndPointItemModel());
                HatchLoop loop = dbText.GetLoopAt(i);

                 ColorAndPointItemModel cpModel = new ColorAndPointItemModel();
                if (i==0)
                {
                    cpModel.Color = dbText.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(dbText.LayerId) : System.Drawing.ColorTranslator.ToHtml(dbText.Color.ColorValue);
                }
                else
                {
                    cpModel.Color = "#FFFFFF";
                  //  cpModel.ZIndex = "1";
                }
                if (loop.IsPolyline)
                {
                    for (int j = 0; j < loop.Polyline.Count - 1; j++)
                    {
                        BulgeVertex vertex = loop.Polyline[j];
                        BulgeVertex vertex1 = loop.Polyline[j + 1];
                        if (vertex.Bulge != 0)
                        {
                           
                            cpModel.loopPoints.AddRange(MethodCommand.GetArcPointsByBulge(vertex.Vertex, vertex1.Vertex, vertex.Bulge));
                        }
                        else
                        {
                            cpModel.loopPoints.Add(Point2d2Pointf(vertex.Vertex));
                        }
                    }
                    if (dbText.NumberOfHatchLines > 0)
                    {
                        Line2dCollection cl = dbText.GetHatchLinesData();
                    } //foreach (Line2d itemi in )
                      //{

                    //}

                }
                else
                {
                    Curve2dCollection col_cur2d = loop.Curves;
                    foreach (Curve2d item in col_cur2d)
                    {
                        Point2d[] M_point2d = item.GetSamplePoints(20);
                        foreach (Point2d point in M_point2d)
                        {
                            cpModel.loopPoints.Add(Point2d2Pointf(point));
                        }
                    }
                }
                if (cpModel.loopPoints[0] != cpModel.loopPoints[cpModel.loopPoints.Count - 1])
                {
                    cpModel.loopPoints.Add(cpModel.loopPoints[0]);
                }
                dbModel.loopPoints[i]=cpModel;
            }

            for (int i = 0; i < dbModel.loopPoints.Count; i++)
            {
                
                for (int j = 0; j < dbModel.loopPoints.Count; j++)
                {
                    if (i != j)
                    {
                        if (MethodCommand.PointsAllInPoints(dbModel.loopPoints[j].loopPoints, dbModel.loopPoints[i].loopPoints))
                        {
                            dbModel.loopPoints[j].ZIndex = "2";
                        }
                    }
                }
            }
            dbModel.Area = dbText.Area;
            //   dbModel.Color =
            return dbModel;
        }

        public static LineModel Line2Model(Line line)
        {
            LineModel dbModel = new LineModel();

            dbModel.StartPoint = Point3d2Pointf(line.StartPoint);

            dbModel.EndPoint = Point3d2Pointf(line.EndPoint);

            dbModel.Angle = line.Angle;

            dbModel.Length = line.Length;
            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);

            return dbModel;
        }

        public static CircleModel Circle2Model(Circle line)
        {
            CircleModel dbModel = new CircleModel();
            dbModel.Center= Point3d2Pointf(line.Center);
            dbModel.Radius =line.Radius;
            MyPoint spt = new MyPoint(line.StartPoint.X,line.StartPoint.Y);
            MyPoint ept = new MyPoint(line.EndPoint.X, line.EndPoint.Y);
            MyPoint center = new MyPoint(dbModel.Center.X, dbModel.Center.Y);
            dbModel.pointList = MethodCommand.GetArcPoints(line,line.Circumference);
            dbModel.Color= line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            return dbModel;
        }

        public static PolyLineModel Polyline2Model(Autodesk.AutoCAD.DatabaseServices.Polyline polyLine)
        {
            PolyLineModel polylineModel = new PolyLineModel();
            polylineModel.Area = polyLine.Area;
            polylineModel.Closed = polyLine.Closed;
           polylineModel.Color=polyLine.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(polyLine.LayerId) : System.Drawing.ColorTranslator.ToHtml(polyLine.Color.ColorValue);
            polylineModel.Vertices = new System.Collections.ArrayList();
            int vn = polyLine.NumberOfVertices;  //lwp已知的多段线
            for (int i = 0; i < vn; i++)
            {
                Point2d pt = polyLine.GetPoint2dAt(i);
                SegmentType st = polyLine.GetSegmentType(i);
                if (st == SegmentType.Arc)
                {
                    ArcModel arc = new ArcModel();
                    CircularArc2d cir = polyLine.GetArcSegment2dAt(i);
                    //  arc.Center = new System.Drawing.PointF((float)cir.Center.X,(float)cir.Center.Y);
                    arc.Center = Point2d2Pointf(cir.Center);
                    arc.Radius = cir.Radius;
                    arc.Startangel = cir.StartAngle;
                    arc.EndAngel = cir.EndAngle;
                    //  arc.StartPoint = new System.Drawing.PointF((float)cir.StartPoint.X, (float)cir.StartPoint.Y);
                    if (cir.HasStartPoint)
                    {
                        arc.StartPoint = Point2d2Pointf(cir.StartPoint);
                    }
                    //  arc.EndPoint = new System.Drawing.PointF((float)cir.EndPoint.X, (float)cir.EndPoint.Y);
                    if (cir.HasEndPoint)
                    {
                        arc.EndPoint = Point2d2Pointf(cir.EndPoint);
                    }
           

                    MyPoint spt = new MyPoint(arc.StartPoint.X, arc.StartPoint.Y);
                    MyPoint ept = new MyPoint(arc.EndPoint.X,arc.EndPoint.Y);
                    MyPoint center = new MyPoint(arc.Center.X, arc.Center.Y);
                    arc.Color = polylineModel.Color;
                    // arc.pointList = MethodCommand.GetRoationPoint(spt, ept, center, arc.Startangel,arc.EndAngel,cir.IsClockWise);
                    arc.pointList = MethodCommand.GetArcPointsByPoint2d(cir.GetSamplePoints(20));
                    //arc.pointList = MethodCommand.GetArcPoints(arc.Center,arc.Startangel,arc.EndAngel,arc.Radius);
                  //  arc.pointList.Insert(0, arc.StartPoint);
                 //   arc.pointList.Add(arc.EndPoint);
                    polylineModel.Vertices.Add(arc);
                }
                else if (st == SegmentType.Line)
                {
                    LineModel line = new LineModel();
                    LineSegment2d lineSe = polyLine.GetLineSegment2dAt(i);
                    if (lineSe.HasStartPoint)
                    {
                        line.StartPoint = Point2d2Pointf(lineSe.StartPoint);
                    }
                    if (lineSe.HasEndPoint)
                    {
                        line.EndPoint = Point2d2Pointf(lineSe.EndPoint);
                    }
                    if (line.StartPoint.X == line.EndPoint.X && line.StartPoint.Y == line.EndPoint.Y)
                    {
                        line.Angle = 0;
                        line.Length = 0;
                    }
                    else if (line.StartPoint.X == line.EndPoint.X)
                    {
                        line.Angle = 90;

                    }
                    line.Color = polylineModel.Color;
                    polylineModel.Vertices.Add(line);
                }
            }
            return polylineModel;
        }

        public static System.Drawing.PointF Point2d2Pointf(Point2d point2d)
        {
            return new System.Drawing.PointF((float)point2d.X, (float)point2d.Y);
        }

        public static System.Drawing.PointF Point3d2Pointf(Point3d point2d)
        {
            return new System.Drawing.PointF((float)point2d.X, (float)point2d.Y);
        }
    }
}
