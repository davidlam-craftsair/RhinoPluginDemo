using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PolylineTrimHandler
    {
        public PolylineTrimHandler(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }

        public Polyline Trim(Polyline polyline, Point3d pt)
        {
            // Get segments
            var segs = polyline.GetSegments();

            // Get the closest segment relative to pt
            var segsByDistance = segs.OrderBy(x => Distance(x, pt));
            if (segsByDistance.Any())
            {
                var lineToTrim = segsByDistance.FirstOrDefault();
                var pts = new Point3d[2] { lineToTrim.From, lineToTrim.To };
                var allLines = new List<Line>();
                foreach (var item in segs)
                {
                    if (lineToTrim == item)
                    {
                        continue;
                    }
                    if (pts.Contains(item.From) || pts.Contains(item.To))
                    {
                        continue;
                    }
                    if (Intersection.LineLine(lineToTrim, item, out double a, out double b, RhinoTolerance.Tol, true))
                    {
                        Point3d intersectPt = lineToTrim.PointAt(a);
                        var c1 = new Line(lineToTrim.From, intersectPt);
                        var c2 = new Line(intersectPt, lineToTrim.To);
                        allLines.Add(c1);
                        allLines.Add(c2);
                        //var curve = lineToTrim.ToNurbsCurve();
                        //curve.Domain = new Interval(0, 1);
                        //curve.NormalizedLengthParameter(a, out double normalizedParameter);
                        //var c1 = curve.Trim((double)0, normalizedParameter);
                        //var c2 = curve.Trim(normalizedParameter, 1);

                    }
                }
            }

            return polyline;
        }

        private double Distance(Line x, Point3d pt)
        {
            var ptOnLine = x.ClosestPoint(pt, true);
            return ptOnLine.DistanceToSquared(pt);
        }
    }
}
