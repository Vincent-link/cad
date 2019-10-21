using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace RegulatoryModel.Model
{

    #region 几何模型

    public class PolyLineModel : GemoTypeModel
    {
        public PolyLineModel()
        {
            GeoType = "polyline";
        }

        public string Color { get; set; }
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

        /// <summary>
        /// 个体名称
        /// </summary>
        public string individualName { get; set; }
        /// <summary>
        /// 个体要素
        /// </summary>
        public string individualFactor { get; set; }
        /// <summary>
        /// 个体编码
        /// </summary>
        public string individualCode { get; set; }
        /// <summary>
        /// 是否是虚线
        /// </summary>
        public bool isDashed { get; set; }
    }
    /// <summary>
    /// 直线模型
    /// </summary>
    
    public class LineModel : GemoTypeModel
    {
        public LineModel()
        {
            GeoType = "line";
        }

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
        public string Color { get; set; }
        /// <summary>
        /// 是否是虚线
        /// </summary>
        public bool isDashed { get; set; }
    }
    
    /// <summary>
    /// 曲线模型
    /// </summary>
    public class ArcModel : GemoTypeModel
    {
        public ArcModel()
        {
            GeoType = "arc";
        }
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

        public List<PointF> pointList = new List<PointF>();

        public string Color { get; set; }
        /// <summary>
        /// 是否是虚线
        /// </summary>
        public bool isDashed { get; set; }
    }
    /// <summary>
    /// 圆
    /// </summary>
    public class CircleModel : GemoTypeModel
    {

        public List<PointF> pointList = new List<PointF>();
        public CircleModel()
        {
            GeoType = "circle";
        }
        /// <summary>
        /// 圆心
        /// </summary>
        public PointF Center { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
        public string Color { get; set; }
        /// <summary>
        /// 是否是虚线
        /// </summary>
        public bool isDashed { get; set; }
    }
    /// <summary>
    /// 椭圆
    /// </summary>
    public class EllipseModel : GemoTypeModel
    {

        public List<PointF> pointList = new List<PointF>();
        public EllipseModel()
        {
            GeoType = "ellipse";
        }
        /// <summary>
        /// 圆心
        /// </summary>
        public PointF Center { get; set; }
        /// <summary>
        /// 长边
        /// </summary>
        public double MajorAxis { get; set; }
        /// <summary>
        /// 短边
        /// </summary>
        public double MinorAxis { get; set; }
        public string Color { get; set; }
        /// <summary>
        /// 是否是虚线
        /// </summary>
        public bool isDashed { get; set; }
    }

    public class HatchModel : GemoTypeModel
    {
        public HatchModel()
        {
            GeoType = "polygon";
        }
        public Dictionary<int,ColorAndPointItemModel> loopPoints=new Dictionary<int,ColorAndPointItemModel>();
        public double Area { get; set; }
       // public string Color { get; set; }

    }

    public class ColorAndPointItemModel :GemoTypeModel
    {
        private string zIndex="1";
        public ColorAndPointItemModel()
        {
            loopPoints = new List<PointF>();
            // GeoType = "polygon";
        }
      
        public  List<PointF> loopPoints;
        public string Color { get; set; }
        public string ZIndex { get => zIndex; set => zIndex = value; }
    }

    /// <summary>
    /// 文本
    /// </summary>
    public class DbTextModel:GemoTypeModel
    {
        public DbTextModel()
        {
            GeoType = "text";
        }
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
        public string Color { get; set; }


    }
    /// <summary>
    /// 标注
    /// </summary>
    public class AlignedDimensionModel : GemoTypeModel
    {
        public AlignedDimensionModel()
        {
            GeoType = "dimension";
        }
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
