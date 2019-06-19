using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
  public  class RoadModel:ModelBase
    {
        string roadNameLayer;
         string roadLineLayer;
        string roadSectionName;

        public string RoadNameLayer { get => roadNameLayer; set => roadNameLayer = value; }
        public string RoadLineLayer { get => roadLineLayer; set => roadLineLayer = value; }
        public string RoadSectionLayer { get => roadSectionName; set => roadSectionName = value; }
    }
}
