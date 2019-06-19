using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.Collections;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using RegulatoryPlan.Command;

namespace RegulatoryPlan.Method
{
   public class FenTuZeMethod
    {

        public void ManualSelect()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // 选择实体
            PromptSelectionResult psr = ed.SelectAll();
            SelectionSet SS = psr.Value;
            ObjectId[] idArray = SS.GetObjectIds();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(db.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr;
                btr = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                PromptSelectionResult acSSPrompt = ed.GetSelection();
                ArrayList entities = new ArrayList();

                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        if (acSSObj != null)
                        {
                            Entity ent1 = tr.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Entity;
                            // 筛选实体
                            screenEntities(ent1, entities);
                        }
                    }

                }

                MessageBox.Show(entities.Count.ToString());
                GetEntitiesInfo(entities, tr, btr, 5, doc, ed);
            }
        }

        public void LayerSelect()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(db.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr;
                btr = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                ArrayList entities = new ArrayList();

                // 按图层读取
                List<string> info = LayersToList(db);
                foreach (string lname in info)
                {
                    TypedValue[] tvs =

                       new TypedValue[1] {
                            new TypedValue(
                              (int)DxfCode.LayerName,
                              lname
                            )
                       };

                    SelectionFilter sf = new SelectionFilter(tvs);
                    PromptSelectionResult psr = ed.SelectAll(sf);

                    if (psr.Status == PromptStatus.OK)
                    {
                        // 选择实体
                        //PromptSelectionResult psr = ed.SelectAll();
                        SelectionSet SS = psr.Value;
                        ObjectId[] idArray = SS.GetObjectIds();

                        //PromptSelectionResult acSSPrompt = doc.Editor.GetSelection();

                        for (int j = 0; j < idArray.Length; j++)
                        {
                            Entity ent1 = tr.GetObject(idArray[j], OpenMode.ForWrite) as Entity;
                            // 筛选实体
                            screenEntities(ent1, entities);
                        }
                    }
                }

                MessageBox.Show(entities.Count.ToString());
                MessageBox.Show(info.Count.ToString());

                GetEntitiesInfo(entities, tr, btr, 5, doc, ed);
            }
        }

        // 筛选实体
        public void screenEntities(Entity ent1, ArrayList entities)
        {
            // 点
            if (ent1 is DBPoint)
            {
                DBPoint ent2 = (DBPoint)ent1;
                entities.Add(new Point3d(ent2.Position.X, ent2.Position.Y, 0));
            }

            // 线
            if (ent1 is Polyline)
            {
                if ((ent1 as Polyline).Closed)
                {
                    entities.Add(ent1);
                }
                else
                {
                    entities.Add(ent1);
                }
            }

            // 颜色填充
            if (ent1 is Hatch)
            {
                entities.Add(ent1);
            }

            // 文字
            if (ent1 is MText || ent1 is DBText)
            {
                entities.Add(ent1);
            }

            // 块参照
            if (ent1 is BlockReference)
            {
                entities.Add(ent1);
            }
        }

        // 获取所有图层
        public List<string> LayersToList(Database db)
        {
            List<string> lstlay = new List<string>();

            LayerTableRecord layer;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in lt)
                {
                    layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    if (!layer.IsLocked)
                    {
                        //lstlay.Add(layer.Name);
                    }
                }

            }
            lstlay.Add("地块界限");
            lstlay.Add("幼儿园");
            lstlay.Add("公厕");
            lstlay.Add("地块编码");
            lstlay.Add("城市黄线");
            lstlay.Add("城市蓝线");
            lstlay.Add("城市绿线");
            lstlay.Add("道路红线");
            lstlay.Add("路名");
            lstlay.Add("建筑退让线");
            lstlay.Add("禁止开口路段");
            //lstlay.Add("机动车出入口");

            return lstlay;
        }

        // 获取实体信息
        public void GetEntitiesInfo(ArrayList entities, Transaction trans, BlockTableRecord btr, int numSample, Document doc, Editor ed)
        {
            ArrayList uuid = new ArrayList();
            ArrayList geom = new ArrayList();   // 坐标点集合
            ArrayList colorList = new ArrayList();       // 颜色集合
            ArrayList type = new ArrayList();       // 类型集合

            ArrayList layerName = new ArrayList();
            ArrayList tableName = new ArrayList(); // 表名
            System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
            ArrayList attributeIndexList = new ArrayList(); //属性索引集合

            ArrayList tuliList = new ArrayList(); //图例集合
            string projectId = ""; //项目ID
            string chartName = ""; //表名称
            ArrayList kgGuide = new ArrayList(); //控规引导

            string srid = ""; //地理坐标系统编号
            ArrayList parentId = new ArrayList(); //配套设施所在地块集合
            ArrayList textContent = new ArrayList(); // 文字内容（GIS端展示）
            ArrayList blockContent = new ArrayList(); // 块内容（GIS端展示）

            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

            // 遍历所有实体
            foreach (object entity in entities)
            {
                ArrayList singlePositionList = new ArrayList(); // 单个实体坐标点集合

                //取得边界数
                int loopNum = 1;
                if (entity is Hatch)
                {
                    loopNum = (entity as Hatch).NumberOfLoops;
                    type.Add("polygon");
                }

                Point3dCollection col_point3d = new Point3dCollection();
                BulgeVertexCollection col_ver = new BulgeVertexCollection();
                Curve2dCollection col_cur2d = new Curve2dCollection();

                for (int i = 0; i < loopNum; i++)
                {
                    col_point3d.Clear();
                    HatchLoop hatLoop = null;
                    if (entity is Hatch)
                    {
                        try
                        {
                            hatLoop = (entity as Hatch).GetLoopAt(i);
                        }
                        catch (System.Exception)
                        {
                            continue;
                        }

                        //如果HatchLoop为PolyLine
                        if (hatLoop.IsPolyline)
                        {
                            col_ver = hatLoop.Polyline;
                            foreach (BulgeVertex vertex in col_ver)
                            {
                                col_point3d.Add(new Point3d(vertex.Vertex.X, vertex.Vertex.Y, 0));
                            }
                        }
                    }

                    // 如果实体为Polyline
                    if (entity is Polyline)
                    {
                        // 类型
                        type.Add("polyline");

                        Polyline polyline = (Polyline)entity;

                        int vn = polyline.NumberOfVertices;
                        for (int w = 0; w < vn; w++)
                        {
                            Point2d pt = polyline.GetPoint2dAt(w);

                            col_point3d.Add(new Point3d(pt.X, pt.Y, 0));
                        }
                    }
                    //// 如果实体为Curve
                    //if (entity is Curve)
                    //{
                    //    col_cur2d = hatLoop.Curves;
                    //    foreach (Curve2d item in col_cur2d)
                    //    {
                    //        Point2d[] M_point2d = item.GetSamplePoints(numSample);
                    //        foreach (Point2d pt in M_point2d)
                    //        {
                    //            if (!col_point3d.Contains(new Point3d(pt.X, pt.Y, 0)))
                    //                col_point3d.Add(new Point3d(pt.X, pt.Y, 0));
                    //        }
                    //    }
                    //}
                    // 如果实体为Point2d
                    if (entity is DBPoint)
                    {
                        type.Add("point");

                        DBPoint entity3 = (DBPoint)entity;
                        col_point3d.Add(new Point3d(entity3.Position.X, entity3.Position.Y, 0));
                    }

                    // 如果实体为Point
                    if (entity is Point3d)
                    {
                        type.Add("point");

                        Point3d entity3 = (Point3d)entity;
                        col_point3d.Add(entity3);
                    }

                    // 如果实体为文字
                    if (entity is MText)
                    {
                        type.Add("text");

                        col_point3d.Add(new Point3d((entity as MText).Location.X, (entity as MText).Location.Y, 0));
                    }

                    // 如果实体为文字
                    if (entity is DBText)
                    {
                        type.Add("text");

                        col_point3d.Add(new Point3d((entity as DBText).Position.X, (entity as DBText).Position.Y, 0));
                    }

                    // 块参照
                    if (entity is BlockReference)
                    {
                        type.Add("block");

                        col_point3d.Add(new Point3d((entity as BlockReference).Position.X, (entity as BlockReference).Position.Y, 0));
                    }

                    double[] pointPositionList = new double[2]; //单个点的坐标点集合
                    // 经纬度转换
                    foreach (Point3d point in col_point3d)
                    {
                        pointPositionList = new double[2] { Transform(point)[0], Transform(point)[1] };
                        singlePositionList.Add(pointPositionList);
                    } // 经纬度转换结束

                } // 单个实体几何坐标数量循环结束

                // UUID
                Entity entityLayer = (Entity)entity;

                Guid guid = new Guid();
                guid = Guid.NewGuid();
                string str = guid.ToString();
                uuid.Add(str);

                // 坐标
                geom.Add(singlePositionList);

                // 颜色
                if (entity is Point3d)
                {
                    colorList.Add("");
                    // 图层名
                    layerName.Add("无");
                }
                else
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(entityLayer.LayerId, OpenMode.ForRead);

                    string color;
                    color = entityLayer.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(entityLayer.LayerId) : ColorTranslator.ToHtml(entityLayer.Color.ColorValue);

                    colorList.Add(color);
                    // 图层名
                    layerName.Add(ltr.Name);
                }

                // 表名
                tableName.Add("a");

                // 属性索引
                // 获取每个闭合多段线对应的个体编号和用地代号
                ArrayList tagList = new ArrayList();
                PromptSelectionResult psrss = ed.SelectCrossingPolygon(col_point3d); // 获取闭合区域内实体方法

                if (psrss.Status == PromptStatus.OK)
                {
                    tagList.Clear();

                    SelectionSet SS = psrss.Value;
                    ObjectId[] idArray = SS.GetObjectIds();

                    // 如果读取的块参照数量大于1，取中心点在闭合多段线的块参照
                    if (idArray.Length > 1)
                    {
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            Entity ent1 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
                            if (ent1 is BlockReference)
                            {
                                BlockReference ent2 = (BlockReference)ent1;
                                if (IsInPolygon(ent2.Position, col_point3d))
                                {
                                    foreach (ObjectId rt in ((BlockReference)ent1).AttributeCollection)
                                    {
                                        DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
                                        AttributeReference acAttRef = dbObj as AttributeReference;

                                        tagList.Add(acAttRef.TextString);

                                        //MessageBox.Show("Tag: " + acAttRef.Tag + "\n" +
                                        //                "Value: " + acAttRef.TextString + "\n");
                                    }
                                }

                            }
                        }
                    }
                    // 如果读取的块参照数量等于1，取中心点在闭合多段线的块参照
                    else
                    {
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            Entity ent1 = (Entity)idArray[i].GetObject(OpenMode.ForRead);
                            if (ent1 is BlockReference)
                            {
                                foreach (ObjectId rt in ((BlockReference)ent1).AttributeCollection)
                                {
                                    DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
                                    AttributeReference acAttRef = dbObj as AttributeReference;

                                    tagList.Add(acAttRef.TextString);
                                }
                            }
                        }
                    }

                }
                // 如果地块编码属性只有两个属性值，attributeIndexList，如果少于2个或者多于2个都视为异常，添加空。
                if (tagList.Count == 2)
                {
                    attributeIndexList.Add(tagList[0] + "_" + tagList[1]);
                }
                else
                {
                    attributeIndexList.Add("");
                }

                // 属性
                readAttributeList attributeListObj = new readAttributeList();
                attributeList = attributeListObj.AttributeList();

                // 配套设施所属的地块UUID

                // 获取块参照的属性值
                ArrayList blockAttribute = new ArrayList();
                // 是否是地块编码本身
                string isBlockNum = "";

                // 如果这个块标注是幼儿园、公厕等，找对对应的地块编号或UUID
                if (entity is BlockReference)
                {
                    // 清除原有内容
                    blockAttribute.Clear();

                    // 如果entity有两个属性值，可以判断这是一个地块编码
                    if (((BlockReference)entity).AttributeCollection.Count == 2)
                    {
                        isBlockNum = "Block";
                    }

                    // 获取地块界限图层上的所有实体
                    ArrayList polylineEntities = new ArrayList();

                    // 找出地块界限里的所有闭合多段线，并判断当前块标注实体是否在某一个闭合多段线内，如果在，找出该闭合多段线内的块参照个体编号
                    TypedValue[] tvs =
                       new TypedValue[1] {
                            new TypedValue(
                              (int)DxfCode.LayerName,
                              "地块界限"
                            )
                       };

                    SelectionFilter sf = new SelectionFilter(tvs);
                    PromptSelectionResult psr = ed.SelectAll(sf);

                    if (psr.Status == PromptStatus.OK)
                    {
                        SelectionSet SS = psr.Value;
                        ObjectId[] idArray = SS.GetObjectIds();

                        //MessageBox.Show(idArray.Length.ToString());

                        Point3dCollection polylinePoint3d = new Point3dCollection();

                        for (int j = 0; j < idArray.Length; j++)
                        {
                            // 清除原有内容
                            polylinePoint3d.Clear();

                            Entity ent1 = trans.GetObject(idArray[j], OpenMode.ForWrite) as Entity;
                            if (ent1 is Polyline && (ent1 as Polyline).Closed)
                            {
                                Polyline polyline = (Polyline)ent1;

                                int vn = polyline.NumberOfVertices;
                                for (int w = 0; w < vn; w++)
                                {
                                    Point2d pt = polyline.GetPoint2dAt(w);
                                    polylinePoint3d.Add(new Point3d(pt.X, pt.Y, 0));
                                }

                                // 获取闭合多段线（地块）内的所有实体
                                PromptSelectionResult psrss2 = ed.SelectCrossingPolygon(polylinePoint3d);
                                if (psrss2.Status == PromptStatus.OK)
                                {
                                    SelectionSet SS2 = psrss2.Value;
                                    ObjectId[] idArray2 = SS2.GetObjectIds();

                                    // 如果读取的块参照数量大于1，且包含当前实体，找出当前块参照所在的闭合多段线
                                    if (idArray2.Length > 1)
                                    {
                                        for (int i = 0; i < idArray2.Length; i++)
                                        {
                                            Entity ent2 = (Entity)idArray2[i].GetObject(OpenMode.ForRead);

                                            // 判断是否是配套设施开始
                                            if (ent2 is BlockReference && (ent2 as BlockReference).Position.X == (entity as BlockReference).Position.X)
                                            {
                                                for (int k = 0; k < idArray2.Length; k++)
                                                {
                                                    Entity ent3 = (Entity)idArray2[k].GetObject(OpenMode.ForRead);
                                                    if (ent3 is BlockReference)
                                                    {
                                                        // 判断块参照中心点是否在闭合多段线内，只读取中心点在闭合多段线内的块参照
                                                        if (IsInPolygon((ent3 as BlockReference).Position, polylinePoint3d))
                                                        {
                                                            foreach (ObjectId rt in ((BlockReference)ent3).AttributeCollection)
                                                            {
                                                                DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
                                                                AttributeReference acAttRef = dbObj as AttributeReference;

                                                                blockAttribute.Add(acAttRef.TextString);
                                                            }
                                                        }
                                                    }
                                                }
                                            } // 判断是否是配套设施结束
                                        }
                                    }
                                } // 获取闭合多段线（地块）内的所有实体结束

                            }

                        }
                    }
                } // 如果这个块标注是幼儿园、公厕等，找对对应的地块编号或UUID 结束

                // 如果地块编码属性只有两个属性值，而且不是地块编码块参照，添加到parentId，如果少于2个或者多于2个都视为异常，添加空。
                if (blockAttribute.Count == 2 && isBlockNum != "Block")
                {
                    parentId.Add(blockAttribute[0] + "_" + blockAttribute[1]);
                }
                else
                {
                    parentId.Add("");
                }

                // 文字内容（GIS端展示）
                if (entity is DBText)
                {
                    textContent.Add((entity as DBText).TextString);
                }
                else if (entity is MText)
                {
                    textContent.Add((entity as MText).Text);
                }
                else if (entity is BlockReference)
                {
                    List<string> singleBlockContent = new List<string>();
                    string text = "";

                    if ((entity as BlockReference).AttributeCollection.Count > 0)
                    {
                        foreach (ObjectId rt in ((BlockReference)entity).AttributeCollection)
                        {
                            DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
                            AttributeReference acAttRef = dbObj as AttributeReference;

                            text = text + acAttRef.TextString + "//";
                        }
                        text = text.Substring(0, text.Length - 2);
                    }

                    textContent.Add(text);
                }
                else
                {
                    textContent.Add("");
                }

                // 块内容（GIS端展示）
                if (entity is BlockReference)
                {
                    List<string> singleBlockContent = new List<string>();
                    string text = "//";

                    foreach (ObjectId rt in ((BlockReference)entity).AttributeCollection)
                    {
                        DBObject dbObj = trans.GetObject(rt, OpenMode.ForRead) as DBObject;
                        AttributeReference acAttRef = dbObj as AttributeReference;

                        text = acAttRef.TextString + text;
                        //singleBlockContent.Add(acAttRef.TextString);
                    }
                    blockContent.Add(text);
                    //blockContent.Add(singleBlockContent);
                }
                else
                {
                    blockContent.Add("");
                }

            } // 所有的实体循环结束

            // 图例
            readAttributeList attributeListObj3 = new readAttributeList();
            //tuliList.Add(attributeListObj3.TuliList());
            tuliList.Add("");

            // 项目名
            //string projectIdBaseAddress = "http://172.18.84.70:8081/PDD/pdd/individual-manage!findAllProject.action";
            //var projectIdHttp = (HttpWebRequest)WebRequest.Create(new Uri(projectIdBaseAddress));

            //var response = projectIdHttp.GetResponse();

            //var stream = response.GetResponseStream();
            //var sr = new StreamReader(stream, Encoding.UTF8);
            //var content = sr.ReadToEnd();

            //MessageBox.Show(content);

            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";

            // 图表名或者叫文件名
            chartName = Path.GetFileName(ed.Document.Name);

            // 控规引导
            readAttributeList attributeListObj2 = new readAttributeList();
            kgGuide = attributeListObj2.KgGuide();

            //地理坐标系统编号
            srid = "4326";

            // 发文字信息
            RegulatoryPost.FenTuZe.FenTuZe.SendData(result, uuid, geom, colorList, type, layerName, tableName, attributeIndexList, attributeList, tuliList, projectId, chartName, kgGuide, srid, parentId, textContent, blockContent);
        }
        // 判断点是否在闭合多段线内
        public static bool IsInPolygon(Point3d checkPoint, Point3dCollection col_point2d)
        {
            bool inside = false;
            int pointCount = col_point2d.Count;
            Point3d p1, p2;
            for (int i = 0, j = pointCount - 1; i < pointCount; j = i, i++)//第一个点和最后一个点作为第一条线，之后是第一个点和第二个点作为第二条线，之后是第二个点与第三个点，第三个点与第四个点...
            {
                p1 = col_point2d[i];
                p2 = col_point2d[j];
                if (checkPoint.Y < p2.Y)
                {//p2在射线之上
                    if (p1.Y <= checkPoint.Y)
                    {//p1正好在射线中或者射线下方
                        if ((checkPoint.Y - p1.Y) * (p2.X - p1.X) > (checkPoint.X - p1.X) * (p2.Y - p1.Y))//斜率判断,在P1和P2之间且在P1P2右侧
                        {
                            //射线与多边形交点为奇数时则在多边形之内，若为偶数个交点时则在多边形之外。
                            //由于inside初始值为false，即交点数为零。所以当有第一个交点时，则必为奇数，则在内部，此时为inside=(!inside)
                            //所以当有第二个交点时，则必为偶数，则在外部，此时为inside=(!inside)
                            inside = (!inside);
                        }
                    }
                }
                else if (checkPoint.Y < p1.Y)
                {
                    //p2正好在射线中或者在射线下方，p1在射线上
                    if ((checkPoint.Y - p1.Y) * (p2.X - p1.X) < (checkPoint.X - p1.X) * (p2.Y - p1.Y))//斜率判断,在P1和P2之间且在P1P2右侧
                    {
                        inside = (!inside);
                    }
                }
            }
            return inside;
        }

        // 经纬度转换
        public static double[] Transform(Point3d point)
        {
            double xParam = 94.362163134086399;
            double yParam = -310.26525523306055;

            double xMultiple = 1.19862910076924;
            double yMultiple = 1;

            //旋转中心点
            double centerX = 114.00092403;
            double centerY = 36.14333070;

            //旋转角度
            double Angle = 0.064894377180536;

            double X = Math.Round(point.X, 7) + 4000000;
            double Y = Math.Round(point.Y, 7) + 38500000;

            //double X = 60139 + 4000000;
            //double Y = 34944 + 38500000;

            // 由高斯投影坐标反算成经纬度
            int ProjNo; int ZoneWide; ////带宽
            double[] output = new double[2];
            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;//latitude0,
            double e1, e2, f, a, ee, NN, T, C, M, D, R, u, fai, iPI;
            iPI = 0.0174532925199433; ////3.1415926535898/180.0;
            a = 6378245.0; f = 1.0 / 298.3; //54年北京坐标系参数
                                            //a = 6378140.0; f = 1 / 298.257; //80年西安坐标系参数
            ZoneWide = 6; ////6度带宽
            ProjNo = (int)(X / 1000000L); //查找带号
            longitude0 = (ProjNo - 1) * ZoneWide + ZoneWide / 2;
            longitude0 = longitude0 * iPI; //中央经线

            X0 = ProjNo * 1000000L + 500000L;
            Y0 = 0;
            xval = X - X0; yval = Y - Y0; //带内大地坐标
            e2 = 2 * f - f * f;
            e1 = (1.0 - Math.Sqrt(1 - e2)) / (1.0 + Math.Sqrt(1 - e2));
            ee = e2 / (1 - e2);
            M = yval;
            u = M / (a * (1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256));
            fai = u + (3 * e1 / 2 - 27 * e1 * e1 * e1 / 32) * Math.Sin(2 * u) + (21 * e1 * e1 / 16 - 55 * e1 * e1 * e1 * e1 / 32) * Math.Sin(4 * u)
            + (151 * e1 * e1 * e1 / 96) * Math.Sin(6 * u) + (1097 * e1 * e1 * e1 * e1 / 512) * Math.Sin(8 * u);
            C = ee * Math.Cos(fai) * Math.Cos(fai);
            T = Math.Tan(fai) * Math.Tan(fai);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(fai) * Math.Sin(fai));
            R = a * (1 - e2) / Math.Sqrt((1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin
            (fai) * Math.Sin(fai)));
            D = xval / NN;
            //计算经度(Longitude) 纬度(Latitude)
            longitude1 = longitude0 + (D - (1 + 2 * T + C) * D * D * D / 6 + (5 - 2 * C + 28 * T - 3 * C * C + 8 * ee + 24 * T * T) * D
            * D * D * D * D / 120) / Math.Cos(fai);
            latitude1 = fai - (NN * Math.Tan(fai) / R) * (D * D / 2 - (5 + 3 * T + 10 * C - 4 * C * C - 9 * ee) * D * D * D * D / 24
            + (61 + 90 * T + 298 * C + 45 * T * T - 256 * ee - 3 * C * C) * D * D * D * D * D * D / 720);
            //转换为度 DD

            // 现状图
            output[0] = longitude1 / iPI * xMultiple + xParam;
            output[1] = latitude1 / iPI * yMultiple + yParam;

            //output[0] = (lon1 - centerX) * Math.Cos(Angle) - (lat1 - centerY) * Math.Sin(Angle) + centerX;

            //output[1] = (lon1 - centerX) * Math.Sin(Angle) + (lat1 - centerY) * Math.Cos(Angle) + centerY;

            //output[0] = Math.Round(point.X, 7);
            //output[1] = Math.Round(point.Y, 7);

            return output;
        }

        public static double GetDistance(Point startPoint, Point endPoint)
        {
            int x = System.Math.Abs(endPoint.X - startPoint.X);
            int y = System.Math.Abs(endPoint.Y - startPoint.Y);
            return Math.Sqrt(x * x + y * y);
        }

        public bool AddXdata(ObjectId objId, string appName, string proStr)
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

                        Entity en = (Entity)trans.GetObject(objId, OpenMode.ForWrite);
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
    }
}
