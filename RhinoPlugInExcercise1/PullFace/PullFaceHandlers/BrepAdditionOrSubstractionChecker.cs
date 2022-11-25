using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepAdditionOrSubstractionChecker : IBrepAdditionOrSubstractionChecker
    {
        public int Check(Brep originalBrep, Brep dynamicExtrusion)
        {
            var t1 = GetBoundingBox(originalBrep).Contains(GetBoundingBox(dynamicExtrusion), true);
            if (t1)
            {
                return -1; // originalBrep should be subtracted by dynamicExtrusion
            }
            return 1; // both do not contain each other
        }

        private static BoundingBox GetBoundingBox(GeometryBase geometryBase)
        {
            return geometryBase.GetBoundingBox(true);
        }

    }
}
