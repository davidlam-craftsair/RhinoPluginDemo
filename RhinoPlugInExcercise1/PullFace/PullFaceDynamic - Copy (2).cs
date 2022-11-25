using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceDynamic : GetPoint
    {
        private Line viewLine;
        private Surface brepSrfToPullTmp;
        private Guid brepObjIdTmp;
        private Point3d ptOnSrf;
        public Brep extrusionDynamic;
        private Vector3d extrusionVector;
        public Extrusion extrusionWhole;
        private List<Curve> dynamicOnProfilePlane;
        private Curve profileNew;
        private Curve profileOriginalDisplay;
        public Brep brepWhole;

        public PullFaceDynamic(IRelevantObjsGetter relevantObjsGetter,
                               double tol,
                               IDistanceBtwObjAndViewGetter distanceBtwObjAndViewGetter,
                               IRelevantBrepFaceClosestToViewGetter relevantBrepFaceClosestToViewGetter)
        {
            RelevantObjsGetter = relevantObjsGetter;
            Tol = tol;
            DistanceBtwObjAndViewGetter = distanceBtwObjAndViewGetter;
            RelevantBrepFaceClosestToViewGetter = relevantBrepFaceClosestToViewGetter;
        }

        public IRelevantObjsGetter RelevantObjsGetter { get; }
        public double Tol { get; }
        public IDistanceBtwObjAndViewGetter DistanceBtwObjAndViewGetter { get; }
        public IRelevantBrepFaceClosestToViewGetter RelevantBrepFaceClosestToViewGetter { get; }
        public Surface BrepSrfToPull { get; private set; }
        public Guid ObjId { get; private set; }
        public int Iteration { get; internal set; }
        public Plane CPlane { get; private set; }

        #region Events Methods

        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Iteration == 0)
            {
                SetBrepSrfToPullTmp(e.Viewport, e.WindowPoint);
            }

        }

        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Iteration == 0)
            {
                SetBrepSrfToPullTmp(e.Viewport, e.WindowPoint);
                BrepSrfToPull = brepSrfToPullTmp;
                if (BrepSrfToPull != null)
                {
                    var domainU = BrepSrfToPull.Domain(0);
                    var domainV = BrepSrfToPull.Domain(1);
                    var u = (domainU[1] - domainU[0]) / 2;
                    var v = (domainV[1] - domainV[0]) / 2;
                    if (BrepSrfToPull.FrameAt(u, v, out Plane frame))
                    {
                        e.Viewport.SetConstructionPlane(frame);
                        Constrain(new Line(frame.Origin, frame.ZAxis));
                    }
                }
            }
            if (Iteration == 1)
            {
                // Put the extrusionDynamic to obj
                if (extrusionDynamic != null)
                {
                    Rhino.DocObjects.Tables.ObjectTable objectTable = RhinoDoc.ActiveDoc.Objects;
                    var obj = objectTable.Find(ObjId);

                    if (obj.Geometry is Extrusion originalExtrusion)
                    {
                        if (originalExtrusion.ProfileCount == 1)
                        {
                            int parallelInfo = extrusionVector.IsParallelTo(originalExtrusion.PathTangent, Tol);
                            if (parallelInfo == 1)
                            {
                                var plane = originalExtrusion.GetProfilePlane(0);
                                var profileOriginal = originalExtrusion.Profile3d(0, 0);
                                profileOriginalDisplay = profileOriginal;
                                Case1(originalExtrusion, profileOriginal);
                                //UseCase2(originalExtrusion);

                            }
                            else if (parallelInfo == 0)  // not parallel
                            {
                                UseCase3(originalExtrusion);
                                // scale original profile
                                //if (extrusionVector.IsParallelTo(plane.XAxis, Tol) == 1)
                                //{
                                //    var transform = Transform.Scale(plane, 1.6, default, default);
                                //    profileNew = profileOriginal.DuplicateCurve();
                                //    profileNew.Transform(transform);
                                //}

                                // get new profile
                                //List<Curve> projectedCrvs = GetProjectedCrvsV2(plane, extrusionDynamic);

                                ////dynamicOnProfilePlane = Curve.JoinCurves(projectedCrvs).ToList();
                                ////dynamicOnProfilePlane = extrusionDynamic.Edges.Select(x => x.EdgeCurve).ToList();
                                //dynamicOnProfilePlane = projectedCrvs;
                                //dynamicOnProfilePlane.Add(profileOriginal);

                                //var profileNew = Curve.CreateBooleanUnion(dynamicOnProfilePlane, Tol).FirstOrDefault();
                                //if (profileNew != null)
                                //{
                                //    extrusionWhole = Extrusion.Create(profileNew, originalExtrusion.PathLineCurve().GetLength(), true);

                                //}

                            }
                            else // anti- parallel
                            {

                            }

                        }
                        else
                        {

                        }
                        //var originalExtrusionBrep = originalExtrusion.ToBrep();
                        //Brep resultBrep = null;
                        //if (originalExtrusionBrep.IsPointInside(extrusionDynamic.GetBoundingBox(true).Center, Tol, true))
                        //{
                        //    resultBrep = Brep.CreateBooleanDifference(originalExtrusionBrep, extrusionDynamic.ToBrep(), Tol)?.FirstOrDefault();
                        //}
                        //else
                        //{
                        //    resultBrep = Brep.CreateBooleanUnion(new Brep[] { originalExtrusionBrep, extrusionDynamic.ToBrep() }, Tol).FirstOrDefault();
                        //}
                        //if (resultBrep != null)
                        //{
                        //    if (objectTable.Replace(ObjId, resultBrep))
                        //    {
                        //        RhinoApp.WriteLine("Successin replacing the brep");
                        //    }

                        //}

                    }
                    else if (obj.Geometry is Brep)
                }
            }
        }

        private void UseCase2(Extrusion originalExtrusion)
        {
            var brep = originalExtrusion.ToBrep();
            brep.Append(extrusionDynamic);
            brep.RemoveHoles(Tol);
            brep.MergeCoplanarFaces(Tol);
            brepWhole = brep;
        }

        private void UseCase3(Extrusion originalExtrusion)
        {
            var brep = originalExtrusion.ToBrep();
            var brepTmp = Brep.CreateBooleanUnion(new Brep[] { brep, extrusionDynamic }, Tol).FirstOrDefault();
            if (brepTmp != null)
            {
                brepTmp.MergeCoplanarFaces(1, 1);
                brepWhole = brepTmp;

            }
        }

        private void Case1(Extrusion originalExtrusion, Curve profileOriginal)
        {
            Point3d t = originalExtrusion.PathEnd + extrusionVector;
            Vector3d newVector = t - originalExtrusion.PathStart;
            extrusionWhole = Extrusion.Create(profileOriginal, newVector.Length, true);
        }

        private List<Curve> GetProjectedCrvsV2(Plane plane, Brep extrusion)
        {
            var ts = new List<Curve>();
            foreach (var item in extrusion.Faces)
            {
                ts.AddRange(item.ToBrep().Edges);
            }
            return ts;
        }

        private List<Curve> GetProjectedCrvs(Plane plane, Brep extrusion)
        {
            var edges = extrusion.Curves3D;
            var projectedCrvs = new List<Curve>();
            foreach (var item in edges)
            {
                var projectedCrv = Curve.ProjectToPlane(item, plane);
                if (projectedCrv != null && projectedCrv.IsValid)
                {
                    projectedCrvs.Add(projectedCrv);
                }
            }

            return projectedCrvs;
        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            DrawViewLine(e);

            if (CPlane.IsValid)
            {
                e.Display.DrawLine(GetLineFromPlaneX(CPlane, 4), System.Drawing.Color.Red, 5);
                e.Display.DrawLine(GetLineFromPlaneY(CPlane, 4), System.Drawing.Color.Green, 5);
                e.Display.DrawLine(GetLineFromPlaneZ(CPlane, 4), System.Drawing.Color.Blue, 5);
            }

            if (brepSrfToPullTmp != null && brepSrfToPullTmp.IsValid)
            {
                e.Display.DrawSurface(brepSrfToPullTmp, System.Drawing.Color.DarkRed, 3);
            }
            if (dynamicOnProfilePlane != null)
            {
                foreach (var item in dynamicOnProfilePlane)
                {
                    e.Display.DrawCurve(item, System.Drawing.Color.DarkBlue, 3);

                }
            }
            if (profileNew != null)
            {
                e.Display.DrawCurve(profileNew, System.Drawing.Color.DarkBlue, 3);

            }
            if (profileOriginalDisplay != null)
            {
                e.Display.DrawCurve(profileOriginalDisplay, System.Drawing.Color.DarkBlue, 3);

            }
            if (brepWhole != null)
            {
                e.Display.DrawBrepWires(brepWhole, System.Drawing.Color.Purple, 5);
            }
            if (Iteration == 1)
            {
                if (BrepSrfToPull != null)
                {
                    // Get current construction plane
                    var constructionPlane = e.Viewport.ConstructionPlane();
                    var edges = BrepSrfToPull.ToBrep().Edges;
                    var joinedCurves = Curve.JoinCurves(edges);
                    var profile = joinedCurves.FirstOrDefault();
                    if (profile != null)
                    {
                        extrusionVector = e.CurrentPoint - constructionPlane.Origin;
                        var extrusionTmp = Extrusion.Create(profile, extrusionVector.Length, true);
                        //var srf = Surface.CreateExtrusion(profile, extrusionVector);
                        if (extrusionTmp != null)
                        {
                            extrusionDynamic = extrusionTmp.ToBrep();
                            //extrusionDynamic = srf.ToBrep().CapPlanarHoles(Tol);
                            if (extrusionDynamic != null)
                            {
                                e.Display.DrawBrepWires(extrusionDynamic, System.Drawing.Color.DarkRed, 3);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private void SetBrepSrfToPullTmp(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            viewLine = viewport.ClientToWorld(windowPoint);
            Point3d viewLineStartPt = viewLine.To;

            // find out the visible objs
            var relevantObjs = GetRelevantObjs(viewport, viewLine);
            var closestObj = relevantObjs.OrderBy(x => GetDistance(x, viewLine))
                                         .FirstOrDefault();

            if (closestObj != null)
            {
                ObjId = closestObj.Id;
                if (closestObj.Geometry is Extrusion extrusion)
                {
                    var brep = extrusion.ToBrep();

                    if (GetRelevantBrepFaceClosestToViewAndItsParameters(brep, viewLine, out BrepFace brepFace, out ptOnSrf, out Plane frame))
                    {
                        brepSrfToPullTmp = brepFace;
                        CPlane = frame;
                    }
                    return;
                }
                else if (closestObj.Geometry is Brep brep2)
                {
                    if (GetRelevantBrepFaceClosestToViewAndItsParameters(brep2, viewLine, out BrepFace brepFace, out ptOnSrf, out Plane frame))
                    {
                        brepSrfToPullTmp = brepFace;
                        CPlane = frame;
                    }
                    return;
                }
                else if (closestObj.Geometry is Surface surface)
                {
                    return;
                }
            }

            // if there is no closest obj found, then it means the mouse lies on nothing, then set plane and corner to default
        }

        private bool GetRelevantBrepFaceClosestToViewAndItsParameters(Brep brep,
                                                                      Line viewLine,
                                                                      out BrepFace brepSrf,
                                                                      out Point3d ptOnSrf,
                                                                      out Plane frame)
        {
            return RelevantBrepFaceClosestToViewGetter.Get(brep, viewLine, out brepSrf, out ptOnSrf, out frame);
        }

        private void DrawViewLine(GetPointDrawEventArgs e)
        {
            if (viewLine.IsValid)
            {
                e.Display.DrawLine(viewLine, System.Drawing.Color.DarkRed, 5);
            }
        }

        private IEnumerable<RhinoObject> GetRelevantObjs(RhinoViewport viewport, Line viewLine)
        {
            return RelevantObjsGetter.Get(viewport, viewLine, RhinoDoc.ActiveDoc);
        }

        private double GetDistance(RhinoObject x, Line viewLine)
        {
            return DistanceBtwObjAndViewGetter.Get(x, viewLine);
        }

        private Line GetLineFromPlaneZ(Plane plane, int length)
        {
            Vector3d span = plane.ZAxis * length;
            return CreateLine(plane, span);
        }

        private Line GetLineFromPlaneY(Plane plane, int length)
        {
            Vector3d span = plane.YAxis * length;
            return CreateLine(plane, span);
        }

        private Line GetLineFromPlaneX(Plane plane, double length)
        {
            Vector3d span = plane.XAxis * length;
            return CreateLine(plane, span);
        }

        private static Line CreateLine(Plane plane, Vector3d span)
        {
            return new Line(plane.Origin, span);
        }
    }
}
