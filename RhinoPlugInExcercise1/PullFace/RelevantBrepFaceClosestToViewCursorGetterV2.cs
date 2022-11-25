﻿using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RelevantBrepFaceClosestToViewCursorGetterV2 : IRelevantBrepFaceClosestToViewCursorGetter
    {
        public RelevantBrepFaceClosestToViewCursorGetterV2(double tol)
        {
            Tol = tol;
        }

        public double Tol { get; }

        public bool Get(Brep brep, Line viewLine, out BrepFace brepFace, out Plane frame)
        {
            var viewLineCurve = viewLine.ToNurbsCurve();
            var viewLineStartPt = viewLine.To;
            var intersectInfosOnSrf = new List<(Point3d, BrepFace)>();

            Rhino.Geometry.Collections.BrepFaceList faces = brep.Faces;
            faces.ShrinkFaces();
            foreach (var brepFaceTmp in faces)
            {
                //brepFaceTmp.ShrinkFace(BrepFace.ShrinkDisableSide.ShrinkAllSides);
                IEnumerable<(Point3d, BrepFace)> tmp = GetIntersectInfoBtwViewLineAndSrf(viewLineCurve, brepFaceTmp, Tol);
                intersectInfosOnSrf.AddRange(tmp);
            }

            if (intersectInfosOnSrf.Any())
            {
                // Get tuple which the shortest distance btw intersect pt on srf and the viewLine start pt
                (Point3d, BrepFace) ptAndSrf = intersectInfosOnSrf.OrderBy(x => Distance(x.Item1, viewLineStartPt)).FirstOrDefault();

                var intersectPtOnSrf = ptAndSrf.Item1;
                brepFace = ptAndSrf.Item2;

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

        private static IEnumerable<(Point3d, BrepFace)> GetIntersectInfoBtwViewLineAndSrf(Curve viewLineCurve, BrepFace brepFace, double tol)
        {
            var intersectPtsOnSrf = new List<(Point3d, BrepFace)>();
            //var t = surface.DuplicateFace(false);
            if (Intersection.CurveBrepFace(viewLineCurve, brepFace, tol, out Curve[] overlapCurves, out Point3d[] intersectPts))
            {
                Point3d intersectPt = intersectPts.ElementAtOrDefault(0);
                intersectPtsOnSrf.Add((intersectPt, brepFace));
                
            }
            return intersectPtsOnSrf;
        }

        private static double Distance(Point3d pt1, Point3d pt2)
        {
            return pt1.DistanceToSquared(pt2);
        }
    }
}
