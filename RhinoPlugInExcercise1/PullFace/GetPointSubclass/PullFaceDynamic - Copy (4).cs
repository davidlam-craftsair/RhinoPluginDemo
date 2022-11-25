using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RhinoPlugInExcercise1
{
    public class PullFaceDynamic : GetPoint
    {
        #region Fields
        private Line viewLineDisplay;
        private BrepFace brepFaceToPullTmp;
        public Brep dynamicExtrusion;
        private Vector3d extrusionVector;
        public Plane cplane;

        #endregion

        public PullFaceDynamic(IPlaneDrawer planeDrawer,
                               IBrepFaceGetter brepFaceToPullGetter,
                               IPullFaceFromExtrusionHandler pullFaceFromExtrusionHandler,
                               IPullFaceFromBrepHandler pullFaceFromBrepHandler
                               )
        {
            PlaneDrawer = planeDrawer;
            BrepFaceToPullGetter = brepFaceToPullGetter;
            PullFaceFromExtrusionHandler = pullFaceFromExtrusionHandler;
            PullFaceFromBrepHandler = pullFaceFromBrepHandler;
        }
        #region Properties
        public IPlaneDrawer PlaneDrawer { get; }
        public IBrepFaceGetter BrepFaceToPullGetter { get; }
        public IPullFaceFromExtrusionHandler PullFaceFromExtrusionHandler { get; }
        public IPullFaceFromBrepHandler PullFaceFromBrepHandler { get; }
        public BrepFace BrepFaceToPull { get; private set; }
        public Guid ObjId { get; private set; }
        public int Iteration { get; internal set; }
        public Brep ResultantBrep { get; private set; }

        #endregion

        #region Events Methods

        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            var viewLine = GetViewLine(e.Viewport, e.WindowPoint);
            viewLineDisplay = viewLine;

            if (Iteration == 0)
            {
                if (DecideBrepFaceToPull(e.Viewport, viewLine, out brepFaceToPullTmp, out cplane, out Guid objIdTmp))
                {
                    BrepFaceToPull = brepFaceToPullTmp;
                    ObjId = objIdTmp;
                }
            }
        }

        protected override void OnMouseDown(GetPointMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Iteration == 0) 
            {
                ProcessIteration0ForMouseDown(e);
            }
            if (Iteration == 1)
            {
                ProcessIteration1ForMouseDown();
            }
        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            DrawViewLine(e);
            DrawPlane(e.Display, cplane);
            if (Iteration == 0)
            {
                if (brepFaceToPullTmp != null)
                {
                    e.Display.DrawBrepShaded(brepFaceToPullTmp.DuplicateFace(false), new DisplayMaterial(System.Drawing.Color.DarkRed, 0.5));
                    //DrawSurface(e.Display, brepFaceToPullTmp);

                }
            }
            if (Iteration == 1)
            {
                if (BrepFaceToPull != null)
                {
                    // Get current construction plane
                    var constructionPlane = e.Viewport.ConstructionPlane();
                    Curve profile = GetProfile(BrepFaceToPull);
                    int parallelInfo = 0;
                    if (profile.TryGetPlane(out Plane curvePlane))
                    {
                        parallelInfo = curvePlane.ZAxis.IsParallelTo(extrusionVector);
                    }
                    else
                    {
                        parallelInfo = extrusionVector.IsParallelTo(constructionPlane.ZAxis);
                    }
                    if (profile != null)
                    {
                        extrusionVector = e.CurrentPoint - constructionPlane.Origin;
                        Extrusion extrusionTmp = null;
                        if (parallelInfo == 1)
                        {
                            double height = extrusionVector.Length * (1);
                            extrusionTmp = Extrusion.Create(profile, height, true);

                        }
                        else if (parallelInfo == -1)  // anti-parallel
                        {
                            double height = extrusionVector.Length * (-1);
                            extrusionTmp = Extrusion.Create(profile, height, true);
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
                        if (extrusionTmp != null)
                        {
                            dynamicExtrusion = extrusionTmp.ToBrep();
                            if (dynamicExtrusion != null)
                            {
                                e.Display.DrawBrepWires(dynamicExtrusion, System.Drawing.Color.DarkRed, 3);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Iteration Methods for Mouse Down
        private bool ProcessIteration0ForMouseDown(GetPointMouseEventArgs e)
        {
            var viewLine = GetViewLine(e.Viewport, e.WindowPoint);
            if (DecideBrepFaceToPull(e.Viewport, viewLine, out brepFaceToPullTmp, out Plane cplaneTmp, out Guid objIdTmp))
            {
                BrepFaceToPull = brepFaceToPullTmp;
                ObjId = objIdTmp;
                cplane = cplaneTmp;
                SetConstructionPlaneForViewport(e.Viewport, cplaneTmp);
                ConstrainToPlaneZAxis(cplaneTmp);
                return true;
            }
            return false;
        }
        private bool ProcessIteration1ForMouseDown()
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
                                              out Brep brepWholeTmp))
                    {
                        SetResultantBrep(brepWholeTmp);
                        return true;
                    }
                }
                else if (obj.Geometry is Brep originalBrep)
                {
                    if (PullFaceFromBrep(originalBrep, dynamicExtrusion, out Brep brepWholeTmp))
                    {
                        SetResultantBrep(brepWholeTmp);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        private Curve GetProfile(BrepFace brepFace)
        {
            var brep = brepFace.DuplicateFace(false);
            var edges = brep.Edges;
            var joinedCurves = Curve.JoinCurves(edges);
            var profile = joinedCurves.FirstOrDefault();
            return profile;
        }

        private Curve GetProfileV2(BrepFace srf)
        {
            var edges = srf.ToBrep().Edges;
            var joinedCurves = Curve.JoinCurves(edges);
            var profile = joinedCurves.FirstOrDefault();
            return profile;
        }
        
        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }

        private RhinoObject GetObjById(Guid id)
        {
            var objectTable = RhinoDoc.ActiveDoc.Objects;
            var obj = objectTable.Find(id);
            return obj;
        }

        
        private void SetResultantBrep(Brep brep)
        {
            ResultantBrep = brep;
        }

        private void SetConstructionPlaneForViewport(RhinoViewport viewport, Plane cplane)
        {
            viewport.SetConstructionPlane(cplane);
        }   


        private bool PullFaceFromBrep(Brep originalBrep, Brep dynamicExtrusion, out Brep brepWholeTmp)
        {
            return PullFaceFromBrepHandler.Pull(originalBrep, dynamicExtrusion, out brepWholeTmp);
        }

        private bool PullFaceFromExtrusion(Extrusion originalExtrusion,
                                                  Vector3d extrusionVector,
                                                  Brep dynamicExtrusion,
                                                  out Brep brepWholeTmp)
        {
            return PullFaceFromExtrusionHandler.Pull(originalExtrusion, extrusionVector, dynamicExtrusion, out brepWholeTmp);
        }

        private bool DecideBrepFaceToPull(RhinoViewport viewport,
                                          Line viewLine,
                                          out BrepFace brepFaceToPull,
                                          out Plane frame,
                                          out Guid objId)
        {
            return BrepFaceToPullGetter.Get(viewLine, viewport, out brepFaceToPull, out frame, out objId);

        }
        
        private void ConstrainToPlaneZAxis(Plane plane)
        {
            Constrain(new Line(plane.Origin, plane.ZAxis));
        }


        private void DrawViewLine(GetPointDrawEventArgs e)
        {
            if (viewLineDisplay.IsValid)
            {
                e.Display.DrawLine(viewLineDisplay, System.Drawing.Color.DarkRed, 5);
                e.Display.DrawPoint(viewLineDisplay.To,PointStyle.ControlPoint, 5, System.Drawing.Color.DarkRed);
            }
        }

        private void DrawSurface(DisplayPipeline display, Surface srf)
        {
            if (srf != null && srf.IsValid)
            {
                display.DrawSurface(srf, System.Drawing.Color.DarkRed, 5);
            }
        }

        private void DrawPlane(DisplayPipeline display, Plane plane)
        {
            PlaneDrawer.Draw(display, plane);
        }

    }
}
