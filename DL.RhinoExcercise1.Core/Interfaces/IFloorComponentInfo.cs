using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DL.RhinoExcercise1.Core
{
    public interface IFloorComponentInfo
    {
        Guid FloorRhinoId { get; }
        IEnumerable<Curve> InnerEdges { get; }
        Curve OuterEdge { get; }

        double Thickness { get; }
    }

    public interface IFloorUpdateInfo
    {
        IFloorComponentInfo FloorComponentInfo { get; }

        IEnumerable<Guid> EdgeIds { get; }

        double Thickness { get; }

        bool NeedToUpdate { get; }

        void ResetNeedToUpdateToFalse();
        void SetNeedToUpdate();

        bool CheckIfContainEdgeId(Guid id); 
    }

}