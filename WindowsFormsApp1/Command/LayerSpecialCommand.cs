using System;
using System.Collections.Generic;
using System.Text;
using RegulatoryModel.Model;
using RegulatoryPlan.Method;

namespace RegulatoryPlan.Command
{
    public class LayerSpecialCommand<T> where T : ModelBase
    {

        public void AddSpecialLayerModel(T model)
        {
            switch (model.DerivedType)
            {
                case DerivedTypeEnum.BuildingIntegrated:

                    break;

                case DerivedTypeEnum.UnitPlan:
                    UnitPlanMethod<UnitPlanModel> uMethod = new UnitPlanMethod<UnitPlanModel>();
                    uMethod.GetAllUnitPlaneInfo(model as UnitPlanModel);
                    break;
                case DerivedTypeEnum.PointsPlan:
                    break;
                case DerivedTypeEnum.Power10Kv:
                    break;
                case DerivedTypeEnum.Power35kv:
                    break;
                case DerivedTypeEnum.WaterSupply:
                    break;
                case DerivedTypeEnum.HeatSupply:
                    break;
                case DerivedTypeEnum.FuelGas:
                    break;
                case DerivedTypeEnum.Communication:
                    break;
                case DerivedTypeEnum.TheRoadSection:
                    RoadMethod<RoadSectionModel> mMethod = new RoadMethod<RoadSectionModel>();
                    mMethod.GetAllRoadInfo(model as RoadSectionModel);
                    break;
                case DerivedTypeEnum.PipeLine:
                    break;
                case DerivedTypeEnum.Sewage:
                    PipeMethod<PipeModel> pipeMethod = new PipeMethod<PipeModel>();
                    pipeMethod.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.FiveLine:
                    break;
                case DerivedTypeEnum.LimitFactor:
                    break;
                case DerivedTypeEnum.RainWater:
                    break;
                case DerivedTypeEnum.ReuseWater:
                    break;

            }
        }


    }

    
}
