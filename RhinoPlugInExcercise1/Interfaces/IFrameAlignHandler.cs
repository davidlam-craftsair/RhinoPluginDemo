using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IFrameAlignHandler
    {
        Plane Align(Plane plane);
    }
}