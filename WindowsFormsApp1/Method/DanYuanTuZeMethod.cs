using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.Collections;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System;
using System.IO;

namespace RegulatoryPlan.Method
{
    public class DanYuanTuZeMethod
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

                // 查询所有的图案填充
                //ArrayList resultAll = new ArrayList();
                //for (int i = 0; i < idArray.Length; i++)
                //{
                //    Entity ent1 = (Entity)tr.GetObject(idArray[i], OpenMode.ForRead, false, true);

                //    if (ent1 is Hatch)
                //    {
                //        resultAll.Add(ent1);
                //    }
                //}
                //DrawBorder(resultAll, tr, btr, 5);

                // 手动选择图案填充
                ArrayList resultAll = new ArrayList();

                PromptSelectionResult acSSPrompt = doc.Editor.GetSelection();

                MessageBox.Show(acSSPrompt.ToString());

                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        if (acSSObj != null)
                        {
                            Entity ent1 = tr.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Entity;
                            if (ent1 is Hatch)
                            {
                                resultAll.Add(ent1);
                            }
                        }
                    }

                }
                MessageBox.Show(resultAll.Count.ToString());
                DrawBorder(resultAll, tr, btr, 5, ed);
                //MessageBox.Show(ent1.BlockName.ToString());
            }

        }

        public static void DrawBorder(ArrayList hats, Transaction trans, BlockTableRecord btr, int numSample, Editor ed)
        {
            ArrayList two = new ArrayList();
            System.Collections.Generic.Dictionary<string, string> result = new System.Collections.Generic.Dictionary<string, string>();
            ArrayList attrlist = new ArrayList();

            foreach (Hatch hat in hats)
            {
                ArrayList one = new ArrayList();

                //取得边界数
                int loopNum = hat.NumberOfLoops;
                Point2dCollection col_point2d = new Point2dCollection();
                BulgeVertexCollection col_ver = new BulgeVertexCollection();
                Curve2dCollection col_cur2d = new Curve2dCollection();

                for (int i = 0; i < loopNum; i++)
                {
                    col_point2d.Clear();
                    HatchLoop hatLoop = null;
                    try
                    {
                        hatLoop = hat.GetLoopAt(i);
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
                            col_point2d.Add(vertex.Vertex);
                        }
                    }
                    //如果HatchLoop为Curves
                    else
                    {
                        col_cur2d = hatLoop.Curves;
                        foreach (Curve2d item in col_cur2d)
                        {
                            Point2d[] M_point2d = item.GetSamplePoints(numSample);
                            foreach (Point2d point in M_point2d)
                            {
                                if (!col_point2d.Contains(point))
                                    col_point2d.Add(point);
                            }
                        }
                    }

                    //根据获得的Point2d点集创建闭合Polyline
                    //Polyline pl = new Polyline();
                    //pl.Closed = true;
                    //pl.Color = hat.Color;
                    //PolylineTools.CreatePolyline(pl, col_point2d);
                    //btr.AppendEntity(pl);
                    //trans.AddNewlyCreatedDBObject(pl, true);
                    double[] one_0 = new double[2];

                    foreach (Point2d point in col_point2d)
                    {

                        double X = Math.Round(point.X, 7) + 4000000;
                        double Y = Math.Round(point.Y, 7) + 38500000;

                        //double X = 60146 + 4000000;
                        //double Y = 34953 + 38500000;

                        //  由高斯投影坐标反算成经纬度
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
                        output[0] = longitude1 / iPI + 108.7271360867638 + 0.0002173 - 11 + 0.00061422;
                        output[1] = latitude1 / iPI - 310.2659499330605 + 0.0009752 - 0.0002805;

                        //output[0] = Math.Round(point.X, 7);
                        //output[1] = Math.Round(point.Y, 7);

                        one_0 = new double[2] { output[0], output[1] };

                        one.Add(one_0);

                    }

                } // 单个hatch边界顶点循环结束

                two.Add(one);

                LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(hat.LayerId, OpenMode.ForRead);

                string hatchColor = "hatchColor:" + ltr.Color;
                attrlist.Add(hatchColor);
            } // 所有的Hatch循环结束

            Hashtable geom = new Hashtable();
            geom.Add("rings", two);

            string geomString = string.Empty;
            geomString = JsonConvert.SerializeObject(geom);
            result.Add("geom", geomString);

            //string[] attrlist = new string[] { "componentcategoryType:61" };
            string[] factorid = new string[] { "b73e3fce-8314-4d5b-8e2b-0a3e8844b28b" };
            string type = "polygon";
            string name = "456";
            string srid = "4214";

            string attrlistString = string.Empty;
            attrlistString = JsonConvert.SerializeObject(attrlist);

            string factoridString = string.Empty;
            factoridString = JsonConvert.SerializeObject(factorid);

            result.Add("attrlist", attrlistString);
            result.Add("factorid", factoridString);
            result.Add("type", type);
            result.Add("srid", srid);
            result.Add("name", name);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(result);

            MessageBox.Show(JSONString);

            using (StreamWriter file = new StreamWriter(@"C:\Users\Public\Documents\WriteLines2.json", false))
            {
                file.WriteLine(JSONString);
            }

            ed.WriteMessage("\nFound X：{0} ", JSONString);

            // 发送 开始
            var baseAddress = "http://172.18.84.192:8080/CIM/cim/geom!addCadGeomByType.action";

            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.ContentType = "application/x-www-form-urlencoded";
            http.Method = "POST";

            StringBuilder builder = new StringBuilder();
            int h = 0;
            foreach (var item in result)
            {
                if (h > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                h++;
            }

            //string parsedContent = "srid=4326&attrlist=[\"chColor:51\"]&factorid=[\"b73e3fce-8314-4d5b-8e2b-0a3e8844b28b\"]&type=polygon&geom={\"rings\":[[[60145.4546169,33387.5339155],[59895.3137297,33437.8260557],[59885.7661656,33285.0286724],[59885.4849623,33280.5283488],[59902.1499232,33259.5545569],[59906.5973667,33258.8114325],[60127.4437563,33221.9101578],[60145.4546169,33387.5339155]]]}&name=123";
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(builder.ToString());

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
            // 发送 结束

        } // 发送方法结束

    }
}