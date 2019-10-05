using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;


namespace RegulatoryModel.Model
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
        Communication,//通信管线
        BuildingIntegrated,//建筑整合
        TheRoadSection,//道路断面
        PipeLine,// 管线综合
        Sewage,//污水规划
        FiveLine,//五线图
        LimitFactor,// 限制性要素
        RainWater,// 雨水规划
        ReuseWater,//  再生水规划
        Road,//道路
        CityDesign,//城市设计
        CenterCityUseLandPlan,//中心城区用地规划
        CenterCityLifeUseLandPlan,//中心城区居住用地规划
        UseLandNumber,//用地规划编码图
        None,
        RoadSituation, //道路现状
        FacilityControl, //设施控制
        FiveLineControl, //五线控制
    }
    public class ModelBase
    {
        string docName;
        string name;
        DerivedTypeEnum derivedType= DerivedTypeEnum.None;
        bool isOnlyModel=true;
        public System.Data.DataTable attributeList;
        public System.Collections.ArrayList kgGuide;
        public string projectId;

        public List<string> specailLayers;
        public List<string> LayerList;
        public List<LayerModel> allLines;
        public List<LengedModel> LegendList;
        public List<string> selectedLayerList = new List<string>();

        ///属性字段匹配1.*用地：后缀名含有用地图层，2.道路，全字匹配图层;查询完成后移除所有含有*字段
      //public List<string> attributeLayers;
        public string Name { get => name; set => name = value; }
        public DerivedTypeEnum DerivedType { get => derivedType; set => derivedType = value; }
        private string uuid { get => System.Guid.NewGuid().ToString(); }
        public string DocName { get => docName; set => docName = value; }
        public bool IsOnlyModel { get => isOnlyModel; set => isOnlyModel = value; }

        public System.Data.DataTable AttributeList { get => attributeList; set => attributeList = value; }
        public System.Collections.ArrayList KgGuide { get => kgGuide; set => kgGuide = value; }
        public string ProjectId { get => projectId; set => projectId = value; }

        public virtual string ToJson()
        {
            string json = "";
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("uuid", uuid);
            string gemo = "[";
            try
            {
                foreach (LayerModel lm in allLines)
                {
                    string js = lm.JsonStr;

                    gemo += string.IsNullOrEmpty(js) ? "" : js + ",";

                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }

            gemo = gemo.TrimEnd(',');
            result.Add("gemo", gemo + "]");
            result.Add("srid", "4326");
            json = JsonConvert.SerializeObject(result);
            return json;
        }

    }
}
