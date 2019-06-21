using Autodesk.AutoCAD.ApplicationServices;
using RegulatoryPlan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using RegulatoryPlan.Command;
using System.Drawing;
using Autodesk.AutoCAD.Geometry;
using RegulatoryModel.Model;

namespace RegulatoryPlan.Method
{
   public  class ModelBaseMethod<T> where T:ModelBase
    {
        List<Polyline> lengedList = new List<Polyline>();
        List<ObjectId> allModelId = new List<ObjectId>();
        //public  void GetLengedPoints(T model)
        //{
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    ObjectIdCollection ids = new ObjectIdCollection();

        //    PromptSelectionResult ProSset = null;
        //    TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, MethodCommand.LegendLayer)};
        //    SelectionFilter sfilter = new SelectionFilter(filList);
        //    LayoutManager layoutMgr = LayoutManager.Current;
          
        //   string ss= layoutMgr.CurrentLayout;
        //    ProSset = doc.Editor.SelectAll(sfilter);
        ////  List<ObjectId> idss=  GetEntitiesInModelSpace();
        //    Database db = doc.Database;
        //    if (ProSset.Status == PromptStatus.OK)
        //    {
        //        using (Transaction tran = db.TransactionManager.StartTransaction())
        //        {
        //            SelectionSet sst = ProSset.Value;

        //            ObjectId[] oids = sst.GetObjectIds();
        //            model.LegendList = new Dictionary<int, List<System.Drawing.PointF>>();
        //            int ad = 0;
        //            List<string> aa = new List<string>();
        //            LayerModel lm = new LayerModel();
        //            LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
        //            foreach (ObjectId layerId in lt)
        //            {
        //                LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
        //                if (ltr.Name == MethodCommand.LegendLayer)
        //                {
        //                    lm.Color =  System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
        //                }
        //            }
        //            for (int i = 0; i < oids.Length; i++)
        //            {
        //              //  if (idss.Contains(oids[i]))
        //              //  {
        //                    DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);
        //                if (!aa.Contains((ob as Polyline).BlockName)) { aa.Add((ob as Polyline).BlockName); }
        //                    if (ob is Polyline&&(ob as Polyline).BlockName.ToLower()== "*model_space")
        //                    {
        //                        lengedList.Add(ob as Polyline);
        //                        model.LegendList.Add(ad,PolylineMethod.GetPolyLineInfoPt(ob as Polyline));
        //                        ad++;
        //                    }
        //               // }
    
                   
        //            }
        //            model.allLines = new List<LayerModel>();
                  
        //            model.allLines.Add(lm);
        //        }
        //    }

            
        //}

        public List<Autodesk.AutoCAD.DatabaseServices.ObjectId> GetEntitiesInModelSpace()
        {
            List<Autodesk.AutoCAD.DatabaseServices.ObjectId> objects = new List<Autodesk.AutoCAD.DatabaseServices.ObjectId>();
            using (Autodesk.AutoCAD.DatabaseServices.Transaction transaction = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
            {
                Autodesk.AutoCAD.DatabaseServices.BlockTable blockTable =
                    (Autodesk.AutoCAD.DatabaseServices.BlockTable)transaction.GetObject(
                    Autodesk.AutoCAD.DatabaseServices.HostApplicationServices.WorkingDatabase.BlockTableId,
                    Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead);
                Autodesk.AutoCAD.DatabaseServices.BlockTableRecord blockTableRecord =
                    (Autodesk.AutoCAD.DatabaseServices.BlockTableRecord)transaction.GetObject(
                    blockTable[Autodesk.AutoCAD.DatabaseServices.BlockTableRecord.ModelSpace],
                    Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead);
                foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId objId in blockTableRecord)
                {
                    objects.Add(objId);
                }
                transaction.Commit();
            }
            return objects;
        }
        /// <summary>
        /// 获取图例文本
        /// </summary>
        /// <returns></returns>
        private List<MText> GetAllLengedText()
        {
            List<MText> textList = new List<MText>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();
            string LengedName =UI.MainForm.isOnlyModel?"图例名称":"图例";
            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName,LengedName) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
          //  List<ObjectId> idss =allModelId.Count==0? GetEntitiesInModelSpace():allModelId;
            Database db = doc.Database;
            if (ProSset.Status == PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;

                    ObjectId[] oids = sst.GetObjectIds();
                  //  model.LegendPoints = new Dictionary<int, List<System.Drawing.PointF>>();
                    int ad = 0;
                    for (int i = 0; i < oids.Length; i++)
                    {
                        //if (idss.Contains(oids[i]))
                        //{
                            DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);

                            if (ob is MText && (((ob as MText).BlockName.ToLower() == "*model_space" && UI.MainForm.isOnlyModel) || (!UI.MainForm.isOnlyModel)))
                        {
                                textList.Add(ob as MText);
                               // model.LegendPoints.Add(ad, PolylineMethod.GetPolyLineInfo(ob as Polyline));
                                ad++;
                            }
                       // }


                    }

                }
            }
            return textList;
        }

        public void GetAllLengedGemo(T model)
        {
            List<BlockInfoModel> list = new List<BlockInfoModel>();
            List<Polyline> allLengenPolyine = new List<Polyline>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, "图例") };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            //  List<ObjectId> idss=  GetEntitiesInModelSpace();
            Database db = doc.Database;
            List<BlockReference> blockTableRecords = new List<BlockReference>();
            if (ProSset.Status == PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;

                    ObjectId[] oids = sst.GetObjectIds();
                    
                    int ad = 0;
                    List<string> aa = new List<string>();
                    LayerModel lm = new LayerModel();
                    LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                    foreach (ObjectId layerId in lt)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                        if (ltr.Name == MethodCommand.LegendLayer)
                        {
                            lm.Color =  System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                        }
                    }
                    for (int i = 0; i < oids.Length; i++)
                    {
                        //  if (idss.Contains(oids[i]))
                        //  {
                        DBObject ob = tran.GetObject(oids[i], OpenMode.ForRead);
                       // if (!aa.Contains((ob as Polyline).BlockName)) { aa.Add((ob as Polyline).BlockName); }
                        if (ob is Polyline && (((ob as Polyline).BlockName.ToLower() == "*model_space" && UI.MainForm.isOnlyModel) || (!UI.MainForm.isOnlyModel)))
                        {
                            if (ob == null)
                            {

                            }
                            else
                            {
                                allLengenPolyine.Add((ob as Polyline));
                            }//  lengedList.Add(ob as Polyline);
                            //  model.LegendPoints.Add(ad, PolylineMethod.GetPolyLineInfoPt(ob as Polyline));
                            // ad++;
                        } else if (ob is BlockReference && (((ob as BlockReference).BlockName.ToLower() == "*model_space" && UI.MainForm.isOnlyModel)|| (!UI.MainForm.isOnlyModel)))
                        {
                            blockTableRecords.Add(ob as BlockReference);
                        }
                    }
                    Dictionary<int, List<Polyline>> plList = MethodCommand.FindMaxAreaPoline(allLengenPolyine);
                    model.LegendList = new List<LengedModel>();
                    this.lengedList = plList[0];
                    foreach (Polyline polyline in plList[0])
                    {
                        LengedModel legm = new LengedModel();
                        legm.GemoModels = new List<BlockInfoModel>();
                      
                       legm.BoxPointList=PolylineMethod.GetPolyLineInfoPt(polyline);
                       List<ObjectId> ois = GetCrossObjectIds(doc.Editor,polyline,sfilter,tran);
                        
                        if (ois != null)
                        {
                            foreach (ObjectId lengGemo in ois)
                            {
                                DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                                BlockInfoModel plModel = AnalysisBlcokInfo(ob);
                                if (plModel != null)
                                {
                                    if (plModel.Hatch != null)
                                    {
                                        foreach (HatchModel hatchModel in plModel.Hatch)
                                        {
                                            if (polyline.Closed&& hatchModel.Area ==polyline.Area)
                                            {
                                                legm.BackGround =hatchModel.loopPoints.Count>0? hatchModel.loopPoints[0].Color:"";
                                                break;
                                            }
                                        }
                                    }
                                    List<object> obj = new List<object>() { plModel };
                                    legm.GemoModels.Add(plModel);
                                   
                                }
                                //if (ob is Polyline && (ob as Polyline).BlockName.ToLower() == "*model_space")
                                //{
                                //    BlockInfoModel plModel = new BlockInfoModel();
                                //    plModel.PolyLine = AutoCad2ModelTools.Polyline2Model(ob as Polyline);
                                //    legm.GemoModels.Add(plModel);
                                //}
                                //else if (ob is BlockReference && (ob as BlockReference).BlockName.ToLower() == "*model_space")
                                //{
                                //    legm.GemoModels.Add(BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference));
                                //}
                                //else if (ob is DBText && (ob as DBText).BlockName.ToLower() == "*model_space")
                                //{

                                //    BlockInfoModel plModel = new BlockInfoModel();
                                //    plModel.DbText = AutoCad2ModelTools.DbText2Model(ob as DBText);
                                //    legm.GemoModels.Add(plModel);
                                //}
                                //else if (ob is MText && (ob as MText).BlockName.ToLower() == "*model_space")
                                //{

                                //    BlockInfoModel plModel = new BlockInfoModel();
                                //    plModel.DbText = AutoCad2ModelTools.DbText2Model(ob as MText);
                                //    legm.GemoModels.Add(plModel);
                                //}
                                //else if (ob is Hatch && (ob as Hatch).BlockName.ToLower() == "*model_space")
                                //{
                                //    BlockInfoModel plModel = new BlockInfoModel();
                                //    plModel.Hatch = AutoCad2ModelTools.Hatch2Model(ob as Hatch);
                                //    legm.GemoModels.Add(plModel);
                                //}
                                //else if (ob is Circle && (ob as Circle).BlockName.ToLower() == "*model_space")
                                //{
                                //    BlockInfoModel plModel = new BlockInfoModel();
                                //    plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle));
                                //    legm.GemoModels.Add(plModel);
                                //}
                                //else if (ob is Entity)
                                //{
                                    
                                //}
                            }
                        }
                        model.LegendList.Add(legm);
                       
                    }


                }
            }
            
        }

        public LayerModel GetAllLayerGemo(T model,string layerName)
        {
            LayerModel lyModel = new LayerModel();
            List<BlockInfoModel> list = new List<BlockInfoModel>();
            lyModel.Name = layerName;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            ObjectIdCollection ids = new ObjectIdCollection();

            PromptSelectionResult ProSset = null;
            TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName) };
            SelectionFilter sfilter = new SelectionFilter(filList);
            LayoutManager layoutMgr = LayoutManager.Current;

            string ss = layoutMgr.CurrentLayout;
            ProSset = doc.Editor.SelectAll(sfilter);
            //  List<ObjectId> idss=  GetEntitiesInModelSpace();
            Database db = doc.Database;
            List<BlockReference> blockTableRecords = new List<BlockReference>();
            if (ProSset.Status == PromptStatus.OK)
            {
                lyModel.pointFs = new Dictionary<int, List<object>>();
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    SelectionSet sst = ProSset.Value;

                    ObjectId[] oids = sst.GetObjectIds();

                    int ad = 0;
                    List<string> aa = new List<string>();
                   
                    LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
                    foreach (ObjectId layerId in lt)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)tran.GetObject(layerId, OpenMode.ForRead);
                        if (ltr.Name == layerName)
                        {
                            lyModel.Color = System.Drawing.ColorTranslator.ToHtml(ltr.Color.ColorValue);
                        }
                    }

                    int i = 0;
                    foreach (ObjectId lengGemo in oids)
                    {

                        DBObject ob = tran.GetObject(lengGemo, OpenMode.ForRead);
                        BlockInfoModel plModel = AnalysisBlcokInfo(ob);
                        if (plModel != null)
                        {
                            List<object> obj = new List<object>() { plModel };
                            lyModel.pointFs.Add(i, obj);
                            i++;
                        }
                    
                        }
                    }


                


            }
            
            return lyModel;
        }
        private void AnalysisBlcokInfo(BlockInfoModel plModel, DBObject ob)
        {



            if (ob is Polyline)
            {

                plModel.PolyLine.Add(AutoCad2ModelTools.Polyline2Model(ob as Polyline));

            }
            else if (ob is BlockReference)
            {
                plModel = BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference);
            }
            else if (ob is DBText)
            {
                plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as DBText));
            }
            else if (ob is MText)
            {
                plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as MText));
            }
            else if (ob is Hatch)
            {
                plModel.Hatch.Add(AutoCad2ModelTools.Hatch2Model(ob as Hatch));

            }
            else if (ob is Circle)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle));

            }
            else if (ob is Ellipse)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Ellipse2Model(ob as Ellipse));

            }
            else if (ob is Line)
            {

                plModel.Line.Add(AutoCad2ModelTools.Line2Model(ob as Line));

            }
            else if (ob is Arc)
            {

                plModel.Circle.Add(AutoCad2ModelTools.Arc2Model(ob as Arc));

            }
            else if (ob is Polyline2d)
            {
                plModel.Circle.Add(AutoCad2ModelTools.Polyline2DModel(ob as Polyline2d));
            }
            else if (ob is Entity)
                {
                    Entity ety = ob as Entity;
                    DBObjectCollection objs = new DBObjectCollection();
                    ety.Explode(objs);
                    foreach (DBObject obj in objs)
                    {
                        AnalysisBlcokInfo(plModel, obj);
                    }
                }
               
            
          
        }
        private BlockInfoModel AnalysisBlcokInfo(DBObject ob)
        {
            

            if (ob is Entity && (((ob as Entity).BlockName.ToLower() == "*model_space" && UI.MainForm.isOnlyModel) || (!UI.MainForm.isOnlyModel)))
            {
                BlockInfoModel plModel = new BlockInfoModel();
                try
                {
                    if (ob is Polyline)
                    {

                        plModel.PolyLine.Add(AutoCad2ModelTools.Polyline2Model(ob as Polyline));

                    }
                   else if (ob is Arc)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Arc2Model(ob as Arc));

                    }
                    else if (ob is BlockReference)
                    {
                        plModel = BlockCommand.AnalysisEntryAndExitbr(ob as BlockReference);
                    }
                    else if (ob is DBText)
                    {
                        plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as DBText));
                    }
                    else if (ob is MText)
                    {
                        plModel.DbText.Add(AutoCad2ModelTools.DbText2Model(ob as MText));
                    }
                    else if (ob is Hatch)
                    {
                        plModel.Hatch.Add(AutoCad2ModelTools.Hatch2Model(ob as Hatch));

                    }
                    else if (ob is Circle)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Circle2Model(ob as Circle));

                    }
                    else if (ob is Ellipse)
                    {

                        plModel.Circle.Add(AutoCad2ModelTools.Ellipse2Model(ob as Ellipse));

                    }
                    else if (ob is Line)
                    {

                        plModel.Line.Add(AutoCad2ModelTools.Line2Model(ob as Line));

                    }
                    else if (ob is Polyline2d)
                    {
                        plModel.Circle.Add(AutoCad2ModelTools.Polyline2DModel(ob as Polyline2d));
                    }
                    else if (ob is Entity)
                    {
                        Entity ety = ob as Entity;
                        DBObjectCollection objs = new DBObjectCollection();
                        ety.Explode(objs);
                        foreach (DBObject obj in objs)
                        {
                            AnalysisBlcokInfo(plModel, obj);
                        }
                    }
                   
                }
                catch
                {

                }
                return plModel;
            }
            return null;
        }

        public List<ObjectId> GetCrossObjectIds(Editor ed,Polyline pl,SelectionFilter sf,Transaction tran)
        {
            Point3dCollection point3DCollection = new Point3dCollection();
            ObjectId[] list=null; List<ObjectId> ooids = new List<ObjectId>();
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                point3DCollection.Add(pl.GetPoint3dAt(i));
            }
          PromptSelectionResult psr=  ed.SelectCrossingPolygon(point3DCollection, sf);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet sst = psr.Value;
               ObjectId[] oids=  sst.GetObjectIds();
              
                foreach (ObjectId item in oids)
                {
                    if (item.OldId!=pl.ObjectId.OldId)
                    {
                        ooids.Add(item);
                    }
                }
                
            }
             
            return ooids;
        }

        public void GetExportLayers(T model)
        {
            if (lengedList.Count > 0)
            {
                model.LayerList = new List<string>();
               List<MText> txtList= GetAllLengedText();

                foreach (MText dBText in txtList)
                {
                    if (MethodCommand.FindDBTextIsInPolyine(dBText,model.LegendList))
                    {
                        model.LayerList.Add(dBText.Text);
                    }
                }
            }
        }

        }
    
}
