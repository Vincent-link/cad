using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
   public class ReuseWaterModel : PipeModel
    {
        public ReuseWaterModel() : base()
        {
            this.DerivedType = DerivedTypeEnum.ReuseWater;
        }
    }
}