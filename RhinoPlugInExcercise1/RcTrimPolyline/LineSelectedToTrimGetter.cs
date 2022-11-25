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
    public class LineSelectedToTrimGetter : ILineSelectedToTrimGetter
    {
        public LineSelectedToTrimGetter(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }

        public bool Get(Line lineToBeCut,
                        Line lineCutter,
                        Point3d selectorPt,
                        out Line lineSelectedToTrim,
                        out Point3d intersectPt)
        {
            if (Intersection.LineLine(lineToBeCut,
                                      lineCutter,
                                      out double a,
                                      out double b,
                                      RhinoTolerance.Tol,
                                      true))
            {
                intersectPt = lineToBeCut.PointAt(a);
                var c1 = new Line(lineToBeCut.From, intersectPt);
                var c2 = new Line(intersectPt, lineToBeCut.To);
                var ts = new Line[2] { c1, c2 };
                // Find the closest line to the pt
                lineSelectedToTrim = GetLineSelectedToTrim(selectorPt, ts);
                return true;

            }
            lineSelectedToTrim = default;
            intersectPt = default;
            return false;
        }

        private Line GetLineSelectedToTrim(Point3d selectorPt, Line[] ts)
        {
            Line lineSelectedToTrim;
            var tsByDistance = ts.OrderBy(x => Distance(x, selectorPt));
            lineSelectedToTrim = tsByDistance.First();
            return lineSelectedToTrim;
        }

        private double Distance(Line x, Point3d pt)
        {
            var ptOnLine = x.ClosestPoint(pt, true);
            return ptOnLine.DistanceToSquared(pt);
        }
    }
}
