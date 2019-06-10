using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
  
       
        public class BlockInfoModel
        {
            public BlockInfoModel()
            {
                Line = new List<LineModel>();
                Arc = new List<ArcModel>();
                Circle = new List<CircleModel>();
            }
       
            /// <summary>
            /// 标注的位置点
            /// </summary>
            public PointF DimensionPositon { get; set; }
      
            /// <summary>
            /// 多段线
            /// </summary>
            public PolyLineModel PolyLine { get; set; }

            /// <summary>
            /// 线
            /// </summary>
            public List<LineModel> Line { get; set; }
            /// <summary>
            /// 圆
            /// </summary>
            public List<CircleModel> Circle { get; set; }
            /// <summary>
            /// 圆
            /// </summary>
            public List<ArcModel> Arc { get; set; }
            public HatchModel Hatch { get; set; }

            /// <summary>
            /// 文本
            /// </summary>
            public DbTextModel DbText { get; set; }
         
            /// <summary>
            /// 文本样式
            /// </summary>
            public TextStyleModel TextStyle { get; set; }

        }
    
}
