using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryPlan.Model
{
   public class LengedModel
    {
       string layerName;
       public List<BlockInfoModel> GemoModels;
       public List<PointF> BoxPointList;

        public string LayerName { get => layerName; set => layerName = value; }
    }
}
