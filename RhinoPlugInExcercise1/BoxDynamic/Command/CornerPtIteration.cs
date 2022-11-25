using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public abstract class CornerPtIteration
    {
        public void Iteration(GetBoxDynamic planeDynamic, double xOffsetDefault, double yOffsetDefault, out double xOffset, out double yOffset)
        {
            SetCommandPrompt(planeDynamic);
            AddOptionToggleOffset(planeDynamic, out OptionToggle boolOffsetCorner, out int boolOffset1OptionIdx);
            planeDynamic.AcceptNothing(true);
            var shouldContinue = true;
            var xOffsetOption = new OptionDouble(xOffsetDefault);
            var yOffsetOption = new OptionDouble(yOffsetDefault);
            IBoxCreationV2 boxCreation = planeDynamic.BoxCreation;
            while (shouldContinue)
            {
                var res1 = planeDynamic.Get(true);
                // Get option of HasOffset, if true then add two OptionDouble x and y 
                if (res1 == GetResult.Option)
                {
                    if (boolOffset1OptionIdx == planeDynamic.OptionIndex())
                    {
                        if (boolOffsetCorner.CurrentValue)
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
                                        OffsetBaseCorner(boxCreation, xOffsetOption.CurrentValue, yOffsetOption.CurrentValue);

                                    }
                                }
                                else if (res2 == GetResult.Point)
                                {
                                    OffsetBaseCorner(boxCreation, xOffsetOption.CurrentValue, yOffsetOption.CurrentValue);
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
                    if (boolOffsetCorner.CurrentValue)
                    {
                        OffsetBaseCorner(boxCreation, xOffsetOption.CurrentValue, yOffsetOption.CurrentValue);

                    }
                    break;
                }
                else if (res1 == GetResult.Nothing)
                {
                    if (boolOffsetCorner.CurrentValue)
                    {
                        OffsetBaseCorner(boxCreation, xOffsetOption.CurrentValue, yOffsetOption.CurrentValue);
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

        protected abstract void SetCommandPrompt(GetBoxDynamic planeDynamic);
        

        protected abstract void OffsetBaseCorner(IBoxCreationV2 boxCreation, double dx, double dy);

        private static void AddOptionToggleOffset(GetBoxDynamic planeDynamic, out OptionToggle boolOffset, out int boolOffsetOptionIdx)
        {
            boolOffset = new OptionToggle(false, "No", "Yes");
            boolOffsetOptionIdx = planeDynamic.AddOptionToggle("HasOffset", ref boolOffset);
        }
    }
}
