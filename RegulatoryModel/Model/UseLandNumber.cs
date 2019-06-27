using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
    public class UseLandNumberModel : AttributeBaseModel
    {
        public UseLandNumberModel()
        {
            this.DerivedType = DerivedTypeEnum.UseLandNumber;
            this.attributes = new List<AttributeModel>()
            {
                new AttributeModel()
                {
                LayerName = "地块编码",
                attributeItems = new List<AttributeItemModel>()
                     {
                         new AttributeItemModel()
                         {
                              TargetName ="地块编号",
                              AtGroupType= AttributeGroupType.Property,
                              AtItemType=AttributeItemType.LotNumber,
                         },
                           new AttributeItemModel()
                         {
                               TargetName ="地块编号",
                              AtGroupType= AttributeGroupType.Property,
                              AtItemType=AttributeItemType.UseLandNumber,
                         }

                     }
            }

        };
        }
    }
}
