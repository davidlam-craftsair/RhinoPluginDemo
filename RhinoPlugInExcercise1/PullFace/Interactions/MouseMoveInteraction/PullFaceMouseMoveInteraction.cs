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
    public class PullFaceMouseMoveInteractionIteration0
    {
        public PullFaceMouseMoveInteractionIteration0(IPullFaceCreation pullFaceCreation,
                                                      IBrepFaceGetter brepFaceToPullGetter
                                                      )
        {
            PullFaceCreation = pullFaceCreation;
            BrepFaceToPullGetter = brepFaceToPullGetter;
        }

        public IPullFaceCreation PullFaceCreation { get; }
        public IBrepFaceGetter BrepFaceToPullGetter { get; }

        public void OnMouseMove(GetPointMouseEventArgs e)
        {
            var viewLine = PullFaceCreation.ViewLine;
            if (DecideBrepFaceToPull(e.Viewport, viewLine, out BrepFace brepFaceToPullTmp, out Plane cplane, out Guid objIdTmp))
            {
                PullFaceCreation.SelectedFaceToPull = brepFaceToPullTmp;
                PullFaceCreation.CPlane = cplane;
                PullFaceCreation.ObjId = objIdTmp;
            }
        }

        private bool DecideBrepFaceToPull(RhinoViewport viewport,
                                          Line viewLine,
                                          out BrepFace brepFaceToPull,
                                          out Plane frame,
                                          out Guid objId)
        {
            return BrepFaceToPullGetter.Get(viewLine, viewport, out brepFaceToPull, out frame, out objId);

        }
    }
}
