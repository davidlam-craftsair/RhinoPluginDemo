using Eto.Drawing;
using Rhino;
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
        private Point3d corner1tmp;
        private Point3d corner2Tmp;
        private Rectangle3d baseRect;
        private Point3d topCornerTmp;
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
        public Surface SelectedBrepFaceTmp { get; private set; }
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
                if (e.ShiftKeyDown)
                {
                    return;
                }
                SetCPlane1Tmp(e.Viewport, e.WindowPoint);

            }
        }
        
        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Iteration == 0)
            {
                ProcessIteration0ForMouseDown(e);
            }
            if (Iteration == 1)
            {
                ProcessIteration1ForMouseDown(e);
            }
            if (Iteration == 2)
            {

            }

        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);

            DrawViewLine(e);
            DrawPlane(e.Display, cPlane1Tmp);
            if (Iteration == 0)
            {
                DrawSelectedBrepFaceTmp(e);
            }
            if (Iteration == 1)
            {
                corner2Tmp = CPlane1.ClosestPoint(e.CurrentPoint);
                baseRect = new Rectangle3d(CPlane1, BaseCorner1, corner2Tmp);

                e.Display.DrawPolyline(baseRect.ToPolyline(), System.Drawing.Color.DarkRed);
            }
            if (Iteration == 2)
            {
                // Draw the base rectangle with already determined base corners and plane
                baseRect = new Rectangle3d(CPlane1, BaseCorner1, BaseCorner2);
                e.Display.DrawPolyline(baseRect.ToPolyline(), System.Drawing.Color.DarkRed);

                // Draw the box side with the mouse pt as top corner of the box
                topCornerTmp = CPlane2.ClosestPoint(e.CurrentPoint);
                var boxSideTmp = new Line(CPlane2.Origin, topCornerTmp);
                e.Display.DrawLine(boxSideTmp, System.Drawing.Color.DarkBlue, 5);

                // Draw the box
                Box = new Box(CPlane2, new Point3d[] { BaseCorner1, BaseCorner2, topCornerTmp });
                e.Display.DrawBox(Box, System.Drawing.Color.Purple, 2);
            }

        }

        #endregion
        private void ProcessIteration0ForMouseDown(GetPointMouseEventArgs e)
        {
            // Once mouse is down on 1st iteration, base corner 1 is determined
            // On mouse down, found out 
            if (e.ShiftKeyDown)
            {
                // no need to set cplane1Tmp and corner1Tmp
            }
            else
            {
                SetCPlane1Tmp(e.Viewport, e.WindowPoint);

            }

            // set set the Corner1 and the Construction plane CPlane1 based on the surface
            CPlane1 = cPlane1Tmp;
            //BaseCorner1 = corner1tmp;
            SetConstructionPlaneForViewport(e.Viewport, cPlane1Tmp);
            ConstrainToConstructionPlane(false);
        }

        private void ProcessIteration1ForMouseDown(GetPointMouseEventArgs e)
        {
            // Once mouse is down on 2nd iteration, base corner 2 is determined and CPlane2 is determined
            BaseCorner2 = corner2Tmp;
            CPlane2 = new Plane(BaseCorner2, CPlane1.ZAxis, CPlane1.XAxis);
            // and the Construction plane CPlane2 based on the surface
            e.Viewport.SetConstructionPlane(CPlane2);
            // and the Construction plane CPlane2 based on the surface
            SetBasePoint(BaseCorner2, true);
            ConstrainToConstructionPlane(true);
            // Constrain the side to line from BaseCorner2 and vector CPlane1.Axis
            Constrain(new Line(BaseCorner2, CPlane1.ZAxis));

        }

        private void SetConstructionPlaneForViewport(RhinoViewport viewport, Plane cplane)
        {
            viewport.SetConstructionPlane(cplane);
        }

        private void DrawPlane(DisplayPipeline display, Plane plane)
        {
            PlaneDrawer.Draw(display, plane);
        }

        private void DrawSelectedBrepFaceTmp(GetPointDrawEventArgs e)
        {
            if (SelectedBrepFaceTmp != null)
            {
                e.Display.DrawSurface(SelectedBrepFaceTmp, System.Drawing.Color.DarkRed, 3);
            }
        }

        private bool SetCPlane1Tmp(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            var viewLine = GetViewLine(viewport, windowPoint);
            if (SetGeometricAidsOnBrepFaceForBoxCreation(viewLine, viewport, out Surface brepFaceToPull, out Plane frame, out Guid objId))
            {
                cPlane1Tmp = frame;
                SetConstructionPlaneForViewport(viewport, cPlane1Tmp);
                //var snapPts = GetSnapPoints();
                //if (snapPts.Any())
                //{
                //    corner1tmp = snapPts.First();
                //}   
                //else
                //{
                //    corner1tmp = frame.Origin;
                //}
                SelectedBrepFaceTmp = brepFaceToPull;
                return true;
            }
            SetDefaultCPlaneTmpAndCorner1Tmp(viewLine);
            return false;
        }

        private void SetDefaultCPlaneTmpAndCorner1Tmp(Line viewLine)
        {
            if (!cPlane1Tmp.IsValid)
            {
                cPlane1Tmp = Plane.WorldXY;
                if (Intersection.LinePlane(viewLine, cPlane1Tmp, out double lineParameter))
                {
                    corner1tmp = viewLine.PointAt(lineParameter);
                }
            }
        }

        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }


        private void DrawViewLine(GetPointDrawEventArgs e)
        {
            if (viewLine.IsValid)
            {
                e.Display.DrawLine(viewLine, System.Drawing.Color.DarkRed, 5);
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
