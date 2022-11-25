using Rhino.Input.Custom;

namespace RhinoPlugInExcercise1
{
    public interface IOffsetDistancesGetter
    {
        bool Get(out double dx, out double dy);
    }
}