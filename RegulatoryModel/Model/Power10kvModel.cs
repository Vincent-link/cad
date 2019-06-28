using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
   public class Power10kvModel:PipeModel
    {
        public Power10kvModel()
        {
            this.DerivedType = DerivedTypeEnum.Power10Kv;
            this.attributes = new List<AttributeModel>()
            {
                new AttributeModel()
            {
            LayerName = "*管线",
                     attributeItems = new List<AttributeItemModel>()
                     {
                         new AttributeItemModel()
                         {
                               TargetName ="线条宽度",
                              AtGroupType= AttributeGroupType.Graph,
                              AtItemType=AttributeItemType.Overallwidth,
                         },
                           new AttributeItemModel()
                         {
                               TargetName ="线型比例",
                              AtGroupType= AttributeGroupType.Entity,
                              AtItemType=AttributeItemType.LineScale,
                         }

                     }
                },


            };
        }
    }
}
