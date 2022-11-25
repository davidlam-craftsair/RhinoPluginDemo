using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceDynamicDrawInteractionIteration0
    {
        public PullFaceDynamicDrawInteractionIteration0(IPullFaceCreation extrusionCreation, IDrawer drawer)
        {
            PullFaceCreation = extrusionCreation;
            Drawer = drawer;
        }

        public IPullFaceCreation PullFaceCreation { get; }
        public IDrawer Drawer { get; }

        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            Drawer.DrawDefaultShaded(e.Display, PullFaceCreation.SelectedFaceToPull);
        }
    }
}
