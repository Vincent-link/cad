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

            //ModelBaseMethod<ModelBase> baseMethod = new ModelBaseMethod<ModelBase>();
            //baseMethod.GetAllLengedGemo(model);
            switch (model.DerivedType)
            {
                //case DerivedTypeEnum.BuildingIntegrated:
                    
                //    break;

                case DerivedTypeEnum.UnitPlan:
                    UnitPlanMethod<UnitPlanModel> uMethod = new UnitPlanMethod<UnitPlanModel>();
                    uMethod.GetAllUnitPlaneInfo(model as UnitPlanModel);
                    break;
                case DerivedTypeEnum.PointsPlan:
                    PointsPlanMethod<PointsPlanModel> pMethod = new PointsPlanMethod<PointsPlanModel>();
                    pMethod.GetAllPointsPlaneInfo(model as PointsPlanModel);
                    break;
                case DerivedTypeEnum.Power10Kv:
                    PipeMethod<PipeModel> pipeMethod4 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod4.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.Power35kv:
                    PipeMethod<PipeModel> pipeMethod3 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod3.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.WaterSupply:
                    PipeMethod<PipeModel> pipeMethod1 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod1.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.HeatSupply:
                    PipeMethod<PipeModel> pipeMethod2 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod2.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.FuelGas:
                    PipeMethod<PipeModel> pipeMethod51 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod51.GetAllPipeInfo(model as PipeModel);
                    break;
                //case DerivedTypeEnum.Communication:
                //    break;
                case DerivedTypeEnum.TheRoadSection:
                    RoadMethod<RoadSectionModel> mMethod = new RoadMethod<RoadSectionModel>();
                    mMethod.GetAllRoadInfo(model as RoadSectionModel);
                    break;
                //case DerivedTypeEnum.PipeLine:
                //    break;
                case DerivedTypeEnum.Sewage:
                    PipeMethod<PipeModel> pipeMethod = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.Communication:
                    PipeMethod<PipeModel> pipeComMethod = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeComMethod.GetAllPipeInfo(model as PipeModel);
                    break;
                //case DerivedTypeEnum.FiveLine:
                //    break;
                //case DerivedTypeEnum.LimitFactor:
                //    break;
                case DerivedTypeEnum.RainWater:
                    PipeMethod<PipeModel> pipeMethod5 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod5.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.ReuseWater:
                    PipeMethod<PipeModel> pipeMethod6 = new PipeMethod<PipeModel>(model as PipeModel);
                    pipeMethod6.GetAllPipeInfo(model as PipeModel);
                    break;
                case DerivedTypeEnum.Road:
                    RoadMethod<RoadNoSectionModel> mMethod1 = new RoadMethod<RoadNoSectionModel>();
                    mMethod1.GetAllRoadInfo(model as RoadNoSectionModel);
                    break;
                case DerivedTypeEnum.RoadSituation:
                    RoadSituationMethod<RoadSituationModel> rMethod = new RoadSituationMethod<RoadSituationModel>();
                    rMethod.GetAllRoadInfo(model as RoadSituationModel);
                    break;
                case DerivedTypeEnum.CenterCityUseLandPlan:
                    CenterCityUseLandMethod<CenterCityUseLandPlanModel> ccul = new CenterCityUseLandMethod<CenterCityUseLandPlanModel>(model as CenterCityUseLandPlanModel);
                    ccul.GetAllAttributeInfo(model as CenterCityUseLandPlanModel);
                    break;
                case DerivedTypeEnum.CityDesign:
                    break;
                case DerivedTypeEnum.UseLandNumber:
                    UsePlanNumberMethod<UseLandNumberModel> upnm = new UsePlanNumberMethod<UseLandNumberModel>(model as UseLandNumberModel);
                    break;
                case DerivedTypeEnum.CenterCityLifeUseLandPlan:
                    CenterCityLifeUseLandMethod<CenterCityLifeUseLandPlanModel> cclul = new CenterCityLifeUseLandMethod<CenterCityLifeUseLandPlanModel>(model as CenterCityLifeUseLandPlanModel);
                    break;
                case DerivedTypeEnum.None:
                    break;
            }
        }

   


    }

    
}
