using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
   public class Power10kvModel:ModelBase
    {
        public Power10kvModel()
        {
            this.DerivedType = DerivedTypeEnum.Power10Kv;
        }
    }
}
