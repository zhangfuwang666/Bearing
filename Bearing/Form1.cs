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
            if (atextBox1.Text.Length>0&& dtextBox2.Text.Length > 0&& btextBox4.Text.Length > 0
                && daDtextBox3.Text.Length > 0 && rtextBox5.Text.Length > 0)
            {
                DrawDwg(Convert.ToDouble(atextBox1.Text), Convert.ToDouble(btextBox4.Text), Convert.ToDouble(daDtextBox3.Text), Convert.ToDouble(rtextBox5.Text));
            }
            else
            {
                Application.ShowAlertDialog("请输入有效参数");
            }
        }
        private static void DrawDwg(double a,double b,double daD,double r)
        {
            string sLocalRoot = Application.GetSystemVariable("LOCALROOTPREFIX") as string;
            string sTemplatePath = sLocalRoot + "Template\\acad.dwt";
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Add(sTemplatePath);
            Editor ed = doc.Editor;
            using (DocumentLock docLock = doc.LockDocument())
            {
                using (var trans = doc.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = trans.GetObject(doc.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord space = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    //外框
                    Polyline waiKuang = new Polyline();
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices,new Point2d(-b/2, daD / 2-r),0,0,0);//左上角边框 ，顺时针
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices,new Point2d(-b/2+r,daD/2),0,0,0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices,new Point2d(b/2-r,daD/2),0,0,0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices,new Point2d(b/2,daD/2-r),0,0,0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices,new Point2d(b/2,-(daD/2-r)),0,0,0);//右下
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(b / 2 - r, -(daD / 2)), 0, 0, 0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(-(b / 2 - r), -(daD / 2)), 0, 0, 0);
                    waiKuang.AddVertexAt(waiKuang.NumberOfVertices, new Point2d(-(b / 2), -(daD / 2-r)), 0, 0, 0);
                    waiKuang.Closed = true;

                    //中心线
                    Line centerLine = new Line(new Point3d(-b/2-2,0,0), new Point3d(b / 2 + 2, 0, 0));
                    //球
                    Circle Qiu = new Circle(new Point3d(0, (daD / 2 - a / 2),0),Vector3d.ZAxis,a/4);
                    //球中心线
                    Line centerLine1 = new Line(new Point3d(-b / 2 - 2, (daD / 2 - a / 2), 0), new Point3d(b / 2 + 2, (daD / 2 - a / 2), 0));
                    //竖着
                    Line centerLine2 = new Line(new Point3d(0, (daD/2-a/2)+(a/2+2), 0), new Point3d(0, (daD / 2 - a / 2) - (a / 2 + 2), 0));

                    //下面横着
                    Line centerLine3 = new Line(new Point3d(-b / 2 + 2, (daD / 2 - a / 2), 0), new Point3d(b / 2 - 2, (daD / 2 - a / 2), 0));
                    //下面竖着
                    Line centerLine4 = new Line(new Point3d(0, -((daD / 2 - a / 2) + (a / 2 - 2)), 0), new Point3d(0, -((daD / 2 - a / 2) - (a / 2 - 2)), 0));

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

                    space.AppendEntity(Qiu);
                    trans.AddNewlyCreatedDBObject(Qiu, true);

                    space.AppendEntity(waiKuang);
                    trans.AddNewlyCreatedDBObject(waiKuang,true);

                    trans.Commit();
                }

            }
        }
    }
}
