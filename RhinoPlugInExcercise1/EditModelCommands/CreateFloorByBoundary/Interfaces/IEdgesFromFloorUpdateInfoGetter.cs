using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IEdgesFromFloorUpdateInfoGetter
    {
        bool Get(IFloorUpdateInfo floorUpdateInfo, out IEnumerable<Curve> edges);
    }
}