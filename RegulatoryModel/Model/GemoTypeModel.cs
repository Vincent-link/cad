using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
  public  class GemoTypeModel
    {
        private string geoType;
        private string zIndex="0";
        private string attrIndex = "";

        public string GeoType { get => geoType; set => geoType = value; }
        public string ZIndex { get => zIndex; set => zIndex = value; }
        public string AttrIndex { get => attrIndex; set => attrIndex = value; }

        public virtual string ToInfoString()
        {
            return "";
        }
    }
}
