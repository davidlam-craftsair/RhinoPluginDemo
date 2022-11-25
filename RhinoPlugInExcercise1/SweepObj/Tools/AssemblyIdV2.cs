using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class AssemblyIdV2: IAssemblyIdV2
    {
        public AssemblyIdV2(Guid assemblyId, int groupId, string assemblyTypeName, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            AssemblyId = assemblyId;
            GroupId = groupId;
            AssemblyTypeName = assemblyTypeName;
            ComponentIdsByName = componentIdsByName;
            
        }

        /// <summary>
        /// Id, a guid generated
        /// </summary>
        public Guid Id => AssemblyId;
        public Guid AssemblyId { get; }
        public int GroupId { get; }
        public string AssemblyTypeName { get; }
        public IDictionary<string, IEnumerable<Guid>> ComponentIdsByName { get; }
        public IEnumerable<Guid> ComponentIds => ComponentIdsByName.Values.SelectMany(x => x);

    }
}
