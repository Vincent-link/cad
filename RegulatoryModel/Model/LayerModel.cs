
using RegulatoryModel.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
  public  class LayerModel
    {
        private string name;
        private int color;
        public Dictionary<int,List<object>> pointFs;
        public List<object> modelItemList;
        private string jsonStr;

        public string JsonStr { get
            {
                jsonStr = "";
                if (pointFs == null && modelItemList == null) { } else { jsonStr += "{\"Color\":" + Color + ","; }
               
                if (pointFs != null)
                {
                    jsonStr += "\"GeomType\":" + Name + "\",\"GemoList\":";
                    jsonStr += "[";
                    foreach (KeyValuePair<int, List<object>> pfitem in pointFs)
                    {
                        jsonStr += "{\"GemoIndex\":" + pfitem.Key + ",";
                        jsonStr += "\"GemoPoints\":";
                        jsonStr += "[";
                        jsonStr += JsonCommand.ToJson(pfitem.Value);
                        jsonStr += "]";
                        jsonStr += "},";
                    }
                    jsonStr = jsonStr.TrimEnd(',');
                    jsonStr += "]},";
                }if (modelItemList != null)
                {
                    jsonStr += "\"GeomType\":" + Name + "\",\"GemoList\":";
                    jsonStr += "[";
                    jsonStr += JsonCommand.ToJson(modelItemList);
                    jsonStr += "]";
                }

                return jsonStr;
            } set => jsonStr = value; }

        public int Color { get => color; set => color = value; }
        public string Name { get => name; set => name = value; }
    }
}
