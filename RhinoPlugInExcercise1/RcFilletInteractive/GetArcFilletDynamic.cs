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
    /// <summary>
    /// Get the fillet arc dynamically
    /// </summary>
    public class GetArcFilletDynamic : GetPoint
    {
        private Arc arc;
        private Point3d intersectionPt;

        public GetArcFilletDynamic():base()
        {
            ConstrainToConstructionPlane(true);
        }
        #region Properties
        public Line Line1 { get; set; }
        public Line Line2 { get; set; }

        #endregion

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            var movingPt = e.CurrentPoint;

            NurbsCurve curve1 = Line1.ToNurbsCurve();
            if (GetClosestPtOnCurve(curve1, movingPt, out Point3d closestPt1))
            {
                NurbsCurve curve2 = Line2.ToNurbsCurve();
                if (GetClosestPtOnCurve(curve2, movingPt, out Point3d closestPt2))
                {
                    var angle = GetAngle(Line1, Line2);
                    // Find the intersection of the two lines
                    if(GetIntersectionPt(Line1, Line2, out intersectionPt))
                    {
                        // Draw the intersection pt
                        e.Display.DrawPoint(intersectionPt, System.Drawing.Color.DarkRed);
                        var radius = GetRadius(closestPt1, intersectionPt, angle);

                        // Create fillet
                        arc = Curve.CreateFillet(curve1,
                                                curve2,
                                                radius,
                                                Line1.ClosestParameter(movingPt),
                                                Line2.ClosestParameter(movingPt));
                        if (arc != null && arc.IsValid)
                        {
                            e.Display.DrawArc(arc, System.Drawing.Color.DarkRed);
                            // Draw the arc center 
                            e.Display.DrawPoint(arc.Center, System.Drawing.Color.DarkRed);
                        }
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

        private static double GetDistance(Point3d pt1, Point3d pt2)
        {
            return pt2.DistanceTo(pt1);
        }

        public bool GetResultGeometries(out Line line1, out Line line2, out Arc filletArc)
        {
            filletArc = arc;
            line1 = GetLineToFitFilletArcV2(arc, Line1);
            line2 = GetLineToFitFilletArcV2(arc, Line2);
            return true;
        }


        /// <summary>
        /// Find out the line that its start pt is arc point 
        /// and end pt is the pt of line to be determined by vector direction
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private Line GetLineToFitFilletArcV2(Arc arc, Line line)
        {
            // from the pt on line where the fillet intersect with line from arc center
            var ptOnLine = line.ClosestPoint(arc.Center, false);
            // Vector from intersection Pt to ptOnLine
            var r = ptOnLine - intersectionPt;
            r.Unitize();  // Unitized before comparing direction

            // Vector from ptOnLine to line start pt
            var t1 = line.From - ptOnLine;
            t1.Unitize();

            // Compare the two unit vector if equals (same in direction)
            if (IsParallel(r, t1))  
            {
                // if intersection pt -> ptOnLine == ptOnLine -> line start pt
                // then it is the line we need from ptOnLine to start pt of the line    
                return new Line(ptOnLine, line.From);
            }
            else
            {
                return new Line(ptOnLine, line.To);
            }
        }

        private static bool IsParallel(Vector3d r, Vector3d t1)
        {
            return t1.EpsilonEquals(r, RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
        }
    }
}
