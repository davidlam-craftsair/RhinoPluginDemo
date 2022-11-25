namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamicIteration1
    {
        public GetBoxDynamicIteration1(CornerPtIteration cornerPtIteration1)
        {
            CornerPtIteration1 = cornerPtIteration1;
        }

        public CornerPtIteration CornerPtIteration1 { get; }

        public void Do(GetBoxDynamic planeDynamic, BoxDynamicParameters boxDynamicParameters)
        {
            CornerPtIteration1.Iteration(planeDynamic,
                             boxDynamicParameters.BoxCorner1OffsetX,
                             boxDynamicParameters.BoxCorner1OffsetY,
                             out double xOffset,
                             out double yOffset);
            RecordOffsetValCorner1(boxDynamicParameters, xOffset, yOffset);
            planeDynamic.Iteration += 1;
        }

        private void RecordOffsetValCorner1(BoxDynamicParameters boxDynamicParameters, double xOffsetCorner1, double yOffsetCorner1)
        {
            boxDynamicParameters.BoxCorner1OffsetX = xOffsetCorner1;
            boxDynamicParameters.BoxCorner1OffsetY = yOffsetCorner1;

            // ChangeOffsetValCorner2 to align with changes in Corner1
            boxDynamicParameters.BoxCorner2OffsetX = -xOffsetCorner1;
            boxDynamicParameters.BoxCorner2OffsetY = -yOffsetCorner1;

        }
    }
}
