using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
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
        public static string LegendLayer = "图例框";

        static string fileName = "";
        static string cityName = "";
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
        public static Dictionary<int,List<Polyline>> FindMaxAreaPoline(List<Polyline> polylines)
        {
            Dictionary<int, List<Polyline>> allPolines = new Dictionary<int, List<Polyline>>();
            allPolines.Add(0, new List<Polyline>());
            allPolines.Add(1, new List<Polyline>());
            double maxArea = double.NaN;
            foreach (Polyline line in polylines)
            {
                if (double.IsNaN(maxArea)||line.Area > maxArea)
                {
                    maxArea = line.Area;
                }
            }

            foreach (Polyline line in polylines)
            {
                if (line.Area.ToString("F5") == maxArea.ToString("F5"))
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
    }
}
        
    
