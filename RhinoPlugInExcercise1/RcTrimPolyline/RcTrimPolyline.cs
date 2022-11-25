using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("1131C571-38E5-40CD-AD18-300FF9E690BB")]
    public class RcTrimPolyline : AbsCommand
    {
        public RcTrimPolyline():base()
        {
            PolylineTrimHandler = new PolylineTrimHandlerV3(Get<IRhinoTolerance>(), Get<ILineSelectedToTrimGetter>());
        }
        protected override Result DoRunCommand()
        {
            var go = new GetObject();
            go.SetCommandPrompt("Get a polyline to trim");
            go.SetCustomGeometryFilter(FilterPolyline);
            go.GetMultiple(0, 1);
            if (go.CommandResult() != Result.Success)
            {
                return go.CommandResult();
            }
            ObjRef objRef = go.Object(0);
            if (objRef.Geometry() is PolylineCurve polylineCurve)
            {
                Polyline polyline = polylineCurve.ToPolyline();
                var t1 = polyline.GetSegments();
                var t2 = polyline.Duplicate().GetSegments();
                var gp = new GetTrimPolylineV3(polyline,
                                               Get<IRhinoTolerance>(),
                                               Get<ILineSelectedToTrimGetter>(),
                                               Get<IPolylineTrimInfoGetter>()
                                              );
                gp.Constrain(polylineCurve, false);
                gp.SetCommandPrompt("Select part to trim");
                var getResult = gp.Get(true);

                PolylineTrimHandler.Trim(polyline, gp.PolylineTrimInfo, out IEnumerable<Point3d> polyline1, out IEnumerable<Point3d> polyline2);
                if (polyline1.Any())
                {
                    ActiveDoc.Objects.AddPolyline(polyline1);
                }
                if (polyline2.Any())
                {
                    ActiveDoc.Objects.AddPolyline(polyline2);
                }
                ActiveDoc.Objects.Delete(objRef, false);
            }
            
            return Result.Success;
        }

        private bool FilterPolyline(RhinoObject rhinoObj, GeometryBase geometryBase, ComponentIndex componentIndex)
        {
            if (geometryBase is PolylineCurve)
            {
                return true;
            }
            return false;
        }

        public override string EnglishName => "TrimPolyline";

        public IPolylineTrimHandlerV2 PolylineTrimHandler { get; }
    }
}
