using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingComponentsToSessionAdder : IComponentsToSessionAdder
    {
        public RailingComponentsToSessionAdder(IDocManager docManager, IRailingAssemblyIdFactory railingAssemblyIdFactory)
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
        public void AddToSessionDoc(IEnumerable<Guid> rhinoGeometryIds)
        {
            // Get RailAssemblyCount for the SweepAssemblyId
            int railAssemblyCount = GetRailAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            var groupIdx = GetGroupIdx(rhinoGeometryIds, railAssemblyCount);
            // Create SweepAssemblyId 
            var componentId = CreateRailingAssemblyId(groupIdx, rhinoGeometryIds);
            AddSweepAssemblyIdToSessionDoc(componentId);
        }

        private IRailingAssemblyId CreateRailingAssemblyId(int groupIdx, IEnumerable<Guid> rhinoGeometryIds)
        {
            var d = new Dictionary<string, IEnumerable<Guid>>();
            d[]
            CreateRailingAssemblyId(groupIdx, )
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

        private IRailingAssemblyId CreateRailingAssemblyId(int groupIdx, IDictionary<string, IEnumerable<Guid>> componentIdsByName)
        {
            return RailingAssemblyIdFactory.Create(groupIdx, );
        }
    }
}
