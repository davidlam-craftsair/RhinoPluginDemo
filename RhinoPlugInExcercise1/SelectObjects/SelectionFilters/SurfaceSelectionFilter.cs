using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class SurfaceSelectionFilter: ISelectionFilter
    {
        public bool Filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Surface)
            {
                return true;
            }
            else if (geometry is Brep brep)
            {
                return brep.IsSurface;
            }
            return false;
        }
    }
}
