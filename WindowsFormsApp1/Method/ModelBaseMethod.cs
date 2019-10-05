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
        public List<MText> GetAllLengedText()
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

                    ObjectId[] oids2 = sst.GetObjectIds();

                    // 排序
                    //List<ObjectId> oids = Sort(oids2);

                    //  model.LegendPoints = new Dictionary<int, List<System.Drawing.PointF>>();
                    int ad = 0;
                    for (int i = 0; i < oids2.Length; i++)
                    {
                        //if (idss.Contains(oids[i]))
                        //{
                            DBObject ob = tran.GetObject(oids2[i], OpenMode.ForRead);

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

        private List<ObjectId> Sort(ObjectId[] oids2)
        {
            ObjectId[] idArray = oids2;
            List<ObjectId> oids3 = new List<ObjectId>();
            using (Database db = HostApplicationServices.WorkingDatabase)
            {
                using (Transaction acTrans = db.TransactionManager.StartTransaction())
                {
                    List<MText> compositionTableIndex = new List<MText>();
                    List<MText> mtexts = new List<MText>();
                    List<Entity> mtexts2 = new List<Entity>();


                    for (int j = 0; j < idArray.Length; j++)
                    {
                        Entity ent2 = (Entity)idArray[j].GetObject(OpenMode.ForRead);
                        if (ent2 is MText)
                        {
                            if (((MText)ent2).Text.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "") != "图例")
                            {

                                MText mt = (MText)ent2;

                                mtexts.Add(mt);

                            }

                        }
                    }

                    double minX = double.NaN;
                    double maxX = double.NaN;
                    double maxY = double.NaN;
                    double minY = double.NaN;

                    ///找出排头兵
                    for (int j = 0; j < mtexts.Count; j++)
                    {
                        if (mtexts[j].Location.X > maxX || double.IsNaN(maxX))
                        {
                            maxX = mtexts[j].Location.X;
                        }
                        if (mtexts[j].Location.Y < minY || double.IsNaN(minY))
                        {
                            minY = mtexts[j].Location.Y;
                        }
                        if (mtexts[j].Location.Y > maxY || double.IsNaN(maxY))
                        {
                            maxY = mtexts[j].Location.Y;
                        }
                        if (mtexts[j].Location.X < minX || double.IsNaN(minX))
                        {
                            minX = mtexts[j].Location.X;
                        }
                    }

                    // 找出索引
                    for (int j = 0; j < mtexts.Count; j++)
                    {
                        if (maxY - 50 < ((MText)mtexts[j]).Location.Y && ((MText)mtexts[j]).Location.Y < maxY + 50)
                        {
                            compositionTableIndex.Add(mtexts[j]);
                        }
                    }

                    // 从近到远排序
                    List<int> distances = new List<int>();

                    for (int h = 0; h < compositionTableIndex.Count; h++)
                    {
                        if (minX - 20 < compositionTableIndex[h].Location.X && compositionTableIndex[h].Location.X < minX + 20)
                        {
                            for (int j = 0; j < compositionTableIndex.Count; j++)
                            {

                                    int distance = (int)MethodCommand.DistancePointToPoint(((MText)compositionTableIndex[h]).Location, ((MText)compositionTableIndex[j]).Location);

                                    distances.Add(distance);
                                    mtexts2.Add(mtexts[j]);
                            }
                        }
                    }

                    // 把获取的属性值按照距离大小排序，距离最近的放在第一位，以此类推
                    Entity temp;
                    int tempDis;
                    for (int m = 0; m < distances.Count; m++)
                    {
                        for (int q = 0; q < distances.Count - m - 1; q++)
                        {
                            try
                            {
                                if (distances[q] > distances[q + 1])
                                {
                                    temp = mtexts2[q];
                                    mtexts2[q] = mtexts2[q + 1];
                                    mtexts2[q + 1] = temp;

                                    tempDis = distances[q];
                                    distances[q] = distances[q + 1];
                                    distances[q + 1] = tempDis;
                                }
                            }
                            catch (Exception e)
                            {
                                System.Windows.Forms.MessageBox.Show(e.Message);
                            }
                        }
                    }

                    // 按索引排序
                    for (int j = 0; j < mtexts2.Count; j++)
                    {
                        Entity ent2 = (Entity)mtexts2[j];
                        for (int z = 0; z < mtexts.Count; z++)
                        {
                            Entity ent3 = (Entity)mtexts[z];

                            if (((MText)ent2).Location.X + 50 > ((MText)ent3).Location.X && ((MText)ent2).Location.X - 50 < ((MText)ent3).Location.X)
                            {
                                ObjectId oid = ent3.ObjectId;
                                oids3.Add(oid);
                            }
                        }
                    }

                    

                }
            }

            return oids3;
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
                                BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob);

                                if (plModel != null)
                                {
                                    if (plModel.Hatch != null)
                                    {
                                        HatchModel rem =new HatchModel();
                                        foreach (HatchModel hatchModel in plModel.Hatch)
                                        {
                                            if (polyline.Closed&& hatchModel.Area.ToString("F2") ==polyline.Area.ToString("F2"))
                                            {
                                                legm.BackGround =hatchModel.loopPoints.Count>0? hatchModel.loopPoints[0].Color:"";
                                                rem = hatchModel;
                                                break;
                                            }
                                        }
                                        plModel.Hatch.Remove(rem);
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

        public void GetExportLayers(T model)
        {
            if (lengedList.Count > 0)
            {
                model.LayerList = new List<string>();
                List<MText> txtList = GetAllLengedText();

                foreach (MText dBText in txtList)
                {
                    if (MethodCommand.FindDBTextIsInPolyine(dBText, model.LegendList))
                    {
                        model.LayerList.Add(dBText.Text);
                    }
                }
            }
        }

        public virtual  LayerModel GetAllLayerGemo(T model,string layerName)
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
                        BlockInfoModel plModel = MethodCommand.AnalysisBlcokInfo(ob);

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

       

        }
    
}
