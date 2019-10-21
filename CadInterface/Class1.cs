using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CadInterface.CadService;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: CommandClass(typeof(CadInterface.Class1))]
namespace CadInterface
{
    public class Class1
    {
        public Entity MyEntity { get; set; }
        /// <summary>
        /// 选择实体 操作扩展属性
        /// </summary>
        [CommandMethod("GetData")]
        public void GetData()
        {
            MyEntity = TechnologicalProcess.GetEntity("请选择实体！");
            if (MyEntity != null)
            {
                ResultBuffer tvList = new ResultBuffer() {
                    new TypedValue((int)DxfCode.Text, "写个1进去！"),
                    new TypedValue((int)DxfCode.Text, "写个2进去！"),
                    new TypedValue((int)DxfCode.Text, "写个3进去！"),
                };
                ExtendedDataHelper.ModObjXrecord(MyEntity.ObjectId, "Test", tvList);//写扩展属性

                ResultBuffer resultBuffer = ExtendedDataHelper.GetObjXrecord(MyEntity.ObjectId, "Test");//读扩展属性
                if (resultBuffer != null)
                {
                    var array = resultBuffer.AsArray();
                    if (array != null)
                        foreach (var content in array)
                            System.Windows.Forms.MessageBox.Show(content.Value.ToString());
                }

                bool isSuccess = ExtendedDataHelper.DelObjXrecord(MyEntity.ObjectId, "Test");//删除扩展属性
                if (isSuccess)
                    System.Windows.Forms.MessageBox.Show("删除成功！");
            }
        }
        /// <summary>
        /// 定位
        /// </summary>
        [CommandMethod("Select")]
        public void Select()
        {
            if (MyEntity != null)
                LocationService.FindPolyline(MyEntity.Handle.ToString());
            else
                System.Windows.Forms.MessageBox.Show("实体为空！");
        }
        /// <summary>
        /// 多实体定位
        /// </summary>
        [CommandMethod("SelectAll")]
        public void SelectAll()
        {
            List<string> list = new List<string>();
            while (true)
            {
                Entity entity = TechnologicalProcess.GetEntity("请选择实体！");
                if (entity != null)
                    list.Add(entity.Handle.ToString());
                else
                    break;
            }
            if (list.Count > 0)
                LocationService.FindPolyline(list);
        }
    }
}
