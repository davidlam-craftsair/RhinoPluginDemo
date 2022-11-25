using DL.RhinoExcercise1.Core;
using Rhino.Display;
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
    public class GetTrimPolyline : GetPoint
    {
        private PolylineCurve polylineCurve;
        private Polyline polylineResultDraw;

        public Polyline Polyline { get; set; }
        public IRhinoTolerance RhinoTolerance { get; }
        public Polyline PolylineResultDraw { get => polylineResultDraw; set => polylineResultDraw = value; }

        public GetTrimPolyline(PolylineCurve polylineCurve, IRhinoTolerance rhinoTolerance)
        {
            this.polylineCurve = polylineCurve;
            RhinoTolerance = rhinoTolerance;
            Polyline = polylineCurve.ToPolyline();
        }

        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);

        }
        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            GetLines(e, Polyline.Duplicate(), out Polyline polylineResult);
            PolylineResultDraw = polylineResult;
        }

        private void GetLines(GetPointDrawEventArgs e, Polyline polyline, out Polyline polylineResult)
        {
            var pt = e.CurrentPoint;
            // Get segments
            var segs = polyline.GetSegments();
            var polylinePts = polyline.ToList();

            // Get the closest segment relative to pt
            var segsByDistance = segs.OrderBy(x => Distance(x, pt));
            if (segsByDistance.Any())
            {
                var lineToTrim = segsByDistance.FirstOrDefault();
                var linePts = new Point3d[2] { lineToTrim.From, lineToTrim.To };
                var lines = new List<Line>();
                var count = segs.Count();

                for (int i = 0; i < segs.Count(); i++)
                {
                    var item = segs.ElementAt(i);
                    lines.Add(item);
                    if (lineToTrim == item)
                    {
                        continue;
                    }
                    if (linePts.Contains(item.From) || linePts.Contains(item.To))
                    {
                        continue;
                    }


                    if (Intersection.LineLine(lineToTrim, item, out double a, out double b, RhinoTolerance.Tol, true))
                    {
                        if (lines.Any())
                        {
                            lines.RemoveAt(lines.Count - 1);
                        }
                        Point3d intersectPt = lineToTrim.PointAt(a);
                        var c1 = new Line(lineToTrim.From, intersectPt);
                        var c2 = new Line(intersectPt, lineToTrim.To);
                        var ts = new Line[2] { c1, c2 };
                        // Find the closest line to the pt
                        var tsByDistance = ts.OrderBy(x => Distance(x, pt));
                        var lineSelectedToTrim = tsByDistance.First();
                        var lineNotSelectedToTrim = tsByDistance.ElementAt(1);

                        if (c1.EpsilonEquals(lineSelectedToTrim, RhinoTolerance.Tol))
                        {
                            if (i == 0)  // start
                            {
                                polylinePts.RemoveAt(0);
                                polylinePts.Insert(0, lineNotSelectedToTrim.From);
                            }

                        }
                        else if (c2.EpsilonEquals(lineSelectedToTrim, RhinoTolerance.Tol))
                        {
                            if (i == (segs.Count() - 1))
                            {
                                polylinePts.RemoveAt(polyline.Count-1);
                                polylinePts.Add(lineSelectedToTrim.From);
                            }
                        }
                        e.Display.DrawPoint(intersectPt);

                        DrawLineSelectedToTrim(e, lineSelectedToTrim);
                        e.Display.DrawPolyline(polylinePts, System.Drawing.Color.Cyan, 4);
                        VisualizePolyline(e.Display, polyline);
                    }
                }
            }
            polylineResult = new Polyline(polylinePts);
        }

        private void DrawLineSelectedToTrim(GetPointDrawEventArgs e, Line lineSelectedToTrim)
        {
            e.Display.DrawLine(lineSelectedToTrim, System.Drawing.Color.ForestGreen, 1);
        }

        private void VisualizePolyline(DisplayPipeline display, Polyline polyline)
        {
            var segs = polyline.GetSegments();
            for (int i = 0; i < segs.Count(); i++)
            {
                var seg = segs.ElementAt(i);
                var pt = seg.PointAt(0.5);
                display.DrawDot(pt, i.ToString());
            }
        }

        private void DrawTextDot(DisplayPipeline display, IEnumerable<Line> lines, string prefix, double pos=0.5)
        {
            if (lines != null && lines.Any())
            {
                var count = 0;
                foreach (var item in lines)
                {
                    var t = item.PointAt(pos);
                    display.DrawDot(t, $"{prefix}{count}");
                    count += 1;
                }
            }
        }

        private void Dispatch(Line[] segs, int trimIndex, out IEnumerable<Line> before, out IEnumerable<Line> after)
        {
            var t1 = new List<Line>();
            var t2 = new List<Line>();
            for (int i = 0; i < segs.Count(); i++)
            {
                if (i < trimIndex)
                {
                    t1.Add(segs.ElementAt(i));
                }
                else if (i > trimIndex)
                {
                    t2.Add(segs.ElementAt(i));
                }
            }
            before = t1;
            after = t2;

        }


        private double Distance(Line x, Point3d pt)
        {
            var ptOnLine = x.ClosestPoint(pt, true);
            return ptOnLine.DistanceToSquared(pt);
        }
    }
}
