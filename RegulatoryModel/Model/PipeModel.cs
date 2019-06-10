using RegulatoryModel.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
   public class PipeModel:ModelBase
    {
        
        string pipeInfo ;
        string pipePlanLine;
        string pipeActualityLine;


        public string PipeInfo { get => pipeInfo; set => pipeInfo = value; }
        /// <summary>
        /// 规划图层
        /// </summary>
        public string PipePlanLine { get => pipePlanLine; set => pipePlanLine = value; }
        /// <summary>
        /// 现状图层
        /// </summary>
        public string PipeActualityLine { get => pipeActualityLine; set => pipeActualityLine = value; }
    }

    public class PipeItemModel : CommandItemModel
    {

        string pipeLength;
        string pipeType;
        string colorIndex;
        string pipeWidth;
        PointF txtLocation;
        string pipeLayer;
        public List<PointF> pipeList;

        public string PipeLength { get => pipeLength; set => pipeLength = value; }
        public string PipeType { get => pipeType; set => pipeType = value; }
        public string ColorIndex { get => colorIndex; set => colorIndex = value; }
        public string PipeWidth { get => pipeWidth; set => pipeWidth = value; }
        public PointF TxtLocation { get => txtLocation; set => txtLocation = value; }
        public string PipeLayer { get => pipeLayer; set => pipeLayer = value; }

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
}
