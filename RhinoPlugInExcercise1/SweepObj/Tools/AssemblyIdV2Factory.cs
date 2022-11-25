using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class AssemblyIdV2Factory: IAssemblyIdV2Factory
    {
        public IAssemblyIdV2 Create(Guid assemblyId, int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return new AssemblyIdV2(assemblyId, groupId, assemblyTypeName, componentIdsByName);
        }

        public IAssemblyIdV2 Create(int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return Create(Guid.NewGuid(), groupId, assemblyTypeName, componentIdsByName);
        }

        public IAssemblyIdV2 CreateGeneric(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return Create(Guid.NewGuid(), groupId, "Generic", componentIdsByName);
        }

        public IAssemblyIdV2 CreateSweepGeneric(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return Create(Guid.NewGuid(), groupId, "Sweep Generic", componentIdsByName);
        }

        public IAssemblyIdV2 CreateRailing(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return Create(Guid.NewGuid(), groupId, "Railing", componentIdsByName);
        }
    }
}
