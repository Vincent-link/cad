﻿using Autodesk.AutoCAD.ApplicationServices;
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
    }
}
