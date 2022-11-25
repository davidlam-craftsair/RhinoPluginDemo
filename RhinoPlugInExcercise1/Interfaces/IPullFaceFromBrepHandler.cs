using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IPullFaceFromBrepHandler
    {
        bool Pull(Brep originalBrep, Brep dynamicExtrusion, out Brep brepWholeTmp);
    }
}