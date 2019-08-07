
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

        private static string[] BaseAddresses()
        {
            string[] baseAddresses = new string[] {
                    "http://172.18.84.102:8080/CIM/", // 測試
                    //"http://172.18.84.102:8081/CIM/cim/geom!addCadGeomByType.action", // GIS
                    //"http://172.18.84.70:8081/PDD/pdd/webgl!addIndividual.action" // JAVA
                };
            return baseAddresses;
        }
        // 发送方法
        public static void PostData(Dictionary<string, string> result)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            try
            {
                sw.Start();
                // 发送 开始

                foreach (var baseAddress in BaseAddresses())
                {
                    Dictionary<string, string> resultAll = new Dictionary<string, string>();
                    string resultString = JsonConvert.SerializeObject(result);
                    resultAll.Add("result", resultString);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                    http.ContentType = "application/x-www-form-urlencoded";
                    http.Method = "POST";
                    http.Timeout = 600000;
                    http.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                    http.KeepAlive = false;
                    http.ProtocolVersion = HttpVersion.Version10;
                    http.ServicePoint.Expect100Continue = false;
                    ServicePointManager.DefaultConnectionLimit = 200;

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    http.ServicePoint.ConnectionLimit = 200;

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

                    using (StreamWriter file = new StreamWriter(@"C:\Users\Public\Documents\" + result["chartName"] + ".json", false))
                    {
                        file.WriteLine(builder);
                    }
                    //MessageBox.Show(builder.ToString());

                    Byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    string content = "";

                    var response = http.GetResponse();
                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream, Encoding.UTF8);
                    content = sr.ReadToEnd();
                    sr.Close();
                    response.Close();

                    sw.Stop();

                    WriteLog(result["chartName"], content, sw.ElapsedMilliseconds/1000);

                    MessageBox.Show("发送成功！", "服务器反馈", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                } // 发送 结束
            }
            catch (Exception e)
            {
                WriteLog(result["chartName"], e.Message, sw.ElapsedMilliseconds/1000);

                MessageBox.Show("发送失败！", "服务器反馈", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            }
        }

        public static void AutoPostData(Dictionary<string, string> result)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            try
            {
                sw.Start();
                // 发送 开始

                foreach (var baseAddress in BaseAddresses())
                {
                    Dictionary<string, string> resultAll = new Dictionary<string, string>();
                    string resultString = JsonConvert.SerializeObject(result);
                    resultAll.Add("result", resultString);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                    http.ContentType = "application/x-www-form-urlencoded";
                    http.Method = "POST";
                    http.Timeout = 600000;
                    http.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                    http.KeepAlive = false;
                    http.ProtocolVersion = HttpVersion.Version10;
                    http.ServicePoint.Expect100Continue = false;
                    ServicePointManager.DefaultConnectionLimit = 200;

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    http.ServicePoint.ConnectionLimit = 200;

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

                    using (StreamWriter file = new StreamWriter(@"C:\Users\Public\Documents\" + result["chartName"] + ".json", false))
                    {
                        file.WriteLine(builder);
                    }
                    //MessageBox.Show(builder.ToString());

                    Byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    string content = "";

                    var response = http.GetResponse();
                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream, Encoding.UTF8);
                    content = sr.ReadToEnd();
                    sr.Close();
                    response.Close();

                    sw.Stop();

                    WriteLog(result["chartName"], content, sw.ElapsedMilliseconds / 1000);


                } // 发送 结束
            }
            catch (Exception e)
            {
                WriteLog(result["chartName"], e.Message, sw.ElapsedMilliseconds / 1000);

            }
        }
        public static void WriteLog(string fileName, string content, long time)
        {
            content = content.Replace("\n", "").Replace("\r", "").Replace(" ", "");
            string logFileName = @"C: \Users\Public\Documents\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            using (TextWriter logFile = TextWriter.Synchronized(File.AppendText(logFileName)))
            {
                logFile.WriteLine(DateTime.Now);
                logFile.WriteLine(fileName + "，" + content);
                logFile.WriteLine("耗时：" + time + "秒");
                logFile.WriteLine("\n");
                logFile.Flush();
                logFile.Close();
            }
        }

    }

}