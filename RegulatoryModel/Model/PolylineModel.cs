using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RegulatoryModel.Model
{

    #region 几何模型
    
    public class PolyLineModel : GemoTypeModel
    {
       
        /// <summary>
        /// 面积
        /// </summary>
        public double Area { get; set; }
       
        /// <summary>
        /// 是否有曲线
        /// </summary>
        public bool IsLines { get; set; }
       
        /// <summary>
        /// 是否闭合
        /// </summary>
        public bool Closed { get; set; }
       
        /// <summary>
        /// 顶点坐标
        /// </summary>
        public ArrayList Vertices { get; set; }
    }
    /// <summary>
    /// 直线模型
    /// </summary>
    
    public class LineModel : GemoTypeModel
    {
       
        public double Angle { get; set; }
       
        /// <summary>
        /// 开始位置
        /// </summary>
        public PointF StartPoint { get; set; }
       
        /// <summary>
        /// 结束位置
        /// </summary>
        public PointF EndPoint { get; set; }
       
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; set; }
    }
    
    /// <summary>
    /// 曲线模型
    /// </summary>
    public class ArcModel : GemoTypeModel
    {
       
        /// <summary>
        /// 圆心
        /// </summary>
        public PointF Center { get; set; }
       
        /// <summary>
        /// 起始角度
        /// </summary>

        public double Startangel { get; set; }
       
        /// <summary>
        /// 结束角度
        /// </summary>
        public double EndAngel { get; set; }
       
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
       
        /// <summary>
        /// 起始点
        /// </summary>
        public PointF StartPoint { get; set; }
       
        /// <summary>
        /// 结束点
        /// </summary>
        public PointF EndPoint { get; set; }
    }
    /// <summary>
    /// 圆
    /// </summary>
    public class CircleModel : GemoTypeModel
    {
        /// <summary>
        /// 圆心
        /// </summary>
        public PointF Center { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
    }
    /// <summary>
    /// 椭圆
    /// </summary>
    public class EllipseModel : GemoTypeModel
    {
        /// <summary>
        /// 圆心
        /// </summary>
        public PointF Center { get; set; }
        /// <summary>
        /// 长边
        /// </summary>
        public PointF MajorAxis { get; set; }
        /// <summary>
        /// 短边
        /// </summary>
        public PointF MinorAxis { get; set; }
    }

    public class HatchModel : GemoTypeModel
    {
        public List<PointF> loopPoints=new List<PointF>();
        public double Area { get; set; }
        public string Color { get; set; }

    }

    /// <summary>
    /// 文本
    /// </summary>
    public class DbTextModel:GemoTypeModel
    {
       
        /// <summary>
        /// 位置
        /// </summary>
        public PointF Position { get; set; }
       
        /// <summary>
        /// 文本宽度
        /// </summary>
        public double ThickNess { get; set; }
       
        /// <summary>
        /// 文本高度
        /// </summary>
        public double Height { get; set; }
       
        /// <summary>
        /// 文本倾斜角度
        /// </summary>
        public double Rotation { get; set; }
        /// <summary>
        /// 文字
        /// </summary>
       
        public string Text { get; set; }

   
    }
    /// <summary>
    /// 标注
    /// </summary>
    public class AlignedDimensionModel : GemoTypeModel
    {
        /// <summary>
        /// 标注位置
        /// </summary>
        public string DimLinePoint { get; set; }
        /// <summary>
        /// 角度
        /// </summary>
        public double Oblique { get; set; }
        /// <summary>
        /// 标注点1
        /// </summary>
        public string XLine1Point { get; set; }
        /// <summary>
        /// 标注点2
        /// </summary>
        public string XLine2Point { get; set; }
    }
    #endregion
}
