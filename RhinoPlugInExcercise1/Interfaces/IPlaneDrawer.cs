using Rhino.Display;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IPlaneDrawer
    {
        void Draw(DisplayPipeline display, Plane plane);
    }
}