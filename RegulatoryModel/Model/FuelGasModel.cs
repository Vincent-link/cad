using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
  public  class FuelGasModel:PipeModel
    {
        public FuelGasModel()
        {
            this.LayerList = new List<string>() { "规划燃气管道", "已建晨光燃气管道", "已建茂祥燃气管道"};
            PipeInfo = "管线尺寸";
            this.DerivedType = DerivedTypeEnum.FuelGas;
        }
    }
}
