using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class SweepComponentsToSessionAdder : IComponentsToSessionAdder
    {
        public SweepComponentsToSessionAdder(IDocManager docManager, ISweepAssemblyIdFactory sweepAssemblyIdFactory)
        {
            DocManager = docManager;
            SweepAssemblyIdFactory = sweepAssemblyIdFactory;
        }

        public IDocManager DocManager { get; }
        public ISweepAssemblyIdFactory SweepAssemblyIdFactory { get; }

        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;

        /// <summary>
        /// Add a set of rhino geometry objects to form a SweepAssemblyId and add it to SessionDoc
        /// </summary>
        /// <param name="rhinoGeometryIds"></param>
        public void AddToSessionDoc(IEnumerable<Guid> rhinoGeometryIds)
        {
            // Get RailAssemblyCount for the SweepAssemblyId
            int railAssemblyCount = GetSweepAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            var groupIdx = GetGroupIdx(rhinoGeometryIds, railAssemblyCount);
            // Create SweepAssemblyId 
            ISweepAssemblyId componentId = CreateAssemblyId(groupIdx, rhinoGeometryIds);
            AddAssemblyIdToSessionDoc(componentId);
        }

        private int GetGroupIdx(IEnumerable<Guid> rhinoGeometryIds, int railAssemblyCount)
        {
            return RhinoDoc.AddGroup($"Rail {railAssemblyCount}", rhinoGeometryIds);
        }

        private void AddAssemblyIdToSessionDoc(ISweepAssemblyId componentId)
        {
            DocManager.SessionDoc.Add(componentId);
        }

        private int GetSweepAssemblyCount()
        {
            return DocManager.SessionDoc.GetSweepAssemblyCount();
        }

        private ISweepAssemblyId CreateAssemblyId(int groupIdx, IEnumerable<Guid> componentIds)
        {
            return SweepAssemblyIdFactory.Create(componentIds, groupIdx);
        }
    }
}
