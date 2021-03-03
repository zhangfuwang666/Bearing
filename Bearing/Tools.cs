using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bearing
{
   public static class Tools
    {
        public static void LoadLineType()//加载线性文件
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                var db = doc.Database;
                var ed = doc.Editor;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    var lt = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForRead);
                    try
                    {
                        if (!lt.Has("CENTER"))
                        {
                            db.LoadLineTypeFile("CENTER", "acadiso.lin");
                        }
                        if (!lt.Has("CENTER2"))
                        {
                            db.LoadLineTypeFile("CENTER2", "acadiso.lin");
                        }
                        if (!lt.Has("CENTERX2"))
                        {
                            db.LoadLineTypeFile("CENTERX2", "acadiso.lin");
                        }
                        if (!lt.Has("Continuous"))
                        {
                            db.LoadLineTypeFile("Continuous", "acadiso.lin");
                        }

                        if (!lt.Has("DOT"))
                        {
                            db.LoadLineTypeFile("DOT", "acadiso.lin");
                        }
                        if (!lt.Has("DOT2"))
                        {
                            db.LoadLineTypeFile("DOT2", "acadiso.lin");
                        }
                        if (!lt.Has("DOTX2"))
                        {
                            db.LoadLineTypeFile("DOTX2", "acadiso.lin");
                        }

                        if (!lt.Has("HIDDEN"))
                        {
                            db.LoadLineTypeFile("HIDDEN", "acadiso.lin");
                        }
                        if (!lt.Has("HIDDEN2"))
                        {
                            db.LoadLineTypeFile("HIDDEN2", "acadiso.lin");
                        }
                        if (!lt.Has("HIDDENX2"))
                        {
                            db.LoadLineTypeFile("HIDDENX2", "acadiso.lin");
                        }

                        if (!lt.Has("PHANTOM"))
                        {
                            db.LoadLineTypeFile("PHANTOM", "acadiso.lin");
                        }
                        if (!lt.Has("PHANTOM2"))
                        {
                            db.LoadLineTypeFile("PHANTOM2", "acadiso.lin");
                        }
                        if (!lt.Has("PHANTOMX2"))
                        {
                            db.LoadLineTypeFile("PHANTOMX2", "acadiso.lin");
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                    trans.Commit();
                }
            }
        }
       
        public static ObjectId AddLayer(Database db, string layerName, short LayerColorIndex, string LineTypeName, string Description)//封装添加图层方法
        {
            LayerTable layerTable = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
            if (!layerTable.Has(layerName))
            {
                LayerTableRecord layer = new LayerTableRecord();
                layer.Name = layerName;
                ColorMethod method = ColorMethod.ByLayer;
                layer.Color = Color.FromColorIndex(method, LayerColorIndex);
                var lt = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForRead);
                LoadLineType();
                var id = lt[LineTypeName];
                layer.LinetypeObjectId = id;
                layer.Description = Description;
                layerTable.UpgradeOpen();
                layerTable.Add(layer);
                db.TransactionManager.AddNewlyCreatedDBObject(layer, true);
                layer.DowngradeOpen();
            }
            return layerTable[layerName];
        }
        public static List<Region> CreaterRegion(params Curve[] curves)
        {
            List<Region> regions = new List<Region>();
            DBObjectCollection dBObjectCollection = new DBObjectCollection();
            foreach (var cur in curves)
            {
                if (!cur.IsNewObject&&cur.IsWriteEnabled)
                {
                    return null;

                }
                dBObjectCollection.Add(cur);
            }
            try
            {
                DBObjectCollection regionCloc = Region.CreateFromCurves(dBObjectCollection);
                foreach (Region reg in regionCloc)
                {
                    regions.Add(reg);
                }
                return regions;
            }
            catch (Exception)
            {

                regions.Clear();
                return regions;
            }
        }
        public static ObjectId AddDimStyle(Database db, string DimName)//添加标注样式
        {
            DimStyleTable table = (DimStyleTable)db.DimStyleTableId.GetObject(OpenMode.ForRead);
            if (!table.Has(DimName))
            {
                DimStyleTableRecord record = new DimStyleTableRecord();
                record.Name = DimName;
                record.Dimasz = 0.5;
                record.Dimtxt = 0.5;
                record.Dimtad = 1;
                record.Dimdec = 3;
                record.Dimtad = 1;
                //文字
                record.Dimtih = false;
                record.Dimtoh = false;
                record.Dimtxtdirection = false;
                table.UpgradeOpen();
                table.Add(record);
                db.TransactionManager.AddNewlyCreatedDBObject(record, true);
                table.DowngradeOpen();
            }
            return table[DimName];
        }
    }
}
