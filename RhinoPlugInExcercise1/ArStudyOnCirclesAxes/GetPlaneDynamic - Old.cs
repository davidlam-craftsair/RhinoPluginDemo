using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetPlaneDynamic: GetPoint
    {
        private Line line;

        public Point3d Center { get; set; }
        public Plane Plane { get; set; }
        public Curve Curve1 { get; set; }
        public Curve Curve2 { get; set; }
        public double Tol { get; }

        public GetPlaneDynamic(double tol)
        {
            Tol = tol;
        }
        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            e.Display.DrawPoint(Center, PointStyle.Triangle, 10, System.Drawing.Color.DarkRed);
            if (line.IsValid)
            {
                e.Display.DrawLine(line, System.Drawing.Color.DarkRed, 5);
            }
            if (Plane.IsValid)
            {
                e.Display.DrawLine(GetLineFromPlaneX(Plane, 4), System.Drawing.Color.Red, 5);
                e.Display.DrawLine(GetLineFromPlaneY(Plane, 4), System.Drawing.Color.Green, 5);
                e.Display.DrawLine(GetLineFromPlaneZ(Plane, 4), System.Drawing.Color.Blue, 5);

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

        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            Rhino.Display.RhinoViewport viewport = e.Viewport;
            viewport.GetFrustumNearPlane(out Plane nearPlane);
            viewport.GetFrustumCenter(out Point3d center);
            line = viewport.ClientToWorld(e.WindowPoint);


            Point3d from = line.To;
            //var frustumBox = viewport.GetFrustumBoundingBox();
            // find out the visible objs
            var viewportVisibleObjs = RhinoDoc.ActiveDoc.Objects.Where(x => viewport.IsVisible(x.Geometry.GetBoundingBox(false)));
            //var possibleObjs = RhinoDoc.ActiveDoc.Objects.Where(x => TestForVisibility(x, frustumBox).IsValid);

            var tmps = viewportVisibleObjs.Where(x => Intersection.LineBox(line, x.Geometry.GetBoundingBox(false), Tol, out Interval lineParameters));


            var closestObj = tmps.OrderBy(x => Distance(x.Geometry.GetBoundingBox(false).ClosestPoint(from), from)).FirstOrDefault();
            //nearPlane.Origin = center;
            if (closestObj != null)
            {
                if (closestObj.Geometry is Extrusion t)
                {
                    var brep = t.ToBrep();
                    var lineCurve = line.ToNurbsCurve();
                    var pttmps = new List<(Point3d, BrepFace)>();
                    foreach (var item in brep.Faces)
                    {
                        var curveIntersections = Intersection.CurveSurface(lineCurve, item, Tol, Tol);
                        if (curveIntersections.Any())
                        {
                            Point3d pttmp = curveIntersections.Where(x => x.IsPoint).Select(x => x.PointA).FirstOrDefault();
                            pttmps.Add((pttmp, item));
                        }
                        
                    }

                    var ptAndBrepFace = pttmps.OrderBy(x => Distance(x.Item1, from)).FirstOrDefault();
                    var brepFace = ptAndBrepFace.Item2;
                    var ptOnBrepFace = ptAndBrepFace.Item1;
                    if (brepFace.ClosestPoint(ptOnBrepFace, out double u, out double v))
                    {
                        if (brepFace.FrameAt(u, v, out Plane frame))
                        {
                            Plane = frame;
                            Center = frame.Origin;
                        }

                    }
                    //if (Intersection.CurveBrep(line.ToNurbsCurve(), brep, Tol, out Curve[] intersectCurves, out Point3d[] intersectPts))
                    //{
                    //    Center = intersectPts.FirstOrDefault();
                        
                    //}

                    //if (t.ClosestPoint(from, out double u, out double v))
                    //{
                    //    if (t.Evaluate(u, v, 0, out Point3d ptOnSrf, out Vector3d[] derivatives))
                    //    {
                    //        Center = ptOnSrf;
                    //    }

                    //}
                }
                //RhinoApp.WriteLine(componentIndex.ToString());

            }
            //System.Drawing.Point windowPt = e.WindowPoint;
            //RhinoApp.WriteLine($"windowPt = {windowPt}");
            //RhinoApp.WriteLine($"mouse point3d = {e.Point}");
        }

        private static BoundingBox TestForVisibility(RhinoObject x, BoundingBox frustumBox)
        {
            return BoundingBox.Intersection(x.Geometry.GetBoundingBox(false), frustumBox);
        }

        private double Distance(Point3d point3d, Point3d cameraLocation)
        {
            return point3d.DistanceToSquared(cameraLocation);
        }

        private double CalcuateDist(Point3d x, Point3d movingPt)
        {
            return x.DistanceToSquared(movingPt);
        }

    }
}
