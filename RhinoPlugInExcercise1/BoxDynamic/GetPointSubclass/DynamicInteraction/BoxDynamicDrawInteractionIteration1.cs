using Rhino.Display;
using Rhino.Geometry;
using Rhino.Input.Custom;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicDrawInteractionIteration1
    {
        public BoxDynamicDrawInteractionIteration1(IBoxCreationV2 boxCreation, IDrawer drawer)
        {
            BoxCreation = boxCreation;
            Drawer = drawer;
        }
        public IBoxCreationV2 BoxCreation { get; }
        public IDrawer Drawer { get; }

        #region Properties that needs input
        public Plane CPlane1 => BoxCreation.CPlane1;
        public BrepFace SelectedBrepFaceHovered => BoxCreation.SelectedBrepFaceHovered;
        public Point3d BaseCorner1
        {
            get { return BoxCreation.BaseCorner1; }
            set { BoxCreation.BaseCorner1 = value; }
        }
        #endregion

        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            DrawCPlane1(e.Display);
            DrawSelectedBrepFaceHovered(e.Display);

            SetAndDrawBaseCorner1(e);

        }

        private void DrawSelectedBrepFaceHovered(DisplayPipeline display)
        {
            if (SelectedBrepFaceHovered != null)
            {
                display.DrawSurface(SelectedBrepFaceHovered, System.Drawing.Color.DarkRed, 3);
            }
        }

        private void DrawCPlane1(DisplayPipeline display)
        {
            DrawPlane(display, CPlane1);
        }

        private void DrawPlane(DisplayPipeline display, Plane plane)
        {
            Drawer.Draw(display, plane);
        }

        private void SetAndDrawBaseCorner1(GetPointDrawEventArgs e)
        {
            BaseCorner1 = CPlane1.ClosestPoint(e.CurrentPoint);
            e.Display.DrawPoint(BaseCorner1, PointStyle.ControlPoint, 3, System.Drawing.Color.DarkRed);
        }

    }
}
