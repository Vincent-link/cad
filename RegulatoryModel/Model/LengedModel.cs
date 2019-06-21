using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
    public class LengedModel
    {
        string layerName;
        public List<BlockInfoModel> GemoModels;
        public List<PointF> BoxPointList;
     
        string backGround;
        public string LayerName { get => layerName; set => layerName = value; }
        public string BackGround { get => backGround; set => backGround = value; }
        
    }

    public class LengedJsonModel
    {
        string title { get; set; }
		string backGround { get; set; }
        List<canvas> canvasList = new List<canvas>();
    }

    public class canvas
    {
        List<GemoList> gemoLists = new List<GemoList>();
			
    }

    public class GemoList
    {
        string title { get; set; }
        string backGround { get; set; }

    }

    public class Gemo
    {
        string IsEmpty { get; set; }
        string fillstyle { get; set; }
       string style { get; set; }
        string x { get; set; }
        string y { get; set; }

    }
}
