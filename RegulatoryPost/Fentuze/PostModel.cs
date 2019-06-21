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

namespace RegulatoryPost.FenTuZe
{
    public class PostModel
    {
        public static void PostModelBase(ModelBase model)
        {
            ArrayList uuid = new ArrayList();
            ArrayList geom = new ArrayList();   // 坐标点集合
            ArrayList colorList = new ArrayList();       // 颜色集合
            ArrayList type = new ArrayList();       // 类型集合

            ArrayList layerName = new ArrayList();
            ArrayList tableName = new ArrayList(); // 表名
            System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
            attributeList.Columns.Add(new System.Data.DataColumn("1"));
            System.Data.DataRow row;
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

           

            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总
            if (model.allLines != null)
            {
                foreach (LayerModel layer in model.allLines)
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
                                geoType = "polyline";
                                singlePoint.Add(Transform((PointF)pf));

                                geom.Add(singlePoint);
                                // 道路名称表，入库需要
                                row = attributeList.NewRow();
                                row["1"] = "";
                                attributeList.Rows.Add(row);
                                // 道路名称索引
                                attributeIndexList.Add("");

                                // UUID
                                Guid guid = new Guid();
                                guid = Guid.NewGuid();
                                string str = guid.ToString();
                                uuid.Add(str);
                                zIndex.Add("0");



                                // 实体颜色
                                colorList.Add(layer.Color);
                                // 实体类型
                                type.Add(geoType);

                                // 实体所在图层名字
                                layerName.Add(layer.Name);
                                // 表名，默认a
                                tableName.Add("a");

                                // 图例
                                tuliList.Add("");
                                // 项目ID或叫城市ID
                                projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                // 图表名或者叫文件名
                                chartName = "123";
                                // 控规引导
                                kgGuide.Add("");

                                //地理坐标系统编号
                                srid = "4326";
                                //配套设施所在地块集合
                                parentId.Add("");
                                // 文字内容(单行文字、多行文字、块参照等)
                                textContent.Add("");
                                // 块内容
                                blockContent.Add("");


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
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");
                                        zIndex.Add(arcModel.ZIndex);
                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);

                                        // 实体颜色
                                        colorList.Add(arcModel.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");
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
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");

                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);




                                        // 实体颜色
                                        colorList.Add(circleModel.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");
                                    }
                                }

                                if (blm.DbText != null)
                                {
                                    foreach (DbTextModel circleModel in blm.DbText)
                                    {
                                        geoType = "text";

                                        geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                        // 道路名称表，入库需要
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");

                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);
                                        zIndex.Add(circleModel.ZIndex);



                                        // 实体颜色
                                        colorList.Add(circleModel.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");

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
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");

                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);




                                        // 实体颜色
                                        colorList.Add(layer.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");
                                    }

                                }
                                if (blm.PolyLine != null)
                                {
                                    foreach (PolyLineModel arcModel in blm.PolyLine)
                                    {
                                        geoType = "polyline";
                                        foreach (object arPt in arcModel.Vertices)
                                        {
                                            if (arPt is LineModel)
                                            {
                                                ArrayList arrayList = new ArrayList();

                                                arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                                arrayList.Add(Transform(((LineModel)arPt).EndPoint));
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
                                            // 道路名称表，入库需要
                                            row = attributeList.NewRow();
                                            row["1"] = "";
                                            attributeList.Rows.Add(row);
                                            // 道路名称索引
                                            attributeIndexList.Add("");

                                            // UUID
                                            Guid guid = new Guid();
                                            guid = Guid.NewGuid();
                                            string str = guid.ToString();
                                            uuid.Add(str);




                                            // 实体颜色
                                            colorList.Add(arcModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            // 图例
                                            tuliList.Add("");
                                            // 项目ID或叫城市ID
                                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                            // 图表名或者叫文件名
                                            chartName = "123";
                                            // 控规引导
                                            kgGuide.Add("");

                                            //地理坐标系统编号
                                            srid = "4326";
                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");
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
                                                // 道路名称表，入库需要
                                                row = attributeList.NewRow();
                                                row["1"] = "";
                                                attributeList.Rows.Add(row);
                                                // 道路名称索引
                                                attributeIndexList.Add("");

                                                // UUID
                                                Guid guid = new Guid();
                                                guid = Guid.NewGuid();
                                                string str = guid.ToString();
                                                uuid.Add(str);




                                                // 实体颜色
                                                colorList.Add(cpModel.Color);
                                                // 实体类型
                                                type.Add(geoType);

                                                // 实体所在图层名字
                                                layerName.Add(layer.Name);
                                                // 表名，默认a
                                                tableName.Add("a");

                                                // 图例
                                                tuliList.Add("");
                                                // 项目ID或叫城市ID
                                                projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                                // 图表名或者叫文件名
                                                chartName = "123";
                                                // 控规引导
                                                kgGuide.Add("");

                                                //地理坐标系统编号
                                                srid = "4326";
                                                //配套设施所在地块集合
                                                parentId.Add("");
                                                // 文字内容(单行文字、多行文字、块参照等)
                                                textContent.Add("");
                                                // 块内容
                                                blockContent.Add("");
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

            FenTuZe.PostData(result);
        }


        private static string GetLengedJsonString(List<LengedModel> list)
        {
            string resultJson = "{\"data\":[";
            foreach (LengedModel item in list)
            {
                if (!string.IsNullOrEmpty(item.LayerName))
                {
                    resultJson += "{";
                    resultJson += JsonCommand.ToJson("titel", item.LayerName);
                    resultJson += JsonCommand.ToJson("backGround", item.BackGround == null ? "" :item.BackGround);
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
            return resultJson+="]}";
        }

        private static string GetPointItemJson(PointF pt, string color,bool isFill)
        {
            string resultJson = "{";
            resultJson += JsonCommand.ToJson("IsEmpty", "false");
            if (isFill)
            {
                resultJson += JsonCommand.ToJson("fillstyle", color==null?"":color);
            }
            else {
                resultJson += JsonCommand.ToJson("style", color == null ? "" : color);
            }
            resultJson += JsonCommand.ToJson("x", pt.X);
            resultJson += JsonCommand.ToJson("y", pt.Y);
            string temp = resultJson.TrimEnd(',');
            resultJson = temp;
            resultJson += "},";
            return resultJson;
        }
        private static string GetTextItemJson(PointF pt, string color,string txt )
        {
            string resultJson = "{";
            resultJson += JsonCommand.ToJson("IsEmpty", "false");
            resultJson += JsonCommand.ToJson("txt",txt);
            resultJson += JsonCommand.ToJson("style", color);
            resultJson += JsonCommand.ToJson("x", pt.X);
            resultJson += JsonCommand.ToJson("y", pt.Y);
            string temp = resultJson.TrimEnd(',');
            resultJson = temp;
            resultJson += "},";
            return resultJson;
        }
        public static void PostModelBase(RoadSectionModel model)
        {
            ArrayList uuid = new ArrayList();
            ArrayList geom = new ArrayList();   // 坐标点集合
            ArrayList colorList = new ArrayList();       // 颜色集合
            ArrayList type = new ArrayList();       // 类型集合

            ArrayList layerName = new ArrayList();
            ArrayList tableName = new ArrayList(); // 表名
            System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
            attributeList.Columns.Add(new System.Data.DataColumn(("道路名称"), typeof(string))); System.Data.DataRow row;
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
            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

            foreach (LayerModel layer in model.allLines)
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
                            geoType = "polyline";
                            singlePoint.Add(Transform((PointF)pf));

                            geom.Add(singlePoint);
                            // 道路名称表，入库需要
                            row = attributeList.NewRow();
                            row["1"] = "";
                            attributeList.Rows.Add(row);
                            // 道路名称索引
                            attributeIndexList.Add("");

                            // UUID
                            Guid guid = new Guid();
                            guid = Guid.NewGuid();
                            string str = guid.ToString();
                            uuid.Add(str);
                            zIndex.Add("0");



                            // 实体颜色
                            colorList.Add(layer.Color);
                            // 实体类型
                            type.Add(geoType);

                            // 实体所在图层名字
                            layerName.Add(layer.Name);
                            // 表名，默认a
                            tableName.Add("a");

                            // 图例
                            tuliList.Add("");
                            // 项目ID或叫城市ID
                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                            // 图表名或者叫文件名
                            chartName = "123";
                            // 控规引导
                            kgGuide.Add("");

                            //地理坐标系统编号
                            srid = "4326";
                            //配套设施所在地块集合
                            parentId.Add("");
                            // 文字内容(单行文字、多行文字、块参照等)
                            textContent.Add("");
                            // 块内容
                            blockContent.Add("");


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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");
                                    zIndex.Add(arcModel.ZIndex);
                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);

                                    // 实体颜色
                                    colorList.Add(arcModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);




                                    // 实体颜色
                                    colorList.Add(circleModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
                                }
                            }

                            if (blm.DbText != null)
                            {
                                foreach (DbTextModel circleModel in blm.DbText)
                                {
                                    geoType = "text";

                                    geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                    // 道路名称表，入库需要
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);
                                    zIndex.Add(circleModel.ZIndex);



                                    // 实体颜色
                                    colorList.Add(circleModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");

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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);




                                    // 实体颜色
                                    colorList.Add(layer.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
                                }

                            }
                            if (blm.PolyLine != null)
                            {
                                foreach (PolyLineModel arcModel in blm.PolyLine)
                                {
                                    geoType = "polyline";
                                    foreach (object arPt in arcModel.Vertices)
                                    {
                                        if (arPt is LineModel)
                                        {
                                            ArrayList arrayList = new ArrayList();

                                            arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                            arrayList.Add(Transform(((LineModel)arPt).EndPoint));
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
                                        // 道路名称表，入库需要
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");

                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);




                                        // 实体颜色
                                        colorList.Add(arcModel.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");
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
                                            // 道路名称表，入库需要
                                            row = attributeList.NewRow();
                                            row["1"] = "";
                                            attributeList.Rows.Add(row);
                                            // 道路名称索引
                                            attributeIndexList.Add("");

                                            // UUID
                                            Guid guid = new Guid();
                                            guid = Guid.NewGuid();
                                            string str = guid.ToString();
                                            uuid.Add(str);




                                            // 实体颜色
                                            colorList.Add(cpModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            // 图例
                                            tuliList.Add("");
                                            // 项目ID或叫城市ID
                                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                            // 图表名或者叫文件名
                                            chartName = "123";
                                            // 控规引导
                                            kgGuide.Add("");

                                            //地理坐标系统编号
                                            srid = "4326";
                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");
                                        }
                                    }

                                }

                            }

                        }

                    }
                }
                foreach (RoadInfoItemModel roadModel in layer.modelItemList)
                {
                    // 如果是文字
                    if (roadModel.RoadNameType == "text")
                    {
                        for (int i = 0; i < roadModel.RoadName.Length; i++)
                        {
                            //道路模型接口
                            // UUID
                            Guid guid = new Guid();
                            guid = Guid.NewGuid();
                            string str = guid.ToString();
                            uuid.Add(str);

                            // 单个文字坐标
                            ArrayList singlePoint = new ArrayList();
                            PointF pointf = new PointF(roadModel.RoadNameLocaiton[i].X, roadModel.RoadNameLocaiton[i].Y);
                            singlePoint.Add(Transform(pointf));
                            geom.Add(singlePoint);

                            // 实体颜色
                            colorList.Add("");
                            // 实体类型
                            type.Add(roadModel.RoadNameType);

                            // 实体所在图层名字
                            layerName.Add(roadModel.RoadNameLayer);
                            // 表名，默认a
                            tableName.Add("a");
                            // 道路名称表，入库需要
                            row = attributeList.NewRow(); row["道路名称"] = roadModel.RoadName; attributeList.Rows.Add(row);
                            // 道路名称索引
                            attributeIndexList.Add("");

                            // 图例
                            tuliList.Add("");
                            // 项目ID或叫城市ID
                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                            // 图表名或者叫文件名
                            chartName = "123";
                            // 控规引导
                            kgGuide.Add("");

                            //地理坐标系统编号
                            srid = "4326";
                            //配套设施所在地块集合
                            parentId.Add("");
                            // 文字内容(单行文字、多行文字、块参照等)
                            textContent.Add(roadModel.RoadName[i]);
                            // 块内容
                            blockContent.Add("");
                        }
                    }

                    //如果是道路多段线
                    if (roadModel.RoadType.ToLower() == "polyline")
                    {
                        // UUID
                        Guid guid = new Guid();
                        guid = Guid.NewGuid();
                        string str = guid.ToString();
                        uuid.Add(str);

                        // 坐标
                        ArrayList singlePoint = new ArrayList();
                        foreach (PointF point in roadModel.roadList)
                        {
                            PointF pointf = new PointF(point.X, point.Y);
                            singlePoint.Add(Transform(pointf));
                        }
                        geom.Add(singlePoint);

                        // 实体颜色
                        colorList.Add(roadModel.ColorIndex);
                        // 实体类型
                        type.Add(roadModel.RoadType);

                        // 实体所在图层名字
                        layerName.Add(layer.Name);
                        // 表名，默认a
                        tableName.Add("a");
                        // 道路名称表，入库需要
                        row = attributeList.NewRow(); row["道路名称"] = roadModel.RoadName; attributeList.Rows.Add(row);
                        // 道路名称索引
                        if (roadModel.RoadName is null)
                        {
                            attributeIndexList.Add("");
                        }
                        else
                        {
                            roadModel.RoadName = roadModel.RoadName + "_S1";
                            attributeIndexList.Add(roadModel.RoadName);
                        }

                        // 图例
                        tuliList.Add("");
                        // 项目ID或叫城市ID
                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                        // 图表名或者叫文件名
                        chartName = "123";
                        // 控规引导
                        kgGuide.Add("");

                        //地理坐标系统编号
                        srid = "4326";
                        //配套设施所在地块集合
                        parentId.Add("");
                        // 文字内容(单行文字、多行文字、块参照等)
                        textContent.Add("");
                        // 块内容
                        blockContent.Add("");
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
            result.Add("tuliList", tuliListString);
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

            FenTuZe.PostData(result);
        }

        // 单元图则
        public static void PostModelBase(UnitPlanModel model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (LayerModel layer in model.allLines)
            {
                foreach (object unitPlanModel in layer.modelItemList)
                {
                    result = (Dictionary<string, string>)unitPlanModel;
                }
            }

            FenTuZe.PostData(result);
        }
        public static void PostModelBase(RoadNoSectionModel model)
        {
            ArrayList uuid = new ArrayList();
            ArrayList geom = new ArrayList();   // 坐标点集合
            ArrayList colorList = new ArrayList();       // 颜色集合
            ArrayList type = new ArrayList();       // 类型集合

            ArrayList layerName = new ArrayList();
            ArrayList tableName = new ArrayList(); // 表名
            System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
            attributeList.Columns.Add(new System.Data.DataColumn(("道路名称"), typeof(string))); System.Data.DataRow row;
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
            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

            foreach (LayerModel layer in model.allLines)
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
                            // 道路名称表，入库需要
                            row = attributeList.NewRow();
                            row["1"] = "";
                            attributeList.Rows.Add(row);
                            // 道路名称索引
                            attributeIndexList.Add("");

                            // UUID
                            Guid guid = new Guid();
                            guid = Guid.NewGuid();
                            string str = guid.ToString();
                            uuid.Add(str);
                            zIndex.Add("0");



                            // 实体颜色
                            colorList.Add(layer.Color);
                            // 实体类型
                            type.Add(geoType);

                            // 实体所在图层名字
                            layerName.Add(layer.Name);
                            // 表名，默认a
                            tableName.Add("a");

                            // 图例
                            tuliList.Add("");
                            // 项目ID或叫城市ID
                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                            // 图表名或者叫文件名
                            chartName = "123";
                            // 控规引导
                            kgGuide.Add("");

                            //地理坐标系统编号
                            srid = "4326";
                            //配套设施所在地块集合
                            parentId.Add("");
                            // 文字内容(单行文字、多行文字、块参照等)
                            textContent.Add("");
                            // 块内容
                            blockContent.Add("");


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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");
                                    zIndex.Add(arcModel.ZIndex);
                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);

                                    // 实体颜色
                                    colorList.Add(arcModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);




                                    // 实体颜色
                                    colorList.Add(circleModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
                                }
                            }

                            if (blm.DbText != null)
                            {
                                foreach (DbTextModel circleModel in blm.DbText)
                                {
                                    geoType = "text";

                                    geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                    // 道路名称表，入库需要
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);
                                    zIndex.Add(circleModel.ZIndex);



                                    // 实体颜色
                                    colorList.Add(circleModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");

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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);




                                    // 实体颜色
                                    colorList.Add(layer.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
                                }

                            }
                            if (blm.PolyLine != null)
                            {
                                foreach (PolyLineModel arcModel in blm.PolyLine)
                                {
                                    geoType = "polyline";
                                    foreach (object arPt in arcModel.Vertices)
                                    {
                                        if (arPt is LineModel)
                                        {
                                            ArrayList arrayList = new ArrayList();

                                            arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                            arrayList.Add(Transform(((LineModel)arPt).EndPoint));
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
                                        // 道路名称表，入库需要
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");

                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);




                                        // 实体颜色
                                        colorList.Add(arcModel.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");
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
                                            // 道路名称表，入库需要
                                            row = attributeList.NewRow();
                                            row["1"] = "";
                                            attributeList.Rows.Add(row);
                                            // 道路名称索引
                                            attributeIndexList.Add("");

                                            // UUID
                                            Guid guid = new Guid();
                                            guid = Guid.NewGuid();
                                            string str = guid.ToString();
                                            uuid.Add(str);




                                            // 实体颜色
                                            colorList.Add(cpModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            // 图例
                                            tuliList.Add("");
                                            // 项目ID或叫城市ID
                                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                            // 图表名或者叫文件名
                                            chartName = "123";
                                            // 控规引导
                                            kgGuide.Add("");

                                            //地理坐标系统编号
                                            srid = "4326";
                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");
                                        }
                                    }

                                }

                            }

                        }

                    }
                }
                //特殊数据
                foreach (RoadInfoItemModel roadModel in layer.modelItemList)
                {
                    // 如果是文字
                    if (roadModel.RoadNameType == "text")
                    {
                        for (int i = 0; i < roadModel.RoadName.Length; i++)
                        {
                            //道路模型接口
                            // UUID
                            Guid guid = new Guid();
                            guid = Guid.NewGuid();
                            string str = guid.ToString();
                            uuid.Add(str);

                            // 单个文字坐标
                            ArrayList singlePoint = new ArrayList();
                            PointF pointf = new PointF(roadModel.RoadNameLocaiton[i].X, roadModel.RoadNameLocaiton[i].Y);
                            singlePoint.Add(Transform(pointf));
                            geom.Add(singlePoint);

                            // 实体颜色
                            colorList.Add("");
                            // 实体类型
                            type.Add(roadModel.RoadNameType);

                            // 实体所在图层名字
                            layerName.Add(roadModel.RoadNameLayer);
                            // 表名，默认a
                            tableName.Add("a");
                            // 道路名称表，入库需要
                            row = attributeList.NewRow(); row["道路名称"] = roadModel.RoadName; attributeList.Rows.Add(row);
                            // 道路名称索引
                            attributeIndexList.Add("");

                            // 图例
                            tuliList.Add("");
                            // 项目ID或叫城市ID
                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                            // 图表名或者叫文件名
                            chartName = "123";
                            // 控规引导
                            kgGuide.Add("");

                            //地理坐标系统编号
                            srid = "4326";
                            //配套设施所在地块集合
                            parentId.Add("");
                            // 文字内容(单行文字、多行文字、块参照等)
                            textContent.Add(roadModel.RoadName[i]);
                            // 块内容
                            blockContent.Add("");
                        }
                    }

                    //如果是道路多段线
                    if (roadModel.RoadType.ToLower() == "polyline")
                    {
                        // UUID
                        Guid guid = new Guid();
                        guid = Guid.NewGuid();
                        string str = guid.ToString();
                        uuid.Add(str);

                        // 坐标
                        ArrayList singlePoint = new ArrayList();
                        foreach (PointF point in roadModel.roadList)
                        {
                            PointF pointf = new PointF(point.X, point.Y);
                            singlePoint.Add(Transform(pointf));
                        }
                        geom.Add(singlePoint);

                        // 实体颜色
                        colorList.Add(roadModel.ColorIndex);
                        // 实体类型
                        type.Add(roadModel.RoadType);

                        // 实体所在图层名字
                        layerName.Add(layer.Name);
                        // 表名，默认a
                        tableName.Add("a");
                        // 道路名称表，入库需要
                        row = attributeList.NewRow(); row["道路名称"] = roadModel.RoadName; attributeList.Rows.Add(row);
                        // 道路名称索引
                        if (roadModel.RoadName is null)
                        {
                            attributeIndexList.Add("");
                        }
                        else
                        {
                            roadModel.RoadName = roadModel.RoadName + "_S1";
                            attributeIndexList.Add(roadModel.RoadName);
                        }

                        // 图例
                        tuliList.Add("");
                        // 项目ID或叫城市ID
                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                        // 图表名或者叫文件名
                        chartName = "123";
                        // 控规引导
                        kgGuide.Add("");

                        //地理坐标系统编号
                        srid = "4326";
                        //配套设施所在地块集合
                        parentId.Add("");
                        // 文字内容(单行文字、多行文字、块参照等)
                        textContent.Add("");
                        // 块内容
                        blockContent.Add("");
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

            FenTuZe.PostData(result);
        }


        public static void PostModelBase(PipeModel model)
        {
            ArrayList uuid = new ArrayList();
            ArrayList geom = new ArrayList();   // 坐标点集合
            ArrayList colorList = new ArrayList();       // 颜色集合
            ArrayList type = new ArrayList();       // 类型集合

            ArrayList layerName = new ArrayList();
            ArrayList tableName = new ArrayList(); // 表名
            System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
            attributeList.Columns.Add(new System.Data.DataColumn(("管道名称"), typeof(string))); System.Data.DataRow row;
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
            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总

            foreach (LayerModel layer in model.allLines)
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
                            // 道路名称表，入库需要
                            row = attributeList.NewRow();
                            row["1"] = "";
                            attributeList.Rows.Add(row);
                            // 道路名称索引
                            attributeIndexList.Add("");

                            // UUID
                            Guid guid = new Guid();
                            guid = Guid.NewGuid();
                            string str = guid.ToString();
                            uuid.Add(str);
                            zIndex.Add("0");



                            // 实体颜色
                            colorList.Add(layer.Color);
                            // 实体类型
                            type.Add(geoType);

                            // 实体所在图层名字
                            layerName.Add(layer.Name);
                            // 表名，默认a
                            tableName.Add("a");

                            // 图例
                            tuliList.Add("");
                            // 项目ID或叫城市ID
                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                            // 图表名或者叫文件名
                            chartName = "123";
                            // 控规引导
                            kgGuide.Add("");

                            //地理坐标系统编号
                            srid = "4326";
                            //配套设施所在地块集合
                            parentId.Add("");
                            // 文字内容(单行文字、多行文字、块参照等)
                            textContent.Add("");
                            // 块内容
                            blockContent.Add("");


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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");
                                    zIndex.Add(arcModel.ZIndex);
                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);

                                    // 实体颜色
                                    colorList.Add(arcModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);




                                    // 实体颜色
                                    colorList.Add(circleModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
                                }
                            }

                            if (blm.DbText != null)
                            {
                                foreach (DbTextModel circleModel in blm.DbText)
                                {
                                    geoType = "text";

                                    geom.Add(new ArrayList() { Transform(circleModel.Position) });
                                    // 道路名称表，入库需要
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);
                                    zIndex.Add(circleModel.ZIndex);



                                    // 实体颜色
                                    colorList.Add(circleModel.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");

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
                                    row = attributeList.NewRow();
                                    row["1"] = "";
                                    attributeList.Rows.Add(row);
                                    // 道路名称索引
                                    attributeIndexList.Add("");

                                    // UUID
                                    Guid guid = new Guid();
                                    guid = Guid.NewGuid();
                                    string str = guid.ToString();
                                    uuid.Add(str);




                                    // 实体颜色
                                    colorList.Add(layer.Color);
                                    // 实体类型
                                    type.Add(geoType);

                                    // 实体所在图层名字
                                    layerName.Add(layer.Name);
                                    // 表名，默认a
                                    tableName.Add("a");

                                    // 图例
                                    tuliList.Add("");
                                    // 项目ID或叫城市ID
                                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                    // 图表名或者叫文件名
                                    chartName = "123";
                                    // 控规引导
                                    kgGuide.Add("");

                                    //地理坐标系统编号
                                    srid = "4326";
                                    //配套设施所在地块集合
                                    parentId.Add("");
                                    // 文字内容(单行文字、多行文字、块参照等)
                                    textContent.Add("");
                                    // 块内容
                                    blockContent.Add("");
                                }

                            }
                            if (blm.PolyLine != null)
                            {
                                foreach (PolyLineModel arcModel in blm.PolyLine)
                                {
                                    geoType = "polyline";
                                    foreach (object arPt in arcModel.Vertices)
                                    {
                                        if (arPt is LineModel)
                                        {
                                            ArrayList arrayList = new ArrayList();

                                            arrayList.Add(Transform(((LineModel)arPt).StartPoint));
                                            arrayList.Add(Transform(((LineModel)arPt).EndPoint));
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
                                        // 道路名称表，入库需要
                                        row = attributeList.NewRow();
                                        row["1"] = "";
                                        attributeList.Rows.Add(row);
                                        // 道路名称索引
                                        attributeIndexList.Add("");

                                        // UUID
                                        Guid guid = new Guid();
                                        guid = Guid.NewGuid();
                                        string str = guid.ToString();
                                        uuid.Add(str);




                                        // 实体颜色
                                        colorList.Add(arcModel.Color);
                                        // 实体类型
                                        type.Add(geoType);

                                        // 实体所在图层名字
                                        layerName.Add(layer.Name);
                                        // 表名，默认a
                                        tableName.Add("a");

                                        // 图例
                                        tuliList.Add("");
                                        // 项目ID或叫城市ID
                                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                        // 图表名或者叫文件名
                                        chartName = "123";
                                        // 控规引导
                                        kgGuide.Add("");

                                        //地理坐标系统编号
                                        srid = "4326";
                                        //配套设施所在地块集合
                                        parentId.Add("");
                                        // 文字内容(单行文字、多行文字、块参照等)
                                        textContent.Add("");
                                        // 块内容
                                        blockContent.Add("");
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
                                            // 道路名称表，入库需要
                                            row = attributeList.NewRow();
                                            row["1"] = "";
                                            attributeList.Rows.Add(row);
                                            // 道路名称索引
                                            attributeIndexList.Add("");

                                            // UUID
                                            Guid guid = new Guid();
                                            guid = Guid.NewGuid();
                                            string str = guid.ToString();
                                            uuid.Add(str);




                                            // 实体颜色
                                            colorList.Add(cpModel.Color);
                                            // 实体类型
                                            type.Add(geoType);

                                            // 实体所在图层名字
                                            layerName.Add(layer.Name);
                                            // 表名，默认a
                                            tableName.Add("a");

                                            // 图例
                                            tuliList.Add("");
                                            // 项目ID或叫城市ID
                                            projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                                            // 图表名或者叫文件名
                                            chartName = "123";
                                            // 控规引导
                                            kgGuide.Add("");

                                            //地理坐标系统编号
                                            srid = "4326";
                                            //配套设施所在地块集合
                                            parentId.Add("");
                                            // 文字内容(单行文字、多行文字、块参照等)
                                            textContent.Add("");
                                            // 块内容
                                            blockContent.Add("");
                                        }
                                    }

                                }

                            }

                        }

                    }
                }
                //特殊数据
                foreach (PipeItemModel roadModel in layer.modelItemList)
                {

                    //道路模型接口
                    // UUID
                    Guid guid1 = new Guid();
                    guid1 = Guid.NewGuid();
                    string str1 = guid1.ToString();
                    uuid.Add(str1);

                    // 单个文字坐标
                    ArrayList singlePoint1 = new ArrayList();
                    PointF pointf1 = roadModel.TxtLocation;
                    singlePoint1.Add(Transform(pointf1));
                    geom.Add(singlePoint1);

                    // 实体颜色
                    colorList.Add("");
                    // 实体类型
                    type.Add("text");

                    // 实体所在图层名字
                    layerName.Add(roadModel.PipeLayer);
                    // 表名，默认a
                    tableName.Add("a");
                    // 道路名称表，入库需要
                    row = attributeList.NewRow(); row["管道名称"] = roadModel.PipeText; attributeList.Rows.Add(row);
                    // 道路名称索引
                    attributeIndexList.Add("");

                    // 图例
                    tuliList.Add("");
                    // 项目ID或叫城市ID
                    projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                    // 图表名或者叫文件名
                    chartName = "123";
                    // 控规引导
                    kgGuide.Add("");

                    //地理坐标系统编号
                    srid = "4326";
                    //配套设施所在地块集合
                    parentId.Add("");
                    // 文字内容(单行文字、多行文字、块参照等)
                    textContent.Add(roadModel.PipeText);
                    // 块内容
                    blockContent.Add("");

                    //如果是道路多段线
                    if (roadModel.PipeType.ToLower() == "polyline")
                    {
                        // UUID
                        Guid guid = new Guid();
                        guid = Guid.NewGuid();
                        string str = guid.ToString();
                        uuid.Add(str);

                        // 坐标
                        ArrayList singlePoint = new ArrayList();
                        foreach (PointF point in roadModel.pipeList)
                        {

                            singlePoint.Add(Transform(point));
                        }
                        geom.Add(singlePoint);

                        // 实体颜色
                        colorList.Add(roadModel.ColorIndex);
                        // 实体类型
                        type.Add(roadModel.PipeType);

                        // 实体所在图层名字
                        layerName.Add(layer.Name);
                        // 表名，默认a
                        tableName.Add("a");
                        // 道路名称表，入库需要
                        row = attributeList.NewRow(); row["管道类型"] = roadModel.PipeText; attributeList.Rows.Add(row);
                        // 道路名称索引
                        if (roadModel.PipeText is null)
                        {
                            attributeIndexList.Add("");
                        }
                        else
                        {
                            roadModel.PipeText = roadModel.PipeText + "_S1";
                            attributeIndexList.Add(roadModel.PipeText);
                        }

                        // 图例
                        tuliList.Add("");
                        // 项目ID或叫城市ID
                        projectId = "D3DEC178-2C05-C5F1-F6D3-45729EB9436A";
                        // 图表名或者叫文件名
                        chartName = "123";
                        // 控规引导
                        kgGuide.Add("");

                        //地理坐标系统编号
                        srid = "4326";
                        //配套设施所在地块集合
                        parentId.Add("");
                        // 文字内容(单行文字、多行文字、块参照等)
                        textContent.Add("");
                        // 块内容
                        blockContent.Add("");
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

            FenTuZe.PostData(result);
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

            return output;
        }

    }
}
