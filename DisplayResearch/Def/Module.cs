using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayResearch.Def
{
    internal class Module
    {
        /// <summary>
        /// 实体物件
        /// </summary>
        public Brep BrepObj {  get; set; }
        public Plane Position { get; set; }

        public double Length { get; set; }
        public double Width {  get; set; }
        public double Height { get; set; }

        public bool IsPicked {  get; set; }

        public Module(Plane position, double length = 3, double width = 5, double height = 2)
        {
            Position = position;
            Length = length;
            Width = width;
            Height = height;
            BrepObj = CreateBrep();
        }
        private Brep CreateBrep()
        {
            var origin = Position.Origin;
            Point3d point0 = new Point3d(origin);
            Point3d point1 = point0 + new Vector3d(Position.XAxis * Length);
            Point3d point2 = point1 + new Vector3d(Position.YAxis * Width);
            Point3d point3 = point0 + new Vector3d(Position.YAxis * Width);
            Polyline poly = new Polyline(new Point3d[] { point0, point1, point2, point3, point0 });
            Brep obj = Extrusion.CreateExtrusion(poly.ToNurbsCurve(), new Vector3d(0, 0, Height)).ToBrep();
            obj = obj.CapPlanarHoles(0.001);
            return obj;
        }
    }
}
