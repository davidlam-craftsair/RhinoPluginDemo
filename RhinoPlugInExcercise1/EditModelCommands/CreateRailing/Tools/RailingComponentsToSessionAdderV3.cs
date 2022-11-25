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
    public class RailingComponentsToSessionAdderV3 : IComponentsToSessionAdderV2
    {
        public RailingComponentsToSessionAdderV3(IDocManager docManager, IRailingAssemblyIdFactory railingAssemblyIdFactory)
        {
            DocManager = docManager;
            RailingAssemblyIdFactory = railingAssemblyIdFactory;
        }

        public IDocManager DocManager { get; }
        public IRailingAssemblyIdFactory RailingAssemblyIdFactory { get; }
        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;

        /// <summary>
        /// Add a set of rhino geometry objects to form a SweepAssemblyId and add it to SessionDoc
        /// </summary>
        /// <param name="rhinoGeometryIds"></param>
        public void AddToSessionDoc(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            int groupId = AddRailingComponentsToGroup(componentGeometryIdsByName);
            var assemblyId = CreateRailingAssemblyId(groupId,
                                                      componentGeometryIdsByName);
            AddSweepAssemblyIdToSessionDoc(assemblyId);
        }

        private int AddRailingComponentsToGroup(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            // Get RailAssemblyCount for the SweepAssemblyId
            int railAssemblyCount = GetRailAssemblyCount();
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

        private void AddSweepAssemblyIdToSessionDoc(IRailingAssemblyId componentId)
        {
            DocManager.SessionDoc.Add(componentId);
        }

        private int GetRailAssemblyCount()
        {
            return DocManager.SessionDoc.GetRailAssemblyCount();
        }

        private IRailingAssemblyId CreateRailingAssemblyId(int groupId, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return RailingAssemblyIdFactory.Create(groupId, componentIdsByName);
        }
    }
}
