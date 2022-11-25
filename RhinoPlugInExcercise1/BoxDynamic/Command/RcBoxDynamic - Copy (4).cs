using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Render;
using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace RhinoPlugInExcercise1
{
    [Guid("2EE72D28-B4B7-457F-9AFC-CBD63F537630")]
    public class RcBoxDynamic : AbsCommand
    {
        protected override Result DoRunCommand()
        {
            var boxCreationV2 = new BoxCreationV2(new BoxCreation());
            var planeDynamic = new GetBoxDynamic(Get<IPlaneDrawer>(),
                                                  new BrepFaceGetterFactory(Get<IRhinoTolerance>()).Create(),
                                                  new OffsetDistancesGetterV6(),
                                                  boxCreationV2,
                                                  new FrameAlignHandler(),
                                                  new BoxDynamicMouseMoveInteractionV2(Get<IPlaneDrawer>(), 
                                                                              new BrepFaceGetterFactory(Get<IRhinoTolerance>()).Create(),
                                                                              new FrameAlignHandler(),
                                                                              boxCreationV2
                                                                                ),
                                                  new BoxDynamicDrawInteractionFactory().Create(boxCreationV2)
                                                  );
            BoxDynamicParameters boxDynamicParameters = GetBoxDynamicParameters();

            // Iteration 0
            planeDynamic.SetCommandPrompt("Set Plane of the Box");
            planeDynamic.Get(false);
            if (planeDynamic.CommandResult() != Result.Success)
            {
                return planeDynamic.CommandResult();
            }
            planeDynamic.RegisterCPlane1();
            planeDynamic.View().ActiveViewport.SetConstructionPlane(planeDynamic.CPlane1);
            planeDynamic.ConstrainToConstructionPlane(true);

            planeDynamic.Iteration += 1;



            // Iteration 1

            // Add HasOffset
            Iteration1(planeDynamic,
                       boxDynamicParameters.BoxCorner1OffsetX,
                       boxDynamicParameters.BoxCorner1OffsetY,
                       out double xOffsetCorner1,
                       out double yOffsetCorner1);
            RecordOffsetValCorner1(boxDynamicParameters, xOffsetCorner1, yOffsetCorner1);

            if (planeDynamic.CommandResult() != Result.Success)
            {
                return planeDynamic.CommandResult();
            }
            planeDynamic.Iteration += 1;



            // Iteration 2
            planeDynamic.ClearCommandOptions();  // Clear command options for planeDynamic before call Get() for corner 2
            Iteration2(planeDynamic,
                       boxDynamicParameters.BoxCorner2OffsetX,
                       boxDynamicParameters.BoxCorner2OffsetY,
                       out double xOffsetCorner2,
                       out double yOffsetCorner2);
            RecordOffsetValCorner2(boxDynamicParameters, xOffsetCorner2, yOffsetCorner2);

            if (planeDynamic.CommandResult() != Result.Success)
            {
                return planeDynamic.CommandResult();
            }
            planeDynamic.Iteration += 1;



            // Iteration 3
            planeDynamic.ClearCommandOptions();
            planeDynamic.SetCommandPrompt("Set Top Corner of the Box");
            // settings for planeDynamic for top corner of the box
            planeDynamic.RegisterCPlane2();
            planeDynamic.View().ActiveViewport.SetConstructionPlane(planeDynamic.CPlane2);
            planeDynamic.SetBasePoint(planeDynamic.BaseCorner2, true);
            planeDynamic.ConstrainToConstructionPlane(true);
            planeDynamic.Constrain(new Line(planeDynamic.BaseCorner2, planeDynamic.CPlane1.ZAxis));

            planeDynamic.Get(true);
            planeDynamic.Iteration += 1;
            SaveBoxDynamicParameters(boxDynamicParameters);

            if (planeDynamic.CommandResult() == Result.Success)
            {
                if (planeDynamic.Box.IsValid)
                {
                    ActiveDoc.Objects.AddBox(planeDynamic.Box);
                    return Result.Success;
                }
            }
            return Result.Cancel;

        }

        private void SaveBoxDynamicParameters(BoxDynamicParameters boxDynamicParameters)
        {
            Settings.SetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetX, boxDynamicParameters.BoxCorner1OffsetX);
            Settings.SetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetY, boxDynamicParameters.BoxCorner1OffsetY);
            Settings.SetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetX, boxDynamicParameters.BoxCorner2OffsetX);
            Settings.SetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetY, boxDynamicParameters.BoxCorner2OffsetY);
        }

        private void RecordOffsetValCorner1(BoxDynamicParameters boxDynamicParameters, double xOffsetCorner1, double yOffsetCorner1)
        {
            boxDynamicParameters.BoxCorner1OffsetX = xOffsetCorner1;
            boxDynamicParameters.BoxCorner1OffsetY = yOffsetCorner1;

            // ChangeOffsetValCorner2 to align with changes in Corner1
            boxDynamicParameters.BoxCorner2OffsetX = -xOffsetCorner1;
            boxDynamicParameters.BoxCorner2OffsetY = -yOffsetCorner1;

        }

        private static void RecordOffsetValCorner2(BoxDynamicParameters boxDynamicParameters, double xOffsetCorner2, double yOffsetCorner2)
        {
            boxDynamicParameters.BoxCorner2OffsetX = xOffsetCorner2;
            boxDynamicParameters.BoxCorner2OffsetY = yOffsetCorner2;
        }

        private BoxDynamicParameters GetBoxDynamicParameters(double t=0.3)
        {
            return new BoxDynamicParameters(Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetX, t),
                                            Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetY, -t),
                                            Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetX, -t),
                                            Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetY, t)
                                            );
        }

        private static void Iteration1(GetBoxDynamic planeDynamic, double xOffsetDefault, double yOffsetDefault, out double xOffset, out double yOffset)
        {
            new CornerPtIteration1().Iteration(planeDynamic, xOffsetDefault, yOffsetDefault, out xOffset, out yOffset);
        }

        private static void Iteration2(GetBoxDynamic planeDynamic, double xOffsetDefault, double yOffsetDefault, out double xOffset, out double yOffset)
        {
            new CornerPtIteration2().Iteration(planeDynamic, xOffsetDefault, yOffsetDefault, out xOffset, out yOffset);
        }

        public override string EnglishName => "BoxDynamic";
    }
}
