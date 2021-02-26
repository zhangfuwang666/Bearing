using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bearing
{
    class Tools
    {
        public static void LoadLayer(string layerName)//加载图层
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                var db = doc.Database;
                var ed = doc.Editor;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    switch (layerName)
                    {
                        case "!000#Drawing_Layer": AddLayer(db, "!000#Drawing_Layer", 7, "Continuous", "图框图层"); break;
                        case "!001#Contour_Layer": AddLayer(db, "!001#Contour_Layer", 7, "Continuous", "外轮廓图层"); break;
                        case "!002#CenterLine_Layer": AddLayer(db, "!002#CenterLine_Layer", 1, "CENTER", "中心线图层"); break;
                        case "!003#DotLine_Layer": AddLayer(db, "!003#DotLine_Layer", 1, "Dot", "不可见线图层"); break;
                        case "!004#Text_Layer": AddLayer(db, "!004#Text_Layer", 4, "Continuous", "文字图层"); break;
                        case "!005#Dimention_Layer": AddLayer(db, "!005#Dimention_Layer", 4, "Continuous", "尺寸线图层"); break;
                        case "!006#DimentionPower_Layer": AddLayer(db, "!006#DimentionPower_Layer", 4, "Continuous", "动力分段尺寸线图层"); break;
                        case "!007#DimentionBordSide_Layer": AddLayer(db, "!007#DimentionBordSide_Layer", 7, "Continuous", "侧边分段尺寸线图层"); break;
                        case "!008#FirstFloor_Layer": AddLayer(db, "!008#FirstFloor_Layer", 9, "Continuous", "第一层输送图层"); break;
                        case "!009#SecondFloor_Layer": AddLayer(db, "!009#SecondFloor_Layer", 3, "Continuous", "第二层输送图层"); break;
                        case "!010#ThirdFloor_Layer": AddLayer(db, "!010#ThirdFloor_Layer", 4, "Continuous", "第三层输送图层"); break;
                        case "!011#FourthFloor_Layer": AddLayer(db, "!011#FourthFloor_Layer", 5, "Continuous", "第四层输送图层"); break;
                        case "!012#ConveyorLeg_Layer": AddLayer(db, "!012#ConveyorLeg_Layer", 4, "Continuous", "输送支腿图层"); break;
                        case "!013#EquipmentNumber": AddLayer(db, "!013#EquipmentNumber", 7, "Continuous", "设备编号图层"); break;
                        case "!014#GoodsShelf_Layer": AddLayer(db, "!014#GoodsShelf_Layer", 163, "Continuous", "多穿货架图层"); break;
                        case "!015#3DGoodsShelf_LiZhu_Layer": AddLayer(db, "!015#3DGoodsShelf_LiZhu_Layer", 150, "Continuous", "三维多穿货架立柱图层"); break;
                        case "!015#3DGoodsShelf_LiZhuDiJiao_Layer": AddLayer(db, "!015#3DGoodsShelf_LiZhuDiJiao_Layer", 23, "Continuous", "三维多穿货架立柱地脚图层"); break;
                        case "!015#3DGoodsShelf_DaoGui_Layer": AddLayer(db, "!015#3DGoodsShelf_DaoGui_Layer", 110, "Continuous", "三维多穿货架导轨图层"); break;
                        case "!015#3DGoodsShelf_WaiCeLang_Layer": AddLayer(db, "!015#3DGoodsShelf_WaiCeLang_Layer", 124, "Continuous", "三维多穿货架外侧梁图层"); break;
                        case "!015#3DGoodsShelf_WeiXiuCeng_Layer": AddLayer(db, "!015#3DGoodsShelf_WeiXiuCeng_Layer", 115, "Continuous", "三维多穿货架维修层图层"); break;
                        case "!015#3DGoodsShelf_LiZhuLaGan_Layer": AddLayer(db, "!015#3DGoodsShelf_LiZhuLaGan_Layer", 30, "Continuous", "三维多穿货架立柱拉杆图层"); break;
                        case "!015#3DGoodsShelf_HuoGeHengCheng_Layer": AddLayer(db, "!015#3DGoodsShelf_HuoGeHengCheng_Layer", 8, "Continuous", "三维多穿货架货格横撑图层"); break;
                        case "!015#3DGoodsShelf_DingLaLongMenLiang_Layer": AddLayer(db, "!015#3DGoodsShelf_DingLaLongMenLiang_Layer", 163, "Continuous", "三维多穿货架顶拉龙门梁图层"); break;
                        case "!015#3DGoodsShelf_DingLaXieLa_Layer": AddLayer(db, "!015#3DGoodsShelf_DingLaXieLa_Layer", 163, "Continuous", "三维多穿货架顶拉斜拉图层"); break;
                        case "!015#3DGoodsShelf_BeiLaZuo_Layer": AddLayer(db, "!015#3DGoodsShelf_BeiLaZuo_Layer", 150, "Continuous", "三维多穿货架背拉座图层"); break;
                        case "!015#3DGoodsShelf_BeiLaDiJiao_Layer": AddLayer(db, "!015#3DGoodsShelf_BeiLaDiJiao_Layer", 150, "Continuous", "三维多穿货架背拉地脚图层"); break;
                        case "!015#3DGoodsShelf_BeiLaLiang_Layer": AddLayer(db, "!015#3DGoodsShelf_BeiLaLiang_Layer", 150, "Continuous", "三维多穿货架背拉梁图层"); break;
                        case "!015#3DGoodsShelf_HuoGeDingHengLiang_Layer": AddLayer(db, "!015#3DGoodsShelf_HuoGeDingHengLiang_Layer", 3, "Continuous", "三维多穿货架顶横梁图层"); break;

                        default:
                            break;
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
    }
}
