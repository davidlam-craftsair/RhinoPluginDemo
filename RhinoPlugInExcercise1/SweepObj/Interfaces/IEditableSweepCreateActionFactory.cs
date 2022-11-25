namespace RhinoPlugInExcercise1
{
    public interface IEditableSweepCreateActionFactory
    {
        IEditableSweepCreateAction Create(IComponentsToSessionAdderV2 componentsToSessionAdder);
        IEditableSweepCreateAction CreateGeneric();
        IEditableSweepCreateAction CreateForRailingComponents();

    }
}