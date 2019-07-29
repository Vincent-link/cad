﻿
using RegulatoryModel.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
    public class RoadNoSectionModel: RoadModel
    {
        string roadSectionName;
      

        public RoadNoSectionModel()
        {
            RoadNameLayer = "路名";
            RoadLineLayer = "道路";
            RoadSectionLayer = "横断面符号";
            this.specailLayers = new List<string>() {RoadLineLayer,RoadNameLayer};
            this.DerivedType = DerivedTypeEnum.Road;
        }

        public string RoadSectionLayer  {get => roadSectionName; set => roadSectionName = value; }

        //public override void AddSpecialLayerModel()
        //{
        //    RoadMethod<RoadSectionModel> road = new RoadMethod<RoadSectionModel>();
        //    road.GetAllRoadInfo(this);
        //}
    }

  
}
