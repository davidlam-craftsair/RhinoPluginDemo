using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IBrepSweepsFactory
    {
        IEnumerable<Brep> Create(Curve rail, IEnumerable<Curve> shapes);
    }
}