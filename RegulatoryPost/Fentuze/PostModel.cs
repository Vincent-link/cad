using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;
using RegulatoryModel.Model;
using System.Collections;
using Autodesk.AutoCAD.DatabaseServices;

namespace RegulatoryPost.FenTuZe
{
  public  class PostModel
    {
        public static void PostModelBase(ModelBase model)
        {
            Dictionary<string, string> postInfo = new Dictionary<string, string>();
            FenTuZe.PostData(postInfo);
        }
        public static void PostModelBase(RoadSectionModel model)
        {
            Dictionary<string, string> postInfo = new Dictionary<string, string>();

            List<string> uuid = new List<string>();
            ArrayList geom = new ArrayList();
            foreach (LengedModel legend in model.LegendList)
            {
                foreach (BlockInfoModel blockInfo in legend.GemoModels)
                {
                    

                }
                MessageBox.Show(model.LegendList.ToString());
            }

            // JSON化
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            uuid.Add(str);

            string uuidString = JsonConvert.SerializeObject(uuid);
            string geomString = JsonConvert.SerializeObject(geom);
            //string colorListString = JsonConvert.SerializeObject(colorList);
            //string typeString = JsonConvert.SerializeObject(type);

            //string layerNameString = JsonConvert.SerializeObject(layerName);
            //string tableNameString = JsonConvert.SerializeObject(tableName);
            //string attributeIndexListString = JsonConvert.SerializeObject(attributeIndexList);
            //string attributeListString = JsonConvert.SerializeObject(attributeList);

            //string tuliListString = JsonConvert.SerializeObject(tuliList);
            //string kgGuideString = JsonConvert.SerializeObject(kgGuide);
            //string parentIdString = JsonConvert.SerializeObject(parentId);
            //string textContentString = JsonConvert.SerializeObject(textContent);

            //string blockContentString = JsonConvert.SerializeObject(blockContent);

            //// UUID
            //result.Add("uuid", uuidString);
            //// 实体坐标信息
            //result.Add("geom", geomString);
            //// 实体颜色
            //result.Add("colorList", colorListString);
            //// 实体类型
            //result.Add("type", typeString);

            //// 图层
            //result.Add("layerName", layerNameString);
            //// 表名
            //result.Add("tableName", tableNameString);
            //// 实体属性索引
            //result.Add("attributeIndexList", attributeIndexListString);
            //// 实体属性
            //result.Add("attributeList", attributeListString);

            //// 图例
            //result.Add("tuliList", tuliListString);
            //// 项目ID
            //result.Add("projectId", projectId);
            //// 自定义
            //result.Add("chartName", chartName);
            //// 实体属性
            //result.Add("kgGuide", kgGuideString);

            //// 坐标系代码
            //result.Add("srid", srid);
            //// 配套设施所属的地块编码
            //result.Add("parentId", parentIdString);
            //// 文字内容
            //result.Add("textContent", textContentString);
            //// 块内容
            //result.Add("blockContent", blockContentString);


            FenTuZe.PostData(postInfo);
        }
    }
}
