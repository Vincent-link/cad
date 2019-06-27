using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
   public class WaterSupplyModel:PipeModel
    {
        public WaterSupplyModel():base()
        {
            this.LayerList = new List<string>() { "现状给水管线", "规划给水管线" };
            this.PipeInfo = "给水管径";
            this.DerivedType = DerivedTypeEnum.WaterSupply;
        }
    }
}
