using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailProfileCurveFilter : IRhinoObjectFilter
    {
        public bool Filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Curve)
            {

                return true;
            }
            return false;
        }
    }
}
