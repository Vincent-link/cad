using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using RegulatoryPlan.Model;
using RegulatoryPost.FenTuZe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RegulatoryPlan.Method
{
    public class UnitPlanMethod<T> where T : UnitPlanModel
    {
        //手动选择
        public void ManualSelect(T model)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            FenTuZeMethod fen = new FenTuZeMethod();

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
                            fen.screenEntities(ent1, entities);
                        }
                    }

                }

                GetEntitiesInfo(entities, tr, btr, 5, doc, ed, model);
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
            lstlay.Add("控制单元范围");
            lstlay.Add("坐标");
            //lstlay.Add("幼儿园");
            //lstlay.Add("公厕");
            //lstlay.Add("地块编码");
            //lstlay.Add("城市黄线");
            //lstlay.Add("城市蓝线");
            //lstlay.Add("城市绿线");
            //lstlay.Add("道路红线");
            //lstlay.Add("路名");
            //lstlay.Add("建筑退让线");
            //lstlay.Add("禁止开口路段");
            //lstlay.Add("机动车出入口");

            return lstlay;
        }
        public void GetAllUnitPlaneInfo(T model)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr;
                btr = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                FenTuZeMethod fen = new FenTuZeMethod();
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
                        SelectionSet SS = psr.Value;
                        ObjectId[] idArray = SS.GetObjectIds();

                        for (int j = 0; j < idArray.Length; j++)
                        {
                            Entity ent1 = tr.GetObject(idArray[j], OpenMode.ForWrite) as Entity;
                            // 筛选实体
                            fen.screenEntities(ent1, entities);
                        }
                    }
                }

                MessageBox.Show(entities.Count.ToString());
                MessageBox.Show(info.Count.ToString());

                GetEntitiesInfo(entities, tr, btr, 5, doc, ed, model);
            }
        }

        public void GetEntitiesInfo(ArrayList entities, Transaction trans, BlockTableRecord btr, int numSample, Document doc, Editor ed, T model)
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
            ArrayList controlGuide = new ArrayList();//控规引导

            string srid = ""; //地理坐标系统编号
            ArrayList parentId = new ArrayList(); //配套设施所在地块集合
            ArrayList textContent = new ArrayList(); // 文字内容（GIS端展示）
            ArrayList blockContent = new ArrayList(); // 块内容（GIS端展示）

            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总


            // 遍历所有实体
            ReadDanAttributeList<ModelBase> attributeListObj = new ReadDanAttributeList<ModelBase>();
            int u = 0;
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
                        catch (Exception)
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
                        PostModel postModel = new PostModel();
                        PointF pointf = new PointF((float)point.X, (float)point.Y);
                        pointPositionList = new double[2] { postModel.Transform(pointf)[0], postModel.Transform(pointf)[1] };
                        singlePositionList.Add(pointPositionList);
                    } // 经纬度转换结束

                } // 单个实体几何坐标数量循环结束

                // UUID
                Entity entityLayer = (Entity)entity;
                //DBObject obj = trans.GetObject(entityLayer.ObjectId, OpenMode.ForRead);

                //ResultBuffer rb = obj.XData;
                //ArrayList xAttrs = new ArrayList();
                //if (rb != null)
                //{
                //    foreach (TypedValue tv in rb)
                //    {
                //        xAttrs.Add(tv.Value);
                //    }
                //}

                //if (rb == null || !xAttrs.Contains("UUID"))
                //{
                //    Guid guid = new Guid();
                //    guid = Guid.NewGuid();
                //    string str = guid.ToString();
                //    uuid.Add(str);
                //    AddXdata(entityLayer.ObjectId, "UUID", str);
                //}
                //else
                //{
                //    int n = 0;
                //    foreach (TypedValue tv in rb)
                //    {
                //        ed.WriteMessage("\n类型值{0} - 类型: {1}, 值: {2}", n, tv.TypeCode, tv.Value);
                //        uuid.Add(tv.Value);
                //        n++;
                //    }
                //    rb.Dispose();
                //}

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
                    layerName.Add("无");
                }
                else
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(entityLayer.LayerId, OpenMode.ForRead);
                    layerName.Add(ltr.Name);

                    string color;
                    color = entityLayer.ColorIndex == 256 ? MethodCommand.GetLayerColorByID(entityLayer.LayerId) : ColorTranslator.ToHtml(entityLayer.Color.ColorValue);
                    colorList.Add(color);
                }

                // 表名
                tableName.Add("a");

                // 属性索引
                attributeList = attributeListObj.AttributeList();

                // 获取每个闭合多段线对应的个体编号和用地代号
                ArrayList tagList = new ArrayList();
                // 如果地块编码属性只有两个属性值，attributeIndexList，如果少于2个或者多于2个都视为异常，添加空。
                if (tagList.Count == 2)
                {
                    attributeIndexList.Add(tagList[0] + "_" + tagList[1]);
                }
                else
                {
                    //string[] compositionTableIndex = new string[22] { "\"R\"_R", "R2", "A", "A1", "A33", "A7", "A9", "B", "B1", "B41", "S", "S1", "S42", "U", "U15", "G", "G1", "G2", "G3", "H11", "E", "E1" };
                    // 每次至少要发超过用地代码数量的实体
                    if (u < attributeList.Rows.Count)
                    {
                        string str2 = "\"" + attributeList.Rows[u]["用地代码"] + "\"" + "_" + attributeList.Rows[u]["用地代码"];
                        attributeIndexList.Add(str2);
                    }
                    else
                    {
                        attributeIndexList.Add("");
                    }
                }
                u++;

                // 获取块参照的属性值
                ArrayList blockAttribute = new ArrayList();
                // 是否是地块编码本身
                string isBlockNum = "";
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
            //List<BlockInfoModel> gemoModels = new List<BlockInfoModel>();
            //List<PointF> boxPointList = new List<PointF>();
            //List<string> nameList = new List<string>();

            //foreach (LengedModel lengedModel in model.LegendList)
            //{
            //    foreach (BlockInfoModel blockinfo in lengedModel.GemoModels)
            //    {
            //        gemoModels.Add(blockinfo);
            //    }

            //    foreach (PointF pointf in lengedModel.BoxPointList)
            //    {
            //        boxPointList.Add(pointf);
            //    }

            //    nameList.Add(lengedModel.LayerName);

            //}
            //tuliList.Add(gemoModels);
            //tuliList.Add(boxPointList);
            //tuliList.Add(nameList);

            tuliList.Add("");

            // 项目名
            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";

            // 图表名或者叫文件名
            chartName = Path.GetFileName(ed.Document.Name);

            // 属性
            attributeList = attributeListObj.AttributeList();

            // 控规要求
            controlGuide = attributeListObj.ControlList();

            //地理坐标系统编号
            srid = "4326";

            // JSON化
            string uuidString = JsonConvert.SerializeObject(uuid);
            string geomString = JsonConvert.SerializeObject(geom);
            string colorListString = JsonConvert.SerializeObject(colorList);
            string typeString = JsonConvert.SerializeObject(type);

            string layerNameString = JsonConvert.SerializeObject(layerName);
            string tableNameString = JsonConvert.SerializeObject(tableName);
            string attributeIndexListString = JsonConvert.SerializeObject(attributeIndexList);
            string attributeListString = JsonConvert.SerializeObject(attributeList);

            string tuliListString = JsonConvert.SerializeObject(tuliList);
            string kgGuideString = JsonConvert.SerializeObject(controlGuide);
            string parentIdString = JsonConvert.SerializeObject(parentId);
            string textContentString = JsonConvert.SerializeObject(textContent);

            string blockContentString = JsonConvert.SerializeObject(blockContent);

            result.Add("uuid", uuidString);
            result.Add("geom", geomString);
            result.Add("colorList", colorListString);
            result.Add("type", typeString);

            result.Add("layerName", layerNameString);
            result.Add("tableName", tableNameString);
            result.Add("attributeIndexList", attributeIndexListString);
            result.Add("attributeList", attributeListString);

            result.Add("tuliList", tuliListString);
            result.Add("projectId", projectId);
            result.Add("chartName", chartName);
            result.Add("kgGuide", kgGuideString);

            result.Add("srid", srid);
            result.Add("parentId", parentIdString);
            result.Add("textContent", textContentString);
            result.Add("blockContent", blockContentString);

            LayerModel lm = new LayerModel();
            lm.modelItemList = new List<object>();

            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            model.allLines.Add(lm);
            lm.modelItemList.Add(result);
        }

        /// <summary>
        /// 对实体进行写属性
        /// </summary>
        /// <param name="objId">实体id</param>
        /// <param name="appName">外部数据名</param>
        /// <param name="proStr">属性</param>
        /// <returns>true: 成功 false: 失败</returns>
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