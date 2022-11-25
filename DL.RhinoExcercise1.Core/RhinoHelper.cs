using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public static class RhinoHelper
    {
        public static bool GetCurve(string promptMessage,
            GetObjectGeometryFilter filter,
            out Curve curve, out Guid objectId)
        {
            var go = new GetObject();
            go.DisablePreSelect();
            go.SetCommandPrompt(promptMessage);
            go.SetCustomGeometryFilter(filter);
            go.Get();
            if (go.CommandResult() == Result.Success)
            {
                var objRefTmp = go.Object(0);
                var t = objRefTmp.Curve();
                if (t != null)
                {
                    curve = t;
                    objectId = objRefTmp.ObjectId;
                    return true;
                }
            }

            curve = default;
            objectId = default;
            return false;
        }

        public static bool TryGetPolylineFromCurve(RhinoDoc activeDoc, Curve curve, out Polyline polyline)
        {
            PolylineCurve polylineCurve = curve.ToPolyline(activeDoc.ModelAbsoluteTolerance,
                                                            activeDoc.ModelAngleToleranceRadians,
                                                            activeDoc.ModelAbsoluteTolerance,
                                                            int.MaxValue);
            polyline = polylineCurve.ToPolyline();
            return true;
        }

        public static bool TryGetPolylineFromCurveV2(Curve curve, out Polyline polyline)
        {
            if (curve.TryGetPolyline(out polyline))
            {
                
                return true;
            }
            polyline = default;
            return true;
        }

        public static bool CurveBrepIntersection(Curve curve, Brep brep, double tolerance, out Curve[] overlapCurves, out Point3d[] intersectionPoints)
        {
            if (Intersection.CurveBrep(curve, brep, tolerance, out overlapCurves, out intersectionPoints))
            {
                return intersectionPoints.Any();
            }
            return false;
        }

        public static void SetActiveViewConstructionPlane(this RhinoDoc activeDoc, Plane plane)
        {
            activeDoc.Views.ActiveView.ActiveViewport.SetConstructionPlane(plane);
        }
    }
}
