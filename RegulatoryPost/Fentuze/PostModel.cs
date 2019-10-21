using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using RegulatoryModel.Model;
using RegulatoryModel.Command;
using System.Data;

namespace RegulatoryPost.FenTuZe
{
    public class PostModel
    {
        
        /// <summary>
        /// 用地规划、居住用地规划
        /// </summary>
        /// <param name="model"></param>
        public static void PostModelBase(AttributeBaseModel model)
        {
            try
            {
                ArrayList uuid = new ArrayList();
                ArrayList geom = new ArrayList();   // 坐标点集合
                ArrayList colorList = new ArrayList();       // 颜色集合
                ArrayList type = new ArrayList();       // 类型集合

                ArrayList layerName = new ArrayList();
                ArrayList tableName = new ArrayList(); // 表名
                System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
                attributeList.Columns.Add(new System.Data.DataColumn(("id"), typeof(string)));                                    
                ArrayList attributeIndexList = new ArrayList(); //属性索引集合

                ArrayList tuliList = new ArrayList(); //图例集合
                string projectId = ""; //项目ID
                string chartName = ""; //表名称
                ArrayList kgGuide = new ArrayList(); //控规引导

                string srid = ""; //地理坐标系统编号
                ArrayList parentId = new ArrayList(); //配套设施所在地块集合
                ArrayList textContent = new ArrayList(); // 文字内容（GIS端展示）
                ArrayList blockContent = new ArrayList(); // 块内容（GIS端展示）

                ArrayList zIndex = new ArrayList(); //图层级别
                List<string> selectedLayerList = new List<string>(); //图层
                List<string> individualName = new List<string>(); //图层
                List<string> individualFactor = new List<string>(); //图层
                List<string> individualCode = new List<string>(); //图层

                List<bool> dashes = new List<bool>(); //是否虚线

                int ysIndex = 0;

                Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

                // 图例
                tuliList.Add("");
                // 项目ID或叫城市ID
                projectId = model.projectId;
                // 图表名或者叫文件名
                chartName = model.DocName;
                // 控规引导
                if (model.kgGuide != null)
                {
                    kgGuide = model.kgGuide;
                }
                // 所选图层
                if (model.selectedLayerList != null)
                {
                    selectedLayerList = model.selectedLayerList;
                }

                //地理坐标系统编号
                srid = "4326";

                if (model.allLines != null)
                {
                    foreach (LayerModel layer in model.allLines)
                    {
                        if (layer.pointFs != null)
                        {
                            foreach (List<object> roadModel in layer.pointFs.Values)
                            {
                                string geoType = "";
                                foreach (object pf in roadModel)
                                {
                                    // 坐标
                                    if (pf is PointF)
                                    {
                                        ArrayList singlePoint = new ArrayList();
                                        geoType = "point";
                                        singlePoint.Add(Transform((PointF)pf));

                                        geom.Add(singlePoint);
                                        attributeIndexList.Add("");
                                        uuid.Add(GetUUID());
                                        zIndex.Add("0");

                                        colorList.Add(layer.Color);
                                        type.Add(geoType);
                                        layerName.Add(layer.Name);
                                        tableName.Add("a");

                                        parentId.Add("");
                                        textContent.Add("");
                                        blockContent.Add("");

                                        dashes.Add(false);

                                        individualName.Add("");
                                        individualFactor.Add("");
                                        individualCode.Add("");

                                    }
                                    if (pf is BlockInfoModel)
                                    {
                                        BlockInfoModel blm = pf as BlockInfoModel;

                                        if (blm.Arc != null && blm.Arc.Count > 0)
                                        {
                                            foreach (ArcModel arcModel in blm.Arc)
                                            {
                                                ArrayList singlePoint = new ArrayList();
                                                geoType = "polyline";
                                                foreach (PointF arPt in arcModel.pointList)
                                                {
                                                    singlePoint.Add(Transform(arPt));
                                                }
                                                geom.Add(singlePoint);
                                                string colid = "管线" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, arcModel.attItemList, colid);
                                                attributeIndexList.Add(colid); ysIndex++;

                                                zIndex.Add(arcModel.ZIndex);
                                                uuid.Add(GetUUID());
                                                colorList.Add(arcModel.Color);
                                                type.Add(geoType);

                                                layerName.Add(layer.Name);
                                                tableName.Add("a");
                                                parentId.Add("");
                                                textContent.Add("");

                                                blockContent.Add("");

                                                dashes.Add(arcModel.isDashed);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");
                                            }
                                        }
                                        if (blm.Circle != null && blm.Circle.Count > 0)
                                        {
                                            foreach (CircleModel circleModel in blm.Circle)
                                            {
                                                ArrayList singlePoint = new ArrayList();
                                                geoType = "polyline";

                                                foreach (PointF arPt in circleModel.pointList)
                                                {
                                                    singlePoint.Add(Transform(arPt));
                                                }
                                                zIndex.Add(circleModel.ZIndex);
                                                geom.Add(singlePoint);
                                                string colid = "索引" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, circleModel.attItemList, colid);

                                                attributeIndexList.Add(colid); ysIndex++;
                                                uuid.Add(GetUUID());
                                                colorList.Add(circleModel.Color);
                                                type.Add(geoType);

                                                layerName.Add(layer.Name);
                                                tableName.Add("a");
                                                parentId.Add("");
                                                textContent.Add("");

                                                blockContent.Add("");

                                                dashes.Add(circleModel.isDashed);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");
                                            }
                                        }
                                        if (blm.Ellipse != null && blm.Ellipse.Count > 0)
                                        {
                                            foreach (EllipseModel circleModel in blm.Ellipse)
                                            {
                                                ArrayList singlePoint = new ArrayList();
                                                geoType = "polyline";
                                                //singlePoint.Add(GetGemoSpecialJson(circleModel));

                                                foreach (PointF arpt in circleModel.pointList)
                                                {
                                                    singlePoint.Add(Transform(arpt));
                                                }

                                                zIndex.Add(circleModel.ZIndex);
                                                geom.Add(singlePoint);
                                                string colid = "索引" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, circleModel.attItemList, colid);

                                                attributeIndexList.Add(colid); ysIndex++;
                                                uuid.Add(GetUUID());
                                                colorList.Add(circleModel.Color);
                                                type.Add(geoType);

                                                layerName.Add(layer.Name);
                                                tableName.Add("a");
                                                parentId.Add("");
                                                textContent.Add("");

                                                blockContent.Add("");

                                                dashes.Add(circleModel.isDashed);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");
                                            }
                                        }
                                        if (blm.DbText != null)
                                        {
                                            foreach (DbTextModel circleModel in blm.DbText)
                                            {
                                                geoType = "text";

                                                geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                                string colid = "索引" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, circleModel.attItemList, colid);
                                                attributeIndexList.Add(colid); ysIndex++;

                                                uuid.Add(GetUUID());
                                                zIndex.Add(circleModel.ZIndex);
                                                colorList.Add(circleModel.Color);
                                                type.Add(geoType);

                                                layerName.Add(layer.Name);
                                                tableName.Add("a");
                                                parentId.Add("");
                                                textContent.Add("");

                                                blockContent.Add("");

                                                dashes.Add(false);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");

                                            }
                                        }
                                        if (blm.DimensionPositon != null)
                                        {

                                        }
                                        if (blm.Line != null && blm.Line.Count > 0)
                                        {
                                            foreach (LineModel lineModel in blm.Line)
                                            {
                                                geoType = "polyline";
                                                ArrayList arrayList = new ArrayList();

                                                arrayList.Add(Transform(lineModel.StartPoint));
                                                arrayList.Add(Transform(lineModel.EndPoint));

                                                geom.Add(arrayList);
                                                zIndex.Add(lineModel.ZIndex);
                                                string colid = "索引" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, lineModel.attItemList, colid);

                                                attributeIndexList.Add(colid); ysIndex++;
                                                uuid.Add(GetUUID());
                                                colorList.Add(layer.Color);
                                                type.Add(geoType);

                                                layerName.Add(layer.Name);
                                                tableName.Add("a");
                                                parentId.Add("");
                                                textContent.Add("");

                                                blockContent.Add("");

                                                dashes.Add(lineModel.isDashed);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");
                                            }

                                        }
                                        if (blm.PolyLine != null)
                                        {
                                            foreach (PolyLineModel arcModel in blm.PolyLine)
                                            {

                                                if (arcModel.Closed)
                                                {
                                                    geoType = "polyline";

                                                    ArrayList arrayList = new ArrayList();

                                                    try
                                                    {
                                                        foreach (PointF arPt in arcModel.Vertices)
                                                        {
                                                            arrayList.Add(Transform((PointF)arPt));
                                                        }

                                                        geom.Add(arrayList);

                                                        string colid = "索引" + ysIndex + "_U9-02";
                                                        GetAttributeTable(attributeList, arcModel.attItemList, colid);
                                                        attributeIndexList.Add(colid); ysIndex++;
                                                        zIndex.Add(arcModel.ZIndex);

                                                        uuid.Add(GetUUID());
                                                        colorList.Add(arcModel.Color);
                                                        type.Add(geoType);
                                                        layerName.Add(layer.Name);

                                                        tableName.Add("a");
                                                        parentId.Add("");
                                                        textContent.Add("");
                                                        blockContent.Add("");

                                                        individualName.Add("");
                                                        individualFactor.Add("");
                                                        individualCode.Add("");

                                                        dashes.Add(arcModel.isDashed);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        
                                                    }

                                                }
                                                else
                                                {

                                                    foreach (object arPt in arcModel.Vertices)
                                                    {
                                                        geoType = "polyline";

                                                        if (arPt is LineModel)
                                                        {
                                                            ArrayList arrayList = new ArrayList();
                                                            if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                            {
                                                                geoType = "point";
                                                                arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                            }
                                                            else
                                                            {
                                                                arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                                arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                            }
                                                            geom.Add(arrayList);

                                                        }
                                                        else if (arPt is ArcModel)
                                                        {
                                                            ArrayList arrayList = new ArrayList();
                                                            foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                            {
                                                                arrayList.Add(Transform(arPtt));

                                                            }
                                                            geom.Add(arrayList);

                                                        }

                                                        string colid = "索引" + ysIndex + "_U9-02";
                                                        GetAttributeTable(attributeList, arcModel.attItemList, colid);
                                                        attributeIndexList.Add(colid); ysIndex++;
                                                        zIndex.Add(arcModel.ZIndex);

                                                        uuid.Add(GetUUID());
                                                        colorList.Add(arcModel.Color);
                                                        type.Add(geoType);
                                                        layerName.Add(layer.Name);

                                                        tableName.Add("a");
                                                        parentId.Add("");
                                                        textContent.Add("");
                                                        blockContent.Add("");

                                                        dashes.Add(arcModel.isDashed);

                                                        individualName.Add(arcModel.individualName);
                                                        individualFactor.Add(arcModel.individualFactor);
                                                        individualCode.Add(arcModel.individualCode);
                                                    }

                                                }


                                            }
                                        }
                                        if (blm.Hatch != null)
                                        {
                                            geoType = "polygon";
                                            foreach (HatchModel arcModel in blm.Hatch)
                                            {

                                                foreach (int index in arcModel.loopPoints.Keys)
                                                {
                                                    ArrayList arrayList = new ArrayList();
                                                    //if (arcModel.loopPoints[index].Count < 4)
                                                    //{
                                                    //    continue;
                                                    //}
                                                    ColorAndPointItemModel cpModel = arcModel.loopPoints[index];
                                                    foreach (PointF arPt in cpModel.loopPoints)
                                                    {
                                                        arrayList.Add(Transform(arPt));
                                                    }
                                                    zIndex.Add(cpModel.ZIndex);
                                                    if (arrayList.Count > 0)
                                                    {
                                                        geom.Add(arrayList);
                                                        string colid = "索引" + ysIndex + "_U9-02";
                                                        GetAttributeTable(attributeList, cpModel.attItemList, colid);
                                                        attributeIndexList.Add(colid); ysIndex++;

                                                        uuid.Add(GetUUID());
                                                        colorList.Add(cpModel.Color);
                                                        type.Add(geoType);
                                                        layerName.Add(layer.Name);

                                                        tableName.Add("a");
                                                        parentId.Add("");
                                                        textContent.Add("");
                                                        blockContent.Add("");

                                                        dashes.Add(false);

                                                        individualName.Add("");
                                                        individualFactor.Add("");
                                                        individualCode.Add("");
                                                    }
                                                }

                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }
                }

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
                string kgGuideString = JsonConvert.SerializeObject(kgGuide);
                string parentIdString = JsonConvert.SerializeObject(parentId);
                string textContentString = JsonConvert.SerializeObject(textContent);

                string blockContentString = JsonConvert.SerializeObject(blockContent);
                string zindexstring = JsonConvert.SerializeObject(zIndex);
                string selectedLayerListstring = JsonConvert.SerializeObject(selectedLayerList);

                string individualNamestring = JsonConvert.SerializeObject(individualName);
                string individualFactorstring = JsonConvert.SerializeObject(individualFactor);
                string individualCodestring = JsonConvert.SerializeObject(individualCode);

                string dashesString = JsonConvert.SerializeObject(dashes);

                // UUID
                result.Add("uuid", uuidString);
                // 实体坐标信息
                result.Add("geom", geomString);
                // 实体颜色
                result.Add("colorList", colorListString);
                // 实体类型
                result.Add("type", typeString);

                // 图层
                result.Add("layerName", layerNameString);
                // 表名
                result.Add("tableName", tableNameString);
                // 实体属性索引
                result.Add("attributeIndexList", attributeIndexListString);
                // 实体属性
                result.Add("attributeList", attributeListString);

                // 图例
                result.Add("tuliList", GetLengedJsonString(model.LegendList));
                // 项目ID
                result.Add("projectId", projectId);
                // 自定义
                result.Add("chartName", chartName);
                // 实体属性
                result.Add("kgGuide", kgGuideString);
                // c层级索引
                result.Add("zIndex", zindexstring);

                // 坐标系代码
                result.Add("srid", srid);
                // 配套设施所属的地块编码
                result.Add("parentId", parentIdString);
                // 文字内容
                result.Add("textContent", textContentString);
                // 块内容
                result.Add("blockContent", blockContentString);

                // 块内容
                result.Add("selectedLayerList", selectedLayerListstring);
                result.Add("individualName", individualNamestring);
                result.Add("individualFactor", individualFactorstring);
                result.Add("individualCode", individualCodestring);

                result.Add("dashes", dashesString);

                FenTuZe.PostData(result);
            }
            catch
            { }
        }
        
        /// <summary>
        /// 单元图则、分图则、五线设施、其他
        /// </summary>
        /// <param name="model"></param>
        public static void PostModelBase(ModelBase model)
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
            ArrayList zIndex = new ArrayList(); //图层级别
            List<string> selectedLayerList = new List<string>(); //图层

            List<string> individualName = new List<string>(); //图层
            List<string> individualFactor = new List<string>(); //图层
            List<string> individualCode = new List<string>(); //图层

            List<bool> dashes = new List<bool>(); //是否虚线

            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

            if (model.attributeList != null)
            {
                attributeList = model.attributeList;
            }
            if (model.kgGuide != null)
            {
                kgGuide = model.kgGuide;
            }
            tuliList.Add("");
            projectId = model.projectId;
            chartName = model.DocName;
            srid = "4326";

            // 所选图层
            if (model.selectedLayerList != null)
            {
                selectedLayerList = model.selectedLayerList;
            }

            if (model.allLines != null)
            {
                foreach (LayerModel layer in model.allLines)
                {
                    // 特殊图源信息
                    if (layer.modelItemList != null)
                    {
                        string geoType = "";
                        foreach (PointsPlanItemModel ppim in layer.modelItemList)
                        {
                            if (ppim.Geom != null)
                            {
                                // 用地代码关联地块
                                if (ppim.Geom.Hatch != null)
                                {
                                    foreach (HatchModel arcModel in ppim.Geom.Hatch)
                                    {
                                        geoType = "hatch";
                                        foreach (int index in arcModel.loopPoints.Keys)
                                        {
                                            ArrayList arrayList = new ArrayList();
                                            ColorAndPointItemModel cpModel = arcModel.loopPoints[index];

                                            foreach (PointF arPt in cpModel.loopPoints)
                                            {
                                                arrayList.Add(Transform(arPt));
                                            }

                                            zIndex.Add(cpModel.ZIndex);
                                            if (arrayList.Count > 0)
                                            {
                                                geom.Add(arrayList);
                                                attributeIndexList.Add(ppim.Num != null ? "\"" + ppim.Num + "\"" + "_" + ppim.Num : "");
                                                uuid.Add(GetUUID());

                                                colorList.Add(cpModel.Color);
                                                type.Add(geoType);
                                                layerName.Add(layer.Name);
                                                tableName.Add("a");

                                                parentId.Add("");
                                                textContent.Add("");
                                                blockContent.Add("");

                                                dashes.Add(false);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");
                                            }
                                        }

                                    }
                                }

                                // 分图则地块代码关联地块
                                if (ppim.Geom.PolyLine != null)
                                {
                                    foreach (PolyLineModel arcModel in ppim.Geom.PolyLine)
                                    {
                                        foreach (object arPt in arcModel.Vertices)
                                        {
                                            geoType = "polyline";
                                            ArrayList arrayList = new ArrayList();

                                            if (arPt is LineModel)
                                            {
                                                if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                {
                                                    geoType = "point";
                                                    arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                }
                                                else
                                                {
                                                    if (((LineModel)arPt).StartPoint == null || ((LineModel)arPt).EndPoint == null)
                                                    {
                                                        geoType = "point";
                                                    }
                                                    arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                    arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                }
                                            }

                                            if (arPt is ArcModel)
                                            {
                                                if (((ArcModel)arPt).pointList[0] == ((ArcModel)arPt).pointList[1])
                                                {
                                                    geoType = "point";
                                                    arrayList.Add(Transform(((ArcModel)arPt).pointList[0]));
                                                }
                                                else
                                                {
                                                    if (((ArcModel)arPt).pointList.Count == 1)
                                                    {
                                                        geoType = "point";
                                                    }
                                                    foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                    {
                                                        arrayList.Add(Transform(arPtt));
                                                    }
                                                }
                                            }

                                            attributeIndexList.Add(ppim.Num != null ? ppim.Num : "");

                                            geom.Add(arrayList);
                                            zIndex.Add(arcModel.ZIndex);
                                            uuid.Add(GetUUID());

                                            colorList.Add(arcModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(arcModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }

                                    }
                                }
                                // 单元图则用地代码无关联地块
                                if (ppim.Geom.DbText != null)
                                {
                                    foreach (DbTextModel arcModel in ppim.Geom.DbText)
                                    {
                                        geoType = "text";

                                        geom.Add(new ArrayList() { Transform(arcModel.Position) });
                                        zIndex.Add(arcModel.ZIndex);
                                        attributeIndexList.Add(ppim.Num != null ? "\"" + ppim.Num + "\"" + "_" + ppim.Num : "");
                                        uuid.Add(GetUUID());

                                        colorList.Add(arcModel.Color);
                                        type.Add(geoType);
                                        layerName.Add(layer.Name);
                                        tableName.Add("a");

                                        parentId.Add("");
                                        textContent.Add("");
                                        blockContent.Add("");

                                        dashes.Add(false);

                                        individualName.Add("");
                                        individualFactor.Add("");
                                        individualCode.Add("");
                                    }
                                }
                            }

                        }
                    }

                    // 图元信息 
                    if (layer.pointFs != null)
                    {
                        foreach (List<object> roadModel in layer.pointFs.Values)
                        {
                            string geoType = "";
                            //int u = 0;
                            foreach (object pf in roadModel)
                            {
                                // 坐标
                                if (pf is PointF)
                                {
                                    ArrayList singlePoint = new ArrayList();
                                    geoType = "point";
                                    singlePoint.Add(Transform((PointF)pf));

                                    geom.Add(singlePoint);
                                    attributeIndexList.Add("");
                                    uuid.Add(GetUUID());
                                    zIndex.Add("0");

                                    colorList.Add(layer.Color);
                                    type.Add(geoType);
                                    layerName.Add(layer.Name);
                                    tableName.Add("a");

                                    parentId.Add("");
                                    textContent.Add("");
                                    blockContent.Add("");

                                    dashes.Add(false);

                                    individualName.Add("");
                                    individualFactor.Add("");
                                    individualCode.Add("");
                                }
                                if (pf is BlockInfoModel)
                                {
                                    BlockInfoModel blm = pf as BlockInfoModel;

                                    if (blm.Arc != null && blm.Arc.Count > 0)
                                    {
                                        foreach (ArcModel arcModel in blm.Arc)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            if (arcModel.pointList.Count == 1)
                                            {
                                                geoType = "point";
                                            }
                                            foreach (PointF arPt in arcModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }

                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            zIndex.Add(arcModel.ZIndex);
                                            uuid.Add(GetUUID());

                                            colorList.Add(arcModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(arcModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.Circle != null && blm.Circle.Count > 0)
                                    {
                                        foreach (CircleModel circleModel in blm.Circle)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            if (circleModel.pointList.Count == 1)
                                            {
                                                geoType = "point";
                                            }
                                            foreach (PointF arPt in circleModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }

                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            zIndex.Add(circleModel.ZIndex);
                                            uuid.Add(GetUUID());

                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(circleModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }

                                    if (blm.DbText != null)
                                    {
                                        foreach (DbTextModel circleModel in blm.DbText)
                                        {
                                            geoType = "text";

                                            geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());
                                            zIndex.Add(circleModel.ZIndex);

                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add(circleModel.Text);
                                            blockContent.Add("");

                                            dashes.Add(false);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.DimensionPositon != null)
                                    {

                                    }
                                    if (blm.Line != null && blm.Line.Count > 0)
                                    {
                                        foreach (LineModel lineModel in blm.Line)
                                        {
                                            geoType = "polyline";
                                            ArrayList arrayList = new ArrayList();

                                            if (((LineModel)lineModel).StartPoint == ((LineModel)lineModel).EndPoint)
                                            {
                                                geoType = "point";
                                                arrayList.Add(Transform(((LineModel)lineModel).StartPoint));
                                            }
                                            else
                                            {
                                                arrayList.Add(Transform(lineModel.StartPoint));
                                                arrayList.Add(Transform(lineModel.EndPoint));
                                            }

                                            geom.Add(arrayList);
                                            zIndex.Add(lineModel.ZIndex);
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());

                                            colorList.Add(layer.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(lineModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }

                                    }
                                    if (blm.PolyLine != null)
                                    {
                                        foreach (PolyLineModel arcModel in blm.PolyLine)
                                        {
                                            foreach (object arPt in arcModel.Vertices)
                                            {
                                                geoType = "polyline";

                                                ArrayList arrayList = new ArrayList();

                                                if (arPt is LineModel)
                                                {
                                                    if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                    {
                                                        geoType = "point";
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                    }
                                                    else
                                                    {
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                        arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                    }
                                                }

                                                if (arPt is ArcModel)
                                                {
                                                    if (((ArcModel)arPt).pointList[0] == ((ArcModel)arPt).pointList[1])
                                                    {
                                                        geoType = "point";
                                                        arrayList.Add(Transform(((ArcModel)arPt).pointList[0]));
                                                    }
                                                    else
                                                    {
                                                        if (((ArcModel)arPt).pointList.Count == 1)
                                                        {
                                                            geoType = "point";
                                                            arrayList.Add(Transform(((ArcModel)arPt).pointList[0]));
                                                        }
                                                        else
                                                        {
                                                            foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                            {
                                                                arrayList.Add(Transform(arPtt));
                                                            }
                                                        }

                                                    }
                                                }

                                                geom.Add(arrayList);
                                                zIndex.Add(arcModel.ZIndex);
                                                attributeIndexList.Add("");
                                                uuid.Add(GetUUID());

                                                colorList.Add(arcModel.Color);
                                                type.Add(geoType);
                                                layerName.Add(layer.Name);
                                                tableName.Add("a");

                                                parentId.Add("");
                                                textContent.Add("");
                                                blockContent.Add("");

                                                dashes.Add(arcModel.isDashed);

                                                individualName.Add(arcModel.individualName);
                                                individualFactor.Add(arcModel.individualFactor);
                                                individualCode.Add(arcModel.individualCode);
                                            }

                                        }
                                    }
                                    if (blm.Hatch != null)
                                    {
                                        foreach (HatchModel arcModel in blm.Hatch)
                                        {
                                            geoType = "polygon";

                                            foreach (int index in arcModel.loopPoints.Keys)
                                            {
                                                ArrayList arrayList = new ArrayList();

                                                ColorAndPointItemModel cpModel = arcModel.loopPoints[index];
                                                if (cpModel.loopPoints.Count == 1)
                                                {
                                                    geoType = "point";
                                                }
                                                foreach (PointF arPt in cpModel.loopPoints)
                                                {
                                                    arrayList.Add(Transform(arPt));
                                                }

                                                zIndex.Add(cpModel.ZIndex);
                                                if (arrayList.Count > 0)
                                                {
                                                    geom.Add(arrayList);
                                                    attributeIndexList.Add(arcModel.AttrIndex);
                                                    uuid.Add(GetUUID());
                                                    colorList.Add(cpModel.Color);

                                                    type.Add(geoType);
                                                    layerName.Add(layer.Name);
                                                    tableName.Add("a");
                                                    parentId.Add("");

                                                    textContent.Add("");
                                                    blockContent.Add("");

                                                    dashes.Add(false);

                                                    individualName.Add("");
                                                    individualFactor.Add("");
                                                    individualCode.Add("");
                                                }
                                            }

                                        }

                                    }

                                }

                            }
                        }

                    }// 图层数据结束

                }
            }
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
            string kgGuideString = JsonConvert.SerializeObject(kgGuide);
            string parentIdString = JsonConvert.SerializeObject(parentId);
            string textContentString = JsonConvert.SerializeObject(textContent);

            string blockContentString = JsonConvert.SerializeObject(blockContent);
            string zindexstring = JsonConvert.SerializeObject(zIndex);
            string selectedLayerListstring = JsonConvert.SerializeObject(selectedLayerList);

            string individualNamestring = JsonConvert.SerializeObject(individualName);
            string individualFactorstring = JsonConvert.SerializeObject(individualFactor);
            string individualCodestring = JsonConvert.SerializeObject(individualCode);

            string dashesString = JsonConvert.SerializeObject(dashes);

            result.Add("uuid", uuidString);
            result.Add("geom", geomString);
            result.Add("colorList", colorListString);
            result.Add("type", typeString);

            result.Add("layerName", layerNameString);
            result.Add("tableName", tableNameString);
            result.Add("attributeIndexList", attributeIndexListString);
            result.Add("attributeList", attributeListString);

            result.Add("tuliList", GetLengedJsonString(model.LegendList));
            result.Add("projectId", projectId);
            result.Add("chartName", chartName);
            result.Add("kgGuide", kgGuideString);
            result.Add("zIndex", zindexstring);

            result.Add("srid", srid);
            result.Add("parentId", parentIdString);
            result.Add("textContent", textContentString);
            result.Add("blockContent", blockContentString);

            result.Add("selectedLayerList", selectedLayerListstring);
            result.Add("individualName", individualNamestring);
            result.Add("individualFactor", individualFactorstring);
            result.Add("individualCode", individualCodestring);

            result.Add("dashes", dashesString);

            FenTuZe.PostData(result);
        }
        /// <summary>
        /// 单元图则、分图则、五线和设施图则自动发送
        /// </summary>
        /// <param name="model"></param>
        //attributeIndex数量和uuid相同，attributeIndex属性必须在attributeList属性列表里
        public static void AutoPostModelBase(ModelBase model, List<string> failedFiles)
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

            ArrayList zIndex = new ArrayList(); //图层级别
            List<string> selectedLayerList = new List<string>(); //图层
            List<string> individualName = new List<string>(); //图层
            List<string> individualFactor = new List<string>(); //图层
            List<string> individualCode = new List<string>(); //图层

            List<bool> dashes = new List<bool>(); //是否虚线

            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

            if (model.attributeList != null)
            {
                attributeList = model.attributeList;
            }
            if (model.kgGuide != null)
            {
                kgGuide = model.kgGuide;
            }
            tuliList.Add("");
            projectId = model.projectId;
            chartName = model.DocName;
            srid = "4326";

            // 所选图层
            if (model.LayerList != null)
            {
                selectedLayerList = model.LayerList;
            }


            if (model.allLines != null)
            {
                foreach (LayerModel layer in model.allLines)
                {
                    // 特殊图源信息
                    if (layer.modelItemList != null)
                    {
                        string geoType = "";
                        foreach (PointsPlanItemModel ppim in layer.modelItemList)
                        {
                            if (ppim.Geom != null)
                            {
                                // 用地代码关联地块
                                if (ppim.Geom.Hatch != null)
                                {
                                    foreach (HatchModel arcModel in ppim.Geom.Hatch)
                                    {
                                        geoType = "hatch";
                                        foreach (int index in arcModel.loopPoints.Keys)
                                        {
                                            ArrayList arrayList = new ArrayList();
                                            ColorAndPointItemModel cpModel = arcModel.loopPoints[index];

                                            foreach (PointF arPt in cpModel.loopPoints)
                                            {
                                                arrayList.Add(Transform(arPt));
                                            }

                                            zIndex.Add(cpModel.ZIndex);
                                            if (arrayList.Count > 0)
                                            {
                                                geom.Add(arrayList);
                                                attributeIndexList.Add(ppim.Num != null ? "\"" + ppim.Num + "\"" + "_" + ppim.Num : "");
                                                uuid.Add(GetUUID());

                                                colorList.Add(cpModel.Color);
                                                type.Add(geoType);
                                                layerName.Add(layer.Name);
                                                tableName.Add("a");

                                                parentId.Add("");
                                                textContent.Add("");
                                                blockContent.Add("");

                                                dashes.Add(false);

                                                individualName.Add("");
                                                individualFactor.Add("");
                                                individualCode.Add("");
                                            }
                                        }

                                    }
                                }

                                // 分图则地块代码关联地块
                                if (ppim.Geom.PolyLine != null)
                                {
                                    foreach (PolyLineModel arcModel in ppim.Geom.PolyLine)
                                    {
                                        geoType = "polyline";
                                        foreach (object arPt in arcModel.Vertices)
                                        {
                                            ArrayList arrayList = new ArrayList();

                                            if (arPt is LineModel)
                                            {
                                                if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                {
                                                    geoType = "point";
                                                    arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                }
                                                else
                                                {
                                                    if (((LineModel)arPt).StartPoint == null || ((LineModel)arPt).EndPoint == null)
                                                    {
                                                        geoType = "point";
                                                    }
                                                    arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                    arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                }
                                            }

                                            if (arPt is ArcModel)
                                            {
                                                if (((ArcModel)arPt).pointList[0] == ((ArcModel)arPt).pointList[1])
                                                {
                                                    geoType = "point";
                                                    arrayList.Add(Transform(((ArcModel)arPt).pointList[0]));
                                                }
                                                else
                                                {
                                                    if (((ArcModel)arPt).pointList.Count == 1)
                                                    {
                                                        geoType = "point";
                                                    }
                                                    foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                    {
                                                        arrayList.Add(Transform(arPtt));
                                                    }
                                                }
                                            }

                                            geom.Add(arrayList);
                                            zIndex.Add(arcModel.ZIndex);
                                            attributeIndexList.Add(ppim.Num != null ? ppim.Num : "");
                                            uuid.Add(GetUUID());

                                            colorList.Add(arcModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(arcModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }

                                    }
                                }
                                // 单元图则用地代码无关联地块
                                if (ppim.Geom.DbText != null)
                                {
                                    foreach (DbTextModel arcModel in ppim.Geom.DbText)
                                    {
                                        geoType = "text";

                                        geom.Add(new ArrayList() { Transform(arcModel.Position) });
                                        zIndex.Add(arcModel.ZIndex);
                                        attributeIndexList.Add(ppim.Num != null ? "\"" + ppim.Num + "\"" + "_" + ppim.Num : "");
                                        uuid.Add(GetUUID());

                                        colorList.Add(arcModel.Color);
                                        type.Add(geoType);
                                        layerName.Add(layer.Name);
                                        tableName.Add("a");

                                        parentId.Add("");
                                        textContent.Add("");
                                        blockContent.Add("");

                                        dashes.Add(false);

                                        individualName.Add("");
                                        individualFactor.Add("");
                                        individualCode.Add("");
                                    }
                                }
                            }

                        }
                    }

                    // 图元信息 
                    if (layer.pointFs != null)
                    {
                        foreach (List<object> roadModel in layer.pointFs.Values)
                        {
                            string geoType = "";
                            //int u = 0;
                            foreach (object pf in roadModel)
                            {
                                // 坐标
                                if (pf is PointF)
                                {
                                    ArrayList singlePoint = new ArrayList();
                                    geoType = "point";
                                    singlePoint.Add(Transform((PointF)pf));

                                    geom.Add(singlePoint);
                                    attributeIndexList.Add("");
                                    uuid.Add(GetUUID());
                                    zIndex.Add("0");

                                    colorList.Add(layer.Color);
                                    type.Add(geoType);
                                    layerName.Add(layer.Name);
                                    tableName.Add("a");

                                    parentId.Add("");
                                    textContent.Add("");
                                    blockContent.Add("");

                                    dashes.Add(false);

                                    individualName.Add("");
                                    individualFactor.Add("");
                                    individualCode.Add("");

                                }
                                if (pf is BlockInfoModel)
                                {
                                    BlockInfoModel blm = pf as BlockInfoModel;

                                    if (blm.Arc != null && blm.Arc.Count > 0)
                                    {
                                        foreach (ArcModel arcModel in blm.Arc)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            if (arcModel.pointList.Count == 1)
                                            {
                                                geoType = "point";
                                            }
                                            foreach (PointF arPt in arcModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }

                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            zIndex.Add(arcModel.ZIndex);
                                            uuid.Add(GetUUID());

                                            colorList.Add(arcModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(arcModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.Circle != null && blm.Circle.Count > 0)
                                    {
                                        foreach (CircleModel circleModel in blm.Circle)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            if (circleModel.pointList.Count == 1)
                                            {
                                                geoType = "point";
                                            }
                                            foreach (PointF arPt in circleModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }

                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            zIndex.Add(circleModel.ZIndex);
                                            uuid.Add(GetUUID());

                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(circleModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }

                                    if (blm.DbText != null)
                                    {
                                        foreach (DbTextModel circleModel in blm.DbText)
                                        {
                                            geoType = "text";

                                            geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());
                                            zIndex.Add(circleModel.ZIndex);

                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add(circleModel.Text);
                                            blockContent.Add("");

                                            dashes.Add(false);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.DimensionPositon != null)
                                    {

                                    }
                                    if (blm.Line != null && blm.Line.Count > 0)
                                    {
                                        foreach (LineModel lineModel in blm.Line)
                                        {
                                            geoType = "polyline";
                                            ArrayList arrayList = new ArrayList();

                                            if (((LineModel)lineModel).StartPoint == ((LineModel)lineModel).EndPoint)
                                            {
                                                geoType = "point";
                                                arrayList.Add(Transform(((LineModel)lineModel).StartPoint));
                                            }
                                            else
                                            {
                                                if (lineModel.StartPoint == null || lineModel.EndPoint == null)
                                                {
                                                    geoType = "point";
                                                }
                                                arrayList.Add(Transform(lineModel.StartPoint));
                                                arrayList.Add(Transform(lineModel.EndPoint));
                                            }

                                            geom.Add(arrayList);
                                            zIndex.Add(lineModel.ZIndex);
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());

                                            colorList.Add(layer.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);
                                            tableName.Add("a");

                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(lineModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }

                                    }
                                    if (blm.PolyLine != null)
                                    {
                                        foreach (PolyLineModel arcModel in blm.PolyLine)
                                        {
                                            foreach (object arPt in arcModel.Vertices)
                                            {
                                                geoType = "polyline";

                                                ArrayList arrayList = new ArrayList();

                                                if (arPt is LineModel)
                                                {
                                                    if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                    {
                                                        geoType = "point";
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                    }
                                                    else
                                                    {
                                                        if (((LineModel)arPt).StartPoint == null || ((LineModel)arPt).EndPoint == null)
                                                        {
                                                            geoType = "point";
                                                        }
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                        arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                    }
                                                }

                                                if (arPt is ArcModel)
                                                {
                                                    if (((ArcModel)arPt).pointList[0] == ((ArcModel)arPt).pointList[1])
                                                    {
                                                        geoType = "point";
                                                        arrayList.Add(Transform(((ArcModel)arPt).pointList[0]));
                                                    }
                                                    else
                                                    {
                                                        if (((ArcModel)arPt).pointList.Count == 1)
                                                        {
                                                            geoType = "point";
                                                        }
                                                        foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                        {
                                                            arrayList.Add(Transform(arPtt));
                                                        }
                                                    }
                                                }

                                                geom.Add(arrayList);
                                                zIndex.Add(arcModel.ZIndex);
                                                attributeIndexList.Add("");
                                                uuid.Add(GetUUID());

                                                colorList.Add(arcModel.Color);
                                                type.Add(geoType);
                                                layerName.Add(layer.Name);
                                                tableName.Add("a");

                                                parentId.Add("");
                                                textContent.Add("");
                                                blockContent.Add("");

                                                dashes.Add(arcModel.isDashed);

                                                individualName.Add(arcModel.individualName);
                                                individualFactor.Add(arcModel.individualFactor);
                                                individualCode.Add(arcModel.individualCode);

                                            }

                                        }
                                    }
                                    if (blm.Hatch != null)
                                    {
                                        geoType = "polygon";
                                        foreach (HatchModel arcModel in blm.Hatch)
                                        {

                                            foreach (int index in arcModel.loopPoints.Keys)
                                            {
                                                ArrayList arrayList = new ArrayList();

                                                ColorAndPointItemModel cpModel = arcModel.loopPoints[index];
                                                if (cpModel.loopPoints.Count == 1)
                                                {
                                                    geoType = "point";
                                                }
                                                foreach (PointF arPt in cpModel.loopPoints)
                                                {
                                                    arrayList.Add(Transform(arPt));
                                                }

                                                zIndex.Add(cpModel.ZIndex);
                                                if (arrayList.Count > 0)
                                                {
                                                    geom.Add(arrayList);
                                                    attributeIndexList.Add(arcModel.AttrIndex);
                                                    uuid.Add(GetUUID());
                                                    colorList.Add(cpModel.Color);

                                                    type.Add(geoType);
                                                    layerName.Add(layer.Name);
                                                    tableName.Add("a");
                                                    parentId.Add("");

                                                    textContent.Add("");
                                                    blockContent.Add("");

                                                    dashes.Add(false);

                                                    individualName.Add("");
                                                    individualFactor.Add("");
                                                    individualCode.Add("");
                                                }
                                            }

                                        }

                                    }

                                }

                            }
                        }

                    }// 图层数据结束

                }
            }
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
            string kgGuideString = JsonConvert.SerializeObject(kgGuide);
            string parentIdString = JsonConvert.SerializeObject(parentId);
            string textContentString = JsonConvert.SerializeObject(textContent);

            string blockContentString = JsonConvert.SerializeObject(blockContent);
            string zindexstring = JsonConvert.SerializeObject(zIndex);
            string selectedLayerListstring = JsonConvert.SerializeObject(selectedLayerList);
            string individualNamestring = JsonConvert.SerializeObject(individualName);
            string individualFactorstring = JsonConvert.SerializeObject(individualFactor);
            string individualCodestring = JsonConvert.SerializeObject(individualCode);

            string dashesString = JsonConvert.SerializeObject(dashes);

            result.Add("uuid", uuidString);
            result.Add("geom", geomString);
            result.Add("colorList", colorListString);
            result.Add("type", typeString);

            result.Add("layerName", layerNameString);
            result.Add("tableName", tableNameString);
            result.Add("attributeIndexList", attributeIndexListString);
            result.Add("attributeList", attributeListString);

            result.Add("tuliList", GetLengedJsonString(model.LegendList));
            result.Add("projectId", projectId);
            result.Add("chartName", chartName);
            result.Add("kgGuide", kgGuideString);
            result.Add("zIndex", zindexstring);

            result.Add("srid", srid);
            result.Add("parentId", parentIdString);
            result.Add("textContent", textContentString);
            result.Add("blockContent", blockContentString);

            result.Add("selectedLayerList", selectedLayerListstring);
            result.Add("individualName", individualNamestring);
            result.Add("individualFactor", individualFactorstring);
            result.Add("individualCode", individualCodestring);

            result.Add("dashes", dashesString);

            FenTuZe.AutoPostData(result, failedFiles);
        }
        /// <summary>
        /// 道路截面、道路名称、道路现状图纸
        /// </summary>
        /// <param name="model"></param>
        public static void PostModelBase(RoadModel model)
        {
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总
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

                ArrayList zIndex = new ArrayList(); //图层级别
                List<string> selectedLayerList = new List<string>(); //图层
                List<bool> dashes = new List<bool>(); //是否虚线

                List<string> individualName = new List<string>(); //图层
                List<string> individualFactor = new List<string>(); //图层
                List<string> individualCode = new List<string>(); //图层


                if (model.kgGuide != null)
                {
                    kgGuide = model.kgGuide;
                }
                projectId = model.projectId;
                chartName = model.DocName;
                srid = "4326";

                // 所选图层
                if (model.selectedLayerList != null)
                {
                    selectedLayerList = model.selectedLayerList;
                }

                foreach (LayerModel layer in model.allLines)
                {

                    if (layer.pointFs != null)
                    {
                        //图层数据
                        foreach (List<object> roadModel in layer.pointFs.Values)
                        {

                            string geoType = "";
                            foreach (object pf in roadModel)
                            {
                                // 坐标
                                if (pf is PointF)
                                {
                                    ArrayList singlePoint = new ArrayList();
                                    geoType = "polyline";
                                    singlePoint.Add(Transform((PointF)pf));

                                    geom.Add(singlePoint);
                                    attributeIndexList.Add("");
                                    uuid.Add(GetUUID());

                                    zIndex.Add("0");
                                    colorList.Add(layer.Color);
                                    type.Add(geoType);
                                    layerName.Add(layer.Name);

                                    tableName.Add("a");
                                    parentId.Add("");
                                    textContent.Add("");
                                    blockContent.Add("");

                                    dashes.Add(false);

                                    individualName.Add("");
                                    individualFactor.Add("");
                                    individualCode.Add("");
                                }
                                if (pf is BlockInfoModel)
                                {
                                    BlockInfoModel blm = pf as BlockInfoModel;

                                    if (blm.Arc != null && blm.Arc.Count > 0)
                                    {
                                        foreach (ArcModel arcModel in blm.Arc)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            foreach (PointF arPt in arcModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }

                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());

                                            zIndex.Add(arcModel.ZIndex);
                                            colorList.Add(arcModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);

                                            tableName.Add("a");
                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(arcModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.Circle != null && blm.Circle.Count > 0)
                                    {
                                        foreach (CircleModel circleModel in blm.Circle)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            foreach (PointF arPt in circleModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }

                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());

                                            zIndex.Add(circleModel.ZIndex);
                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);

                                            tableName.Add("a");
                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(circleModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.Ellipse != null && blm.Ellipse.Count > 0)
                                    {
                                        foreach (EllipseModel circleModel in blm.Ellipse)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            //singlePoint.Add(GetGemoSpecialJson(circleModel));

                                            foreach (PointF arpt in circleModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arpt));
                                            }
                                            geom.Add(singlePoint);
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());

                                            zIndex.Add(circleModel.ZIndex);
                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);

                                            tableName.Add("a");
                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(circleModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.DbText != null)
                                    {
                                        foreach (DbTextModel circleModel in blm.DbText)
                                        {
                                            geoType = "text";

                                            geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                            attributeIndexList.Add("");
                                            uuid.Add(GetUUID());

                                            zIndex.Add(circleModel.ZIndex);
                                            colorList.Add(circleModel.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);

                                            tableName.Add("a");
                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(false);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");

                                        }
                                    }
                                    if (blm.DimensionPositon != null)
                                    {

                                    }
                                    if (blm.Line != null && blm.Line.Count > 0)
                                    {
                                        foreach (LineModel lineModel in blm.Line)
                                        {
                                            geoType = "polyline";
                                            ArrayList arrayList = new ArrayList();

                                            arrayList.Add(Transform(lineModel.StartPoint));
                                            arrayList.Add(Transform(lineModel.EndPoint));

                                            geom.Add(arrayList);
                                            zIndex.Add(lineModel.ZIndex);
                                            attributeIndexList.Add("");

                                            uuid.Add(GetUUID());
                                            colorList.Add(layer.Color);
                                            type.Add(geoType);
                                            layerName.Add(layer.Name);

                                            tableName.Add("a");
                                            parentId.Add("");
                                            textContent.Add("");
                                            blockContent.Add("");

                                            dashes.Add(lineModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }

                                    }
                                    if (blm.PolyLine != null)
                                    {
                                        foreach (PolyLineModel arcModel in blm.PolyLine)
                                        {
                                            foreach (object arPt in arcModel.Vertices)
                                            {
                                                geoType = "polyline";

                                                if (arPt is LineModel)
                                                {
                                                    ArrayList arrayList = new ArrayList();
                                                    if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                    {
                                                        geoType = "point";
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                    }
                                                    else
                                                    {
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                        arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                    }
                                                    geom.Add(arrayList);
                                                }
                                                else if (arPt is ArcModel)
                                                {
                                                    ArrayList arrayList = new ArrayList();
                                                    foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                    {
                                                        arrayList.Add(Transform(arPtt));

                                                    }
                                                    geom.Add(arrayList);
                                                }

                                                zIndex.Add(arcModel.ZIndex);
                                                attributeIndexList.Add("");

                                                uuid.Add(GetUUID());
                                                colorList.Add(arcModel.Color);
                                                type.Add(geoType);
                                                layerName.Add(layer.Name);

                                                tableName.Add("a");
                                                parentId.Add("");
                                                textContent.Add("");
                                                blockContent.Add("");

                                                dashes.Add(arcModel.isDashed);

                                                individualName.Add(arcModel.individualName);
                                                individualFactor.Add(arcModel.individualFactor);
                                                individualCode.Add(arcModel.individualCode);
                                            }

                                        }
                                    }
                                    if (blm.Hatch != null)
                                    {
                                        geoType = "polygon";
                                        foreach (HatchModel arcModel in blm.Hatch)
                                        {

                                            foreach (int index in arcModel.loopPoints.Keys)
                                            {
                                                ArrayList arrayList = new ArrayList();
                                                //if (arcModel.loopPoints[index].Count < 4)
                                                //{
                                                //    continue;
                                                //}
                                                ColorAndPointItemModel cpModel = arcModel.loopPoints[index];
                                                foreach (PointF arPt in cpModel.loopPoints)
                                                {
                                                    arrayList.Add(Transform(arPt));
                                                }
                                                zIndex.Add(cpModel.ZIndex);
                                                if (arrayList.Count > 0)
                                                {
                                                    geom.Add(arrayList);
                                                    attributeIndexList.Add("");

                                                    uuid.Add(GetUUID());
                                                    colorList.Add(cpModel.Color);
                                                    type.Add(geoType);
                                                    layerName.Add(layer.Name);

                                                    tableName.Add("a");
                                                    parentId.Add("");
                                                    textContent.Add("");
                                                    blockContent.Add("");

                                                    dashes.Add(false);

                                                    individualName.Add("");
                                                    individualFactor.Add("");
                                                    individualCode.Add("");
                                                }
                                            }

                                        }

                                    }

                                }

                            }
                        }
                    }
                    if (layer.modelItemList != null)
                    {
                        //特殊数据
                        foreach (RoadInfoItemModel roadModel in layer.modelItemList)
                        {
                            // 如果是文字
                            if (roadModel.RoadNameType == "text")
                            {
                                for (int i = 0; i < roadModel.RoadName.Length; i++)
                                {
                                    //道路模型接口
                                    uuid.Add(GetUUID());

                                    // 单个文字坐标
                                    ArrayList singlePoint = new ArrayList();
                                    PointF pointf = new PointF(roadModel.RoadNameLocaiton[i].X, roadModel.RoadNameLocaiton[i].Y);
                                    singlePoint.Add(Transform(pointf));
                                    geom.Add(singlePoint);

                                    colorList.Add("");
                                    type.Add(roadModel.RoadNameType);
                                    layerName.Add(roadModel.RoadNameLayer);
                                    attributeIndexList.Add("");

                                    tableName.Add("a");
                                    parentId.Add("");
                                    textContent.Add(roadModel.RoadName[i]);
                                    blockContent.Add("");

                                    dashes.Add(false);

                                    individualName.Add("");
                                    individualFactor.Add("");
                                    individualCode.Add("");
                                }
                            }

                            //如果是道路多段线
                            if (roadModel.RoadType.ToLower() == "polyline")
                            {
                                uuid.Add(GetUUID());

                                ArrayList singlePoint = new ArrayList();
                                foreach (PointF point in roadModel.roadList)
                                {
                                    PointF pointf = new PointF(point.X, point.Y);
                                    singlePoint.Add(Transform(pointf));
                                }
                                geom.Add(singlePoint);

                                colorList.Add(roadModel.ColorIndex);
                                type.Add(roadModel.RoadType);

                                layerName.Add(layer.Name);
                                tableName.Add("a");
                                //row = attributeList.NewRow(); row["道路名称"] = roadModel.RoadName; attributeList.Rows.Add(row);
                                // 道路名称索引
                                if (roadModel.RoadName is null)
                                {
                                    attributeIndexList.Add("");
                                }
                                else
                                {
                                    string roadName= roadModel.RoadName + "_S1";
                                    attributeIndexList.Add(roadName);
                                    GetRoadAttributeTable(attributeList, roadModel.sectionList, roadModel.RoadName + "_S1");
                                }

                                parentId.Add("");
                                textContent.Add("");
                                blockContent.Add("");

                                dashes.Add(roadModel.isDashed);

                                individualName.Add(roadModel.individualName);
                                individualFactor.Add(roadModel.individualFactor);
                                individualCode.Add(roadModel.individualCode);
                            }

                        }
                    }
                }

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
                string kgGuideString = JsonConvert.SerializeObject(kgGuide);
                string parentIdString = JsonConvert.SerializeObject(parentId);
                string textContentString = JsonConvert.SerializeObject(textContent);

                string blockContentString = JsonConvert.SerializeObject(blockContent);
                string selectedLayerListstring = JsonConvert.SerializeObject(selectedLayerList);
                string individualNamestring = JsonConvert.SerializeObject(individualName);

                string individualFactorstring = JsonConvert.SerializeObject(individualFactor);
                string individualCodestring = JsonConvert.SerializeObject(individualCode);

                string dashesString = JsonConvert.SerializeObject(dashes); 
                // UUID
                result.Add("uuid", uuidString);
                // 实体坐标信息
                result.Add("geom", geomString);
                // 实体颜色
                result.Add("colorList", colorListString);
                // 实体类型
                result.Add("type", typeString);

                // 图层
                result.Add("layerName", layerNameString);
                // 表名
                result.Add("tableName", tableNameString);
                // 实体属性索引
                result.Add("attributeIndexList", attributeIndexListString);
                // 实体属性
                result.Add("attributeList", attributeListString);

                // 图例
                result.Add("tuliList", GetLengedJsonString(model.LegendList));
                // 项目ID
                result.Add("projectId", projectId);
                // 自定义
                result.Add("chartName", chartName);
                // 实体属性
                result.Add("kgGuide", kgGuideString);

                // 坐标系代码
                result.Add("srid", srid);
                // 配套设施所属的地块编码
                result.Add("parentId", parentIdString);
                // 文字内容
                result.Add("textContent", textContentString);
                // 块内容
                result.Add("blockContent", blockContentString);
                result.Add("selectedLayerList", selectedLayerListstring);

                result.Add("individualName", individualNamestring);
                result.Add("individualFactor", individualFactorstring);
                result.Add("individualCode", individualCodestring);
                result.Add("dashes", dashesString);
                
                FenTuZe.PostData(result);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 通用管道数据图纸
        /// </summary>
        /// <param name="model"></param>
        public static void PostModelBase(PipeModel model)
        {
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总
                ArrayList uuid = new ArrayList();
                ArrayList geom = new ArrayList();   // 坐标点集合
                ArrayList colorList = new ArrayList();       // 颜色集合
                ArrayList type = new ArrayList();       // 类型集合

                ArrayList layerName = new ArrayList();
                ArrayList tableName = new ArrayList(); // 表名
                System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
                 attributeList.Columns.Add(new System.Data.DataColumn(("id"), typeof(string))); 
                ArrayList attributeIndexList = new ArrayList(); //属性索引集合

                ArrayList tuliList = new ArrayList(); //图例集合
                string projectId = ""; //项目ID
                string chartName = ""; //表名称
                ArrayList kgGuide = new ArrayList(); //控规引导

                string srid = ""; //地理坐标系统编号
                ArrayList parentId = new ArrayList(); //配套设施所在地块集合
                ArrayList textContent = new ArrayList(); // 文字内容（GIS端展示）
                ArrayList blockContent = new ArrayList(); // 块内容（GIS端展示）

                ArrayList zIndex = new ArrayList(); // 块内容（GIS端展示）
                List<string> selectedLayerList = new List<string>(); //图层

                List<string> individualName = new List<string>(); //图层
                List<string> individualFactor = new List<string>(); //图层
                List<string> individualCode = new List<string>(); //图层

                List<bool> dashes = new List<bool>(); //是否虚线

                // 图例
                tuliList.Add("");
                // 项目ID或叫城市ID
                projectId = model.projectId;
                // 图表名或者叫文件名
                chartName = model.DocName;
                // 控规引导
                if (model.kgGuide != null)
                {
                    kgGuide = model.kgGuide;
                }

                //地理坐标系统编号
                srid = "4326";

                // 所选图层
                if (model.selectedLayerList != null)
                {
                    selectedLayerList = model.selectedLayerList;
                }

                int ysIndex = 0;
                foreach (LayerModel layer in model.allLines)
                {
                    //图层数据
                    if (layer.pointFs != null)
                    {
                        foreach (List<object> roadModel in layer.pointFs.Values)
                        {
                            string geoType = "";

                            foreach (object pf in roadModel)
                            {
                                // 坐标
                                if (pf is PointF)
                                {
                                    ArrayList singlePoint = new ArrayList();
                                    geoType = "point";
                                    singlePoint.Add(Transform((PointF)pf));

                                    geom.Add(singlePoint);
                                    // 道路名称表，入库需要
                                    //   row = attributeList.NewRow();
                                    //    row["1"] = "";

                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    uuid.Add(GetUUID());

                                    // 实体颜色
                                    colorList.Add(layer.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");

                                    dashes.Add(false);

                                    individualName.Add("");
                                    individualFactor.Add("");
                                    individualCode.Add("");
                                }
                                
                                if (pf is BlockInfoModel)
                                {
                                    BlockInfoModel blm = pf as BlockInfoModel;

                                    if (blm.Arc != null && blm.Arc.Count > 0)
                                    {
                                        foreach (ArcModel arcModel in blm.Arc)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            foreach (PointF arPt in arcModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }
                                            geom.Add(singlePoint);
                                            // 道路名称表，入库需要
                                            if (arcModel.attItemList.Count != 0)
                                            {
                                                string colid = "管线" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, arcModel.attItemList, colid);
                                                attributeIndexList.Add(colid); ysIndex++;
                                            }
                                            else
                                            {
                                                attributeIndexList.Add("");
                                            }

                                            zIndex.Add(arcModel.ZIndex);
                                            uuid.Add(GetUUID());

                                            // 实体颜色
                                            colorList.Add(arcModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);

                                            tableName.Add("a");
                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");

                                            dashes.Add(arcModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.Circle != null && blm.Circle.Count > 0)
                                    {
                                        foreach (CircleModel circleModel in blm.Circle)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";


                                            foreach (PointF arPt in circleModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arPt));
                                            }
                                            zIndex.Add(circleModel.ZIndex);
                                            geom.Add(singlePoint);

                                            // 道路名称表，入库需要
                                            if (circleModel.attItemList.Count != 0)
                                            {
                                                string colid = "管线" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, circleModel.attItemList, colid);
                                                attributeIndexList.Add(colid); ysIndex++;
                                            }
                                            else
                                            {
                                                attributeIndexList.Add("");
                                            }

                                            uuid.Add(GetUUID());

                                            // 实体颜色
                                            colorList.Add(circleModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");
                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");

                                            dashes.Add(circleModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.Ellipse != null && blm.Ellipse.Count > 0)
                                    {
                                        foreach (EllipseModel circleModel in blm.Ellipse)
                                        {
                                            ArrayList singlePoint = new ArrayList();
                                            geoType = "polyline";
                                            //singlePoint.Add(GetGemoSpecialJson(circleModel));

                                            foreach (PointF arpt in circleModel.pointList)
                                            {
                                                singlePoint.Add(Transform(arpt));
                                            }
                                            zIndex.Add(circleModel.ZIndex);
                                            geom.Add(singlePoint);
                                            // 道路名称表，入库需要
                                            //row = attributeList.NewRow();
                                            //row["1"] = "";
                                            //attributeList.Rows.Add(row);

                                            // 道路名称表，入库需要
                                            if (circleModel.attItemList.Count != 0)
                                            {
                                                string colid = "管线" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, circleModel.attItemList, colid);
                                                attributeIndexList.Add(colid); ysIndex++;
                                            }
                                            else
                                            {
                                                attributeIndexList.Add("");
                                            }

                                            uuid.Add(GetUUID());

                                            // 实体颜色
                                            colorList.Add(circleModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");

                                            dashes.Add(circleModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }
                                    }
                                    if (blm.DbText != null)
                                    {
                                        foreach (DbTextModel circleModel in blm.DbText)
                                        {
                                            geoType = "text";

                                            geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                            // 道路名称表，入库需要
                                            if (circleModel.attItemList.Count != 0)
                                            {
                                                string colid = "管线" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, circleModel.attItemList, colid);
                                                attributeIndexList.Add(colid); ysIndex++;
                                            }
                                            else
                                            {
                                                attributeIndexList.Add("");
                                            }

                                            uuid.Add(GetUUID());
                                            zIndex.Add(circleModel.ZIndex);

                                            // 实体颜色
                                            colorList.Add(circleModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");

                                            dashes.Add(false);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");

                                        }
                                    }
                                    if (blm.DimensionPositon != null)
                                    {

                                    }
                                    if (blm.Line != null && blm.Line.Count > 0)
                                    {
                                        foreach (LineModel lineModel in blm.Line)
                                        {
                                            geoType = "polyline";
                                            ArrayList arrayList = new ArrayList();

                                            arrayList.Add(Transform(lineModel.StartPoint));
                                            arrayList.Add(Transform(lineModel.EndPoint));
                                            geom.Add(arrayList);
                                            zIndex.Add(lineModel.ZIndex);

                                            // 道路名称表，入库需要
                                            if (lineModel.attItemList.Count != 0)
                                            {
                                                string colid = "管线" + ysIndex + "_U9-02";
                                                GetAttributeTable(attributeList, lineModel.attItemList, colid);
                                                // 道路名称索引
                                                attributeIndexList.Add(colid); ysIndex++;
                                            }
                                            else
                                            {
                                                attributeIndexList.Add("");
                                            }

                                            uuid.Add(GetUUID());
                                            // 实体颜色
                                            colorList.Add(layer.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");

                                            dashes.Add(lineModel.isDashed);

                                            individualName.Add("");
                                            individualFactor.Add("");
                                            individualCode.Add("");
                                        }

                                    }
                                    if (blm.PolyLine != null)
                                    {
                                        foreach (PolyLineModel arcModel in blm.PolyLine)
                                        {
                                            foreach (object arPt in arcModel.Vertices)
                                            {
                                                geoType = "polyline";
                                                string colid = "";

                                                if (arPt is LineModel)
                                                {
                                                    ArrayList arrayList = new ArrayList();
                                                    if (((LineModel)arPt).StartPoint == ((LineModel)arPt).EndPoint)
                                                    {
                                                        geoType = "point";
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                    }
                                                    else
                                                    {
                                                        arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                        arrayList.Add(Transform(((LineModel)arPt).EndPoint));
                                                    }
                                                    geom.Add(arrayList);

                                                    if (((LineModel)arPt).attItemList.Count != 0)
                                                    {
                                                        colid = "管线" + ysIndex + "_U9-02";
                                                        // 道路名称表，入库需要
                                                        GetAttributeTable(attributeList, ((LineModel)arPt).attItemList, colid);
                                                        // 道路名称索引
                                                        attributeIndexList.Add(colid); ysIndex++;
                                                    }
                                                    else
                                                    {
                                                        attributeIndexList.Add(""); 
                                                    }

                                                }
                                                else if (arPt is ArcModel)
                                                {
                                                    ArrayList arrayList = new ArrayList();
                                                    foreach (PointF arPtt in ((ArcModel)arPt).pointList)
                                                    {
                                                        arrayList.Add(Transform(arPtt));
                                                    }
                                                    geom.Add(arrayList);


                                                    if (((ArcModel)arPt).attItemList.Count != 0)
                                                    {
                                                        colid = "管线" + ysIndex + "_U9-02";
                                                        // 道路名称表，入库需要
                                                        GetAttributeTable(attributeList, ((ArcModel)arPt).attItemList, colid);
                                                        // 道路名称索引
                                                        attributeIndexList.Add(colid); ysIndex++;
                                                    }
                                                    else
                                                    {
                                                        attributeIndexList.Add("");
                                                    }
                                                }
                                                zIndex.Add(arcModel.ZIndex);

                                                uuid.Add(GetUUID());

                                                // 实体颜色
                                                colorList.Add(arcModel.Color);
                                                // 实体类型
                                                type.Add(geoType);

                                                // 实体所在图层名字
                                                layerName.Add(layer.Name);
                                                // 表名，默认a
                                                tableName.Add("a");

                                                //配套设施所在地块集合
                                                parentId.Add("");
                                                // 文字内容(单行文字、多行文字、块参照等)
                                                textContent.Add("");
                                                // 块内容
                                                blockContent.Add("");

                                                dashes.Add(arcModel.isDashed);

                                                individualName.Add(arcModel.individualName);
                                                individualFactor.Add(arcModel.individualFactor);
                                                individualCode.Add(arcModel.individualCode);
                                            }

                                        }
                                    }
                                    if (blm.Hatch != null)
                                    {
                                        geoType = "polygon";
                                        foreach (HatchModel arcModel in blm.Hatch)
                                        {

                                            foreach (int index in arcModel.loopPoints.Keys)
                                            {
                                                ArrayList arrayList = new ArrayList();

                                                ColorAndPointItemModel cpModel = arcModel.loopPoints[index];
                                                foreach (PointF arPt in cpModel.loopPoints)
                                                {
                                                    arrayList.Add(Transform(arPt));
                                                }
                                                zIndex.Add(cpModel.ZIndex);
                                                if (arrayList.Count > 0)
                                                {
                                                    geom.Add(arrayList);

                                                    // 道路名称表，入库需要
                                                    if (arcModel.attItemList.Count != 0)
                                                    {
                                                        string colid = "管线" + ysIndex + "_U9-02";
                                                        GetAttributeTable(attributeList, arcModel.attItemList, colid);
                                                        attributeIndexList.Add(colid); ysIndex++;
                                                    }
                                                    else
                                                    {
                                                        attributeIndexList.Add("");
                                                    }

                                                    uuid.Add(GetUUID());

                                                    // 实体颜色
                                                    colorList.Add(cpModel.Color);
                                                    // 实体类型
                                                    type.Add(geoType);

                                                    // 实体所在图层名字
                                                    layerName.Add(layer.Name);
                                                    // 表名，默认a
                                                    tableName.Add("a");

                                                    //配套设施所在地块集合
                                                    parentId.Add("");
                                                    // 文字内容(单行文字、多行文字、块参照等)
                                                    textContent.Add("");
                                                    // 块内容
                                                    blockContent.Add("");

                                                    dashes.Add(false);

                                                    individualName.Add("");
                                                    individualFactor.Add("");
                                                    individualCode.Add("");
                                                }
                                            }

                                        }

                                    }

                                }

                            }
                        }
                    }
                    
                }

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
                string kgGuideString =JsonConvert.SerializeObject(kgGuide);
                string parentIdString = JsonConvert.SerializeObject(parentId);
                string textContentString = JsonConvert.SerializeObject(textContent);

                string blockContentString = JsonConvert.SerializeObject(blockContent);
                string selectedLayerListstring = JsonConvert.SerializeObject(selectedLayerList);

                string individualNamestring = JsonConvert.SerializeObject(individualName);
                string individualFactorstring = JsonConvert.SerializeObject(individualFactor);
                string individualCodestring = JsonConvert.SerializeObject(individualCode);

                string dashString = JsonConvert.SerializeObject(dashes);

                // UUID
                result.Add("uuid", uuidString);
                // 实体坐标信息
                result.Add("geom", geomString);
                // 实体颜色
                result.Add("colorList", colorListString);
                // 实体类型
                result.Add("type", typeString);

                // 图层
                result.Add("layerName", layerNameString);
                // 表名
                result.Add("tableName", tableNameString);
                // 实体属性索引
                result.Add("attributeIndexList", attributeIndexListString);
                // 实体属性
                result.Add("attributeList", attributeListString);

                // 图例
                result.Add("tuliList", GetLengedJsonString(model.LegendList));
                // 项目ID
                result.Add("projectId", projectId);
                // 自定义
                result.Add("chartName", chartName);
                // 实体属性
                result.Add("kgGuide", kgGuideString);

                // 坐标系代码
                result.Add("srid", srid);
                // 配套设施所属的地块编码
                result.Add("parentId", parentIdString);
                // 文字内容
                result.Add("textContent", textContentString);
                // 块内容
                result.Add("blockContent", blockContentString);
                result.Add("selectedLayerList", selectedLayerListstring);

                result.Add("individualName", individualNamestring);
                result.Add("individualFactor", individualFactorstring);
                result.Add("individualCode", individualCodestring);

                result.Add("dashes", dashString);

                FenTuZe.PostData(result);
            }
            catch
            { }
        }
        /// <summary>
        /// 图例框数据图纸
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string GetLengedJsonString(List<LengedModel> list)
        {
            if (list == null)
            {
                return "";
            }
            string resultJson = "{\"data\":[";
            foreach (LengedModel item in list)
            {
                if (!string.IsNullOrEmpty(item.LayerName))
                {
                    resultJson += "{";
                    resultJson += JsonCommand.ToJson("titel", item.LayerName);
                    resultJson += JsonCommand.ToJson("backGround", item.BackGround == null ? "" : item.BackGround);
                    resultJson += "\"canvas\":{";
                    int num = 0;
                    foreach (BlockInfoModel bclItem in item.GemoModels)
                    {

                        if (bclItem.Arc.Count > 0)
                        {

                            foreach (ArcModel arcModel in bclItem.Arc)
                            {
                                resultJson += "\"arc" + num + "\":[";
                                foreach (PointF ptitem in arcModel.pointList)
                                {
                                    resultJson += GetPointItemJson(ptitem, arcModel.Color, false);

                                }
                                string temp = resultJson.TrimEnd(',');
                                resultJson = temp;
                                resultJson += "],";
                                num++;
                            }

                        }
                        if (bclItem.Circle.Count > 0)
                        {
                            foreach (CircleModel arcModel in bclItem.Circle)
                            {
                                resultJson += "\"circle" + num + "\":[";
                                foreach (PointF ptitem in arcModel.pointList)
                                {

                                    resultJson += GetPointItemJson(ptitem, arcModel.Color, false);

                                }
                                string temp = resultJson.TrimEnd(',');
                                resultJson = temp;
                                resultJson += "],";
                                num++;
                            }
                        }
                        if (bclItem.DbText.Count > 0)
                        {
                            foreach (DbTextModel arcModel in bclItem.DbText)
                            {
                                resultJson += "\"txt" + num + "\":[";


                                resultJson += GetTextItemJson(arcModel.Position, arcModel.Color, arcModel.Text);

                                string temp = resultJson.TrimEnd(',');
                                resultJson = temp;
                                resultJson += "],";
                                num++;
                            }
                        }
                        if (bclItem.Hatch.Count > 0)
                        {
                            foreach (HatchModel arcModel in bclItem.Hatch)
                            {

                                foreach (ColorAndPointItemModel cpitem in arcModel.loopPoints.Values)
                                {
                                    resultJson += "\"hatch" + num + "\":[";
                                    foreach (PointF ptitem in cpitem.loopPoints)
                                    {

                                        resultJson += GetPointItemJson(ptitem, cpitem.Color, true);

                                    }
                                    string temp = resultJson.TrimEnd(',');
                                    resultJson = temp;
                                    resultJson += "],";
                                    num++;
                                }

                            }

                        }
                        if (bclItem.Line.Count > 0)
                        {
                            foreach (LineModel arcModel in bclItem.Line)
                            {
                                resultJson += "\"line" + num + "\":[";

                                resultJson += GetPointItemJson(arcModel.StartPoint, arcModel.Color, false);

                                resultJson += GetPointItemJson(arcModel.EndPoint, arcModel.Color, false);

                                string temp = resultJson.TrimEnd(',');
                                resultJson = temp;
                                resultJson += "],";
                                num++;
                            }
                        }
                        if (bclItem.PolyLine.Count > 0)
                        {
                            foreach (PolyLineModel arcModel in bclItem.PolyLine)
                            {
                                foreach (object cpitem in arcModel.Vertices)
                                {
                                    if (cpitem is LineModel)
                                    {
                                        resultJson += "\"line" + num + "\":[";

                                        resultJson += GetPointItemJson((cpitem as LineModel).StartPoint, arcModel.Color, false);

                                        resultJson += GetPointItemJson((cpitem as LineModel).EndPoint, arcModel.Color, false);

                                        string temp = resultJson.TrimEnd(',');
                                        resultJson = temp;
                                        resultJson += "],";
                                        num++;
                                    }
                                    else if (cpitem is ArcModel)
                                    {
                                        resultJson += "\"arc" + num + "\":[";
                                        foreach (PointF ptitem in (cpitem as ArcModel).pointList)
                                        {
                                            resultJson += GetPointItemJson(ptitem, arcModel.Color, false);

                                        }
                                        string temp = resultJson.TrimEnd(',');
                                        resultJson = temp;
                                        resultJson += "],";
                                        num++;
                                    }
                                }
                            }
                        }

                    }
                    string temp1 = resultJson.TrimEnd(',');
                    resultJson = temp1;
                    resultJson += "}";
                    resultJson += "},";
                }
            }
            string temp2 = resultJson.TrimEnd(',');
            resultJson = temp2;
            return resultJson += "]}";
        }

        /// <summary>
        /// 点样式解析至Json
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="color"></param>
        /// <param name="isFill"></param>
        /// <returns></returns>
        private static string GetPointItemJson(PointF pt, string color, bool isFill)
        {
            string resultJson = "{";
            resultJson += JsonCommand.ToJson("IsEmpty", "false");
            if (isFill)
            {
                resultJson += JsonCommand.ToJson("fillstyle", color == null ? "" : color);
            }
            else
            {
                resultJson += JsonCommand.ToJson("style", color == null ? "" : color);
            }
            resultJson += JsonCommand.ToJson("x", pt.X);
            resultJson += JsonCommand.ToJson("y", pt.Y);
            string temp = resultJson.TrimEnd(',');
            resultJson = temp;
            resultJson += "},";
            return resultJson;
        }
        /// <summary>
        /// 文本类型解析至json
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="color"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string GetTextItemJson(PointF pt, string color, string txt)
        {
            string resultJson = "{";
            resultJson += JsonCommand.ToJson("IsEmpty", "false");
            resultJson += JsonCommand.ToJson("txt", txt);
            resultJson += JsonCommand.ToJson("style", color);
            resultJson += JsonCommand.ToJson("x", pt.X);
            resultJson += JsonCommand.ToJson("y", pt.Y);
            string temp = resultJson.TrimEnd(',');
            resultJson = temp;
            resultJson += "},";
            return resultJson;
        }

        public static double[] Transform(PointF point)
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

            if (output[0] == 114.67382285563774)
            {

            }

            return output;
        }

        public static string GetUUID()
        {
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
        /// <summary>
        /// 属性数据解析至表格数据
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="infos"></param>
        public static void GetAttributeTable(DataTable tb, List<AttributeItemModel> infos, string id)
        {
            if (id == "管线643_U9-02")
            {

            }
            try
            {
                DataRow dr = tb.NewRow();
                if (infos != null && infos.Count > 0)
                {

                    foreach (AttributeItemModel type in infos)
                    {
                        string colName = type.TargetName + Enum.GetName(typeof(AttributeItemType), type.AtItemType);
                        if (!tb.Columns.Contains(colName))
                        {
                            tb.Columns.Add(colName);
                        }
                        dr[colName] = type.AtValue;
                    }


                }
                dr["id"] = id;
                tb.Rows.Add(dr);
            }
            catch
            {
            }
        }

        public static void GetRoadAttributeTable(DataTable tb, List<RoadSectionItemModel> infos, string roadName)
        {
            try
            {
                DataRow dr = tb.NewRow();
                if (infos != null && infos.Count > 0)
                {
                    string type = "";

                    foreach (RoadSectionItemModel RoadSectionItem in infos)
                    {
                        if (!tb.Columns.Contains("横截面类型"))
                        {
                            tb.Columns.Add("横截面类型");
                        }
                        if (RoadSectionItem.SectionName != null)
                        {
                            type = RoadSectionItem.SectionName.Text;
                        }
                    }
                    dr["横截面类型"] = type + "-" + type;
                }
                if (!tb.Columns.Contains("道路名称"))
                {
                    tb.Columns.Add("道路名称");
                }
                dr["道路名称"] = roadName;
                tb.Rows.Add(dr);
            }
            catch
            {
            }
        }

        

    }
}
