using Rhino.Display;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicDrawInteractionIteration2
    {
        public BoxDynamicDrawInteractionIteration2(IBoxCreationV2 boxCreation, IDrawer drawer)
        {
            BoxCreation = boxCreation;
            Drawer = drawer;
        }

        public IBoxCreationV2 BoxCreation { get; }
        public IDrawer Drawer { get; }

        public Plane CPlane1 => BoxCreation.CPlane1;
        public Point3d BaseCorner1 => BoxCreation.BaseCorner1;
        public Point3d BaseCorner2
        { get => BoxCreation.BaseCorner2;
          set
            {
                BoxCreation.BaseCorner2 = value;
            }
        }
        public BrepFace SelectedBrepFaceHovered => BoxCreation.SelectedBrepFaceHovered;


        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            DrawCPlane1(e.Display);
            DrawSelectedBrepFaceHovered(e.Display);

            UpdateBaseCorner2(e);
            // Draw the base rectangle
            DrawBaseRect(e, GetBaseRect());
        }

        private void UpdateBaseCorner2(GetPointDrawEventArgs e)
        {
            BaseCorner2 = CPlane1.ClosestPoint(e.CurrentPoint);
        }

        private Rectangle3d GetBaseRect()
        {
            return new Rectangle3d(CPlane1, BaseCorner1, BaseCorner2);
        }

        private static void DrawBaseRect(GetPointDrawEventArgs e, Rectangle3d baseRect)
        {
            e.Display.DrawPolyline(baseRect.ToPolyline(), System.Drawing.Color.DarkRed);
        }

        private void DrawCPlane1(DisplayPipeline display)
        {
            DrawPlane(display, CPlane1);
        }

        private void DrawPlane(DisplayPipeline display, Plane plane)
        {
            Drawer.Draw(display, plane);
        }

        private void DrawSelectedBrepFaceHovered(DisplayPipeline display)
        {
            Drawer.DrawDefaultShaded(display, SelectedBrepFaceHovered);
        }
    }
}