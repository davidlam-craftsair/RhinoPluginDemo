using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GenericAssemblyId: IAssemblyId
    {
        public GenericAssemblyId(Guid id, string assemblyTypeName, int groupId, IEnumerable<Guid> componentIds)
        {
            Id = id;
            AssemblyTypeName = assemblyTypeName;
            GroupId = groupId;
            ComponentIds = componentIds;
        }

        public Guid Id { get; }
        public string AssemblyTypeName { get; }
        public int GroupId { get; }
        public IEnumerable<Guid> ComponentIds { get; }
    }
}
