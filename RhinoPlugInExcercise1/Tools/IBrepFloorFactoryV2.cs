using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IBrepFloorFactoryV2
    {
        bool CreateFloor(IDocManager doc, IEnumerable<Curve> edges, double floorHeight);
    }
}