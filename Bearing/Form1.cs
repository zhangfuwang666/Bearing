using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Bearing
{
    public partial class Form1 : Form
    {
        //通过系统变量获取样板的路径

        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Class1.form1 = null;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(((e.KeyChar >= '0') && (e.KeyChar <= '9')) || e.KeyChar <= 31))
            {
                if (e.KeyChar == '.')
                {
                    if (((TextBox)sender).Text.Trim().IndexOf('.') > -1)
                        e.Handled = true;
                }
                else
                    e.Handled = true;
            }
            else
            {
                if (e.KeyChar <= 31)
                {
                    e.Handled = false;
                }
                else if (((TextBox)sender).Text.Trim().IndexOf('.') > -1)
                {
                    if (((TextBox)sender).Text.Trim().Substring(((TextBox)sender).Text.Trim().IndexOf('.') + 1).Length >= 4)
                        e.Handled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (atextBox1.Text.Length > 0 && dtextBox2.Text.Length > 0 && btextBox4.Text.Length > 0
                && daDtextBox3.Text.Length > 0 && rtextBox5.Text.Length > 0)
            {
                DrawDwg(Convert.ToDouble(atextBox1.Text), Convert.ToDouble(btextBox4.Text), Convert.ToDouble(daDtextBox3.Text), Convert.ToDouble(rtextBox5.Text));
            }
            else
            {
                Application.ShowAlertDialog("请输入有效参数");
            }
        }
        private static void DrawDwg(double a, double b, double daD, double r)
        {


            string sLocalRoot = Application.GetSystemVariable("LOCALROOTPREFIX") as string;
            string sTemplatePath = sLocalRoot + "Template\\acad.dwt";
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Add(sTemplatePath);
            Database db = doc.Database;
            Editor ed = doc.Editor;
            using (DocumentLock docLock = doc.LockDocument())
            {
                using (var trans = doc.TransactionManager.StartTransaction())
                {
                    Tools.AddLayer(db, "Contour_Layer", 7, "Continuous", "外轮廓图层");
                    Tools.AddLayer(db, "Dim_Layer", 7, "Continuous", "外轮廓图层");
                    Tools.AddLayer(db, "CenterLine_Layer", 1, "CENTER", "中心线图层");

                    BlockTable blockTable = trans.GetObject(doc.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord space = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    //外框
                    Polyline waiKuang1 = new Polyline();
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(-b / 2, daD / 2 - r), -Math.PI / 8, 0, 0);//左上角边框 ，顺时针
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(-b / 2 + r, daD / 2), 0, 0, 0);
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(b / 2 - r, daD / 2), -Math.PI / 8, 0, 0);
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(b / 2, daD / 2 - r), 0, 0, 0);
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(b / 2, -(daD / 2 - r)), -Math.PI / 8, 0, 0);//右下
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(b / 2 - r, -(daD / 2)), 0, 0, 0);
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(-(b / 2 - r), -(daD / 2)), -Math.PI / 8, 0, 0);
                    waiKuang1.AddVertexAt(waiKuang1.NumberOfVertices, new Point2d(-(b / 2), -(daD / 2 - r)), 0, 0, 0);
                    waiKuang1.Closed = true;
                    waiKuang1.LineWeight = LineWeight.LineWeight050;
                    waiKuang1.Layer = "Contour_Layer";
                    //中心线
                    Line centerLine = new Line(new Point3d(-b / 2 - 2, 0, 0), new Point3d(b / 2 + 2, 0, 0));
                    centerLine.Layer = "CenterLine_Layer";
                    centerLine.LinetypeScale = 0.1;
                    //球
                    Circle Qiu = new Circle(new Point3d(0, (daD / 2 - a / 2), 0), Vector3d.ZAxis, a / 4);
                    Qiu.LineWeight = LineWeight.LineWeight050;
                    Qiu.Layer = "Contour_Layer";
                    //球中心线
                    Line centerLine1 = new Line(new Point3d(-b / 2 - 2, (daD / 2 - a / 2), 0), new Point3d(b / 2 + 2, (daD / 2 - a / 2), 0));
                    centerLine1.Layer = "CenterLine_Layer";
                    centerLine1.LinetypeScale = 0.1;
                    //竖着
                    Line centerLine2 = new Line(new Point3d(0, (daD / 2 - a / 2) + (a / 2 + 2), 0), new Point3d(0, (daD / 2 - a / 2) - (a / 2 + 2), 0));
                    centerLine2.Layer = "CenterLine_Layer";
                    centerLine2.LinetypeScale = 0.1;
                    //下面十字横着
                    Line centerLine3 = new Line(new Point3d(-b / 2 + 1, -(daD / 2 - a / 2), 0), new Point3d(b / 2 - 1, -(daD / 2 - a / 2), 0));
                    //下面十字竖着
                    centerLine3.Layer = "Contour_Layer";
                    Line centerLine4 = new Line(new Point3d(0, -(daD / 2 - a / 2) + (a / 2 - 1), 0), new Point3d(0, -(daD / 2 - a / 2) - (a / 2 - 1), 0));
                    centerLine4.Layer = "Contour_Layer";

                    //下孔轮廓
                    Polyline kongLunKuoXia = new Polyline();
                    kongLunKuoXia.AddVertexAt(kongLunKuoXia.NumberOfVertices, new Point2d(-b / 2, -(daD - 2 * a) / 2 - r), -Math.PI / 8, 0, 0);
                    kongLunKuoXia.AddVertexAt(kongLunKuoXia.NumberOfVertices, new Point2d(-b / 2 + r, -(daD - 2 * a) / 2), 0, 0, 0);
                    kongLunKuoXia.AddVertexAt(kongLunKuoXia.NumberOfVertices, new Point2d(b / 2 - r, -(daD - 2 * a) / 2), -Math.PI / 8, 0, 0);
                    kongLunKuoXia.AddVertexAt(kongLunKuoXia.NumberOfVertices, new Point2d(b / 2, -(daD - 2 * a) / 2 - r), 0, 0, 0);
                    kongLunKuoXia.LineWeight = LineWeight.LineWeight050;
                    Polyline kongLunKuoShang = (Polyline)kongLunKuoXia.GetTransformedCopy(Matrix3d.Mirroring(new Plane(Point3d.Origin, Vector3d.YAxis)));
                    kongLunKuoShang.LineWeight = LineWeight.LineWeight050;
                    //圆珠截取点

                    double x = a / 4 * Math.Cos(Math.PI / 6);
                    double y = a / 4 * Math.Sin(Math.PI / 6);
                    Line line1 = new Line(new Point3d(x, daD / 2 - a / 2 + y, 0), new Point3d(b / 2, daD / 2 - a / 2 + y, 0));
                    Line line2 = new Line(new Point3d(x, daD / 2 - a / 2 - y, 0), new Point3d(b / 2, daD / 2 - a / 2 - y, 0));

                    Line line3 = new Line(new Point3d(-x, daD / 2 - a / 2 + y, 0), new Point3d(-b / 2, daD / 2 - a / 2 + y, 0));
                    Line line4 = new Line(new Point3d(-x, daD / 2 - a / 2 - y, 0), new Point3d(-b / 2, daD / 2 - a / 2 - y, 0));
                    line1.LineWeight = LineWeight.LineWeight050;
                    line2.LineWeight = LineWeight.LineWeight050;
                    line3.LineWeight = LineWeight.LineWeight050;
                    line4.LineWeight = LineWeight.LineWeight050;

                    line1.Layer = "Contour_Layer";
                    line2.Layer = "Contour_Layer";
                    line3.Layer = "Contour_Layer";
                    line4.Layer = "Contour_Layer";

                    



                    //面域
                    Polyline waiKuang = new Polyline();
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(-b / 2, daD / 2 - r), -Math.PI / 8, 0, 0);//左上角边框 ，顺时针
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(-b / 2 + r, daD / 2), 0, 0, 0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(b / 2 - r, daD / 2), -Math.PI / 8, 0, 0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(b / 2, daD / 2 - r), 0, 0, 0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(b / 2, daD / 2 - a / 2 + y), 0, 0, 0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(-b / 2, daD / 2 - a / 2 + y), 0, 0, 0);
                    waiKuang.Closed = true;

                    Polyline waiKuang2 = new Polyline();
                    waiKuang2.AddVertexAt(waiKuang2.NumberOfVertices, new Point2d(-b / 2, daD / 2 - a / 2 - y), 0, 0, 0);
                    waiKuang2.AddVertexAt(waiKuang2.NumberOfVertices, new Point2d(b / 2, daD / 2 - a / 2 - y), 0, 0, 0);
                    waiKuang2.AddVertexAt(waiKuang2.NumberOfVertices, new Point2d(b / 2, -(daD / 2 - r) + (daD - a)), -Math.PI / 8, 0, 0);//右下
                    waiKuang2.AddVertexAt(waiKuang2.NumberOfVertices, new Point2d(b / 2 - r, -(daD / 2) + (daD - a)), 0, 0, 0);
                    waiKuang2.AddVertexAt(waiKuang2.NumberOfVertices, new Point2d(-(b / 2 - r), -(daD / 2) + (daD - a)), -Math.PI / 8, 0, 0);
                    waiKuang2.AddVertexAt(waiKuang2.NumberOfVertices, new Point2d(-(b / 2), -(daD / 2 - r) + (daD - a)), 0, 0, 0);
                    waiKuang2.Closed = true;


                    var regin1 = Tools.CreaterRegion(waiKuang)[0];
                    var regin2 = Tools.CreaterRegion(Qiu)[0];
                    var regin3 = Tools.CreaterRegion(waiKuang2)[0];
                    var regin4 = Tools.CreaterRegion(Qiu)[0];

                    regin1.BooleanOperation(BooleanOperationType.BoolSubtract, regin2);
                    regin3.BooleanOperation(BooleanOperationType.BoolSubtract, regin4);

                    var id8 = space.AppendEntity(regin1);
                    trans.AddNewlyCreatedDBObject(regin1, true);
                    var id9 = space.AppendEntity(regin3);
                    trans.AddNewlyCreatedDBObject(regin3, true);
                    //填充
                    Hatch hatch = new Hatch();

                    space.AppendEntity(hatch);
                    trans.AddNewlyCreatedDBObject(hatch, true);

                    hatch.SetDatabaseDefaults();
                    hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");//ANSI31为金属剖面线

                    hatch.Associative = true;
                    ObjectIdCollection acObjIdColl = new ObjectIdCollection();
                    acObjIdColl.Add(id8);

                    hatch.AppendLoop(HatchLoopTypes.Outermost, acObjIdColl);

                    hatch.EvaluateHatch(true);

                    //填充
                    Hatch hatch2 = new Hatch();

                    space.AppendEntity(hatch2);
                    trans.AddNewlyCreatedDBObject(hatch2, true);

                    hatch2.SetDatabaseDefaults();
                    hatch2.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");//ANSI31为金属剖面线

                    hatch2.Associative = true;
                    ObjectIdCollection acObjIdColl2 = new ObjectIdCollection();
                    acObjIdColl2.Add(id9);

                    hatch2.AppendLoop(HatchLoopTypes.Outermost, acObjIdColl2);

                    hatch2.EvaluateHatch(true);


                    //标注
                    addHorizonRotatedDimension(db, new Point3d(-b / 2, daD / 2, 0), new Point3d(b / 2, daD / 2, 0), 4);
                    addHorizonRotatedDimension(db, new Point3d(0, daD / 2, 0), new Point3d(b / 2, daD / 2, 0), 2);

                    addVerticalRotatedDimension(db, new Point3d(-b / 2, -(daD - 2 * a) / 2, 0), new Point3d(-b / 2, (daD - 2 * a) / 2, 0), -1);
                    addVerticalRotatedDimension(db, new Point3d(-b / 2, daD / 2, 0), new Point3d(-b / 2, (daD - 2 * a) / 2, 0), -1);
                    addVerticalRotatedDimension(db, new Point3d(-b / 2, daD / 2 - a / 2 - y, 0), new Point3d(-b / 2, daD / 2 - a / 2 + y, 0), -0.5);


                    addVerticalRotatedDimension(db, new Point3d(b / 2, daD / 2, 0), new Point3d(b / 2, daD / 2 - a / 2, 0), 0.5);
                    addVerticalRotatedDimension(db, new Point3d(b / 2, daD / 2, 0), new Point3d(b / 2, -daD / 2, 0), 1);

                    addDiametricDimension(db, new Point3d(0, daD / 2 - a / 2, 0), new Point3d(0, daD / 2 - a / 2, 0), new Point3d(0, daD / 2 - a, 0), new Point3d(-x, daD / 2 - a / 2 - y, 0), 5);

                    var id7 = space.AppendEntity(line1);
                    trans.AddNewlyCreatedDBObject(line1, true);
                    var id6 = space.AppendEntity(line2);
                    trans.AddNewlyCreatedDBObject(line2, true);
                    var id5 = space.AppendEntity(line3);
                    trans.AddNewlyCreatedDBObject(line3, true);
                    var id4 = space.AppendEntity(line4);
                    trans.AddNewlyCreatedDBObject(line4, true);

                    kongLunKuoXia.Layer = "Contour_Layer";
                    kongLunKuoShang.Layer = "Contour_Layer";

                    var id3 = space.AppendEntity(kongLunKuoShang);
                    trans.AddNewlyCreatedDBObject(kongLunKuoShang, true);

                    space.AppendEntity(kongLunKuoXia);
                    trans.AddNewlyCreatedDBObject(kongLunKuoXia, true);

                    space.AppendEntity(centerLine3);
                    trans.AddNewlyCreatedDBObject(centerLine3, true);

                    space.AppendEntity(centerLine4);
                    trans.AddNewlyCreatedDBObject(centerLine4, true);

                    space.AppendEntity(centerLine1);
                    trans.AddNewlyCreatedDBObject(centerLine1, true);

                    space.AppendEntity(centerLine2);
                    trans.AddNewlyCreatedDBObject(centerLine2, true);

                    space.AppendEntity(centerLine);
                    trans.AddNewlyCreatedDBObject(centerLine, true);

                    var id2 = space.AppendEntity(Qiu);
                    trans.AddNewlyCreatedDBObject(Qiu, true);

                    var id1 = space.AppendEntity(waiKuang1);
                    trans.AddNewlyCreatedDBObject(waiKuang1, true);


                    trans.Commit();
                }

            }
        }
        public static void addHorizonRotatedDimension(Database db, Point3d pt1, Point3d pt2, double distance)
        {
            using (Transaction trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTable blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord space = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                RotatedDimension dimRotated = new RotatedDimension();
                dimRotated.XLine1Point = pt1;
                dimRotated.XLine2Point = pt2;
               // dimRotated.DimLinePoint = new Point3d((pt1.X + pt2.X), (pt1.Y + pt2.Y), (pt1.Z + pt2.Z)).TransformBy(Matrix3d.Displacement(Vector3d.YAxis * distance)); ;
                //dimRotated.DimensionText = text;//<>代表標注的主尺寸，此處在標注線上插入文字
                dimRotated.DimensionStyle = db.Dimstyle;
                dimRotated.Layer = "Dim_Layer";
                dimRotated.TextPosition= new Point3d((pt1.X + pt2.X)/2, (pt1.Y + pt2.Y)/2, (pt1.Z + pt2.Z)/2).TransformBy(Matrix3d.Displacement(Vector3d.YAxis * distance)); ;
                space.AppendEntity(dimRotated);
                trans.AddNewlyCreatedDBObject(dimRotated, true);
                trans.Commit();
            }
        }
        public static void addVerticalRotatedDimension(Database db, Point3d pt1, Point3d pt2, double distance)
        {
            using (Transaction trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTable blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord space = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                RotatedDimension dimRotated = new RotatedDimension();

                dimRotated.Rotation = Math.PI / 2;
                dimRotated.XLine1Point = pt1;
                dimRotated.XLine2Point = pt2;

                dimRotated.DimLinePoint = new Point3d((pt1.X + pt2.X)/2, (pt1.Y + pt2.Y)/2, (pt1.Z + pt2.Z)/2).TransformBy(Matrix3d.Displacement(Vector3d.XAxis * distance));
                dimRotated.Layer = "Dim_Layer";
                dimRotated.DimensionStyle = db.Dimstyle;
                space.AppendEntity(dimRotated);
                trans.AddNewlyCreatedDBObject(dimRotated, true);
                trans.Commit();
            }
        }
        public static void addDiametricDimension(Database db, Point3d start1, Point3d start2, Point3d end1, Point3d end2, double distance)
        {
            using (Transaction trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTable blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord space = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                LineAngularDimension2 dimLineAngular = new LineAngularDimension2();
                //圓或圓弧的圓心、或兩個尺寸界線間的共有頂點的座標
                dimLineAngular.XLine1Start = start1;
                dimLineAngular.XLine2Start = start2;
                dimLineAngular.XLine1End = end1;
                dimLineAngular.XLine2End = end2;
                dimLineAngular.TextPosition = new Point3d((end1.X + end2.X) / 2, (end1.Y + end2.Y) / 2, (end1.Z + end2.Z) / 2);

                dimLineAngular.DimensionStyle = db.Dimstyle;
                dimLineAngular.Layer= "Dim_Layer";
                space.AppendEntity(dimLineAngular);
                trans.AddNewlyCreatedDBObject(dimLineAngular, true);
                trans.Commit();
            }
        }
    }
}
