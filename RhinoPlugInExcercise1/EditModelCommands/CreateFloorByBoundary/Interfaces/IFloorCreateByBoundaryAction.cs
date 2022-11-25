using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IFloorCreateByBoundaryAction
    {
        Result Create(IEnumerable<Curve> edges, IDocManager doc);
    }

    public interface IFloorCreateByBoundaryActionV2
    {
        Result Create(IEnumerable<ObjRef>objRefs);
    }
}