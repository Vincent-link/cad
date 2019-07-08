using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
   public class CenterCityLifeUseLandPlanModel:AttributeBaseModel
    {
        public CenterCityLifeUseLandPlanModel()
        {
            this.DerivedType = DerivedTypeEnum.CenterCityLifeUseLandPlan;
            this.attributes = new List<AttributeModel>()
            {
                new AttributeModel()
                {
                    LayerName="字",
                     attributeItems=new List<AttributeItemModel>()
                     {
                         new AttributeItemModel()
                         {
                               TargetName ="总户数",
                              AtGroupType= AttributeGroupType.Txt,
                              AtItemType=AttributeItemType.Content,
                         }, new AttributeItemModel()
                         {
                               TargetName ="总人数",
                              AtGroupType= AttributeGroupType.Txt,
                              AtItemType=AttributeItemType.Content,
                         }
                     }
                }
                
            };
        }
    }
}
