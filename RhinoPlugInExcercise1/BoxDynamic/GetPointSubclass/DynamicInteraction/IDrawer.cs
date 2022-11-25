using Eto.Drawing;
using Rhino.Display;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IDrawer
    {
        void DrawDefaultShaded(DisplayPipeline display, BrepFace brepFace);
        void Draw(DisplayPipeline display, BrepFace brepFace, DisplayMaterial displayMaterial);
        void DrawDefaultWires(DisplayPipeline display, Brep brep);
        void Draw(DisplayPipeline display, Brep brep);
        void Draw(DisplayPipeline display, Plane plane);
    }

    public interface ISelectedBrepFaceHoveredDrawer
    {
        void Draw(DisplayPipeline display);
    }
}