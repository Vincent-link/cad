using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
   public class CenterCityUseLandPlanModel:AttributeBaseModel
    {
        public CenterCityUseLandPlanModel()
        {
            this.DerivedType = DerivedTypeEnum.CenterCityUseLandPlan;
            this.attributes = new List<AttributeModel>()
            {
                //new AttributeModel()
                //{
                //    LayerName="用地代码",
                //     attributeItems=new List<AttributeItemModel>()
                //     {
                //         new AttributeItemModel()
                //         {
                //               TargetName ="用地代码",
                //              AtGroupType= AttributeGroupType.Txt,
                //              AtItemType=AttributeItemType.Content,
                //         },
                //     }
                //},
                // new AttributeModel()
                //{
                //   LayerName="公园绿地",
                //     attributeItems=new List<AttributeItemModel>()
                //     {
                //         new AttributeItemModel()
                //         {    TargetName="绿地面积",

                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.Area,
                //         },
                //          new AttributeItemModel()
                //         {
                //              TargetName="绿地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.TotalArea,
                //         },
                //            new AttributeItemModel()
                //         {
                //            TargetName="用地名称",
                //              AtGroupType= AttributeGroupType.Entity,
                //              AtItemType=AttributeItemType.LayerName,
                //         },
                //             new AttributeItemModel()
                //         {
                //              TargetName="用地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.TotalArea,
                //         },
                //            new AttributeItemModel()
                //         {
                //              TargetName="用地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.Area,
                //         },
                //          new AttributeItemModel()
                //         {
                //           TargetName="地块颜色",
                //              AtGroupType= AttributeGroupType.Entity,
                //              AtItemType=AttributeItemType.Color
                //         },
                //     }
                //},
                //   new AttributeModel()
                //{
                //    LayerName="防护绿地",
                //     attributeItems=new List<AttributeItemModel>()
                //     {
                //      new AttributeItemModel()
                //         {    TargetName="绿地面积",

                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.Area,
                //         },
                //          new AttributeItemModel()
                //         {
                //              TargetName="绿地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.TotalArea,
                //         },
                //            new AttributeItemModel()
                //         {
                //            TargetName="用地名称",
                //              AtGroupType= AttributeGroupType.Entity,
                //              AtItemType=AttributeItemType.LayerName,
                //         },

                //            new AttributeItemModel()
                //         {
                //              TargetName="用地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.Area,
                //         },
                //          new AttributeItemModel()
                //         {
                //           TargetName="地块颜色",
                //              AtGroupType= AttributeGroupType.Entity,
                //              AtItemType=AttributeItemType.Color
                //         } }
                //},
                //     new AttributeModel()
                //{
                //     LayerName="广场用地",
                //     attributeItems=new List<AttributeItemModel>()
                //     {
                //      new AttributeItemModel()
                //         {
                //             TargetName ="绿地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.Area,
                //         },
                //          new AttributeItemModel()
                //         {
                //              TargetName="绿地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.TotalArea,
                //         },
                //     }
                //},
                //     new AttributeModel()
                //{
                //     LayerName="*用地",
                //     attributeItems=new List<AttributeItemModel>()
                //     {

                //            new AttributeItemModel()
                //         {
                //            TargetName="用地名称",
                //              AtGroupType= AttributeGroupType.Entity,
                //              AtItemType=AttributeItemType.LayerName,
                //         },
                //              new AttributeItemModel()
                //         {
                //              TargetName="用地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.TotalArea,
                //         },
                //            new AttributeItemModel()
                //         {
                //              TargetName="用地面积",
                //              AtGroupType= AttributeGroupType.Graph,
                //              AtItemType=AttributeItemType.Area,
                //         },
                //          new AttributeItemModel()
                //         {
                //           TargetName="地块颜色",
                //              AtGroupType= AttributeGroupType.Entity,
                //              AtItemType=AttributeItemType.Color
                //         },
                //     }
                //},
                //     new AttributeModel()
                //    {
                //         LayerName="*绿地",
                //         attributeItems=new List<AttributeItemModel>()
                //         {

                //                new AttributeItemModel()
                //             {
                //                TargetName="用地名称",
                //                  AtGroupType= AttributeGroupType.Entity,
                //                  AtItemType=AttributeItemType.LayerName,
                //             },
                //                  new AttributeItemModel()
                //             {
                //                  TargetName="用地面积",
                //                  AtGroupType= AttributeGroupType.Graph,
                //                  AtItemType=AttributeItemType.TotalArea,
                //             },
                //                new AttributeItemModel()
                //             {
                //                  TargetName="用地面积",
                //                  AtGroupType= AttributeGroupType.Graph,
                //                  AtItemType=AttributeItemType.Area,
                //             },
                //              new AttributeItemModel()
                //             {
                //               TargetName="地块颜色",
                //                  AtGroupType= AttributeGroupType.Entity,
                //                  AtItemType=AttributeItemType.Color
                //             },
                //         }
                //    },
                    new AttributeModel()
                    {
                         LayerName="路名",
                         attributeItems=new List<AttributeItemModel>()
                         {

                            //new AttributeItemModel()
                            // {
                            //    TargetName="路名",
                            //      AtGroupType= AttributeGroupType.Txt,
                            //      AtItemType=AttributeItemType.Content,
                            // },
                         }
                    },

            };
        }
    }
}
