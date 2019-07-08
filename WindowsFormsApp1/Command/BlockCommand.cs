using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using RegulatoryModel.Command;
using RegulatoryModel.Model;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Command
{
  public static  class BlockCommand
    {
        /// <summary>
        /// 获取块参照数据
        /// </summary>
        /// <param name="block2">块参照</param>
        /// <returns>提取的数据字符串</returns>
        public static List<string> AnalysisBlockReferenceFun(Autodesk.AutoCAD.DatabaseServices.BlockReference _br)
        {
            List<string> result = new List<string>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                result.Add("#BlockReference#");
                result.Add(ReflectionClass.GetAllPropertyInfoEx(_br, "BlockReference"));

                BlockTableRecord block = trans.GetObject(_br.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;

                foreach (ObjectId id in block)
                {
                    DBObject ODB = id.GetObject(OpenMode.ForRead);
                    if (ODB.GetRXClass().Equals(RXClass.GetClass(typeof(BlockReference))))
                    {
                        BlockReference br = trans.GetObject(id, OpenMode.ForRead) as BlockReference;
                        BlockTableRecord block1 = trans.GetObject(br.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;

                        result.Add("#Sub_BlockReference#");
                        result.Add(ReflectionClass.GetAllPropertyInfoEx(br, "Sub_BlockReference"));

                        foreach (var item in block1)
                        {
                            result.Add("#" + item.GetObject(OpenMode.ForRead).GetRXClass().Name + "#");
                            result.Add(ReflectionClass.GetAllPropertyInfoEx(trans.GetObject(item, OpenMode.ForRead), item.GetObject(OpenMode.ForRead).GetRXClass().Name));
                            result.Add("#" + item.GetObject(OpenMode.ForRead).GetRXClass().Name + "#");
                        }

                        result.Add("#Sub_BlockReference#");
                        break;
                    }
                    else
                    {
                        result.Add("#" + id.GetObject(OpenMode.ForRead).GetRXClass().Name + "#");
                        result.Add(ReflectionClass.GetAllPropertyInfoEx(trans.GetObject(id, OpenMode.ForRead), id.GetObject(OpenMode.ForRead).GetRXClass().Name));
                        result.Add("#" + id.GetObject(OpenMode.ForRead).GetRXClass().Name + "#");
                    }
                }
            }

            //扩展数据


            result.Add("#BlockReference#");
            return result;
        }
 
        /// <summary>
        /// 获取图层下的快参照
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<BlockReference> GetBlockReferenceByLayerType(string layerName)
        {
            List<BlockReference> references = new List<BlockReference>();
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            BlockReference _br = null;
            Transaction trans = null;

            var list = MethodCommand.GetObjectIdsAtLayer(layerName);
            using (trans = db.TransactionManager.StartTransaction())
            {
                if (list != null && list.Count > 0)
                {
                    foreach (ObjectId id in list)
                    {
                        _br = trans.GetObject(id, OpenMode.ForRead) as BlockReference;
                        if (_br != null)
                        {
                            references.Add(_br);
                        }
                    }
                }

                return references;
            }

        }

        public static BlockInfoModel AnalysisEntryAndExitbr(BlockReference br)
        {
            BlockInfoModel item = new BlockInfoModel();
            
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTableRecord block = trans.GetObject(br.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                foreach (ObjectId id in block)
                {
                    Entity entity = trans.GetObject(id, OpenMode.ForRead) as Entity;
                    if (entity is DBText)
                    {
                        DBText dbText = entity as DBText;
                        item.DbText.Add(AutoCad2ModelTools.DbText2Model(dbText)); ;
                    }
                    else if (entity is Polyline)
                    {
                        Polyline polyLine = entity as Polyline;
                        item.PolyLine .Add(AutoCad2ModelTools.Polyline2Model(polyLine));
                    }
                    else if (entity is Line)
                    {
                        item.Line.Add(AutoCad2ModelTools.Line2Model(entity as Line));
                    }
                    else if (entity is Hatch)
                    {
                        item.Hatch.Add( AutoCad2ModelTools.Hatch2Model(entity as Hatch));
                      

                    }
                    else if(entity is Circle)
                    {
                        item.Circle.Add(AutoCad2ModelTools.Circle2Model(entity as Circle));
                    }

                }
            }

            //扩展数据
            return item;
        }

        public static BlockInfoModel AnalysisEntryAndExitbr(BlockReference br,AttributeModel attModel)
        {
            BlockInfoModel item = new BlockInfoModel();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Dictionary<AttributeItemType, string> dicList = new Dictionary<AttributeItemType, string>();
                foreach (AttributeItemModel itemm in attModel.attributeItems)
                {

                    switch (itemm.AtItemType)
                    {
                        case AttributeItemType.UseLandNumber:
                            dicList.Add(AttributeItemType.UseLandNumber, GetValueByAttributeName(br, "用地代码", trans));
                            break;
                        case AttributeItemType.LotNumber:
                            dicList.Add(AttributeItemType.LotNumber, GetValueByAttributeName(br, "地块编号", trans));
                            break;

                    }
                }
               
                BlockTableRecord block = trans.GetObject(br.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                foreach (ObjectId id in block)
                {
                    Entity entity = trans.GetObject(id, OpenMode.ForRead) as Entity;
                    object result = null;
                    if (entity is DBText)
                    {
                        DBText dbText = entity as DBText;
                        result= AutoCad2ModelTools.DbText2Model(dbText, attModel);
                        item.DbText.Add(result as DbTextModel) ;
                    }
                    if (entity is MText)
                    {
                        MText dbText = entity as MText;
                        result = AutoCad2ModelTools.DbText2Model(dbText, attModel);
                        item.DbText.Add(result as DbTextModel);
                    }
                    else if (entity is Polyline)
                    {
                        Polyline polyLine = entity as Polyline;
                        result = AutoCad2ModelTools.Polyline2Model(polyLine, attModel);
                        item.PolyLine.Add(result as PolyLineModel);
                    }
                    else if (entity is Line)
                    {
                        result = AutoCad2ModelTools.Line2Model(entity as Line, attModel);
                        item.Line.Add(result as LineModel);
                    }
                    else if (entity is Hatch)
                    {
                        result = AutoCad2ModelTools.Hatch2Model(entity as Hatch, attModel);
                        item.Hatch.Add(result as HatchModel);
                    }
                    else if (entity is Circle)
                    {
                        result = AutoCad2ModelTools.Circle2Model(entity as Circle, attModel);
                        item.Circle.Add(result as CircleModel);
                    }
                  
                    if (result is PolyLineModel)
                    {
                        foreach (GemoTypeModel gemoType in (result as PolyLineModel).Vertices)
                        {
                            foreach (AttributeItemModel atm in (gemoType as GemoTypeModel).attItemList)
                            {
                                if (dicList.ContainsKey(atm.AtItemType))
                                {
                                    atm.AtValue = dicList[atm.AtItemType];
                                }
                            }
                        }
                    }
                    else if (result is HatchModel)
                    {
                        foreach (GemoTypeModel gemoType in (result as HatchModel).loopPoints.Values)
                        {
                            foreach (AttributeItemModel atm in (result as GemoTypeModel).attItemList)
                            {
                                if (dicList.ContainsKey(atm.AtItemType))
                                {
                                    atm.AtValue = dicList[atm.AtItemType];
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (AttributeItemModel atm in (result as GemoTypeModel).attItemList)
                        {
                            if (dicList.ContainsKey(atm.AtItemType))
                            {
                                atm.AtValue = dicList[atm.AtItemType];
                            }
                        }
                    }
                }
            }

            //扩展数据
            return item;
        }

        /////  
        ///// 根据当前文档和块名取得当前块的引用 
        /////  
        //public dbx.AcadBlockReference GetBlockReference(dbx.AxDbDocument thisDrawing, string blkName)
        //{
        //    dbx.AcadBlockReference blkRef = null;
        //    bool found = false;
        //    try
        //    {
        //        foreach (dbx.AcadEntity entity in thisDrawing.ModelSpace)
        //        {
        //            if (entity.EntityName == "AcDbBlockReference")
        //            {
        //                blkRef = (dbx.AcadBlockReference)entity;
        //                //System.Windows.Forms.MessageBox.Show(blkRef.Name); 
        //                if (blkRef.Name.ToLower() == blkName.ToLower())
        //                {
        //                    found = true;
        //                    break;
        //                }
        //            }//end of entity.EntityName=="AcDbBlockReference" 
        //        }// end of foreach thisDrawing.ModelSpace 
        //    }//end of try 
        //    catch (Exception e)
        //    {
        //        System.Windows.Forms.MessageBox.Show("图形中有未知的错误，格式不正确或图形数据库需要修愎。系统错误提示：" + e.Message, "信息", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        //        thisDrawing = null;
        //    }//end of catch 
        //    if (!found) blkRef = null;
        //    return blkRef;
        //}//end of function GetBlockReference 
        ///  
        /// 根据给定的块引用(dbx.AcadBlockReference)和属性名返回属性值 
        ///  
        public static string GetValueByAttributeName(BlockReference blkRef, string AttributeName,Transaction trans)
        {
           AttributeCollection Atts =blkRef.AttributeCollection;
            string attValue = null;
            for (int i = 0; i<Atts.Count;i++)
            {
                AttributeReference attRef= trans.GetObject(Atts[i], OpenMode.ForRead) as AttributeReference;
          
                if (attRef.Tag == AttributeName)
                {
                    attValue = attRef.TextString;
                    break;
                }
            }//end of for i 
            return attValue;
        }// end of function 


    }
}
