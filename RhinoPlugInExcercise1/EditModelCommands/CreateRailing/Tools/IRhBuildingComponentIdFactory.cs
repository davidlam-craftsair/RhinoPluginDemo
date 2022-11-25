using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRhBuildingComponentIdFactory
    {
        IBuildingComponentId Create(Guid id, string buildingComponentName, IEnumerable<IBuildingComponentId> constituentIds);
        IBuildingComponentId CreateSingle(Guid id, string buildingComponentName, Guid rhinoObjectId);
    }
}