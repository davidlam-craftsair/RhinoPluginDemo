using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RelevantBrepFaceClosestToViewCursorGetterV4 : IRelevantBrepFaceClosestToViewCursorGetter
    {
        public RelevantBrepFaceClosestToViewCursorGetterV4(double tol)
        {
            Tol = tol;
        }

        public double Tol { get; }

        public bool Get(Brep brep, Line viewLine, out BrepFace brepFace, out Plane frame)
        {
            var viewLineCurve = viewLine.ToNurbsCurve();
            var viewLineStartPt = viewLine.To;
            var intersectInfosOnSrf = new List<(Point3d, Brep)>();

            IEnumerable<Brep> facesAsBrep = brep.Faces.Select(x => x.DuplicateFace(false)).ToArray();
            foreach (var brepFaceTmp in facesAsBrep)
            {
                //brepFaceTmp.ShrinkFace(BrepFace.ShrinkDisableSide.ShrinkAllSides);
                IEnumerable<(Point3d, Brep)> tmp = GetIntersectInfoBtwViewLineAndFaceAsBrep(viewLineCurve, brepFaceTmp, Tol);
                intersectInfosOnSrf.AddRange(tmp);
            }

            if (intersectInfosOnSrf.Any())
            {
                // Get tuple which the shortest distance btw intersect pt on srf and the viewLine start pt
                (Point3d, Brep) ptAndFaceAsBrep = intersectInfosOnSrf.OrderBy(x => Distance(x.Item1, viewLineStartPt)).FirstOrDefault();

                var intersectPtOnSrf = ptAndFaceAsBrep.Item1;
                brepFace = ptAndFaceAsBrep.Item2.Faces[0];

                if (brepFace.ClosestPoint(intersectPtOnSrf, out double u, out double v))
                {
                    if (brepFace.FrameAt(u, v, out frame))
                    {
                        return true;
                    }
                }
            }
            brepFace = default;
            frame = default;
            return false;
        }

        private static IEnumerable<(Point3d, Brep)> GetIntersectInfoBtwViewLineAndFaceAsBrep(Curve viewLineCurve, Brep brepFaceAsBrep, double tol)
        {
            var intersectPtsOnSrf = new List<(Point3d, Brep)>();
            //var t = surface.DuplicateFace(false);
            if (Intersection.CurveBrep(viewLineCurve, brepFaceAsBrep, tol, RhinoDoc.ActiveDoc.ModelAngleToleranceRadians, out double[] ps))
            {
                if (ps.Any())
                {
                    var intersectPt = viewLineCurve.PointAt(ps.First());
                    intersectPtsOnSrf.Add((intersectPt, brepFaceAsBrep));

                }
                else
                {
                    Debug.WriteLine("it has intersection but no intersect pts");
                }

            }
            return intersectPtsOnSrf;
        }

        private static double Distance(Point3d pt1, Point3d pt2)
        {
            return pt1.DistanceToSquared(pt2);
        }
    }
}
