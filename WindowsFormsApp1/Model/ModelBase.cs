using Newtonsoft.Json;
using RegulatoryPlan.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryPlan.Model
{
    public enum DerivedTypeEnum
    {

        UnitPlan,//单元图则
        PointsPlan,//分图则
        Power10Kv,
        Power35kv,
        WaterSupply,//给水
        HeatSupply,//供热
        FuelGas,//燃气
        Communication,//通信
        BuildingIntegrated,//建筑整合
        TheRoadSection,//道路断面
        PipeLine,// 管线综合
        Sewage,//污水规划
        FiveLine,//五线图
        LimitFactor,// 限制性要素
        RainWater,// 雨水规划
        ReuseWater,//  再生水规划
        None

    }
    public class ModelBase
    {
        string name;
        DerivedTypeEnum derivedType;
        public List<string> specailLayers;
        public List<string> LayerList;
        public List<LayerModel> allLines;
        public List<LengedModel> LegendList;
        public string Name { get => name; set => name = value; }
        public DerivedTypeEnum DerivedType { get => derivedType; set => derivedType = value; }
        private string uuid { get => System.Guid.NewGuid().ToString(); }

        public virtual string ToJson()
        {
            string json = "";
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("uuid", uuid);
            string gemo = "[";

            foreach (LayerModel lm in allLines)
            {
                string js = lm.JsonStr;
                
                gemo += string.IsNullOrEmpty(js)?"":js+",";
            }
             gemo= gemo.TrimEnd(',');
            result.Add("gemo", gemo + "]");
            result.Add("srid", "4326");
            json = JsonConvert.SerializeObject(result);
            return json;
        }
        /// <summary>
        /// 每个图纸特有数据的处理函数
        /// </summary>
        public virtual void AddSpecialLayerModel()
        {

        }

    }
}
