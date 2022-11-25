using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IComponentsToRhinoGroupAdder
    {
        int AddToGroup(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName);
    }

    public interface IComponentsToRhinoGroupAdderV2
    {
        int AddToGroup(ICompositeObjId buildingComponentId);
    }

}