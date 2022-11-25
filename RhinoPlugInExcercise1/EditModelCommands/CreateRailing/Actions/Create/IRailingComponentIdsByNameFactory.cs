using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRailingComponentIdsByNameFactory
    {
        IDictionary<string, IEnumerable<GeometryBase>> Create(IEnumerable<Curve> rails, IEnumerable<Curve> profiles);
        IDictionary<string, IEnumerable<Guid>> Create(IEnumerable<Guid> rails, IEnumerable<Guid> profiles);
    }
}