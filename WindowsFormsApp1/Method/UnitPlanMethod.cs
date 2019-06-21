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
        public void GetAllUnitPlaneInfo(T model)
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
            ArrayList kgGuide = new ArrayList();//控规引导

            string srid = ""; //地理坐标系统编号
            ArrayList parentId = new ArrayList(); //配套设施所在地块集合
            ArrayList textContent = new ArrayList(); // 文字内容（GIS端展示）
            ArrayList blockContent = new ArrayList(); // 块内容（GIS端展示）

            Dictionary<string, string> result = new Dictionary<string, string>(); // 汇总


            // 遍历所有实体
            ReadDanAttributeList<ModelBase> attributeListObj = new ReadDanAttributeList<ModelBase>();

            // 图例
            tuliList.Add("");

            // 属性
            attributeList = attributeListObj.AttributeList();

            // 控规要求
            kgGuide = attributeListObj.ControlList();

            LayerModel lm = new LayerModel();

            if (model.allLines == null)
            {
                model.allLines = new List<LayerModel>();
            }
            model.allLines.Add(lm);
            lm.modelItemList.Add(attributeList);
            lm.modelItemList.Add(kgGuide);
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