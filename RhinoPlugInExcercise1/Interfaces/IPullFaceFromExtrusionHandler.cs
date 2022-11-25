using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IPullFaceFromExtrusionHandler
    {
        bool Pull(Extrusion originalExtrusion, Vector3d extrusionVector, Brep dynamicExtrusion, out Brep brepWholeTmp);
    }
}