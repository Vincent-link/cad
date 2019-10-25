using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Models
{
    public class Stage
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Stage> child { get; set; }

       
    }
    public class StageJsonData
    {
        public List<Stage> result { get; set; }
        public string msg { get; set; }
        public bool noData { get; set; }
        public bool noLogin { get; set; }
        public bool noRight { get; set; }
        public string returncode { get; set; }
        public int code { get; set; }
    }
}
