namespace RhinoPlugInExcercise1
{
    public interface IGetBoxDynamicIteration2
    {
        CornerPtIteration CornerPtIteration2 { get; }

        void Do(GetBoxDynamic planeDynamic, BoxDynamicParameters boxDynamicParameters);
    }
}