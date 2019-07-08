
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
    public class SewageModel : PipeModel
    {
        public SewageModel() : base()
        {
            this.LayerList = new List<string>() { "现状污水管线", "规划污水管线" };
     
           PipeInfo = "管道管径";
            this.DerivedType = DerivedTypeEnum.Sewage;
        }
        //public override void AddSpecialLayerModel()
        //{
        //    PipeMethod<PipeModel> road = new PipeMethod<PipeModel>();
        //    road.GetAllPipeInfo(this);
        //}


    }

}
