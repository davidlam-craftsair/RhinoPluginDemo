using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IEdgesSorter
    {
        bool Find(IEnumerable<Curve> curves, out Curve outerEdge, out IEnumerable<Curve> remaining);
    }
}