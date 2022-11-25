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
    public class PolylineTrimHandlerV3 : IPolylineTrimHandlerV2
    {
        public PolylineTrimHandlerV3(IRhinoTolerance rhinoTolerance,
            ILineSelectedToTrimGetter lineSelectedToTrimGetter
            )
        {
            RhinoTolerance = rhinoTolerance;
            LineSelectedToTrimGetter = lineSelectedToTrimGetter;
        }

        public IRhinoTolerance RhinoTolerance { get; }
        public ILineSelectedToTrimGetter LineSelectedToTrimGetter { get; }

        public double Tol => RhinoTolerance.Tol;
        public void Trim(Polyline polyline,
                         IPolylineTrimInfoV2 polylineTrimInfo,
                         out IEnumerable<Point3d> polyline1,
                         out IEnumerable<Point3d> polyline2)
        {
            if (polylineTrimInfo is null)
            {
                polyline1 = new List<Point3d>();
                polyline2 = new List<Point3d>();
                return;
            }

            var pts = polyline.ToList();
            var segs = polyline.GetSegments().ToList();

            var polyline1tmp = new List<Point3d>();
            var polyline2tmp = new List<Point3d>();

            var lineCutter = segs[polylineTrimInfo.LineCutterIndex];
            var lineToBeCut = segs[polylineTrimInfo.LineToBeCutIndex];

            if (LineSelectedToTrimGetter.Get(lineToBeCut,
                                             lineCutter,
                                             polylineTrimInfo.Selector,
                                             out Line lineSelectedToTrim,
                                             out Point3d intersectPt))
            {
                if (lineToBeCut.From.EpsilonEquals(lineSelectedToTrim.From, Tol))  // trim is on the upper part of line
                {
                    // Get Pts before
                    int ptIndex = polylineTrimInfo.LineToBeCutIndex;
                    for (int i = 0; i < pts.Count; i++)
                    {
                        if (i == ptIndex)
                        {
                            // Add the intersect pt
                            // Add the trimPt2 to polyline2 
                            polyline2tmp.Add(lineSelectedToTrim.To);
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
                else if (lineToBeCut.To.EpsilonEquals(lineSelectedToTrim.To, Tol))  // trim is on the lower part of line
                {
                    int ptIndex = polylineTrimInfo.LineToBeCutIndex;
                    for (int i = 0; i < pts.Count; i++)
                    {
                        if (i == ptIndex)
                        {
                            polyline1tmp.Add(pts[i]);
                            polyline1tmp.Add(lineSelectedToTrim.From);  // add the intersect pt
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

            }
            polyline1 = polyline1tmp;
            polyline2 = polyline2tmp;

            
        }
    }
}
