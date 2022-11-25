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
        private int _iteration;

        #endregion

        public PullFaceDynamic(PullFaceMouseMoveInteractionIteration0 pullFaceMouseMoveInteractionIteration0,
                               PullFaceDynamicDrawInteraction pullFaceDynamicDrawInteraction,
                               IPullFaceCreation pullFaceCreation
                               )
        {
            PullFaceMouseMoveInteractionIteration0 = pullFaceMouseMoveInteractionIteration0;
            PullFaceDynamicDrawInteraction = pullFaceDynamicDrawInteraction;
            PullFaceCreation = pullFaceCreation;
        }
        #region Properties
        public PullFaceMouseMoveInteractionIteration0 PullFaceMouseMoveInteractionIteration0 { get; }
        public PullFaceDynamicDrawInteraction PullFaceDynamicDrawInteraction { get; }
        public IPullFaceCreation PullFaceCreation { get; }
        public int Iteration 
        {
            get { return _iteration; }
            internal set
            {
                if (_iteration != value)
                {
                    _iteration = value;
                    PullFaceDynamicDrawInteraction.Iteration = value;
                }
            }
        }

        #endregion

        #region Events Methods

        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateViewLine(e);

            if (Iteration == 0)
            {
                PullFaceMouseMoveInteractionIteration0.OnMouseMove(e);
            }
        }

        private void UpdateViewLine(GetPointMouseEventArgs e)
        {
            PullFaceCreation.ViewLine = GetViewLine(e.Viewport, e.WindowPoint);
        }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            PullFaceDynamicDrawInteraction.OnDynamicDraw(e);
        }

        #endregion


        private static Line GetViewLine(RhinoViewport viewport, System.Drawing.Point windowPoint)
        {
            return viewport.ClientToWorld(windowPoint);
        }

    }
}
