using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class DistanceBtwObjAndViewGetter : IDistanceBtwObjAndViewGetter
    {
        public double Get(RhinoObject x, Line viewLine)
        {
            var viewLineStartPt = viewLine.To;
            return Distance(x.Geometry.GetBoundingBox(true).ClosestPoint(viewLineStartPt), viewLineStartPt);
        }

        private static double Distance(Point3d pt1, Point3d pt2)
        {
            return pt1.DistanceToSquared(pt2);
        }
    }
}
    