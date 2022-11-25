using Rhino.Geometry;
using Rhino.Input.Custom;
using System;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicDrawInteractionIteration3
    {
        public BoxDynamicDrawInteractionIteration3(IBoxCreationV2 boxCreation)
        {
            BoxCreation = boxCreation;
        }

        public IBoxCreationV2 BoxCreation { get; }
        public Plane CPlane1 => BoxCreation.CPlane1;
        public Plane CPlane2 => BoxCreation.CPlane2;
        public Point3d BaseCorner1 => BoxCreation.BaseCorner1;
        public Point3d BaseCorner2 => BoxCreation.BaseCorner2;
        public Point3d TopCorner
        {
            get { return BoxCreation.TopCorner; }
            set { BoxCreation.TopCorner = value; }
        }
        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            // Draw the base rectangle with already determined base corners and plane
            DrawBaseRect(e);

            // Draw the box side with the mouse pt as top corner of the box
            UpdateTopCorner(e);

            var boxSide = GetBoxSide();
            e.Display.DrawLine(boxSide, System.Drawing.Color.DarkBlue, 5);

            // Draw the box
            DrawBox(e);
        }

        private void DrawBox(GetPointDrawEventArgs e)
        {
            var box = BoxCreation.Get();
            e.Display.DrawBox(box, System.Drawing.Color.Purple, 2);
        }

        private void UpdateTopCorner(GetPointDrawEventArgs e)
        {
            TopCorner = CPlane2.ClosestPoint(e.CurrentPoint);
        }

        private Line GetBoxSide()
        {
            return new Line(CPlane2.Origin, TopCorner);
        }

        private void DrawBaseRect(GetPointDrawEventArgs e)
        {
            var baseRect = new Rectangle3d(CPlane1, BaseCorner1, BaseCorner2);
            e.Display.DrawPolyline(baseRect.ToPolyline(), System.Drawing.Color.DarkRed);
        }
    }
}