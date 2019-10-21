using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Models
{
    public class City
    {
        public string name{ get; set; }
        public string oid { get; set; }
    }
    public class JsonCityData
    {
        public List<City> result { get; set; }
        public string msg { get; set; }
        public bool noData { get; set; }
        public bool noLogin { get; set; }
        public bool noRight { get; set; }
        public string returncode { get; set; }
        public int code { get; set; }
    }
}
