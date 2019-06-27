using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
    public enum AttributeGroupType
    {
        /// <summary>
        /// 基本
        /// </summary>
        Entity,
        /// <summary>
        /// 图形
        /// </summary>
        Graph,
        /// <summary>
        /// 文本
        /// </summary>
        Txt,
        /// <summary>
        /// 属性
        /// </summary>
        Attribute,
        /// <summary>
        /// 其他
        /// </summary>
        Other,
        /// <summary>
        /// 属性
        /// </summary>
        Property
    }

    public enum AttributeItemType
    {
        Color,
        /// <summary>
        /// 内容
        /// </summary>
        Content,
        /// <summary>
        /// 线型比例
        /// </summary>
        LineScale,
        /// <summary>
        /// 文本高度
        /// </summary>
        TxtHeight,
        /// <summary>
        /// 线型
        /// </summary>
        LineType,
        /// <summary>
        /// 全局宽度
        /// </summary>
        Overallwidth,
        /// <summary>
        /// 面积
        /// </summary>
        Area,
        /// <summary>
        /// 累计面积
        /// </summary>
        TotalArea,
        /// <summary>
        /// 图层名
        /// </summary>
            LayerName,
            /// <summary>
            /// 地块编码
            /// </summary>
            LotNumber,
            /// <summary>
            /// 用地代码
            /// </summary>
            UseLandNumber
    }

    public class AttributeBaseModel:ModelBase
    {
        
    
        public   List<AttributeModel> attributes = new List<AttributeModel>();

    }

    public class AttributeItemModel
    {
        string targetName;

        AttributeGroupType atGroupType;
        AttributeItemType atItemType;
        string atValue;
        public string TargetName { get => targetName; set => targetName = value; }

        public AttributeGroupType AtGroupType { get => atGroupType; set => atGroupType = value; }
        public AttributeItemType AtItemType { get => atItemType; set => atItemType = value; }
        public string AtValue { get => atValue; set => atValue = value; }
    }

    public class AttributeModel
    {
    
        public List<AttributeItemModel> attributeItems = new List<AttributeItemModel>();
        string layerName;
        public string LayerName { get => layerName; set => layerName = value; }

    }
}
