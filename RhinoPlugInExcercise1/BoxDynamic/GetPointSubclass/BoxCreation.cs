using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BoxCreation : IBoxCreation
    {
        public Plane CPlane1 { get; set; }

        public Point3d BaseCorner1 { get; set; }
        public Point3d BaseCorner2 { get; set; }
        public Point3d TopCorner { get; set; }
        public Plane CPlane2 => new Plane(BaseCorner2, CPlane1.ZAxis, CPlane1.XAxis);
        public Box Get()
        {
            return new Box(CPlane2, new Point3d[] { BaseCorner1, BaseCorner2, TopCorner });
        }
    }
}
