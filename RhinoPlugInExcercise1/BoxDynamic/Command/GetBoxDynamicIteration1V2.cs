using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System.Windows;
using System.Windows.Forms;

namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamicIteration1V2 : IGetBoxDynamicIteration1
    {
        public GetBoxDynamicIteration1V2(CornerPtIteration cornerPtIteration1, IRhinoDocContainer rhinoDocContainer)
        {
            CornerPtIteration1 = cornerPtIteration1;
            RhinoDocContainer = rhinoDocContainer;
        }

        public CornerPtIteration CornerPtIteration1 { get; }
        public IRhinoDocContainer RhinoDocContainer { get; }

        public RhinoDoc ActiveDoc => RhinoDocContainer.RhinoDoc;

        public void Do(GetBoxDynamic planeDynamic, BoxDynamicParameters boxDynamicParameters)
        {
            // Add option to set xAxis
            var orientToggle = new OptionToggle(false, "No", "Yes");
            var orientXAxisToggleIdx = planeDynamic.AddOptionToggle("Orient", ref orientToggle);

            var getResult = planeDynamic.Get(false);
            if (getResult == GetResult.Option) // Choose to orient the plane to user input
            {
                Orient(planeDynamic.BoxCreation, planeDynamic, orientXAxisToggleIdx);
            }

            CornerPtIteration1.Iteration(planeDynamic,
                             boxDynamicParameters.BoxCorner1OffsetX,
                             boxDynamicParameters.BoxCorner1OffsetY,
                             out double xOffset,
                             out double yOffset);
            RecordOffsetValCorner1(boxDynamicParameters, xOffset, yOffset);
            planeDynamic.Iteration += 1;
        }

        private void Orient(IBoxCreationV2 boxCreation, GetBoxDynamic planeDynamic, int orientXAxisToggleIdx)
        {
            if (planeDynamic.OptionIndex() == orientXAxisToggleIdx)
            {
                planeDynamic.SetCommandPrompt("Pick origin");
                var getResult2 = planeDynamic.Get(true);
                if (getResult2 == GetResult.Point)
                {
                    var cplane = boxCreation.CPlane1;
                    cplane.Origin = planeDynamic.Point();
                    boxCreation.CPlane1 = cplane;
                    planeDynamic.SetCommandPrompt("Set X Axis");
                    planeDynamic.SetBasePoint(cplane.Origin, false);
                    var getResult3 = planeDynamic.Get(true);

                    if (getResult3 == GetResult.Point)
                    {
                        var xAxisEnd = planeDynamic.Point();
                        var newXAxis = xAxisEnd - cplane.Origin;
                        var angle = Vector3d.VectorAngle(cplane.XAxis, newXAxis);
                        cplane.Rotate(angle, cplane.ZAxis);
                        boxCreation.CPlane1 = cplane;
                        ActiveDoc.SetActiveViewConstructionPlane(boxCreation.CPlane1);
                    }
                }
            }
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
