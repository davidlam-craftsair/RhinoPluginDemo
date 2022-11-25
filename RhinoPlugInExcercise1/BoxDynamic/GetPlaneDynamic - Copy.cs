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
        public int Iteration { get; set; }

        public Plane CPlane1 { get; private set; }
        public Plane CPlane2 { get; private set; }
        public Point3d BaseCorner1 { get; private set; }
        public Point3d BaseCorner2 { get; private set; }
        public Box Box { get; private set; }
        public IRelevantObjsGetter RelevantObjsGetter { get; private set; }
        #endregion

        public GetPlaneDynamic(double tol, IRelevantObjsGetter relevantObjsGetter)
        {
            Tol = tol;
            RelevantObjsGetter = relevantObjsGetter;
            Iteration = 0;
        }

        #region Events Methods
        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);

            DrawViewLine(e);
            if (CPlane1.IsValid)
            {
                e.Display.DrawLine(GetLineFromPlaneX(CPlane1, 4), System.Drawing.Color.Red, 5);
                e.Display.DrawLine(GetLineFromPlaneY(CPlane1, 4), System.Drawing.Color.Green, 5);
                e.Display.DrawLine(GetLineFromPlaneZ(CPlane1, 4), System.Drawing.Color.Blue, 5);
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

        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Iteration == 0)
            {
                // Once mouse is down on 1st iteration, base corner 1 is determined
                // On mouse down, found out 
                SetCPlane1TmpAndCorner1Tmp(e.Viewport, e.WindowPoint);

                if (!cPlane1Tmp.IsValid)
                {
                    cPlane1Tmp = Plane.WorldXY;
                    if (Intersection.LinePlane(viewLine, cPlane1Tmp, out double lineParameter))
                    {
                        corner1tmp = viewLine.PointAt(lineParameter);
                    }
                }

                // set set the Corner1 and the Construction plane CPlane1 based on the surface
                CPlane1 = cPlane1Tmp;
                BaseCorner1 = corner1tmp;
                e.Viewport.SetConstructionPlane(CPlane1);
                ConstrainToConstructionPlane(false);
            }
            if (Iteration == 1)
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
            
        }
        
        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Iteration == 0)
            {
                SetCPlane1TmpAndCorner1Tmp(e.Viewport, e.WindowPoint);

            }
        }
        #endregion

        private void SetCPlane1TmpAndCorner1Tmp(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            viewLine = viewport.ClientToWorld(windowPoint);
            Point3d viewLineStartPt = viewLine.To;

            // find out the visible objs
            var relevantObjs = GetRelevantObjs(viewport);
            var closestObj = relevantObjs.OrderBy(x => GetDistance(x, viewLineStartPt))
                                         .FirstOrDefault();

            if (closestObj != null)
            {
                if (closestObj.Geometry is Extrusion extrusion)
                {
                    var brep = extrusion.ToBrep();
                    SetCPlane1AndCorner1TmpForBrep(viewLineStartPt, brep);
                    return;
                }
                else if (closestObj.Geometry is Brep brep2)
                {
                    SetCPlane1AndCorner1TmpForBrep(viewLineStartPt, brep2);
                    return;
                }
                else if (closestObj.Geometry is Surface surface)
                {
                    SetCPlane1AndCorner1TmpForBrep(viewLineStartPt, surface.ToBrep());
                    //GetIntersectionPtsBtwViewLineAndSrf(viewLine.ToNurbsCurve(), surface);
                    //SetCPlane1TmpAndCorner1TmpForSrf(viewLineStartPt, surface);
                    return;
                }
            }

            // if there is no closest obj found, then it means the mouse lies on nothing, then set plane and corner to default
            cPlane1Tmp = default;
            corner1tmp = default;
        }

        private IEnumerable<RhinoObject> GetRelevantObjs(RhinoViewport viewport)
        {
            return RelevantObjsGetter.Get(viewport, viewLine, RhinoDoc.ActiveDoc);
        }
        
        private void DrawViewLine(GetPointDrawEventArgs e)
        {
            if (viewLine.IsValid)
            {
                e.Display.DrawLine(viewLine, System.Drawing.Color.DarkRed, 5);
            }
        }

        private Line GetLineFromPlaneZ(Plane plane, int length)
        {
            Vector3d span = plane.ZAxis * length;
            return CreateLine(plane, span);
        }

        private Line GetLineFromPlaneY(Plane plane, int length)
        {
            Vector3d span = plane.YAxis * length;
            return CreateLine(plane, span);
        }

        private Line GetLineFromPlaneX(Plane plane, double length)
        {
            Vector3d span = plane.XAxis * length;
            return CreateLine(plane, span);
        }

        private static Line CreateLine(Plane plane, Vector3d span)
        {
            return new Line(plane.Origin, span);
        }

        private double GetDistance(RhinoObject x, Point3d viewLineStartPt)
        {
            return Distance(x.Geometry.GetBoundingBox(false).ClosestPoint(viewLineStartPt), viewLineStartPt);
        }

        private void SetCPlane1TmpAndCorner1TmpForSrf(Point3d viewLineStartPt, Surface surface)
        {
            var viewLineCurve = viewLine.ToNurbsCurve();
            var intersectPtsOnSrf = GetIntersectionPtsBtwViewLineAndSrf(viewLineCurve, surface);
            if (GetSelectedFaceFrameAndCorner1OnFace(viewLineStartPt, intersectPtsOnSrf, out Plane frameOnSrf, out Point3d ptOnSrf))
            {
                cPlane1Tmp = frameOnSrf;
                corner1tmp = ptOnSrf;
            }
        }

        private void SetCPlane1AndCorner1TmpForBrep(Point3d viewLineStartPt, Brep brep)
        {
            var viewLineCurve = viewLine.ToNurbsCurve();
            var intersectPtsOnSrf = new List<(Point3d, BrepFace)>();

            foreach (var brepFace in brep.Faces)
            {
                var tmp = GetIntersectionPtsBtwViewLineAndSrf(viewLineCurve, brepFace);
                intersectPtsOnSrf.AddRange(tmp);
            }
            if (GetSelectedFaceFrameAndCorner1OnFace(viewLineStartPt, intersectPtsOnSrf, out Plane frameOnSrf, out Point3d ptOnSrf))
            {
                cPlane1Tmp = frameOnSrf;
                corner1tmp = ptOnSrf;
            }
        }
        private bool GetSelectedFace(Point3d viewLineStartPt,
                                    IEnumerable<(Point3d, BrepFace)> intersectPtsOnSrf,
                                    out BrepFace face,
                                    out Plane frameOnFace
                                    )
        {
            if (!intersectPtsOnSrf.Any())
            {
                face = default;
                frameOnFace = default;
                return false;
            }
            (Point3d, BrepFace) ptAndFace = GetSelectedFaceIntersectInfo(viewLineStartPt, intersectPtsOnSrf);
            face = ptAndFace.Item2;
            return GetFrame(face, ptAndFace.Item1, out frameOnFace);

        }
        private bool GetSelectedFaceFrameAndCorner1OnFace(Point3d viewLineStartPt,
                                             IEnumerable<(Point3d, BrepFace)> intersectPtsOnSrf,
                                             out Plane frame,
                                             out Point3d corner1)
        {
            if (intersectPtsOnSrf.Any())
            {
                (Point3d, Surface) ptAndSrf = GetSelectedFaceIntersectInfo(viewLineStartPt, intersectPtsOnSrf);

                var ptOnSrf = ptAndSrf.Item1;
                var srf = ptAndSrf.Item2;

                if (srf.ClosestPoint(ptOnSrf, out double u, out double v))
                {
                    if (srf.FrameAt(u, v, out frame))
                    {
                        corner1 = frame.Origin;
                        return true;
                    }

                }
            }
            frame = default;
            corner1 = default;
            return false;
        }

        private bool GetFrame(Surface face, Point3d ptOnFace, out Plane frame)
        {
            if (face.ClosestPoint(ptOnFace, out double u, out double v))
            {
                return face.FrameAt(u, v, out frame);
            }
            frame = default;
            return false;

        }

        private (Point3d, BrepFace) GetSelectedFaceIntersectInfo(Point3d viewLineStartPt, IEnumerable<(Point3d, BrepFace)> intersectPtsOnSrf)
        {
            // Get tuple which the shortest distance btw intersect pt on srf and the viewLine start pt
            return intersectPtsOnSrf.OrderBy(x => Distance(x.Item1, viewLineStartPt)).FirstOrDefault();
        }

        private IEnumerable<(Point3d, Surface)> GetIntersectionPtsBtwViewLineAndSrf(NurbsCurve viewLineCurve, Surface surface)
        {
            var intersectPtsOnSrf = new List<(Point3d, Surface)>();

            var curveIntersections = Intersection.CurveSurface(viewLineCurve, surface, Tol, Tol);
            if (curveIntersections.Any())
            {
                Point3d intersectPt = curveIntersections.Where(x => x.IsPoint).Select(x => x.PointA).FirstOrDefault();
                intersectPtsOnSrf.Add((intersectPt, surface));
            }
            return intersectPtsOnSrf;
        }

        private double Distance(Point3d pt1, Point3d pt2)
        {
            return pt1.DistanceToSquared(pt2);
        }


    }
}
