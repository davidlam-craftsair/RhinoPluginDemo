using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("2EE72D28-B4B7-457F-9AFC-CBD63F537630")]
    public class RcBoxDynamic : AbsCommand
    {
        protected override Result DoRunCommand()
        {
            var planeDynamic = new GetPlaneDynamic(new RelevantBrepFaceClosestToViewCursorGetter(ActiveDoc.ModelAbsoluteTolerance), 
                                                  new RelevantClosestObjToViewCursorGetterFactory().Create(),
                                                  IoC.Get<IPlaneDrawer>(),
                                                  new BrepFaceGetterFactory().Create(ActiveDoc),
                                                  new OffsetDistancesGetterV4(),
                                                  ActiveDoc);
            planeDynamic.SetCommandPrompt("Set Plane of the Box");
            planeDynamic.AcceptNothing(true);
            planeDynamic.Get(true);
            planeDynamic.Iteration += 1;

            planeDynamic.SetCommandPrompt("Set Corner 1 of the Box");
            planeDynamic.AcceptNothing(true);
            planeDynamic.Get(true);
            planeDynamic.Iteration += 1;

            planeDynamic.SetCommandPrompt("Set Corner 2 of the Box");
            planeDynamic.AcceptNothing(false);
            planeDynamic.Get(true);
            planeDynamic.Iteration += 1;

            planeDynamic.SetCommandPrompt("Set Top Corner of the Box");
            planeDynamic.AcceptNothing(false);
            planeDynamic.Get(true);
            planeDynamic.Iteration += 1;

            //if (planeDynamic.CommandResult() != Result.Success)
            //    return planeDynamic.CommandResult();
            //planeDynamic.AcceptNothing(false);
            //planeDynamic.AcceptEnterWhenDone(true);

            //while (planeDynamic.Iteration < 4)
            //{
            //    if (GetResult.Cancel == planeDynamic.Get(true))
            //    {
            //        return Result.Failure;
            //    }
            //    planeDynamic.Iteration += 1;
            //}

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

        public override string EnglishName => "BoxDynamic";
    }
}
