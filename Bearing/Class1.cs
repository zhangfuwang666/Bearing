using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

namespace Bearing
{

    public class Class1
    {
        public static Form1 form1 = null;
        [CommandMethod("66666")]
        public static void cmd1()
        {
            if (form1 == null)
            {
                form1 = new Form1();
                Application.ShowModelessDialog(form1);
            }

        }
    }
}
