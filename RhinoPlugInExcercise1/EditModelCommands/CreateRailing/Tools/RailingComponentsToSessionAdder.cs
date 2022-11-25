using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingComponentsToSessionAdderV2 : IRailingComponentsToSessionAdder
    {
        public RailingComponentsToSessionAdderV2(IDocManager docManager, IRailingAssemblyIdFactory railingAssemblyIdFactory)
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
        public void AddToSessionDoc(IEnumerable<Guid> railId, IEnumerable<Guid> profileId)
        {
            // Get RailAssemblyCount for the SweepAssemblyId
            int railAssemblyCount = GetRailAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            List<Guid> rhinoGeometryIds = GetRhinoGeometryIds(railId, profileId);
            var groupIdx = GetGroupIdx(rhinoGeometryIds, railAssemblyCount);
            // Create SweepAssemblyId 
            var componentId = CreateRailingAssemblyId(groupIdx, railId, profileId);
            AddSweepAssemblyIdToSessionDoc(componentId);
        }

        private static List<Guid> GetRhinoGeometryIds(IEnumerable<Guid> railId, IEnumerable<Guid> profileId)
        {
            var rhinoGeometryIds = new List<Guid>();
            rhinoGeometryIds.AddRange(railId);
            rhinoGeometryIds.AddRange(profileId);
            return rhinoGeometryIds;
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

        private IRailingAssemblyId CreateRailingAssemblyId(int groupId, IEnumerable<Guid> railId, IEnumerable<Guid> profileId)
        {
            return RailingAssemblyIdFactory.Create(groupId, railId, profileId);
        }
    }
}
