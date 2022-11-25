using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class CornerPtIteration1: CornerPtIteration
    {

        protected override void SetCommandPrompt(GetBoxDynamic planeDynamic)
        {
            planeDynamic.SetCommandPrompt("Set Corner 1 of the Box");
        }

        protected override void OffsetBaseCorner(IBoxCreationV2 boxCreation, double dx, double dy)
        {
            boxCreation.OffsetBaseCorner1(dx, dy);
        }
    }

    public class CornerPtIteration2 : CornerPtIteration
    {
        protected override void SetCommandPrompt(GetBoxDynamic planeDynamic)
        {
            planeDynamic.SetCommandPrompt("Set Corner 2 of the Box");

        }

        protected override void OffsetBaseCorner(IBoxCreationV2 boxCreation, double dx, double dy)
        {
            boxCreation.OffsetBaseCorner2(dx, dy);

        }
    }

}
