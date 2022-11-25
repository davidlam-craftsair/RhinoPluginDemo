using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using System;

namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamic: GetPoint
    {
        #region Fields
        private int _iteration;
        #endregion

        #region Properties
        public int Iteration 
        {
            get { return _iteration; }
            set
            { 
                if (_iteration != value)
                {
                    _iteration = value;
                    BoxDynamicMouseMoveInteraction.Iteration = value;
                    BoxDynamicDrawInteraction.Iteration = value;
                }
            }
        }

        public IBoxCreationV2 BoxCreation { get; private set; }
        public IMouseMoveInteraction BoxDynamicMouseMoveInteraction { get; }
        public IDynamicDrawInteraction BoxDynamicDrawInteraction { get; }
        #endregion

        public GetBoxDynamic(IBoxCreationV2 boxCreation,
                             IMouseMoveInteraction boxDynamicMoveInteraction,
                             IDynamicDrawInteraction boxDynamicDrawInteraction
                             )
        {
            BoxCreation = boxCreation;
            BoxDynamicMouseMoveInteraction = boxDynamicMoveInteraction;
            BoxDynamicDrawInteraction = boxDynamicDrawInteraction;
            Iteration = 0;
            EnableObjectSnapCursors(true);
            PermitObjectSnap(true);
        }


        #region Events Methods
        protected override void OnMouseMove(GetPointMouseEventArgs e)
        {
            base.OnMouseMove(e);
            BoxDynamicMouseMoveInteraction.OnMouseMove(e);
        }
        

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            BoxDynamicDrawInteraction.OnDynamicDraw(e);
        }
        #endregion



    }
}
