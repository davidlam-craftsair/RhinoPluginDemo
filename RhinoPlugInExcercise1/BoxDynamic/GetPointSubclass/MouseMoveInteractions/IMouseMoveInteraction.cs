using Rhino.Geometry;
using Rhino.Input.Custom;

namespace RhinoPlugInExcercise1
{
    public interface IMouseMoveInteraction
    {
        int Iteration { get; set; }
        void OnMouseMove(GetPointMouseEventArgs e);
    }
}