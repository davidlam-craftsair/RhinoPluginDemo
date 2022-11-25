using Rhino.Display;
using Rhino.Input.Custom;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicDrawInteractionIteration0
    {
        public BoxDynamicDrawInteractionIteration0(IBoxCreationV2 boxCreation, IDrawer drawer)
        {
            BoxCreation = boxCreation;
            Drawer = drawer;
        }
        public IBoxCreationV2 BoxCreation { get; }
        public IDrawer Drawer { get; }

        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            DrawCPlane1(e.Display);
            DrawSelectedBrepFaceHovered(e);

        }

        private void DrawSelectedBrepFaceHovered(GetPointDrawEventArgs e)
        {
            Drawer.DrawDefaultShaded(e.Display, BoxCreation.SelectedBrepFaceHovered);
        }

        private void DrawCPlane1(DisplayPipeline display)
        {
            Drawer.Draw(display, BoxCreation.CPlane1);
        }
    }
}
