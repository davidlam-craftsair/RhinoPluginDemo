using Eto.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetPlaneDynamic: GetPoint
    {
        #region Fields
        private Line viewLine;
        private Plane cPlane1Tmp;
        private Point3d corner1Tmp;
        private Point3d corner2Tmp;
        private Point3d topCornerTmp;
        private Surface selectedBrepFaceHovered;
        private Surface selectedBrepFaceLocked;
        #endregion

        #region Properties
        public double Tol { get; }
        public IRelevantBrepFaceClosestToViewCursorGetter RelevantBrepFaceClosestToViewCursorGetter { get; }
        public IRelevantClosestObjToViewCursorGetter RelevantClosestObjToViewCursorGetter { get; }
        public IPlaneDrawer PlaneDrawer { get; }
        public IBrepFaceGetter BrepFaceGetter { get; }
        public RhinoDoc ActiveDoc { get; }
        public int Iteration { get; set; }

        public Plane CPlane1 { get; private set; }
        public Plane CPlane2 { get; private set; }
        public Point3d BaseCorner1 { get; private set; }
        public Point3d BaseCorner2 { get; private set; }
        public Box Box { get; private set; }
        public Surface SelectedBrepFace { get; private set; }
        #endregion

        public GetPlaneDynamic(IRelevantBrepFaceClosestToViewCursorGetter relevantBrepFaceClosestToViewCursorGetter,
                               IRelevantClosestObjToViewCursorGetter relevantClosestObjToViewCursorGetter,
                               IPlaneDrawer planeDrawer,
                               IBrepFaceGetter brepFaceGetter,
                               RhinoDoc activeDoc)
        {
            RelevantBrepFaceClosestToViewCursorGetter = relevantBrepFaceClosestToViewCursorGetter;
            RelevantClosestObjToViewCursorGetter = relevantClosestObjToViewCursorGetter;
            PlaneDrawer = planeDrawer;
            BrepFaceGetter = brepFaceGetter;
            ActiveDoc = activeDoc;
            Tol = ActiveDoc.ModelAbsoluteTolerance;
            Iteration = 0;
            EnableObjectSnapCursors(true);
            PermitObjectSnap(true);
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
                    SetDefaultCPlaneTmpAndCorner1Tmp(viewLine);

                }

            }
        }
        
        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (Iteration)
            {
                case 0: MouseDownIteration0(e); break;
                case 1: MouseDownIteration1(e); break;
                case 2: MouseDownIteration2(e); break;
                case 3: break;
                default:
                    break;
            }

        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            switch (Iteration)
            {
                case 0: DynamicDrawIteration0(e); break;
                case 1: DynamicDrawIteration1(e); break;
                case 2: DynamicDrawIteration2(e); break;
                case 3: DynamicDrawIteration3(e); break;
                default:
                    break;
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
            RegisterCPlane1();
            SetConstructionPlaneForViewport(e.Viewport, cPlane1Tmp);
            ConstrainToConstructionPlane(false);
        }

        private void SetSelectedBrepFace(Surface srf)
        {
            SelectedBrepFace = srf;
        }

        private void RegisterCPlane1()
        {
            CPlane1 = cPlane1Tmp;
        }

        private void MouseDownIteration1(GetPointMouseEventArgs e)
        {
            if (e.ShiftKeyDown)
            {
                if (GetOffsetDistances(out double t1, out double t2))
                {
                    corner1Tmp = GetOffsetPt(e, t1, t2);
                }

            }
            RegisterBaseCorner1();

        }

        private void RegisterBaseCorner1()
        {
            BaseCorner1 = corner1Tmp;
        }

        private void MouseDownIteration2(GetPointMouseEventArgs e)
        {
            if (e.ShiftKeyDown)
            {
                if (GetOffsetDistances(out double t1, out double t2))
                {
                    corner2Tmp = GetOffsetPt(e, t1, t2);
                }

            }
            // Once mouse is down on 2nd iteration, base corner 2 is determined and CPlane2 is determined
            RegisterBaseCorner2();
            // and the Construction plane CPlane2 based on the surface
            RegisterCPlane2();
            SetConstructionPlaneForViewport(e.Viewport, CPlane2);

            SetBasePoint(BaseCorner2, true);
            ConstrainToConstructionPlane(true);
            // Constrain the side to line from BaseCorner2 and vector CPlane1.Axis
            Constrain(new Line(BaseCorner2, CPlane1.ZAxis));

        }


        private void RegisterBaseCorner2()
        {
            BaseCorner2 = corner2Tmp;
        }

        private void RegisterCPlane2()
        {
            CPlane2 = new Plane(BaseCorner2, CPlane1.ZAxis, CPlane1.XAxis);
        }

        #endregion

        #region Iteration Methods for Dynamic Draw
        private void DynamicDrawAllIterations(GetPointDrawEventArgs e)
        {
            DrawViewLine(e.Display);
            DrawCPlane1Tmp(e.Display);
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

            corner1Tmp = CPlane1.ClosestPoint(e.CurrentPoint);
            e.Display.DrawPoint(corner1Tmp, PointStyle.ControlPoint, 3, System.Drawing.Color.DarkRed);
        }

        private void DynamicDrawIteration2(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);

            var corner2 = CPlane1.ClosestPoint(e.CurrentPoint);
            var baseRect = new Rectangle3d(CPlane1, BaseCorner1, corner2);

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
            var boxSideTmp = new Line(CPlane2.Origin, topCornerTmp);
            e.Display.DrawLine(boxSideTmp, System.Drawing.Color.DarkBlue, 5);

            // Draw the box
            Box = new Box(CPlane2, new Point3d[] { BaseCorner1, BaseCorner2, topCornerTmp });
            e.Display.DrawBox(Box, System.Drawing.Color.Purple, 2);
        }

        #endregion

        private bool SetCPlane1Tmp(RhinoViewport viewport, System.Drawing.Point windowPoint, out Surface selectedBrepFace)
        {
            var viewLine = GetViewLine(viewport, windowPoint);
            if (SetGeometricAidsOnBrepFaceForBoxCreation(viewLine, viewport, out Surface brepFaceToPull, out Plane frame, out Guid objId))
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


        private bool GetOffsetDistances(out double dx, out double dy)
        {
            var gp = new GetNumber();
            gp.SetCommandPrompt("Input the relative x to the click pt");
            gp.Get();
            if (gp.CommandResult() != Rhino.Commands.Result.Success)
            {
                dx = default;
                dy = default;
                return false;
            }
            dx = gp.Number();

            var gp2 = new GetNumber();
            gp2.SetCommandPrompt("Input the relative y to the click pt");
            gp2.Get();
            if (gp2.CommandResult() != Rhino.Commands.Result.Success)
            {
                dy = default;
                return false;
            }
            dy = gp2.Number();
            RhinoApp.WriteLine($"Offset distances = {dx}, {dy}");

            return true;
        }
        
        private Point3d GetOffsetPt(GetPointMouseEventArgs e, double t1, double t2)
        {
            var cplane = e.Viewport.ConstructionPlane();
            Transform transform = GetTransform(t1, t2, cplane);
            var pt = e.Point;
            pt.Transform(transform);
            return pt;
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

        private void DrawSelectedBrepFaceTmp(GetPointDrawEventArgs e)
        {
            if (SelectedBrepFace != null)
            {
                e.Display.DrawSurface(SelectedBrepFace, System.Drawing.Color.DarkRed, 3);
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
                                                              out Surface brepFaceToPull,
                                                              out Plane frame,
                                                              out Guid objId)
        {
            return BrepFaceGetter.Get(ActiveDoc, viewLine, viewport, out brepFaceToPull, out frame, out objId);

        }


    }
}
