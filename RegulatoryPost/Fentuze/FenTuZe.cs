
using System.Collections;

using Newtonsoft.Json;
using System.Net;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RegulatoryPost.FenTuZe
{
    public class FenTuZe
    {
      

        // 发送块参照方法
        public static void SendData(Dictionary<string, string> result, ArrayList uuid, ArrayList geom, ArrayList colorList, ArrayList type, ArrayList layerName, ArrayList tableName, 
            ArrayList attributeIndexList, System.Data.DataTable attributeList, ArrayList tuliList, string projectId, string chartName, ArrayList kgGuide, String srid, ArrayList parentId, ArrayList textContent, ArrayList blockContent)
        {
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


            // 发送
            PostData(result);
        }

        // 发送方法
        public static void PostData(Dictionary<string, string> result)
        {

            // 发送 开始
            string[] baseAddresses = new string[] {
                "http://172.18.84.102:8080/CIM/", // 測試
               //"http://172.18.84.102:8080/CIM/cim/geom!addCadGeomByType.action", // GIS
                //"http://172.18.84.70:8081/PDD/pdd/webgl!addIndividual.action" // JAVA
            };

            foreach (var baseAddress in baseAddresses)
            {
                string resultString = JsonConvert.SerializeObject(result);
                using (StreamWriter file = new StreamWriter(@"C:\Users\Public\Documents\WriteLines2.json", false))
                {
                    file.WriteLine(resultString);
                }

                Dictionary<string, string> resultAll = new Dictionary<string, string>();
                resultAll.Add("result", resultString);

                var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                http.ContentType = "application/x-www-form-urlencoded";
                http.Method = "POST";

                StringBuilder builder = new StringBuilder();
                int h = 0;
                foreach (var item in resultAll)
                {
                    if (h > 0)
                        builder.Append("&");
                    string value = "";
                    value = item.Value.ToString().Replace("%", "百分号");

                    builder.AppendFormat("{0}={1}", item.Key, value);
                    h++;
                }

                using (StreamWriter file = new StreamWriter(@"C:\Users\Public\Documents\WriteLines2.json", false))
                {
                    file.WriteLine(builder);
                }

                MessageBox.Show(builder.ToString());

                //string parsedContent = "srid=4326&attrlist=[\"chColor:51\"]&factorid=[\"b73e3fce-8314-4d5b-8e2b-0a3e8844b28b\"]&type=polygon&geom={\"rings\":[[[60145.4546169,33387.5339155],[59895.3137297,33437.8260557],[59885.7661656,33285.0286724],[59885.4849623,33280.5283488],[59902.1499232,33259.5545569],[59906.5973667,33258.8114325],[60127.4437563,33221.9101578],[60145.4546169,33387.5339155]]]}&name=123";
                Byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                try
                {
                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream, Encoding.UTF8);
                    var content = sr.ReadToEnd();

                    MessageBox.Show(content);
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }

            // 发送 结束
        }

       
    }

}