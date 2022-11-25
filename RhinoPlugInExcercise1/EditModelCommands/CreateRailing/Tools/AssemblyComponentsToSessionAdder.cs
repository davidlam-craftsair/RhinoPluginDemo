using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class AssemblyComponentsToSessionAdder : IComponentsToSessionAdderV2
    {
        public AssemblyComponentsToSessionAdder(IDocManager docManager, IAssemblyIdV2Factory assemblyIdFactory)
        {
            DocManager = docManager;
            AssemblyIdFactory = assemblyIdFactory;
        }

        public IDocManager DocManager { get; }
        public IAssemblyIdV2Factory AssemblyIdFactory { get; }

        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;

        /// <summary>
        /// Add a set of rhino geometry objects to form a SweepAssemblyId and add it to SessionDoc
        /// </summary>
        /// <param name="rhinoGeometryIds"></param>
        public void AddToSessionDoc(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            int groupId = AddComponentsToGroup(componentGeometryIdsByName);
            var assemblyId = CreateAssemblyIdV2(groupId,
                                                componentGeometryIdsByName);
            AddAssemblyIdToSessionDoc(assemblyId);
        }

        private int AddComponentsToGroup(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            int railAssemblyCount = GetAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            var groupIdx = GetGroupIdx(GetRhinoGeometryIds(componentGeometryIdsByName), railAssemblyCount);
            return groupIdx;
        }

        private IEnumerable<Guid> GetRhinoGeometryIds(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            return componentGeometryIdsByName.Values.SelectMany(x => x).ToList();
        }

        private int GetGroupIdx(IEnumerable<Guid> rhinoGeometryIds, int railAssemblyCount)
        {
            return RhinoDoc.AddGroup($"Rail {railAssemblyCount}", rhinoGeometryIds);
        }

        private void AddAssemblyIdToSessionDoc(IAssemblyIdV2 assemblyId)
        {
            DocManager.SessionDoc.Add(assemblyId);
        }

        private int GetAssemblyCount()
        {
            return DocManager.SessionDoc.GetRailAssemblyCount();
        }

        private IAssemblyIdV2 CreateAssemblyIdV2(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return AssemblyIdFactory.CreateRailing(groupId, componentIdsByName);
        }
    }
}
