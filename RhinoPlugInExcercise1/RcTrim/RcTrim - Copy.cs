using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("DD911578-09D4-40D7-911A-EE58F4FE7D23")]
    public class RcTrim : AbsCommand
    {
        public RcTrim()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            GetLine("Get Line 1", out Line line1, out Curve curve1, out Guid objectId1);
            GetLine("Get Line 2", out Line line2, out Curve curve2, out Guid objectId2);

            if (Intersection.LineLine(line1, line2, out double a, out double b))
            {
                // Get the intersection point
                var intersectionPt = line1.PointAt(a);
                ProcessCurve(curve1, objectId1, intersectionPt);
                ProcessCurve(curve2, objectId2, intersectionPt);

            }
            return Result.Cancel;

        }

        private void ProcessCurve(Curve curve, Guid objectId, Point3d intersectionPt)
        {
            bool v = IsPtFartherWithCurveStartThanCurveEnd(curve, intersectionPt);
            if (v)  
            {
                // Start further than end
                SetIntersectionPtAsEndPoint(curve, objectId, intersectionPt);

            }
            else
            {
                SetIntersectionPtAsStartPoint(curve, objectId, intersectionPt);
            }
        }

        private bool SetIntersectionPtAsEndPoint(Curve curve, Guid objectId, Point3d intersectionPt)
        {
            if (curve.SetEndPoint(intersectionPt))
            {
                return ReplaceObjectWithCurve(objectId, curve);
            }
            return false;
        }

        private bool SetIntersectionPtAsStartPoint(Curve curve, Guid objectId, Point3d intersectionPt)
        {
            if (curve.SetStartPoint(intersectionPt))
            {
                return ReplaceObjectWithCurve(objectId, curve);
            }
            return false;
        }

        /// <summary>
        /// Replace an object with a curve input
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        private bool ReplaceObjectWithCurve(Guid objectId, Curve curve)
        {
            return ActiveDoc.Objects.Replace(objectId, curve);
        }

        /// <summary>
        /// Returns true if the given pt is farther with curve start than curve end
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        private static bool IsPtFartherWithCurveStartThanCurveEnd(Curve curve, Point3d pt)
        {
            return pt.DistanceToSquared(curve.PointAtStart) > pt.DistanceToSquared(curve.PointAtEnd);
        }

        private bool GetLine(string commandPrompt, out Line line, out Curve curve, out Guid objectId)
        {
            var go = new GetObject();
            go.DisablePreSelect();
            go.SetCommandPrompt(commandPrompt);
            go.SetCustomGeometryFilter(filter);
            go.Get();
            if (go.CommandResult() == Result.Success)
            {
                var objRefTmp = go.Object(0);
                var t = objRefTmp.Curve();
                if (t != null)
                {
                    curve = t;
                    line = GetLine(curve);
                    objectId = objRefTmp.ObjectId;
                    return true;
                }
            }

            line = default;
            curve = default;
            objectId = default;
            return false;
        }

        private Line GetLine(Curve curve)
        {
            RhinoDoc activeDoc = RhinoDoc.ActiveDoc;
            PolylineCurve polylineCurve = curve.ToPolyline(activeDoc.ModelAbsoluteTolerance,
                activeDoc.ModelAngleToleranceRadians,
                activeDoc.ModelAbsoluteTolerance,
                int.MaxValue);
            return polylineCurve.ToPolyline().GetSegments().FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rhObject"></param>
        /// <param name="geometry"></param>
        /// <param name="componentIndex"></param>
        /// <returns></returns>
        private bool filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry.ObjectType == ObjectType.Curve)
            {
                if (geometry is Curve t && t.IsLinear())
                {
                    return true;

                }
            }
            return false;
        }

        public override string EnglishName => "RcTrim";

        public static RcTrim Instance { get; private set; }
    }
}
