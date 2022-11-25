using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PolylineTrimHandlerV2 : IPolylineTrimHandler
    {
        public PolylineTrimHandlerV2(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }
        public double Tol => RhinoTolerance.Tol;
        public void Trim(Polyline polyline,
                         IPolylineTrimInfo polylineTrimInfo,
                         out IEnumerable<Point3d> polyline1,
                         out IEnumerable<Point3d> polyline2)
        {
            var pts = polyline.ToList();
            var segs = polyline.GetSegments();

            var polyline1tmp = new List<Point3d>();
            var polyline2tmp = new List<Point3d>();

            var seg = segs.ElementAt(polylineTrimInfo.LineSegIndex);
            if (seg.From.EpsilonEquals(polylineTrimInfo.TrimPt1, Tol))  // trim is on the upper part of line
            {
                // Get Pts before
                int ptIndex = polylineTrimInfo.LineSegIndex;
                for (int i = 0; i < pts.Count; i++)
                {
                    if (i == ptIndex)
                    {
                        // Add the intersect pt
                        // Add the trimPt2 to polyline2
                        polyline2tmp.Add(polylineTrimInfo.TrimPt2);
                    }
                    else if (i < ptIndex)
                    {
                        polyline1tmp.Add(pts[i]);
                    }
                    else  // i > ptIndex add to polyline2
                    {
                        polyline2tmp.Add(pts[i]);
                    }

                }
                pts.ElementAt(polylineTrimInfo.LineSegIndex);
            }
            else if (seg.To.EpsilonEquals(polylineTrimInfo.TrimPt2, Tol))  // trim is on the lower part of line
            {
                int ptIndex = polylineTrimInfo.LineSegIndex;
                for (int i = 0; i < pts.Count; i++)
                {
                    if (i == ptIndex)
                    {
                        polyline1tmp.Add(pts[i]);
                        polyline1tmp.Add(polylineTrimInfo.TrimPt2);  // add the intersect pt
                    }
                    else if (i < ptIndex)
                    {
                        polyline1tmp.Add(pts[i]);

                    }
                    else  // i > ptIndex add to polyline2
                    {
                        polyline2tmp.Add(pts[i]);
                    }
                }
            }

            polyline1 = polyline1tmp;
            polyline2 = polyline2tmp;
        }
    }
}
