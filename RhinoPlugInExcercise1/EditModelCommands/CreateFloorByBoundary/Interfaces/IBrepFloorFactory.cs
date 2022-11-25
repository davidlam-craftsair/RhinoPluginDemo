using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IBrepFloorFactory
    {
        Brep Create(Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight);
    }
    public interface IBrepFactoryExt
    {
        IEnumerable<Brep> Create(IEnumerable<Curve> edges, double height);
    }
}