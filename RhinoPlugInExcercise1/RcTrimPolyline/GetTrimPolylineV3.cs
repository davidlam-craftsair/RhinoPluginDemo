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
    public class GetTrimPolylineV3 : GetPoint
    {
        public Polyline Polyline { get; set; }
        public IRhinoTolerance RhinoTolerance { get; }
        public ILineSelectedToTrimGetter LineSelectedToTrimGetter { get; }
        public IPolylineTrimInfoGetter PolylineTrimInfoGetter { get; }
        public IPolylineTrimInfoV2 PolylineTrimInfo { get; private set; }

        public GetTrimPolylineV3(Polyline polyline,
                                 IRhinoTolerance rhinoTolerance,
                                 ILineSelectedToTrimGetter lineSelectedToTrimGetter,
                                 IPolylineTrimInfoGetter polylineTrimInfoGetter)
        {
            RhinoTolerance = rhinoTolerance;
            LineSelectedToTrimGetter = lineSelectedToTrimGetter;
            PolylineTrimInfoGetter = polylineTrimInfoGetter;
            Polyline = polyline;
        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            GetLines(e, Polyline.Duplicate());
        }

        private void GetLines(GetPointDrawEventArgs e, Polyline polyline)
        {
            if (PolylineTrimInfoGetter.Get(e.CurrentPoint,
                                           polyline,
                                           out IPolylineTrimInfoV2 polylineTrimInfo,
                                           out Point3d intersectPt,
                                           out Line lineSelectedToTrim))
            {
                PolylineTrimInfo = polylineTrimInfo;

                e.Display.DrawPoint(intersectPt);
                DrawLineSelectedToTrim(e, lineSelectedToTrim);
                VisualizePolyline(e.Display, polyline);
            }
        }

        private void DrawLineSelectedToTrim(GetPointDrawEventArgs e, Line lineSelectedToTrim)
        {
            e.Display.DrawLine(lineSelectedToTrim, System.Drawing.Color.ForestGreen, 4);
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

        private void DrawTextDot(DisplayPipeline display, IEnumerable<Line> lines, string prefix="", double pos=0.5)
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
    }
}
