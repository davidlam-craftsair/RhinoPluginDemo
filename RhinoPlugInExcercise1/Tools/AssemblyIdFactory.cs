using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class AssemblyIdFactory: IAssemblyIdFactory
    {
        public IAssemblyId Create(IEnumerable<Guid> componentIds, int groupIdx)
        {
            return new GenericAssemblyId(Guid.NewGuid(), "Generic", groupIdx, componentIds);
        }

        public IAssemblyIdV2 Create(Guid assemblyId, int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return new AssemblyIdV2(assemblyId, groupId, assemblyTypeName, componentIdsByName);
        }
    }
}
