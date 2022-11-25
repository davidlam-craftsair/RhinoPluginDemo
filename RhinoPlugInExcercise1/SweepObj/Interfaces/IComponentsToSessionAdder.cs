using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IComponentsToSessionAdder
    {
        void AddToSessionDoc(IEnumerable<Guid> componentGeometryIds);
    }

    public interface IRailingComponentsToSessionAdder
    {
        void AddToSessionDoc(IEnumerable<Guid> railId, IEnumerable<Guid> profileId);
    }

    public interface IComponentsToSessionAdderV2
    {
        void AddToSessionDoc(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName);
    }

    public interface IBuildingComponentsToSessionAdder
    {
        void AddToSessionDoc(ICompositeObjId buildingComponentId);
    }
}