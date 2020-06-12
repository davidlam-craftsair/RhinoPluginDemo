using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetCircleV3: GetPoint
    {

        public double Radius { get; set; }
        public Point3d Center { get; set; }
        public Plane Plane { get; set; }
        public Line Line1 { get; set; }
        public Line Line2 { get; set; }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            var tol = e.RhinoDoc.ModelAbsoluteTolerance;
            var movingPt = e.CurrentPoint;

            NurbsCurve curve1 = Line1.ToNurbsCurve();
            if (GetClosestPtOnCurve(curve1, movingPt, out Point3d closestPt1))
            {
                NurbsCurve curve2 = Line2.ToNurbsCurve();
                if (GetClosestPtOnCurve(curve2, movingPt, out Point3d closestPt2))
                {
                    var angle = GetAngle(Line1, Line2);
                    if(GetIntersectionPt(Line1, Line2, out Point3d intersectionPt))
                    {
                        var radius1 = GetRadius(closestPt1, intersectionPt, angle);
                        Arc arc = Curve.CreateFillet(curve1,
                                           curve2,
                                           radius1,
                                           Line1.ClosestParameter(movingPt),
                                           Line2.ClosestParameter(movingPt));
                        e.Display.DrawArc(arc, System.Drawing.Color.DarkRed);

                        // Output the results
                        FilletArc = arc;
                        NewLine1 = new Line(Line1.PointAt(Line1.ClosestParameter(closestPt1)), Line1.To);
                    }

                }
            }
        }

        private double GetAngle(Line line1, Line line2)
        {
            var vector1 = line1.From - line1.To;
            var vector2 = line2.From - line2.To;
            return Vector3d.VectorAngle(vector1, vector2);
        }

        private bool GetIntersectionPt(Line line1, Line line2, out Point3d intersectionPt)
        {
            if (Intersection.LineLine(line1, line2, out double a, out double b))
            {
                intersectionPt = line1.PointAt(a);
                return true;
            }
            intersectionPt = default;
            return false;
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

        private double GetRadius(Point3d closestPt, Point3d intersectionPt, double angle)
        {
            var d = GetDistance(intersectionPt, closestPt);
            var a = angle / 2;
            var r = d * Math.Tan(a);
            return r;
        }

        private static double GetDistance(Point3d movingPt, Point3d ptOnCircle)
        {
            return ptOnCircle.DistanceTo(movingPt);
        }


        private void DrawCircle(Rhino.Display.DisplayPipeline display)
        {
            display.DrawCircle(new Circle(Plane, Center, Radius), System.Drawing.Color.DarkRed);
        }
    }
}
