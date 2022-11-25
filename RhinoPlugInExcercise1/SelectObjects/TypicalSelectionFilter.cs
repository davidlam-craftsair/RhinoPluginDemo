using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class TypicalSelectionFilter<T> : ISelectionFilter
    {
        public bool Filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            return geometry is T;
        }
    }
}
