using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryModel.Model;
using RegulatoryPlan.Command;
using System.Collections;
using System.Collections.Generic;

namespace RegulatoryPlan.Method
{
    public class UnitPlanMethod<T> where T : UnitPlanModel
    {
        public void GetAllUnitPlaneInfo(T model)
        {
            LayerModel lm = new LayerModel();

            // 坐标点图层 特殊处理
            ModelBaseMethod<ModelBase> mbm = new ModelBaseMethod<ModelBase>();
            //lm = mbm.GetAllLayerGemo(model, UnitPlanModel.unitPlanLineLayer);

            // 获取图表数据（特殊数据）
            System.Data.DataTable attributeList = new System.Data.DataTable();  // 属性集合
            ArrayList kgGuide = new ArrayList();//控规引导

            ReadDanAttributeList<ModelBase> attributeListObj = new ReadDanAttributeList<ModelBase>();
            // 属性
            attributeList = attributeListObj.AttributeList();
            // 控规要求
            kgGuide = attributeListObj.ControlList();

            if (lm.modelItemList == null)
            {
                lm.modelItemList = new List<object>();
            }

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