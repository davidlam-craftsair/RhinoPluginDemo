using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepFaceGetter : IBrepFaceGetter
    {
        public IRelevantClosestObjToViewCursorGetter RelevantClosestObjToViewCursorGetter { get; }
        public IRelevantBrepFaceClosestToViewCursorGetter RelevantBrepFaceClosestToViewCursorGetter { get; }

        public BrepFaceGetter(IRelevantClosestObjToViewCursorGetter relevantClosestObjToViewCursorGetter,
                               IRelevantBrepFaceClosestToViewCursorGetter relevantBrepFaceClosestToViewCursorGetter
            )
        {
            RelevantClosestObjToViewCursorGetter = relevantClosestObjToViewCursorGetter;
            RelevantBrepFaceClosestToViewCursorGetter = relevantBrepFaceClosestToViewCursorGetter;
        }
        public bool Get(
                        Line viewLine,
                        RhinoViewport viewport,
                        out BrepFace brepFaceToPull,
                        out Plane frame,
                        out Guid objId)
        {

            RhinoObject closestObj = GetClosestObj(viewport, viewLine);

            if (closestObj != null)
            {
                objId = closestObj.Id;
                if (closestObj.Geometry is Extrusion extrusion)
                {
                    return GetBrepFaceToPull(extrusion.ToBrep(), viewLine, out brepFaceToPull, out frame);
                }
                else if (closestObj.Geometry is Brep brep)
                {
                    return GetBrepFaceToPull(brep, viewLine, out brepFaceToPull, out frame);
                }
                else if (closestObj.Geometry is Surface surface)
                {
                    return GetBrepFaceToPull(surface.ToBrep(), viewLine, out brepFaceToPull, out frame);
                }
            }

            // if there is no closest obj found,
            brepFaceToPull = default;
            frame = default;
            objId = default;
            return false;
        }

        private bool GetBrepFaceToPull(Brep brep, Line viewLine, out BrepFace brepFaceUnderlaySrfToPull, out Plane frame)
        {
            if (GetRelevantBrepFaceClosestToViewAndItsParameters(brep, viewLine, out BrepFace brepFace, out frame))
            {
                //PrepareBrepface(brepFace);
                brepFaceUnderlaySrfToPull = brepFace;//.UnderlyingSurface();
                return true;
            }
            brepFaceUnderlaySrfToPull = default;
            frame = default;
            return false;
        }

        private bool GetRelevantBrepFaceClosestToViewAndItsParameters(Brep brep,
                                                                      Line viewLine,
                                                                      out BrepFace brepSrf,
                                                                      out Plane frame)
        {
            return RelevantBrepFaceClosestToViewCursorGetter.Get(brep, viewLine, out brepSrf, out frame);
        }

        private RhinoObject GetClosestObj(RhinoViewport viewport, Line viewLine)
        {
            return RelevantClosestObjToViewCursorGetter.Get(viewport, viewLine);
        }
    }
}
