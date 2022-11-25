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
    public class BoxDynamicMouseMoveInteractionV2: IMouseMoveInteraction
    {
        public BoxDynamicMouseMoveInteractionV2(IPlaneDrawer planeDrawer,
                                              IBrepFaceGetter brepFaceGetter,
                                              IFrameAlignHandler frameAlignHandler,
                                              IBoxCreationV2 boxCreation)
        {
            PlaneDrawer = planeDrawer;
            BrepFaceGetter = brepFaceGetter;
            FrameAlignHandler = frameAlignHandler;
            BoxCreation = boxCreation;
        }

        public int Iteration { get; set; }
        public IFrameAlignHandler FrameAlignHandler { get; }
        public IBoxCreationV2 BoxCreation { get; }
        public IPlaneDrawer PlaneDrawer { get; }
        public IBrepFaceGetter BrepFaceGetter { get; }

        #region Fields that generated in OnMouseMove
        public Line ViewLine 
        {
            get
            {
                return BoxCreation.ViewLine;
            }
            set
            {
                BoxCreation.ViewLine = value;
            }
        }
        public Plane CPlane1 
        {
            get
            {
                return BoxCreation.CPlane1;
            }
            set
            {
                BoxCreation.CPlane1 = value;
            }
        }
        public BrepFace SelectedBrepFaceHovered 
        { 
            get { return BoxCreation.SelectedBrepFaceHovered; }
            set
            {
                BoxCreation.SelectedBrepFaceHovered = value;
            }
        }
        public Point3d Corner1Tmp { get; set; }
        #endregion
        public void OnMouseMove(GetPointMouseEventArgs e)
        {
            if (Iteration == 0)
            {
                UpdateViewLine(e);
                if (UpdateCPlane1(e.Viewport, out BrepFace selectedBrepFace))
                {
                    SetViewportConstructionPlane(e.Viewport, CPlane1);

                    if (e.ShiftKeyDown)
                    {
                        //SelectedBrepFaceLocked = SelectedBrepFaceHovered;
                        return;
                    }
                    else
                    {
                        //SelectedBrepFaceLocked = null;
                        SelectedBrepFaceHovered = selectedBrepFace;

                    }
                }
                else
                {
                    CPlane1 = Plane.WorldXY;
                    Corner1Tmp = CPlane1.ClosestPoint(e.Point);
                    RelocateOriginCPlane1Tmp(Corner1Tmp);
                    SetViewportConstructionPlane(e.Viewport, CPlane1);
                }

            }
        }

        private void UpdateViewLine(GetPointMouseEventArgs e)
        {
            ViewLine = GetViewLine(e.Viewport, e.WindowPoint);
        }

        private void RelocateOriginCPlane1Tmp(Point3d pt)
        {
            var t = CPlane1;
            t.Origin = pt;
            CPlane1 = t;
        }

        private void SetViewportConstructionPlane(RhinoViewport viewport, Plane cplane)
        {
            viewport.SetConstructionPlane(cplane);
        }

        private bool UpdateCPlane1(RhinoViewport viewport, out BrepFace selectedBrepFace)
        {
            if (SetGeometricAidsOnBrepFaceForBoxCreation(viewport, out BrepFace brepFaceToPull, out Plane frame, out Guid objId))
            {
                CPlane1 = frame;
                selectedBrepFace = brepFaceToPull;
                return true;
            }
            selectedBrepFace = default;
            return false;
        }

        private bool SetGeometricAidsOnBrepFaceForBoxCreation(
                                                              RhinoViewport viewport,
                                                              out BrepFace brepFaceToPull,
                                                              out Plane frame,
                                                              out Guid objId)
        {
            if (BrepFaceGetter.Get(ViewLine, viewport, out brepFaceToPull, out Plane frameTmp, out objId))
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
