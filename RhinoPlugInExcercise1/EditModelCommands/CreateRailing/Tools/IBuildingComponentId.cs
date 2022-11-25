using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IBuildingComponentId
    {
        string BuildingComponentName { get; }
        Guid Id { get; }
        IEnumerable<IBuildingComponentId> ConstituentIds { get; }
        
        bool IsRoot { get; }
        
    }
}