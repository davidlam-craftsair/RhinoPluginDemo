using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class ComponentsToRhinoGroupAdder : IComponentsToRhinoGroupAdder
    {
        public ComponentsToRhinoGroupAdder(IDocManager docManager, string assemblyName, Func<int> assemblyCount)
        {
            DocManager = docManager;
            AssemblyName = assemblyName;
            AssemblyCount = assemblyCount;
        }

        public IDocManager DocManager { get; }
        public string AssemblyName { get; }
        public Func<int> AssemblyCount { get; }

        public int AddToGroup(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            // Get RailAssemblyCount for the SweepAssemblyId
            int railAssemblyCount = GetRailAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            var groupIdx = GetGroupIdx(GetRhinoGeometryIds(componentGeometryIdsByName), railAssemblyCount);
            return groupIdx;
        }

        private int GetRailAssemblyCount()
        {
            return DocManager.SessionDoc.GetRailAssemblyCount();
        }

        private int GetGroupIdx(IEnumerable<Guid> rhinoGeometryIds, int assemblyCount)
        {
            return DocManager.RhinoDoc.AddGroup($"{AssemblyName} {assemblyCount}", rhinoGeometryIds);
        }

        private IEnumerable<Guid> GetRhinoGeometryIds(IDictionary<string, IEnumerable<Guid>> componentGeometryIdsByName)
        {
            return componentGeometryIdsByName.Values.SelectMany(x => x).ToList();
        }
    }
}
