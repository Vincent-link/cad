using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Model
{
    public enum DerivedTypeEnum
    {
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
        ReuseWater//  再生水规划
    }
  public  class ModelBase
    {
        string name;
        DerivedTypeEnum derivedType;
        public string Name { get => name; set => name = value; }
        public DerivedTypeEnum DerivedType { get => derivedType; set => derivedType = value; }
    }
}
