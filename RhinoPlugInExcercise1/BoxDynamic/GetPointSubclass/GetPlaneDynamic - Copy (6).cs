using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;

namespace RhinoPlugInExcercise1
{
    public class GetPlaneDynamic: GetPoint
    {
        #region Fields
        private Line viewLine;
        private Plane cPlane1Tmp;
        private Point3d corner1Tmp;
        private Point3d topCornerTmp;
        private Surface selectedBrepFaceHovered;
        private Surface selectedBrepFaceLocked;
        #endregion

        #region Properties
        public IPlaneDrawer PlaneDrawer { get; }
        public IBrepFaceGetter BrepFaceGetter { get; }
        public IOffsetDistancesGetter OffsetDistancesGetter { get; }
        public int Iteration { get; set; }

        public Plane CPlane1 => BoxCreation.CPlane1;
        public Plane CPlane2 => BoxCreation.CPlane2;
        public Point3d BaseCorner1 => BoxCreation.BaseCorner1;
        public Point3d BaseCorner2 => BoxCreation.BaseCorner2;
        public Box Box { get; private set; }
        public Surface SelectedBrepFace { get; private set; }
        public IBoxCreation BoxCreation { get; private set; }
        public IFrameAlignHandler FrameAlignHandler { get; }
        #endregion

        public GetPlaneDynamic(IPlaneDrawer planeDrawer,
                               IBrepFaceGetter brepFaceGetter,
                               IOffsetDistancesGetter offsetDistancesGetter,
                               IBoxCreation boxCreation,
                               IFrameAlignHandler frameAlignHandler
                               )
        {
            PlaneDrawer = planeDrawer;
            BrepFaceGetter = brepFaceGetter;
            OffsetDistancesGetter = offsetDistancesGetter;
            BoxCreation = boxCreation;
            FrameAlignHandler = frameAlignHandler;
            Iteration = 0;
            EnableObjectSnapCursors(true);
            PermitObjectSnap(true);
        }

        internal void OffsetBaseCorner1(double dx, double dy)
        {
            var pt = BaseCorner1;
            BoxCreation.BaseCorner1 = GetTransformedPt(dx, dy, pt);
        }

        private Point3d GetTransformedPt(double t1, double t2, Point3d pt)
        {
            Transform transform = GetTransform(t1, t2, CPlane1);
            pt.Transform(transform);
            return pt;
        }

        internal void OffsetBaseCorner2(double dx, double dy)
        {
            var pt = BaseCorner2;
            BoxCreation.BaseCorner2 = GetTransformedPt(dx, dy, pt);
        }

        #region Events Methods
        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Iteration == 0)
            {
                viewLine = GetViewLine(e.Viewport, e.WindowPoint);
                if (SetCPlane1Tmp(e.Viewport, e.WindowPoint, out Surface selectedBrepFace))
                {
                    SetConstructionPlaneForViewport(e.Viewport, cPlane1Tmp);

                    if (e.ShiftKeyDown)
                    {
                        selectedBrepFaceLocked = selectedBrepFaceHovered;
                        return;
                    }
                    else
                    {
                        selectedBrepFaceLocked = null;
                        selectedBrepFaceHovered = selectedBrepFace;

                    }
                }
                else
                {
                    cPlane1Tmp = Plane.WorldXY;
                    corner1Tmp = cPlane1Tmp.ClosestPoint(e.Point);
                    cPlane1Tmp.Origin = corner1Tmp;
                    SetConstructionPlaneForViewport(e.Viewport, cPlane1Tmp);
                }

            }
        }
        
        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Iteration == 0)
            {
                MouseDownIteration0(e);
            }
            else if (Iteration == 1)
            {
                MouseDownIteration1(e);
            }
            else if (Iteration == 2)
            {
                MouseDownIteration2(e);
            }
            else if (Iteration == 3)
            {
                MouseDownIteration3(e);
            }

        }



        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            if (Iteration == 0)
            {
                DynamicDrawIteration0(e);
            }
            else if (Iteration == 1)
            {
                DynamicDrawIteration1(e);
            }
            else if (Iteration == 2)
            {
                DynamicDrawIteration2(e);
            }
            else if (Iteration == 3)
            {
                DynamicDrawIteration3(e);
            }
        }




        #endregion

        #region Iteration Methods for MouseDown
        private void MouseDownIteration0(GetPointMouseEventArgs e)
        {
            // Once mouse is down on 1st iteration, base corner 1 is determined
            // On mouse down, found out 
            if (e.ShiftKeyDown)
            {
                // if shift key is down, use the locked selected brep face previously 
                if (SetCPlane1TmpFromSurface(selectedBrepFaceLocked))
                {
                    SetSelectedBrepFace(selectedBrepFaceLocked);
                }
                else
                {
                    SetDefaultCPlaneTmpAndCorner1Tmp(GetViewLine(e.Viewport, e.WindowPoint));
                }
            }
            else
            {
                if (SetCPlane1Tmp(e.Viewport, e.WindowPoint, out Surface selectedBrepFace))
                {
                    SetSelectedBrepFace(selectedBrepFace);
                }
                else
                {
                    SetDefaultCPlaneTmpAndCorner1Tmp(GetViewLine(e.Viewport, e.WindowPoint));
                }

            }

            // set the Construction plane CPlane1 based on the surface
            //RegisterCPlane1();
            //SetConstructionPlaneForViewport(e.Viewport, cPlane1Tmp);
            //ConstrainToConstructionPlane(false);
        }

        private void MouseDownIteration1(GetPointMouseEventArgs e)
        {
        }

        /// <summary>
        /// Set the base corner 2 of the rectangular box base
        /// </summary>
        /// <param name="e"></param>
        private void MouseDownIteration2(GetPointMouseEventArgs e)
        {

        }

        private void MouseDownIteration3(GetPointMouseEventArgs e)
        {
            RegisterTopCorner(e.Point);
            Box = BoxCreation.Get();
        }

        #endregion
        private void RegisterTopCorner(Point3d point)
        {
            BoxCreation.TopCorner = point;
        }


        #region Iteration Methods for Dynamic Draw
        private void DynamicDrawAllIterations(GetPointDrawEventArgs e)
        {
            DrawViewLine(e.Display);
        }

        private void DrawCPlane1Tmp(DisplayPipeline display)
        {
            DrawPlane(display, cPlane1Tmp);
        }

        /// <summary>
        /// Draw any face that the mouse hovers over
        /// </summary>
        /// <param name="e"></param>
        private void DynamicDrawIteration0(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);

            DrawSelectedBrepFaceHovered(e);
            //DrawSelectedBrepFaceTmp(e);
        }

        /// <summary>
        /// Draw the temporary corner 1
        /// </summary>
        /// <param name="e"></param>
        private void DynamicDrawIteration1(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);
            DrawCPlane1Tmp(e.Display);

            DrawSelectedBrepFaceHovered(e);
            SetAndDrawCorner1Tmp(e);
        }

        private void SetAndDrawCorner1Tmp(GetPointDrawEventArgs e)
        {
            corner1Tmp = CPlane1.ClosestPoint(e.CurrentPoint);
            BoxCreation.BaseCorner1 = corner1Tmp;
            e.Display.DrawPoint(corner1Tmp, PointStyle.ControlPoint, 3, System.Drawing.Color.DarkRed);
        }

        private void DynamicDrawIteration2(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);
            DrawCPlane1Tmp(e.Display);
            DrawSelectedBrepFaceHovered(e);

            var corner2 = CPlane1.ClosestPoint(e.CurrentPoint);
            var baseRect = new Rectangle3d(CPlane1, BaseCorner1, corner2);
            BoxCreation.BaseCorner2 = corner2;
            // Draw the base rectangle
            e.Display.DrawPolyline(baseRect.ToPolyline(), System.Drawing.Color.DarkRed);
        }

        private void DynamicDrawIteration3(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);

            // Draw the base rectangle with already determined base corners and plane
            var baseRect = new Rectangle3d(CPlane1, BaseCorner1, BaseCorner2);
            e.Display.DrawPolyline(baseRect.ToPolyline(), System.Drawing.Color.DarkRed);

            // Draw the box side with the mouse pt as top corner of the box
            topCornerTmp = CPlane2.ClosestPoint(e.CurrentPoint);
            BoxCreation.TopCorner = topCornerTmp;

            var boxSideTmp = new Line(CPlane2.Origin, topCornerTmp);
            e.Display.DrawLine(boxSideTmp, System.Drawing.Color.DarkBlue, 5);

            // Draw the box
            Box = BoxCreation.Get();
            //Box = new Box(CPlane2, new Point3d[] { BaseCorner1, BaseCorner2, topCornerTmp });
            e.Display.DrawBox(Box, System.Drawing.Color.Purple, 2);
        }

        #endregion

        private void SetSelectedBrepFace(Surface srf)
        {
            SelectedBrepFace = srf;
        }
        public void RegisterCPlane1()
        {
            BoxCreation.CPlane1 = cPlane1Tmp;
        }

        public void RegisterCPlane2()
        {
            BoxCreation.CPlane2 = new Plane(BaseCorner2, CPlane1.ZAxis, CPlane1.XAxis);
        }

        private bool SetCPlane1Tmp(RhinoViewport viewport, System.Drawing.Point windowPoint, out Surface selectedBrepFace)
        {
            var viewLine = GetViewLine(viewport, windowPoint);
            if (SetGeometricAidsOnBrepFaceForBoxCreation(viewLine, viewport, out BrepFace brepFaceToPull, out Plane frame, out Guid objId))
            {
                cPlane1Tmp = frame;
                selectedBrepFace = brepFaceToPull;
                return true;
            }
            selectedBrepFace = default;
            return false;
        }
        
        private bool SetCPlane1TmpFromSurface(Surface srf)
        {
            var u = srf.Domain(0);
            var v = srf.Domain(1);
            var ut = (u[1] - u[0]) / 2;
            var vt = (v[1] - v[0]) / 2;

            if (srf.FrameAt(ut, vt, out Plane frame))
            {
                cPlane1Tmp = frame;
                return true;
            }
            return false;
        }
        
        private void SetConstructionPlaneForViewport(RhinoViewport viewport, Plane cplane)
        {
            viewport.SetConstructionPlane(cplane);
        }
        
        private void SetDefaultCPlaneTmpAndCorner1Tmp(Line viewLine)
        {
            if (!cPlane1Tmp.IsValid)
            {
                cPlane1Tmp = Plane.WorldXY;
                if (Intersection.LinePlane(viewLine, cPlane1Tmp, out double lineParameter))
                {
                    corner1Tmp = viewLine.PointAt(lineParameter);
                }
            }
        }

        private Transform GetTransform(double t1, double t2, Plane cplane)
        {
            var xVector = cplane.XAxis;
            xVector.Unitize();
            var yVector = cplane.YAxis;
            yVector.Unitize();
            var v = xVector * t1 + yVector * t2;
            var transform = Transform.Translation(v);
            return transform;
        }

        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }


        private void DrawPlane(DisplayPipeline display, Plane plane)
        {
            PlaneDrawer.Draw(display, plane);
        }
        
        private void DrawViewLine(DisplayPipeline display)
        {
            if (viewLine.IsValid)
            {
                display.DrawLine(viewLine, System.Drawing.Color.DarkRed, 5);
            }
        }

        private void DrawSelectedBrepFaceHovered(GetPointDrawEventArgs e)
        {
            if (selectedBrepFaceHovered != null)
            {
                e.Display.DrawSurface(selectedBrepFaceHovered, System.Drawing.Color.DarkRed, 3);
            }
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


    }
}
