using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IAssemblyIdFactory
    {
        IAssemblyId Create(IEnumerable<Guid> componentIds, int groupIdx);
        IAssemblyIdV2 Create(Guid assemblyId, int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName);

    }

}