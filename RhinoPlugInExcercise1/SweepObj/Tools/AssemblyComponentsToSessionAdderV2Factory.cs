using DL.Framework;

namespace RhinoPlugInExcercise1
{
    public class AssemblyComponentsToSessionAdderV2Factory : IAssemblyComponentsToSessionAdderV2Factory
    {
        public IComponentsToSessionAdderV2 Create(string assemblyTypeName)
        {
            return new RailingComponentsToSessionAdder(DI.DocManager, IoC.Get<IAssemblyIdV2Factory>(), assemblyTypeName);
        }

        public IComponentsToSessionAdderV2 CreateForRailingComponentsAssembly()
        {
            return Create(AssemblyTypeNames.Railing);
        }

        public IComponentsToSessionAdderV2 CreateGeneric()
        {
            return Create(AssemblyTypeNames.Generic);
        }
    }
}
