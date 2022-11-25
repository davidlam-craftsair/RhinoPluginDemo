using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceDynamicDrawInteractionIteration1
    {
        public PullFaceDynamicDrawInteractionIteration1(IPullFaceCreation extrusionCreation, IDrawer drawer)
        {
            PullFaceCreation = extrusionCreation;
            Drawer = drawer;
        }

        public IPullFaceCreation PullFaceCreation { get; }
        public IDrawer Drawer { get; }

        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            // Draw dynamic extrusion
            UpdateExtrusionVector(e);
            var brep = PullFaceCreation.GetDynamicExtrusion();
            if (brep != null)
            {
                Drawer.DrawDefaultWires(e.Display, brep);
            }
        }

        private void UpdateExtrusionVector(GetPointDrawEventArgs e)
        {
            Plane cPlane = PullFaceCreation.CPlane;
            PullFaceCreation.ExtrusionVector = e.CurrentPoint - cPlane.Origin;
        }
    }
}
