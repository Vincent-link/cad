
using RegulatoryModel.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
    public class RoadSectionModel: RoadModel
    {
     
      

        public RoadSectionModel()
        {
            RoadNameLayer = "TX-道路名";
            RoadLineLayer = "RD-中线";
            RoadSectionLayer = "横断面符号";
            this.specailLayers = new List<string>() {RoadLineLayer,RoadNameLayer};
            this.DerivedType = DerivedTypeEnum.TheRoadSection;
        }

       

        //public override void AddSpecialLayerModel()
        //{
        //    RoadMethod<RoadSectionModel> road = new RoadMethod<RoadSectionModel>();
        //    road.GetAllRoadInfo(this);
        //}
    }

    public class RoadInfoItemModel:CommandItemModel
    {
        string roadName;
        string roadLength;
        string roadType;
        string colorIndex;
        string roadWidth;
        string roadNameLayer;
        string roadNameType;
        public List<PointF> roadList=new List<PointF>();
        public List<RoadSectionItemModel> sectionList= new List<RoadSectionItemModel>();

        public string RoadName { get => roadName; set => roadName = value; }
        public string RoadLength { get => roadLength; set => roadLength = value; }
        public string RoadType { get => roadType; set => roadType = value; }
        public string ColorIndex { get => colorIndex; set => colorIndex = value; }
        public string RoadWidth { get => roadWidth; set => roadWidth = value; }
        public List<PointF> RoadNameLocaiton = new List<PointF>();
        public string RoadNameLayer { get => roadNameLayer; set => roadNameLayer = value; }
        public string RoadNameType { get => roadNameType; set => roadNameType = value; }

        public override string ItemToJson()
        {
            string outJson = "{";
            outJson += JsonCommand.ToJson("roadName", RoadName);
            outJson += JsonCommand.ToJson("roadLength", RoadLength);
            outJson += JsonCommand.ToJson("ColorIndex", ColorIndex);
            if (roadList != null)
            {
                outJson +=  JsonCommand.ToJson("roadList") + ":" + JsonCommand.ToJson(roadList);
            }
            return outJson+"}";
        }
    }
    public class RoadSectionItemModel 
    {
        DbTextModel sectionName;
        PolyLineModel line;

        public DbTextModel SectionName { get => sectionName; set => sectionName = value; }
        public PolyLineModel Line { get => line; set => line = value; }
    }
}
