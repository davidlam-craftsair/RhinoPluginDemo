using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class TrimRcAction: IRhinoAction
    {
        public Result Get()
        {
            if (GetLine("Get Line 1", out Line line1, out Curve curve1, out Guid objectId1))
            {
                if (GetLine("Get Line 2", out Line line2, out Curve curve2, out Guid objectId2))
                {
                    if (Intersection.LineLine(line1, line2, out double a, out double b))
                    {
                        // Get the intersection point
                        var intersectionPt = line1.PointAt(a);
                        // Trim or extend the lines to the intersection pt
                        TrimCurve(curve1, objectId1, intersectionPt);
                        TrimCurve(curve2, objectId2, intersectionPt);
                        return Result.Success;
                    }
                    return Result.Failure;

                }

            }

            return Result.Cancel;
        }

        private void TrimCurve(Curve curve, Guid objectId, Point3d intersectionPt)
        {
            if (IsPtFartherWithCurveStartThanCurveEnd(curve, intersectionPt))
            {
                // Start further than end
                SetIntersectionPtAsEndPtOfCurve(curve, objectId, intersectionPt);

            }
            else
            {
                SetIntersectionPtAsStartPtOfCurve(curve, objectId, intersectionPt);
            }
        }

        private bool SetIntersectionPtAsEndPtOfCurve(Curve curve, Guid objectId, Point3d intersectionPt)
        {
            if (curve.SetEndPoint(intersectionPt))
            {
                return ReplaceObjectWithCurve(objectId, curve);
            }
            return false;
        }

        private bool SetIntersectionPtAsStartPtOfCurve(Curve curve, Guid objectId, Point3d intersectionPt)
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
            return DI.RhinoReplaceManager.Replace(objectId, curve);
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
            return DI.RhinoGetManager.TryGetLine(commandPrompt, out line, out curve, out objectId);
        }

    }
}
