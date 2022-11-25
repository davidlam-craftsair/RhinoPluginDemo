using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PolylineCreation
    {
        public IEnumerable<Point3d> Point3ds { get; set; }

        public Polyline Get()
        {
            return new Polyline(Point3ds);
        }
    }
}
