using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{
    
    /// <summary>
    /// 文本线条样式
    /// </summary>
    public class StyleModel
    {
       
        public TextStyleModel TextStyle { get; set; }
       
        public LineStyleModel LineStyle { get; set; }
    }
   
    /// <summary>
    /// 文本样式模型
    /// </summary>
    public class TextStyleModel
    {
       
        public Color TextColor { get; set; }
       
        public Size TextSize { get; set; }
    }
 
    /// <summary>
    /// 线条样式
    /// </summary>
    public class LineStyleModel
    {
       
        public Color LineColor { get; set; }
       
        public int LineThickness { get; set; }
    }
  
    /// <summary>
    /// 标注样式
    /// </summary>
    public class DimensionStyleModel
    {
       
        public LineStyleModel LineStyle { get; set; }
       
        public Size DimensionSize { get; set; }
    }
}
