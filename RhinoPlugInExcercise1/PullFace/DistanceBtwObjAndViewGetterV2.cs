using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class DistanceBtwObjAndViewGetterV2 : IDistanceBtwObjAndViewGetter
    {
        public double Get(RhinoObject x, Line viewLine)
        {
            var viewLineStartPt = viewLine.To;
            if (x.Geometry is Brep brep)
            {
                return Distance(brep.ClosestPoint(viewLineStartPt), viewLineStartPt);
            }
            else if (x.Geometry is Surface surface)
            {
                var brep2 = surface.ToBrep();
                return Distance(brep2.ClosestPoint(viewLineStartPt), viewLineStartPt);

            }
            else
            {
                return Distance(x.Geometry.GetBoundingBox(true).ClosestPoint(viewLineStartPt), viewLineStartPt);

            }
        }

        private static double Distance(Point3d pt1, Point3d pt2)
        {
            return pt1.DistanceToSquared(pt2);
        }
    }
}
    