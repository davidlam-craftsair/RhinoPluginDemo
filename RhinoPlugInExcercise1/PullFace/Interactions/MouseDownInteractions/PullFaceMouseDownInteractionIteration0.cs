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
    public class PullFaceMouseDownInteractionIteration0
    {
        public IExtrusionCreation ExtrusionCreation { get; }
        public IBrepFaceGetter BrepFaceToPullGetter { get; }

        public PullFaceMouseDownInteractionIteration0(IExtrusionCreation extrusionCreation,
                                                      IBrepFaceGetter brepFaceToPullGetter)
        {
            ExtrusionCreation = extrusionCreation;
            BrepFaceToPullGetter = brepFaceToPullGetter;
        }

        public void OnMouseDown(GetPointMouseEventArgs e)
        {
            var viewLine = GetViewLine(e.Viewport, e.WindowPoint);
            if (DecideBrepFaceToPull(e.Viewport, viewLine, out BrepFace brepFaceToPullTmp, out Plane cplaneTmp, out Guid objIdTmp))
            {
                ExtrusionCreation.SelectedFaceToPull = brepFaceToPullTmp;
                ExtrusionCreation.ObjId = objIdTmp;
                ExtrusionCreation.CPlane = cplaneTmp;
                SetViewportConstructionPlane(e.Viewport, cplaneTmp);
                ConstrainToPlaneZAxis(cplaneTmp);
            }
        }

        private void SetViewportConstructionPlane(RhinoViewport viewport, Plane cplaneTmp)
        {
            viewport.
        }

        private bool DecideBrepFaceToPull(RhinoViewport viewport,
                                  Line viewLine,
                                  out BrepFace brepFaceToPull,
                                  out Plane frame,
                                  out Guid objId)
        {
            return BrepFaceToPullGetter.Get(viewLine, viewport, out brepFaceToPull, out frame, out objId);

        }

        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }
    }
}
