using Rhino.Geometry;
using Rhino.Input.Custom;

namespace RhinoPlugInExcercise1
{
    public interface IDynamicDrawInteraction
    {
        int Iteration { get; set; }
        void OnDynamicDraw(GetPointDrawEventArgs e);
    }
}