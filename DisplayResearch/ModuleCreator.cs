using DisplayResearch.Def;
using DisplayResearch.DisplayConduit;
using DisplayResearch.MouseClick;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayResearch
{
    internal class ModuleCreator
    {
        public CustomDisplayConduit Displayer { get; set; }
        public CustomMouseCallback MouseCaller { get; set; }
        public List<Module> Modules { get; set; }
        
        public ModuleCreator()
        {
            Init();
        }
        private void DummyCreate()
        {
            Modules = new List<Module>();
            // 4*2*2阵列
            double length = 3;
            double width = 5;
            double height = 2;

            Plane plane = Plane.WorldXY;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    for(int k = 0; k < 2; k++)
                    {
                        Point3d point = new Point3d(i * length, j * width, k * height);
                        Plane curPlane = new Plane(point, Vector3d.ZAxis);
                        Module module = new Module(curPlane);
                        Modules.Add(module);
                    }
                }
            }
        }
        private void Init()
        {
            DummyCreate();
            Displayer = new CustomDisplayConduit(Modules);
            Displayer.Enabled = true;
            MouseCaller = new CustomMouseCallback(Modules);
            MouseCaller.Enabled = true;
        }
    }
}
