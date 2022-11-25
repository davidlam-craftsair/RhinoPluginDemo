using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using RhinoPlugInExcercise1.PullFace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceDynamic : GetPoint
    {
        private Line viewLineDisplay;
        private Surface brepSrfToPullTmp;
        private Guid brepObjIdTmp;
        private Point3d ptOnSrf;
        public Brep dynamicExtrusion;
        private Vector3d extrusionVector;
        private List<Curve> dynamicOnProfilePlane;
        private Curve profileNew;
        private Curve profileOriginalDisplay;
        public Brep ResultantBrep { get; private set; }

        public PullFaceDynamic(IRelevantObjsGetter relevantObjsGetter,
                               double tol,
                               IDistanceBtwObjAndViewGetter distanceBtwObjAndViewGetter,
                               IRelevantBrepFaceClosestToViewGetter relevantBrepFaceClosestToViewGetter,
                               IRelevantClosestObjToViewCursorGetter relevantClosestObjToViewCursorGetter)
        {
            RelevantObjsGetter = relevantObjsGetter;
            Tol = tol;
            DistanceBtwObjAndViewGetter = distanceBtwObjAndViewGetter;
            RelevantBrepFaceClosestToViewGetter = relevantBrepFaceClosestToViewGetter;
            RelevantClosestObjToViewCursorGetter = relevantClosestObjToViewCursorGetter;
        }

        public IRelevantObjsGetter RelevantObjsGetter { get; }
        public double Tol { get; }
        public IDistanceBtwObjAndViewGetter DistanceBtwObjAndViewGetter { get; }
        public IRelevantBrepFaceClosestToViewGetter RelevantBrepFaceClosestToViewGetter { get; }
        public IRelevantClosestObjToViewCursorGetter RelevantClosestObjToViewCursorGetter { get; }
        public Surface BrepSrfToPull { get; private set; }
        public Guid ObjId { get; private set; }
        public int Iteration { get; internal set; }
        public Plane cplane;
        private IList<BoundingBox> boundingBoxes;

        #region Events Methods

        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Iteration == 0)
            {
                if (GetWhichBrepSrfToPull(e.Viewport, e.WindowPoint, out brepSrfToPullTmp, out cplane))
                {
                    BrepSrfToPull = brepSrfToPullTmp;
                }
            }

        }

        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Iteration == 0)
            {
                ProcessIteration0(e);
            }
            if (Iteration == 1)
            {
                ProcessIteration1();
            }
        }

        private bool ProcessIteration0(GetPointMouseEventArgs e)
        {
            BrepSrfToPull = brepSrfToPullTmp;
            if (BrepSrfToPull != null)
            {
                // Get the frame of brep face
                if (GetFrameFromBrepFace(BrepSrfToPull, out Plane frame))
                {
                    // Set viewport construction plane to this frame
                    e.Viewport.SetConstructionPlane(frame);
                    // Constrain the mouse movement to the zAxis of this frame
                    ConstrainMouseMovementToZAxisOfFrame(frame);
                    return true;
                }
            }
            return false;
        }

        private bool ProcessIteration1()
        {
            // Put the extrusionDynamic to obj
            if (dynamicExtrusion != null)
            {
                RhinoObject obj = GetObjById(ObjId);

                if (obj.Geometry is Extrusion originalExtrusion)
                {
                    if (PullFaceFromExtrusion(originalExtrusion,
                                              extrusionVector,
                                              dynamicExtrusion,
                                              Tol,
                                              out Brep brepWholeTmp))
                    {
                        SetResultantBrep(brepWholeTmp);
                        return true;
                    }
                }
                else if (obj.Geometry is Brep originalBrep)
                {
                    // Visualize the bounding boxes 
                    boundingBoxes = new Brep[] { originalBrep, dynamicExtrusion }.Select(x => GetBoundingBox(x)).ToList();

                    if (PullFaceFromBrep(originalBrep, dynamicExtrusion, Tol, out Brep brepWholeTmp))
                    {
                        SetResultantBrep(brepWholeTmp);
                        return true;
                    }
                }
            }
            return false;
        }
        private void SetResultantBrep(Brep brep)
        {
            ResultantBrep = brep;
            if (ResultantBrep.MergeCoplanarFaces(Tol))
            {
                Debug.WriteLine("Merge coplanar faces");
            }
            ResultantBrep.Standardize();
            ResultantBrep.Compact();
        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            DrawViewLine(e);

            if (cplane.IsValid)
            {
                e.Display.DrawLine(GetLineFromPlaneX(cplane, 4), System.Drawing.Color.Red, 5);
                e.Display.DrawLine(GetLineFromPlaneY(cplane, 4), System.Drawing.Color.Green, 5);
                e.Display.DrawLine(GetLineFromPlaneZ(cplane, 4), System.Drawing.Color.Blue, 5);
            }

            if (brepSrfToPullTmp != null && brepSrfToPullTmp.IsValid)
            {
                e.Display.DrawSurface(brepSrfToPullTmp, System.Drawing.Color.Green, 3);
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
            //if (brepWhole != null)
            //{
            //    e.Display.DrawBrepWires(brepWhole, System.Drawing.Color.Purple, 5);
            //}

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
                        var parallelInfo = extrusionVector.IsParallelTo(constructionPlane.ZAxis);
                        Extrusion extrusionTmp = null;
                        if (parallelInfo == 1)
                        {
                            extrusionTmp = Extrusion.Create(profile, extrusionVector.Length, true);

                        }
                        else if (parallelInfo == -1)  // anti-parallel
                        {
                            extrusionTmp = Extrusion.Create(profile, extrusionVector.Length*(-1), true);
                        }
                        else
                        {
                            if (!extrusionVector.IsZero)
                            {
                                // extrusionVector is perpendicular to ZAxis of construction plane
                                //var t = extrusionVector.IsPerpendicularTo(constructionPlane.ZAxis);
                                //if (t)
                                //{
                                    
                                //}
                                //throw new NotImplementedException();
                            }
                           
                        }
                        //var srf = Surface.CreateExtrusion(profile, extrusionVector);
                        if (extrusionTmp != null)
                        {
                            dynamicExtrusion = extrusionTmp.ToBrep();
                            //extrusionDynamic = srf.ToBrep().CapPlanarHoles(Tol);
                            if (dynamicExtrusion != null)
                            {
                                e.Display.DrawBrepWires(dynamicExtrusion, System.Drawing.Color.DarkRed, 3);
                            }
                        }
                    }
                }
            }
            if (Iteration == 2)
            {
                if (boundingBoxes != null)
                {
                    foreach (var item in boundingBoxes)
                    {
                        e.Display.DrawBox(item, System.Drawing.Color.DarkCyan, 3);
                    }
                }
            }
        }
        #endregion
        private bool ConstrainMouseMovementToZAxisOfFrame(Plane frame)
        {
            return Constrain(new Line(frame.Origin, frame.ZAxis));
        }

        private RhinoObject GetObjById(Guid id)
        {
            var objectTable = RhinoDoc.ActiveDoc.Objects;
            var obj = objectTable.Find(id);
            return obj;
        }

        private bool PullFaceFromBrep(Brep originalBrep, Brep dynamicExtrusion, double tol, out Brep brepWholeTmp)
        {
            // Determine whether to use Union or Difference
            var r = CheckBrepAdditionOrSubtraction(originalBrep, dynamicExtrusion);
            IEnumerable<Brep> brepsResult = Enumerable.Empty<Brep>();
            if (r == 1)
            {
                brepsResult = Union(originalBrep, dynamicExtrusion, tol);
            }
            else
            {
                //brepsResult = Union(originalBrep, dynamicExtrusion, tol);
                brepsResult = Difference(originalBrep, dynamicExtrusion, tol);
            }

            if (brepsResult.Count() > 1)
            {
                throw new NotImplementedException();
            }
            brepWholeTmp = brepsResult.FirstOrDefault();

            return brepWholeTmp != null;
        }

        private static Brep[] Difference(Brep originalBrep, Brep dynamicExtrusion, double tol)
        {
            return Brep.CreateBooleanDifference(originalBrep, dynamicExtrusion, tol, true);
        }

        private static Brep[] Union(Brep originalBrep, Brep dynamicExtrusion, double tol)
        {
            return Brep.CreateBooleanUnion(new Brep[2] { originalBrep, dynamicExtrusion }, tol);
        }

        private int CheckBrepAdditionOrSubtraction(Brep originalBrep, Brep dynamicExtrusion)
        {
            var t1 = GetBoundingBox(originalBrep).Contains(GetBoundingBox(dynamicExtrusion));
            if (t1)
            {
                return -1; // originalBrep should be subtracted by dynamicExtrusion
            }
            return 1; // both do not contain each other
        }

        private static BoundingBox GetBoundingBox(GeometryBase geometryBase)
        {
            return geometryBase.GetBoundingBox(true);
        }

        private static bool PullFaceFromExtrusion(Extrusion originalExtrusion,
                                                  Vector3d extrusionVector,
                                                  Brep dynamicExtrusion,
                                                  double tol,
                                                  out Brep brepWholeTmp)
        {
            int parallelInfo = extrusionVector.IsParallelTo(originalExtrusion.PathTangent, tol);
            if (parallelInfo == -1) // anti - parallel
            {
                brepWholeTmp = dynamicExtrusion;
                return true;
            }
            else if (parallelInfo == 1)
            {
                if (originalExtrusion.ProfileCount == 1)
                {
                    var t = GetResultExtrusionForExtrusionVectorAlignedWithXAxis(originalExtrusion, extrusionVector);
                    brepWholeTmp = t.ToBrep();
                    return true;
                }
            }
            else // not parallel
            {       
                return GetBrepWholeAfterExtrusion(originalExtrusion, dynamicExtrusion, tol, out brepWholeTmp);

            }

            brepWholeTmp = default;
            return false;
        }

        private bool GetFrameFromBrepFace(Surface brepSrfToPull, out Plane frame)
        {
            var domainU = brepSrfToPull.Domain(0);
            var domainV = brepSrfToPull.Domain(1);
            var u = (domainU[1] - domainU[0]) / 2;
            var v = (domainV[1] - domainV[0]) / 2;
            return brepSrfToPull.FrameAt(u, v, out frame);
        }

        private static bool GetBrepWholeAfterExtrusion(Extrusion originalExtrusion, Brep dynamicExtrusion, double tol, out Brep brepWholeTmp)
        {
            var brep = originalExtrusion.ToBrep();
            var brepTmp = Brep.CreateBooleanUnion(new Brep[] { brep, dynamicExtrusion }, tol).FirstOrDefault();
            if (brepTmp != null)
            {
                brepTmp.MergeCoplanarFaces(1, 1);
                brepWholeTmp = brepTmp;
                return true;
            }
            brepWholeTmp = default;
            return false;
        }

        private static Extrusion GetResultExtrusionForExtrusionVectorAlignedWithXAxis(Extrusion originalExtrusion, Vector3d extrusionVector)
        {
            var originalOuterProfile = originalExtrusion.Profile3d(0, 0);

            Point3d t = originalExtrusion.PathEnd + extrusionVector;
            Vector3d newVector = t - originalExtrusion.PathStart;
            return Extrusion.Create(originalOuterProfile, newVector.Length, true);
        }

        private bool GetWhichBrepSrfToPull(RhinoViewport viewport, System.Drawing.Point windowPoint, out Surface brepFaceToPull, out Plane frame)
        {
            var viewLine = GetViewLine(viewport, windowPoint);
            viewLineDisplay = viewLine;
            RhinoObject closestObj = GetClosestObj(viewport, viewLine);

            if (closestObj != null)
            {
                ObjId = closestObj.Id;
                if (closestObj.Geometry is Extrusion extrusion)
                {
                    var brep = extrusion.ToBrep();

                    if (GetRelevantBrepFaceClosestToViewAndItsParameters(brep, viewLine, out BrepFace brepFace, out ptOnSrf, out frame))
                    {
                        //brepFaceToPull = brepFace;
                        PrepareBrepface(brepFace);
                        brepFaceToPull = brepFace.UnderlyingSurface();
                        return true;
                    }
                }
                else if (closestObj.Geometry is Brep brep2)
                {
                    if (GetRelevantBrepFaceClosestToViewAndItsParameters(brep2, viewLine, out BrepFace brepFace, out ptOnSrf, out frame))
                    {
                        //brepFaceToPull = brepFace;
                        PrepareBrepface(brepFace);
                        brepFaceToPull = brepFace.UnderlyingSurface();
                        return true; 
                    }
                }
                else if (closestObj.Geometry is Surface surface)
                {
                }
            }

            // if there is no closest obj found,
            brepFaceToPull = default;
            frame = default;
            return false;
        }

        private static void PrepareBrepface(BrepFace brepFace)
        {
            var t = brepFace.ShrinkFace(BrepFace.ShrinkDisableSide.ShrinkAllSides);
            if (t)
            {
                Debug.WriteLine("face shrinked");
            }
        }

        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }

        private RhinoObject GetClosestObj(RhinoViewport viewport, Line viewLine)
        {

            // find out the visible objs
            var relevantObjs = GetRelevantObjs(viewport, viewLine);
            RhinoApp.WriteLine($"No of relevant objs = {relevantObjs.Count()}");
            var closestObj = relevantObjs.OrderBy(x => GetDistance(x, viewLine))
                                         .FirstOrDefault();
            return closestObj;
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
            if (viewLineDisplay.IsValid)
            {
                e.Display.DrawLine(viewLineDisplay, System.Drawing.Color.DarkRed, 5);
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
