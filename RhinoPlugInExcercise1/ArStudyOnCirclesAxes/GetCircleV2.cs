using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetCircleV2: GetPoint
    {

        public double Radius { get; set; }
        public Point3d Center { get; set; }
        public Plane Plane { get; set; }
        public Curve Curve1 { get; set; }
        public Curve Curve2 { get; set; }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            var tol = e.RhinoDoc.ModelAbsoluteTolerance;
            var movingPt = e.CurrentPoint;

            if (GetClosestPtOnCurve(Curve1, movingPt, out Point3d closestPt1))
            {
                if (GetClosestPtOnCurve(Curve2, movingPt, out Point3d closestPt2))
                {
                    // Find mid pt of the two closestPt
                    var line = new Line(closestPt1, closestPt2);
                    var midPt = line.PointAt(0.5);
                    e.Display.DrawLine(line, System.Drawing.Color.DarkRed);
                    e.Display.DrawPoint(midPt, System.Drawing.Color.DarkRed);

                    // Draw circle for the perspective fillet
                    if (Plane.FitPlaneToPoints(new Point3d[] { movingPt, closestPt1, closestPt2}, out Plane plane1) == PlaneFitResult.Success)
                    {
                        plane1.Origin = midPt;
                        var radius1 = new Point3d[]
                        {
                            closestPt1,
                            closestPt2
                        }.ToLookup(x => GetDistance(x, midPt))
                        .OrderBy(x => x.Key).FirstOrDefault()?.Key??0;
                        if (radius1 > tol)
                        {
                            e.Display.DrawCircle(new Circle(plane1, radius1), System.Drawing.Color.DarkRed);
                        }
                        
                    }
                }
            }
            Radius = GetRadius(movingPt);
            if (Radius < tol)
            {
                return;
            }
            Center = movingPt;
            Plane = e.RhinoDoc.Views.ActiveView.ActiveViewport.ConstructionPlane();
            DrawCircle(e.Display);
        }

        private bool GetClosestPtOnCurve(Curve curve, Point3d movingPt, out Point3d cp)
        {
            if (curve.ClosestPoint(movingPt, out double t))
            {
                cp = curve.PointAt(t);
                return true;
            }
            cp = default;
            return false;
        }

        private double GetRadius(Point3d movingPt)
        {
            Point3d ptOnCircle = FindPtOnCircle(movingPt);

            return GetDistance(movingPt, ptOnCircle);
        }

        private static double GetDistance(Point3d movingPt, Point3d ptOnCircle)
        {
            return ptOnCircle.DistanceTo(movingPt);
        }

        private Point3d FindPtOnCircle(Point3d movingPt)
        {
            var ts = new (Point3d, double)[4]{

                (Curve1.PointAtStart, movingPt.DistanceToSquared(Curve1.PointAtStart)),
                (Curve1.PointAtEnd, movingPt.DistanceToSquared(Curve1.PointAtEnd)),
                (Curve2.PointAtStart, movingPt.DistanceToSquared(Curve2.PointAtStart)),
                (Curve2.PointAtEnd, movingPt.DistanceToSquared(Curve2.PointAtEnd)),
            };

            var ptOnCircle = ts.OrderBy(x => x.Item2).First().Item1;
            return ptOnCircle;
        }

        private void DrawCircle(Rhino.Display.DisplayPipeline display)
        {
            display.DrawCircle(new Circle(Plane, Center, Radius), System.Drawing.Color.DarkRed);
        }
    }
}
