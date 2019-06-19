using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
    public class UnitPlanModel : ModelBase
    {
        public static string roadNameLayer = "";
        public static string roadLineLayer = "道路";
        public UnitPlanModel()
        {
            this.specailLayers = new List<string>() { roadLineLayer, roadNameLayer };
            this.DerivedType = DerivedTypeEnum.UnitPlan;
        }
    }
}
