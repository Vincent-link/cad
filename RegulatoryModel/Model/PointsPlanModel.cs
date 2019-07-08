using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
    public class PointsPlanModel : ModelBase
    {
        public static string unitPlanNameLayer = "";
        public static string unitPlanLineLayer = "坐标";
        public PointsPlanModel()
        {
            this.IsOnlyModel = false;
            this.specailLayers = new List<string>() { unitPlanNameLayer, unitPlanLineLayer };
            this.DerivedType = DerivedTypeEnum.PointsPlan;
        }


    }

    public class PointsPlanItemModel
    {
        public string Num { get; set; }
        public string RoadWidth { get; set; }

        public BlockInfoModel Geom {get;set;}
 
    }
}
