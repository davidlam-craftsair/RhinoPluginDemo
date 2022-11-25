using Rhino.Display;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceDynamicDrawInteraction
    {
        public PullFaceDynamicDrawInteraction(IPullFaceCreation pullFaceCreation, IDrawer drawer,
                                PullFaceDynamicDrawInteractionIteration0 pullFaceDynamicDrawInteractionIteration0,
                                PullFaceDynamicDrawInteractionIteration1 pullFaceDynamicDrawInteractionIteration1
            )
        {
            PullFaceCreation = pullFaceCreation;
            Drawer = drawer;
            PullFaceDynamicDrawInteractionIteration0 = pullFaceDynamicDrawInteractionIteration0;
            PullFaceDynamicDrawInteractionIteration1 = pullFaceDynamicDrawInteractionIteration1;
        }
        public int Iteration { get; set; }
        public IPullFaceCreation PullFaceCreation { get; }
        public IDrawer Drawer { get; }
        public PullFaceDynamicDrawInteractionIteration0 PullFaceDynamicDrawInteractionIteration0 { get; }
        public PullFaceDynamicDrawInteractionIteration1 PullFaceDynamicDrawInteractionIteration1 { get; }

        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            DrawViewLine(e);
            DrawPlane(e.Display, PullFaceCreation.CPlane);
            if (Iteration == 0)
            {
                PullFaceDynamicDrawInteractionIteration0.OnDynamicDraw(e);
            }
            if (Iteration == 1)
            {
                PullFaceDynamicDrawInteractionIteration1.OnDynamicDraw(e);
            }
        }

        private void DrawViewLine(GetPointDrawEventArgs e)
        {
            var viewLineDisplay = PullFaceCreation.ViewLine;
            if (viewLineDisplay.IsValid)
            {
                e.Display.DrawLine(viewLineDisplay, System.Drawing.Color.DarkRed, 5);
                e.Display.DrawPoint(viewLineDisplay.To, PointStyle.ControlPoint, 5, System.Drawing.Color.DarkRed);
            }
        }

        private void DrawPlane(DisplayPipeline display, Plane plane)
        {
            Drawer.Draw(display, plane);
        }

    }
}
