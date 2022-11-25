using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IBrepAdditionOrSubstractionChecker
    {
        int Check(Brep originalBrep, Brep dynamicExtrusion);
    }
}