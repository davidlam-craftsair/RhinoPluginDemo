using Rhino.Display;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicDrawInteraction : IDynamicDrawInteraction
    {
        public int Iteration { get; set; }
        public Line ViewLine
        {
            get { return BoxCreation.ViewLine; }
            set { BoxCreation.ViewLine = value; }
        }
        public Surface SelectedBrepFaceHovered { get; set; }
        public BoxDynamicDrawInteractionIteration0 BoxDynamicDrawInteractionIteration0 { get; }
        public BoxDynamicDrawInteractionIteration1 BoxDynamicDrawInteractionIteration1 { get; }
        public BoxDynamicDrawInteractionIteration2 BoxDynamicDrawInteractionIteration2 { get; }
        public BoxDynamicDrawInteractionIteration3 BoxDynamicDrawInteractionIteration3 { get; }
        public IBoxCreationV2 BoxCreation { get; }

        public BoxDynamicDrawInteraction(BoxDynamicDrawInteractionIteration0 boxDynamicDrawInteractionIteration0,
                                         BoxDynamicDrawInteractionIteration1 boxDynamicDrawInteractionIteration1,
                                         BoxDynamicDrawInteractionIteration2 boxDynamicDrawInteractionIteration2,
                                         BoxDynamicDrawInteractionIteration3 boxDynamicDrawInteractionIteration3,
                                         IBoxCreationV2 boxCreation
                                         )
        {
            BoxDynamicDrawInteractionIteration0 = boxDynamicDrawInteractionIteration0;
            BoxDynamicDrawInteractionIteration1 = boxDynamicDrawInteractionIteration1;
            BoxDynamicDrawInteractionIteration2 = boxDynamicDrawInteractionIteration2;
            BoxDynamicDrawInteractionIteration3 = boxDynamicDrawInteractionIteration3;
            BoxCreation = boxCreation;
        }

        public void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            if (Iteration == 0)
            {
                DynamicDrawIteration0(e);
            }
            else if (Iteration == 1)
            {
                DynamicDrawIteration1(e);
            }
            else if (Iteration == 2)
            {
                DynamicDrawIteration2(e);
            }
            else if (Iteration == 3)
            {
                DynamicDrawIteration3(e);
            }
        }

        private void DynamicDrawIteration3(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);
            BoxDynamicDrawInteractionIteration3.OnDynamicDraw(e);
        }

        private void DynamicDrawIteration2(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);
            BoxDynamicDrawInteractionIteration2.OnDynamicDraw(e);
        }

        private void DynamicDrawIteration1(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);
            BoxDynamicDrawInteractionIteration1.OnDynamicDraw(e);

        }

        /// <summary>
        /// Draw any face that the mouse hovers over
        /// </summary>
        /// <param name="e"></param>
        private void DynamicDrawIteration0(GetPointDrawEventArgs e)
        {
            DynamicDrawAllIterations(e);
            BoxDynamicDrawInteractionIteration0.OnDynamicDraw(e);
            //DrawSelectedBrepFaceTmp(e);
        }

        private void DynamicDrawAllIterations(GetPointDrawEventArgs e)
        {
            DrawViewLine(e.Display);
        }

        private void DrawViewLine(DisplayPipeline display)
        {
            if (ViewLine.IsValid)
            {
                display.DrawLine(ViewLine, System.Drawing.Color.DarkRed, 5);
            }
        }



    }
}
