using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PolylineTrimInfoGetter : IPolylineTrimInfoGetter
    {
        public PolylineTrimInfoGetter(ILineSelectedToTrimGetter lineSelectedToTrimGetter)
        {
            LineSelectedToTrimGetter = lineSelectedToTrimGetter;
        }

        public ILineSelectedToTrimGetter LineSelectedToTrimGetter { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectorPt">Mouse pt</param>
        /// <param name="polyline"></param>
        /// <param name="polylineTrimInfo"></param>
        /// <param name="intersectPt"></param>
        /// <param name="lineSelectedToTrim"></param>
        /// <returns></returns>
        public bool Get(Point3d selectorPt,
                        Polyline polyline,
                        out IPolylineTrimInfoV2 polylineTrimInfo,
                        out Point3d intersectPt,
                        out Line lineSelectedToTrim)
        {
            // Get segments
            var segs = polyline.GetSegments();

            // Get the closest segment relative to pt
            Line lineToBeCut = GetLineToBeCut(selectorPt, segs);
            int lineToBeCutIndex = GetLineToBeCutIndex(segs, lineToBeCut);
            var linePts = new Point3d[2] { lineToBeCut.From, lineToBeCut.To };
            var count = segs.Count();

            for (int lineCutterIdx = 0; lineCutterIdx < count; lineCutterIdx++)
            {
                var lineCutter = segs.ElementAt(lineCutterIdx);
                if (lineToBeCut == lineCutter)
                {
                    continue;
                }
                if (linePts.Contains(lineCutter.From) || linePts.Contains(lineCutter.To))
                {
                    continue;
                }

                if (LineSelectedToTrimGetter.Get(lineToBeCut,
                                                 lineCutter,
                                                 selectorPt,
                                                 out lineSelectedToTrim,
                                                 out intersectPt))
                {
                    polylineTrimInfo = new PolylineTrimInfoV2(lineToBeCutIndex, lineCutterIdx, selectorPt);
                    return true;
                }
            }

            polylineTrimInfo = default;
            intersectPt = default;
            lineSelectedToTrim = default;
            return false;
        }

        private static int GetLineToBeCutIndex(Line[] segs, Line lineToBeCut)
        {
            return segs.ToList().IndexOf(lineToBeCut);
        }

        private Line GetLineToBeCut(Point3d selectorPt, Line[] segs)
        {
            var segsByDistance = segs.OrderBy(x => Distance(x, selectorPt));
            var lineToBeCut = segsByDistance.FirstOrDefault();
            return lineToBeCut;
        }

        private double Distance(Line x, Point3d pt)
        {
            var ptOnLine = x.ClosestPoint(pt, true);
            return ptOnLine.DistanceToSquared(pt);
        }
    }
}
