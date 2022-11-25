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
    public class BoxDynamicMouseMoveInteraction : IBoxDynamicMouseMoveInteraction
    {
        public BoxDynamicMouseMoveInteraction(IPlaneDrawer planeDrawer,
                                              IBrepFaceGetter brepFaceGetter,
                                              IFrameAlignHandler frameAlignHandler
                                              )
        {
            PlaneDrawer = planeDrawer;
            BrepFaceGetter = brepFaceGetter;
            FrameAlignHandler = frameAlignHandler;
        }

        public int Iteration { get; set; }
        public IFrameAlignHandler FrameAlignHandler { get; }
        public IPlaneDrawer PlaneDrawer { get; }
        public IBrepFaceGetter BrepFaceGetter { get; }

        #region Fields that generated in OnMouseMove
        public Line ViewLine { get; set; }
        public Plane CPlane1Tmp { get; set; }
        public Surface SelectedBrepFaceLocked { get; set; }
        public Surface SelectedBrepFaceHovered { get; set; }
        public Point3d Corner1Tmp { get; set; }
        #endregion
        public void OnMouseMove(GetPointMouseEventArgs e)
        {
            if (Iteration == 0)
            {
                ViewLine = GetViewLine(e.Viewport, e.WindowPoint);
                if (SetCPlane1Tmp(e.Viewport, e.WindowPoint, out Surface selectedBrepFace))
                {
                    SetViewportConstructionPlane(e.Viewport, CPlane1Tmp);

                    if (e.ShiftKeyDown)
                    {
                        SelectedBrepFaceLocked = SelectedBrepFaceHovered;
                        return;
                    }
                    else
                    {
                        SelectedBrepFaceLocked = null;
                        SelectedBrepFaceHovered = selectedBrepFace;

                    }
                }
                else
                {
                    CPlane1Tmp = Plane.WorldXY;
                    Corner1Tmp = CPlane1Tmp.ClosestPoint(e.Point);
                    RelocateOriginCPlane1Tmp(Corner1Tmp);
                    SetViewportConstructionPlane(e.Viewport, CPlane1Tmp);
                }

            }
        }

        private void RelocateOriginCPlane1Tmp(Point3d pt)
        {
            var t = CPlane1Tmp;
            t.Origin = pt;
            CPlane1Tmp = t;
        }

        private void SetViewportConstructionPlane(RhinoViewport viewport, Plane cplane)
        {
            viewport.SetConstructionPlane(cplane);
        }

        private bool SetCPlane1Tmp(RhinoViewport viewport, System.Drawing.Point windowPoint, out Surface selectedBrepFace)
        {
            var viewLine = GetViewLine(viewport, windowPoint);
            if (SetGeometricAidsOnBrepFaceForBoxCreation(viewLine, viewport, out BrepFace brepFaceToPull, out Plane frame, out Guid objId))
            {
                CPlane1Tmp = frame;
                selectedBrepFace = brepFaceToPull;
                return true;
            }
            selectedBrepFace = default;
            return false;
        }

        private bool SetGeometricAidsOnBrepFaceForBoxCreation(Line viewLine,
                                                              RhinoViewport viewport,
                                                              out BrepFace brepFaceToPull,
                                                              out Plane frame,
                                                              out Guid objId)
        {
            if (BrepFaceGetter.Get(viewLine, viewport, out brepFaceToPull, out Plane frameTmp, out objId))
            {
                frame = FrameAlignHandler.Align(frameTmp);
                return true;
            }
            frame = default;
            return false;

        }

        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }

    }
}
