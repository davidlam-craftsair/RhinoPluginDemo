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
            var planeDynamic = new GetPlaneDynamic(IoC.Get<IPlaneDrawer>(),
                                                  new BrepFaceGetterFactory().Create(ActiveDoc),
                                                  new OffsetDistancesGetterV6(),
                                                  new BoxCreation(),
                                                  new FrameAlignHandler(),
                                                  ActiveDoc);

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

        private static void Iteration1(GetPlaneDynamic planeDynamic, double xOffsetDefault, double yOffsetDefault, out double xOffset, out double yOffset)
        {
            planeDynamic.SetCommandPrompt("Set Corner 1 of the Box");
            AddOptionToggleOffset(planeDynamic, out OptionToggle boolOffsetCorner1, out int boolOffset1OptionIdx);
            planeDynamic.AcceptNothing(true);
            var shouldContinue = true;
            var xOffsetOption = new OptionDouble(xOffsetDefault);
            var yOffsetOption = new OptionDouble(yOffsetDefault);
            while (shouldContinue)
            {
                var res1 = planeDynamic.Get(true);
                // Get option of HasOffset, if true then add two OptionDouble x and y 
                if (res1 == GetResult.Option)
                {
                    if (boolOffset1OptionIdx == planeDynamic.OptionIndex())
                    {
                        if (boolOffsetCorner1.CurrentValue)
                        {
                            var xOffsetOptionIdx = planeDynamic.AddOptionDouble("x", ref xOffsetOption);
                            var yOffsetOptionIdx = planeDynamic.AddOptionDouble("y", ref yOffsetOption);

                            planeDynamic.AcceptNothing(true);
                            while (true)
                            {
                                var res2 = planeDynamic.Get();
                                if (res2 == GetResult.Option)
                                {
                                    if (xOffsetOptionIdx == planeDynamic.OptionIndex() ||
                                        yOffsetOptionIdx == planeDynamic.OptionIndex())
                                    {
                                        xOffsetDefault = xOffsetOption.CurrentValue;
                                        yOffsetDefault = yOffsetOption.CurrentValue;
                                        OffsetBaseCorner1(planeDynamic, xOffsetOption, yOffsetOption);
                                        
                                    }
                                }
                                else if (res2 == GetResult.Point)
                                {
                                    OffsetBaseCorner1(planeDynamic, xOffsetOption, yOffsetOption);
                                    // if point is clicked, then this stage for Corner1 is done and stop while loop for corner 1
                                    shouldContinue = false;
                                    break;
                                }
                                else if (res2 == GetResult.Nothing) // press enter if AcceptNothing set to true
                                {
                                    break;
                                }
                                else if (res2 == GetResult.Cancel)
                                {
                                    break;
                                }

                            }
                        }
                    }
                }
                else if (res1 == GetResult.Point)
                {
                    if (boolOffsetCorner1.CurrentValue)
                    {
                        OffsetBaseCorner1(planeDynamic, xOffsetOption, yOffsetOption);

                    }
                    break;
                }
                else if (res1 == GetResult.Nothing)
                {
                    if (boolOffsetCorner1.CurrentValue)
                    {
                        OffsetBaseCorner1(planeDynamic, xOffsetOption, yOffsetOption);

                    }
                    break;
                }
                else if (res1 == GetResult.Cancel)
                {
                    break;
                }

            }
            xOffset = xOffsetDefault;
            yOffset = yOffsetDefault;
        }

        private static void Iteration2(GetPlaneDynamic planeDynamic, double xOffsetDefault, double yOffsetDefault, out double xOffset, out double yOffset)
        {
            planeDynamic.SetCommandPrompt("Set Corner 2 of the Box");
            AddOptionToggleOffset(planeDynamic, out OptionToggle boolOffsetCorner2, out int boolOffset2OptionIdx);
            planeDynamic.AcceptNothing(true);
            var shouldContinue = true;
            while (shouldContinue)
            {
                var res1 = planeDynamic.Get(true);
                var xOffsetOption = new OptionDouble(xOffsetDefault);
                var yOffsetOption = new OptionDouble(yOffsetDefault);
                // Get option of HasOffset, if true then add two OptionDouble x and y 
                if (res1 == GetResult.Option)
                {
                    if (boolOffset2OptionIdx == planeDynamic.OptionIndex())
                    {
                        if (boolOffsetCorner2.CurrentValue)
                        {
                            var xOffsetOptionIdx = planeDynamic.AddOptionDouble("x", ref xOffsetOption);
                            var yOffsetOptionIdx = planeDynamic.AddOptionDouble("y", ref yOffsetOption);

                            planeDynamic.AcceptNothing(true);
                            while (true)
                            {
                                var res2 = planeDynamic.Get();
                                if (res2 == GetResult.Option)
                                {
                                    if (xOffsetOptionIdx == planeDynamic.OptionIndex() ||
                                        yOffsetOptionIdx == planeDynamic.OptionIndex())
                                    {
                                        xOffsetDefault = xOffsetOption.CurrentValue;
                                        yOffsetDefault = yOffsetOption.CurrentValue;
                                        OffsetBaseCorner2(planeDynamic, xOffsetOption, yOffsetOption);
                                    }
                                }
                                else if (res2 == GetResult.Point)
                                {
                                    OffsetBaseCorner2(planeDynamic, xOffsetOption, yOffsetOption);
                                    // if point is clicked, then this stage for Corner1 is done and stop while loop for corner 1
                                    shouldContinue = false;
                                    break;
                                }
                                else if (res2 == GetResult.Nothing) // press enter if AcceptNothing set to true
                                {
                                    break;
                                }
                                else if (res2 == GetResult.Cancel)
                                {
                                    break;
                                }

                            }
                        }
                    }
                }
                else if (res1 == GetResult.Point)
                {
                    if (boolOffsetCorner2.CurrentValue)
                    {
                        OffsetBaseCorner2(planeDynamic, xOffsetOption, yOffsetOption);

                    }
                    break;
                }
                else if (res1 == GetResult.Nothing)
                {
                    if (boolOffsetCorner2.CurrentValue)
                    {
                        OffsetBaseCorner2(planeDynamic, xOffsetOption, yOffsetOption);

                    }
                    break;
                }
                else if (res1 == GetResult.Cancel)
                {
                    break;
                }

            }
            xOffset = xOffsetDefault;
            yOffset = yOffsetDefault;
        }

        private static void AddOptionToggleOffset(GetPlaneDynamic planeDynamic, out OptionToggle boolOffset, out int boolOffsetOptionIdx)
        {
            boolOffset = new OptionToggle(false, "No", "Yes");
            boolOffsetOptionIdx = planeDynamic.AddOptionToggle("HasOffset", ref boolOffset);
        }

        private static void OffsetBaseCorner1(GetPlaneDynamic planeDynamic, OptionDouble xOffsetOption, OptionDouble yOffsetOption)
        {
            planeDynamic.OffsetBaseCorner1(xOffsetOption.CurrentValue, yOffsetOption.CurrentValue);
        }


        private static void OffsetBaseCorner2(GetPlaneDynamic planeDynamic, OptionDouble xOffsetOption, OptionDouble yOffsetOption)
        {
            planeDynamic.OffsetBaseCorner2(xOffsetOption.CurrentValue, yOffsetOption.CurrentValue);
        }


        public override string EnglishName => "BoxDynamic";
    }
}
