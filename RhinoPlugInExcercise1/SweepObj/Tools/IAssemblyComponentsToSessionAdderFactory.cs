namespace RhinoPlugInExcercise1
{
    public interface IAssemblyComponentsToSessionAdderFactory
    {
        IComponentsToSessionAdder Create();
        IComponentsToSessionAdder CreateForRailingComponentsAssembly();
    }

    public interface IAssemblyComponentsToSessionAdderV2Factory
    {
        IComponentsToSessionAdderV2 Create(string assemblyTypeName);
        IComponentsToSessionAdderV2 CreateForRailingComponentsAssembly();
        IComponentsToSessionAdderV2 CreateGeneric();
    }
}