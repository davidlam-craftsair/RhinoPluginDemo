using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GenericAssemblyComponentsToSessionAdder: IComponentsToSessionAdder
    {
        public GenericAssemblyComponentsToSessionAdder(IDocManager docManager, IAssemblyIdFactory assemblyIdFactory, string assemblyTypeName)
        {
            DocManager = docManager;
            AssemblyIdFactory = assemblyIdFactory;
            AssemblyTypeName = assemblyTypeName;
        }

        public IDocManager DocManager { get; }
        public IAssemblyIdFactory AssemblyIdFactory { get; }
        public string AssemblyTypeName { get; }

        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;

        /// <summary>
        /// Add a set of rhino geometry objects to form a AssemblyId and add it to SessionDoc
        /// </summary>
        /// <param name="rhinoGeometryIds"></param>
        public void AddToSessionDoc(IEnumerable<Guid> rhinoGeometryIds)
        {
            int groupIdx = AddAssemblyComponentsToGroup(rhinoGeometryIds);
            // Create AssemblyId 
            IAssemblyId componentId = CreateAssemblyId(groupIdx, rhinoGeometryIds);
            AddAssemblyIdToSessionDoc(componentId);
        }

        private int AddAssemblyComponentsToGroup(IEnumerable<Guid> rhinoGeometryIds)
        {
            // Get AssemblyCount for the AssemblyId
            int assemblyCount = GetAssemblyCount();
            // Add a rhino group to Rhino, with members from ts
            var groupIdx = GetGroupIdx(rhinoGeometryIds, assemblyCount);
            return groupIdx;
        }

        private int GetGroupIdx(IEnumerable<Guid> rhinoGeometryIds, int assemblyCount)
        {
            return RhinoDoc.AddGroup($"{AssemblyTypeName} {assemblyCount}", rhinoGeometryIds);
        }

        private void AddAssemblyIdToSessionDoc(IAssemblyId componentId)
        {
            DocManager.SessionDoc.Add(componentId);
        }

        private int GetAssemblyCount()
        {
            return DocManager.SessionDoc.GetGenericAssemblyCount();
        }

        private IAssemblyId CreateAssemblyId(int groupIdx, IEnumerable<Guid> componentIds)
        {
            return AssemblyIdFactory.Create(componentIds, groupIdx);
        }
    }
}
