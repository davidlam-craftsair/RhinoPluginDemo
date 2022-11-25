using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRailingAssemblyIdFactory
    {
        IRailingAssemblyId Create(Guid assemblyId, int groupId, IEnumerable<Guid> railId, IEnumerable<Guid> profileId);
        IRailingAssemblyId Create(int groupId, IEnumerable<Guid> railId, IEnumerable<Guid> profileId);
        IRailingAssemblyId Create(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName);
    }

    public interface IAssemblyIdV2Factory
    {
        IAssemblyIdV2 Create(Guid assemblyId, int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName);
        IAssemblyIdV2 Create(int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName);
        IAssemblyIdV2 CreateGeneric(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName);
        IAssemblyIdV2 CreateSweepGeneric(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName);
        IAssemblyIdV2 CreateRailing(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName);
    }
}