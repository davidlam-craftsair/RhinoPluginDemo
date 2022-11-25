namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamicIteration2 : IGetBoxDynamicIteration2
    {
        public GetBoxDynamicIteration2(CornerPtIteration cornerPtIteration2)
        {
            CornerPtIteration2 = cornerPtIteration2;
        }

        public CornerPtIteration CornerPtIteration2 { get; }

        public void Do(GetBoxDynamic planeDynamic, BoxDynamicParameters boxDynamicParameters)
        {
            CornerPtIteration2.Iteration(planeDynamic,
                                   boxDynamicParameters.BoxCorner2OffsetX,
                                   boxDynamicParameters.BoxCorner2OffsetY,
                                   out double xOffset,
                                   out double yOffset);
            RecordOffsetValCorner2(boxDynamicParameters, xOffset, yOffset);
            planeDynamic.Iteration += 1;
        }

        private static void RecordOffsetValCorner2(BoxDynamicParameters boxDynamicParameters, double xOffsetCorner2, double yOffsetCorner2)
        {
            boxDynamicParameters.BoxCorner2OffsetX = xOffsetCorner2;
            boxDynamicParameters.BoxCorner2OffsetY = yOffsetCorner2;
        }
    }
}
