using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
  public  class RainWaterModel : PipeModel
    {
        public RainWaterModel()
        {
            this.LayerList = new List<string>() { "现状雨水管线", "现状污水管线" ,"规划雨水管线"};
            this.PipeInfo = "雨水管径";
            this.DerivedType = DerivedTypeEnum.RainWater;
        }
    }
}
