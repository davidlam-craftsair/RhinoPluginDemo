using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class ComponentsToRhinoGroupAdderV2 : IComponentsToRhinoGroupAdderV2
    {
        public ComponentsToRhinoGroupAdderV2(IDocManager docManager, string assemblyName, Func<int> assemblyCount)
        {
            DocManager = docManager;
            AssemblyName = assemblyName;
            AssemblyCount = assemblyCount;
        }

        public IDocManager DocManager { get; }
        public string AssemblyName { get; }
        public Func<int> AssemblyCount { get; }

        public int AddToGroup(ICompositeObjId buildingComponentId)
        {
            // Get RailAssemblyCount for the SweepAssemblyId
            int railAssemblyCount = GetRailAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            var groupIdx = GetGroupIdx(GetRhinoGeometryIds(buildingComponentId), railAssemblyCount);
            return groupIdx;
        }

        private IEnumerable<Guid> GetRhinoGeometryIds(ICompositeObjId buildingComponentId)
        {
            return buildingComponentId.ConstituentsByName.Values.SelectMany(x => x).ToArray();
        }

        private int GetRailAssemblyCount()
        {
            return DocManager.SessionDoc.GetRailAssemblyCount();
        }

        private int GetGroupIdx(IEnumerable<Guid> rhinoGeometryIds, int assemblyCount)
        {
            return DocManager.RhinoDoc.AddGroup($"{AssemblyName} {assemblyCount}", rhinoGeometryIds);
        }

    }
}
