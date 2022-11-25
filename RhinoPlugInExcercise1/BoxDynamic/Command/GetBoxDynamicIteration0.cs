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
    public class GetBoxDynamicIteration0 : IGetBoxDynamicIteration0
    {
        public GetBoxDynamicIteration0(IRhinoDocContainer rhinoDocContainer)
        {
            RhinoDocContainer = rhinoDocContainer;
        }

        public IRhinoDocContainer RhinoDocContainer { get; }

        public void Do(IBoxCreationV2 boxCreation, GetBoxDynamic planeDynamic, RhinoDoc rhinoDoc)
        {
            planeDynamic.SetCommandPrompt("Set Plane of the Box");
            planeDynamic.Get(false);
            rhinoDoc.SetActiveViewConstructionPlane(boxCreation.CPlane1);
            planeDynamic.ConstrainToConstructionPlane(true);
            planeDynamic.Iteration += 1;
        }

        public void Do(IBoxCreationV2 boxCreation, GetBoxDynamic planeDynamic)
        {
            planeDynamic.SetCommandPrompt("Set Plane of the Box");
            planeDynamic.Get(false);
            RhinoDocContainer.RhinoDoc.SetActiveViewConstructionPlane(boxCreation.CPlane1);
            planeDynamic.ConstrainToConstructionPlane(true);
            planeDynamic.Iteration += 1;
        }
    }
}
