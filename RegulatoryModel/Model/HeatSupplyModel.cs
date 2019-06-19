using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
  public  class HeatSupplyModel:PipeModel
    {
        public HeatSupplyModel()
        {
            this.LayerList = new List<string>() { "规划供热管道", "供热管道", "三水厂给水" };
            PipeInfo = "管线尺寸";
            this.DerivedType = DerivedTypeEnum.HeatSupply;
        }
    }
}
