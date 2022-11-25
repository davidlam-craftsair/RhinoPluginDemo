using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRhGeometryDoc
    {
        Guid Add(Brep brep);
    }
}