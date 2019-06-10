
using RegulatoryModel.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
    public class RoadSectionModel : ModelBase
    {

        public static string roadNameLayer = "Tx-道路名";
        public static string roadLineLayer = "道路";
        public RoadSectionModel()
        {
            this.specailLayers = new List<string>() {roadLineLayer,roadNameLayer};
            this.DerivedType = DerivedTypeEnum.TheRoadSection;
        }

        //public override void AddSpecialLayerModel()
        //{
        //    RoadMethod<RoadSectionModel> road = new RoadMethod<RoadSectionModel>();
        //    road.GetAllRoadInfo(this);
        //}
    }

    public class RoadSectionItemModel:CommandItemModel
    {
        string roadName;
        string roadLength;
        string roadType;
        string colorIndex;
        string roadWidth;
        public List<PointF> roadList;

        public string RoadName { get => roadName; set => roadName = value; }
        public string RoadLength { get => roadLength; set => roadLength = value; }
        public string RoadType { get => roadType; set => roadType = value; }
        public string ColorIndex { get => colorIndex; set => colorIndex = value; }
        public string RoadWidth { get => roadWidth; set => roadWidth = value; }
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
}
