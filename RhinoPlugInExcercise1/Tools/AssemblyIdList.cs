using DL.Framework;
using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class AssemblyIdList : IAssemblyIdList
    {
        public AssemblyIdList()
        {
            AssemblyIdsByComponentId = new Dictionary<Guid, IAssemblyIdV2>();
            AssemblyIds = new HashSet<IAssemblyIdV2>();
        }

        public IDictionary<Guid, IAssemblyIdV2> AssemblyIdsByComponentId { get; }
        public HashSet<IAssemblyIdV2> AssemblyIds { get; }

        public void Add(IAssemblyIdV2 assemblyId)
        {
            AssemblyIds.Add(assemblyId);
            AddToAssemblyIdsByComponentId(assemblyId);
        }

        /// <summary>
        /// Add to AssemblyIdsByComponentId 
        /// </summary>
        /// <param name="assemblyId"></param>
        private void AddToAssemblyIdsByComponentId(IAssemblyIdV2 assemblyId)
        {
            foreach (var componentId in assemblyId.ComponentIds)
            {
                AssemblyIdsByComponentId.Add(componentId, assemblyId);
            }
        }

        public int Count()
        {
            return AssemblyIds.Count;
        }

        public IAssemblyIdV2 GetByComponentId(Guid componentId)
        {
            return AssemblyIdsByComponentId.GetOrDefault(componentId);
        }
    }
}
