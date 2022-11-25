using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamicIteration0V2
    {
        public void Do(BoxCreationV2 boxCreation, GetBoxDynamic planeDynamic, RhinoDoc rhinoDoc)
        {
            planeDynamic.SetCommandPrompt("Set Plane of the Box");
            // Add option to set xAxis
            var orientToggle = new OptionToggle(false, "No", "Yes");
            var orientXAxisToggleIdx = planeDynamic.AddOptionToggle("Orient", ref orientToggle);

            var getResult = planeDynamic.Get(false);
            if (getResult == GetResult.Option) // Choose to orient the plane to user input
            {
                Orient(boxCreation, planeDynamic, orientXAxisToggleIdx);
            }
            rhinoDoc.SetActiveViewConstructionPlane(boxCreation.CPlane1);
            planeDynamic.ConstrainToConstructionPlane(true);
            planeDynamic.Iteration += 1;
        }

        private static void Orient(IBoxCreationV2 boxCreation, GetBoxDynamic planeDynamic, int orientXAxisToggleIdx)
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
                        cplane.XAxis = xAxisEnd - cplane.Origin;
                    }
                }
            }
        }
    }
}
