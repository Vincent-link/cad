using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                    if (i == 0)
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
                    dbModel.loopPoints[i] = cpModel;
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
            try
            {
                dbModel.Area = dbText.Area;
            }
            catch
            { }
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


        public static CircleModel Ellipse2Model(Ellipse line)
        {
            CircleModel dbModel = new CircleModel();
            dbModel.Center = Point3d2Pointf(line.Center);
            //dbModel.MajorAxis= line.MajorRadius;
            //dbModel.MinorAxis = line.MinorRadius;
            MyPoint spt = new MyPoint(line.StartPoint.X, line.StartPoint.Y);
            MyPoint ept = new MyPoint(line.EndPoint.X, line.EndPoint.Y);
            MyPoint center = new MyPoint(dbModel.Center.X, dbModel.Center.Y);
           
            double length = line.RadiusRatio * (line.MinorRadius+line.MajorRadius);
           dbModel.pointList= MethodCommand.GetArcPoints(line,length);
            
            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
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



        internal static CircleModel Polyline2DModel(Polyline2d line)
        {
            CircleModel dbModel = new CircleModel();


            double length = line.Length;
            dbModel.pointList = MethodCommand.GetArcPoints(line, length);

            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            return dbModel;
        }

        internal static CircleModel Arc2Model(Arc line)
        {
            CircleModel dbModel = new CircleModel();
            dbModel.Center = Point3d2Pointf(line.Center);
            //dbModel.MajorAxis= line.MajorRadius;
            //dbModel.MinorAxis = line.MinorRadius;
            MyPoint spt = new MyPoint(line.StartPoint.X, line.StartPoint.Y);
            MyPoint ept = new MyPoint(line.EndPoint.X, line.EndPoint.Y);
            MyPoint center = new MyPoint(dbModel.Center.X, dbModel.Center.Y);
          
            double length = line.Length;
            dbModel.pointList = MethodCommand.GetArcPoints(line, length);

            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            return dbModel;
        }

 


        public static DbTextModel DbText2Model(DBText dbText,AttributeModel atModel)
        {
            DbTextModel dbModel = new DbTextModel();
            dbModel.attItemList = new List<AttributeItemModel>();
            dbModel.Height = dbText.Height;
            //dbModel.Position = new System.Drawing.PointF((float)dbText.Position.X, (float)dbText.Position.Y);
            dbModel.Position = Point3d2Pointf(dbText.Position);
            dbModel.Rotation = dbText.Rotation;
            dbModel.ThickNess = dbText.Thickness;
            dbModel.Text = dbText.TextString;
            dbModel.Color = dbText.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(dbText.LayerId) : System.Drawing.ColorTranslator.ToHtml(dbText.Color.ColorValue);
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.TxtHeight:
                        attValue = dbText.Height.ToString();
                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:
                        attValue = dbText.TextString;
                        break;
                    case AttributeItemType.LayerName:
                        attValue = dbText.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = dbText.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(dbText);
                        break;
                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }

        public static DbTextModel DbText2Model(MText dbText, AttributeModel atModel)
        {
            DbTextModel dbModel = new DbTextModel();
            dbModel.Height = dbText.ActualHeight;
            //dbModel.Position = new System.Drawing.PointF((float)dbText.Position.X, (float)dbText.Position.Y);
            dbModel.Position = Point3d2Pointf(dbText.Location);
            dbModel.Rotation = dbText.Rotation;
            //      dbModel.ThickNess = dbText.TextHeight;
            dbModel.Text = dbText.Text;
            dbModel.Color = dbText.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(dbText.LayerId) : System.Drawing.ColorTranslator.ToHtml(dbText.Color.ColorValue);
         
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.TxtHeight:
                        attValue = dbText.TextHeight.ToString();
                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:
                        attValue = dbText.Text;
                        break;
                    case AttributeItemType.LayerName:
                        attValue = dbText.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = dbText.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(dbText);
                        break;
                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }

        public static HatchModel Hatch2Model(Hatch dbText, AttributeModel atModel)
        {
            HatchModel dbModel = new HatchModel();

            int cont = dbText.NumberOfLoops;
            string color = "";
            for (int i = 0; i < cont; i++)
            {
                dbModel.loopPoints.Add(i, new ColorAndPointItemModel());
                HatchLoop loop = dbText.GetLoopAt(i);

                ColorAndPointItemModel cpModel = new ColorAndPointItemModel();
                if (i == 0)
                {
                   color= cpModel.Color = dbText.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(dbText.LayerId) : System.Drawing.ColorTranslator.ToHtml(dbText.Color.ColorValue);
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
                cpModel.attItemList = new List<AttributeItemModel>();
                foreach (AttributeItemModel item in atModel.attributeItems)
                {
                    string attValue = "";

                    switch (item.AtItemType)
                    {
                        case AttributeItemType.Area:
                            attValue = dbModel.Area.ToString();
                            break;
                        case AttributeItemType.TxtHeight:

                            break;
                        case AttributeItemType.Color:
                            attValue = color;
                            break;
                        case AttributeItemType.Content:

                            break;
                        case AttributeItemType.LayerName:
                            attValue = dbText.Layer;
                            break;
                        case AttributeItemType.LineScale:
                            attValue = dbText.LinetypeScale.ToString();
                            break;
                        case AttributeItemType.LineType:
                            attValue = GetLayerLineTypeByID(dbText);
                            break;
                        case AttributeItemType.Overallwidth:
                            break;
                        case AttributeItemType.TotalArea:
                            attValue = dbModel.Area.ToString();
                            break;

                    }
                    if (!string.IsNullOrEmpty(attValue))
                    {
                        item.AtValue = attValue;
                       cpModel.attItemList.Add(item);
                    }
                }
                dbModel.loopPoints[i] = cpModel;
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
            try
            {
                dbModel.Area = dbText.Area;
            }
            catch
            {  }
          
            //   dbModel.Color =
            return dbModel;
        }

        public static LineModel Line2Model(Line line, AttributeModel atModel)
        {
            LineModel dbModel = new LineModel();

            dbModel.StartPoint = Point3d2Pointf(line.StartPoint);

            dbModel.EndPoint = Point3d2Pointf(line.EndPoint);

            dbModel.Angle = line.Angle;

            dbModel.Length = line.Length;
            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.Area:
                        
                        break;
                    case AttributeItemType.TxtHeight:

                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:

                        break;
                    case AttributeItemType.LayerName:
                        attValue =line.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = line.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(line);
                        break;
                    case AttributeItemType.Overallwidth:
                    
                        break;
                    case AttributeItemType.TotalArea:
                      
                        break;

                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }

        public static CircleModel Circle2Model(Circle line, AttributeModel atModel)
        {
            CircleModel dbModel = new CircleModel();
            dbModel.Center = Point3d2Pointf(line.Center);
            dbModel.Radius = line.Radius;
            MyPoint spt = new MyPoint(line.StartPoint.X, line.StartPoint.Y);
            MyPoint ept = new MyPoint(line.EndPoint.X, line.EndPoint.Y);
            MyPoint center = new MyPoint(dbModel.Center.X, dbModel.Center.Y);
            dbModel.pointList = MethodCommand.GetArcPoints(line, line.Circumference);
            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.Area:
                        attValue = line.Area.ToString();
                        break;
                    case AttributeItemType.TxtHeight:

                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:

                        break;
                    case AttributeItemType.LayerName:
                        attValue = line.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = line.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(line);
                        break;
                    case AttributeItemType.Overallwidth:
                        break;
                    case AttributeItemType.TotalArea:

                        break;

                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }


        public static CircleModel Ellipse2Model(Ellipse line, AttributeModel atModel)
        {
            CircleModel dbModel = new CircleModel();
            dbModel.Center = Point3d2Pointf(line.Center);
            //dbModel.MajorAxis= line.MajorRadius;
            //dbModel.MinorAxis = line.MinorRadius;
            MyPoint spt = new MyPoint(line.StartPoint.X, line.StartPoint.Y);
            MyPoint ept = new MyPoint(line.EndPoint.X, line.EndPoint.Y);
            MyPoint center = new MyPoint(dbModel.Center.X, dbModel.Center.Y);

            double length = line.RadiusRatio * (line.MinorRadius + line.MajorRadius);
            dbModel.pointList = MethodCommand.GetArcPoints(line, length);

            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.Area:
                        attValue = line.Area.ToString();
                        break;
                    case AttributeItemType.TxtHeight:

                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:

                        break;
                    case AttributeItemType.LayerName:
                        attValue = line.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = line.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(line);
                        break;
                    case AttributeItemType.Overallwidth:
                        break;
                    case AttributeItemType.TotalArea:

                        break;

                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }

        public static PolyLineModel Polyline2Model(Autodesk.AutoCAD.DatabaseServices.Polyline polyLine, AttributeModel atModel)
        {
            PolyLineModel polylineModel = new PolyLineModel();
            polylineModel.Area = polyLine.Area;
            polylineModel.Closed = polyLine.Closed;
            polylineModel.Color = polyLine.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(polyLine.LayerId) : System.Drawing.ColorTranslator.ToHtml(polyLine.Color.ColorValue);
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
                    MyPoint ept = new MyPoint(arc.EndPoint.X, arc.EndPoint.Y);
                    MyPoint center = new MyPoint(arc.Center.X, arc.Center.Y);
                    arc.Color = polylineModel.Color;
                    // arc.pointList = MethodCommand.GetRoationPoint(spt, ept, center, arc.Startangel,arc.EndAngel,cir.IsClockWise);
                    arc.pointList = MethodCommand.GetArcPointsByPoint2d(cir.GetSamplePoints(20));
                    //arc.pointList = MethodCommand.GetArcPoints(arc.Center,arc.Startangel,arc.EndAngel,arc.Radius);
                    //  arc.pointList.Insert(0, arc.StartPoint);
                    //   arc.pointList.Add(arc.EndPoint);
                    foreach (AttributeItemModel item in atModel.attributeItems)
                    {
                        string attValue = "";

                        switch (item.AtItemType)
                        {
                            case AttributeItemType.Area:
                                attValue = polyLine.Area.ToString();
                                break;
                            case AttributeItemType.TxtHeight:

                                break;
                            case AttributeItemType.Color:
                                attValue = polylineModel.Color;
                                break;
                            case AttributeItemType.Content:

                                break;
                            case AttributeItemType.LayerName:
                                attValue = polyLine.Layer;
                                break;
                            case AttributeItemType.LineScale:
                                attValue = polyLine.LinetypeScale.ToString();
                                break;
                            case AttributeItemType.LineType:
                                attValue = GetLayerLineTypeByID(polyLine);
                                break;
                            case AttributeItemType.Overallwidth:
                                attValue = polyLine.ConstantWidth.ToString();
                                break;
                            case AttributeItemType.TotalArea:

                                break;

                        }
                        if (!string.IsNullOrEmpty(attValue))
                        {
                            item.AtValue = attValue;
                            arc.attItemList.Add(item);
                        }
                    }
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
                    foreach (AttributeItemModel item in atModel.attributeItems)
                    {
                        string attValue = "";

                        switch (item.AtItemType)
                        {
                            case AttributeItemType.Area:
                                attValue = polyLine.Area.ToString();
                                break;
                            case AttributeItemType.TxtHeight:

                                break;
                            case AttributeItemType.Color:
                                attValue = polylineModel.Color;
                                break;
                            case AttributeItemType.Content:

                                break;
                            case AttributeItemType.LayerName:
                                attValue = polyLine.Layer;
                                break;
                            case AttributeItemType.LineScale:
                                attValue = polyLine.LinetypeScale.ToString();
                                break;
                            case AttributeItemType.LineType:
                                attValue = GetLayerLineTypeByID(polyLine);
                                break;
                            case AttributeItemType.Overallwidth:
                                attValue = polyLine.ConstantWidth.ToString();
                                break;
                            case AttributeItemType.TotalArea:

                                break;

                        }

                        item.AtValue = attValue;
                       line.attItemList.Add(item);
                    }
                    polylineModel.Vertices.Add(line);
                }
            }

            return polylineModel;
        }



        internal static CircleModel Polyline2DModel(Polyline2d line, AttributeModel atModel)
        {
            CircleModel dbModel = new CircleModel();


            double length = line.Length;
            dbModel.pointList = MethodCommand.GetArcPoints(line, length);

            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.Area:
                        attValue =line.Area.ToString();
                        break;
                    case AttributeItemType.TxtHeight:

                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:

                        break;
                    case AttributeItemType.LayerName:
                        attValue = line.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = line.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(line);
                        break;
                    case AttributeItemType.Overallwidth:
                        attValue = line.ConstantWidth.ToString();
                        break;
                    case AttributeItemType.TotalArea:

                        break;

                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }

        internal static CircleModel Arc2Model(Arc line, AttributeModel atModel)
        {
            CircleModel dbModel = new CircleModel();
            dbModel.Center = Point3d2Pointf(line.Center);
            //dbModel.MajorAxis= line.MajorRadius;
            //dbModel.MinorAxis = line.MinorRadius;
            MyPoint spt = new MyPoint(line.StartPoint.X, line.StartPoint.Y);
            MyPoint ept = new MyPoint(line.EndPoint.X, line.EndPoint.Y);
            MyPoint center = new MyPoint(dbModel.Center.X, dbModel.Center.Y);

            double length = line.Length;
            dbModel.pointList = MethodCommand.GetArcPoints(line, length);

            dbModel.Color = line.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(line.LayerId) : System.Drawing.ColorTranslator.ToHtml(line.Color.ColorValue);
            foreach (AttributeItemModel item in atModel.attributeItems)
            {
                string attValue = "";

                switch (item.AtItemType)
                {
                    case AttributeItemType.Area:
                        attValue = line.Area.ToString();
                        break;
                    case AttributeItemType.TxtHeight:

                        break;
                    case AttributeItemType.Color:
                        attValue = dbModel.Color;
                        break;
                    case AttributeItemType.Content:

                        break;
                    case AttributeItemType.LayerName:
                        attValue = line.Layer;
                        break;
                    case AttributeItemType.LineScale:
                        attValue = line.LinetypeScale.ToString();
                        break;
                    case AttributeItemType.LineType:
                        attValue = GetLayerLineTypeByID(line);
                        break;
                    case AttributeItemType.Overallwidth:
                   
                        break;
                    case AttributeItemType.TotalArea:

                        break;

                }
                if (!string.IsNullOrEmpty(attValue))
                {
                    item.AtValue = attValue;
                    dbModel.attItemList.Add(item);
                }
            }
            return dbModel;
        }
    }
}
