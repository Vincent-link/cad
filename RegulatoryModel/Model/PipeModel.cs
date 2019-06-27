using RegulatoryModel.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
   public class PipeModel:AttributeBaseModel
    {
        public PipeModel()
        {
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
                         },
                            new AttributeItemModel()
                         {
                              TargetName ="管线线型",
                              AtGroupType= AttributeGroupType.Entity,
                              AtItemType=AttributeItemType.LineType,
                         },
                     }
                },
                 new AttributeModel()
        {
                    LayerName = "*管径",
                     attributeItems = new List<AttributeItemModel>()
                     {
                         new AttributeItemModel()
                         {
                              TargetName ="文字高度",
                              AtGroupType= AttributeGroupType.Txt,
                              AtItemType=AttributeItemType.TxtHeight,
                         }
                     }
                },
                   new AttributeModel()
        {
            LayerName = "*管长",
                     attributeItems = new List<AttributeItemModel>()
                     {
                      new AttributeItemModel()
                         {    TargetName="文字高度",

                              AtGroupType= AttributeGroupType.Txt,
                              AtItemType=AttributeItemType.TxtHeight,
                         } }
                }
                 
            };
            ///// <summary>
        }
        string pipeInfo ;
        //string pipePlanLine;
        //string pipeActualityLine;
        List<string> pipeNameList;

        public string PipeInfo { get => pipeInfo; set => pipeInfo = value; }

     
    ///// 规划图层
    ///// </summary>
    //public string PipePlanLine { get => pipePlanLine; set => pipePlanLine = value; }
    ///// <summary>
    ///// 现状图层
    ///// </summary>
    //public string PipeActualityLine { get => pipeActualityLine; set => pipeActualityLine = value; }
}

    public class PipeItemModel : CommandItemModel
    {

        string pipeLength;
        string pipeType="polyline";
        string pipeText;
        string colorIndex;
        PipeItemStyleModel style=new PipeItemStyleModel();
        //string pipeWidth;
        PointF txtLocation;
        string pipeLayer;
        public List<PointF> pipeList;
      

        public string PipeLength { get => pipeLength; set => pipeLength = value; }
        public string PipeText{ get => pipeText; set => pipeText = value; }
        public string PipeType { get => pipeType; set => pipeType = value; }
        public string ColorIndex { get => colorIndex; set => colorIndex = value; }
      //  public string PipeWidth { get => pipeWidth; set => pipeWidth = value; }
        public PointF TxtLocation { get => txtLocation; set => txtLocation = value; }
        public string PipeLayer { get => pipeLayer; set => pipeLayer = value; }
        public PipeItemStyleModel Style { get => style; set => style = value; }

        public override string ItemToJson()
        {
            string outJson = "{";
            outJson += JsonCommand.ToJson("pipeLength", PipeLength);
            outJson += JsonCommand.ToJson("piepType", PipeType);
            outJson += JsonCommand.ToJson("colorIndex", ColorIndex);
            if (pipeList != null)
            {
                outJson += JsonCommand.ToJson("roadList") + ":" + JsonCommand.ToJson(pipeList);
            }
            return outJson + "}";
        }
    }

    public class PipeItemStyleModel
    { 
        string textHeight;
        string lineWidth;
        string lineScale;
        string lineType;

        public string TextHeight { get => textHeight; set => textHeight = value; }
        public string LineWidth { get => lineWidth; set => lineWidth = value; }
        public string LineScale { get => lineScale; set => lineScale = value; }
        public string LineType { get => lineType; set => lineType = value; }
    }
}
