using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("076679B7-F2B2-486D-95C6-2FFE9ABCA1C2")]
    public class RcPullFace: AbsCommand
    {
        protected override Result DoRunCommand()
        {
            var pullFaceCreation = new PullFaceCreation(new ExtrusionCreation(),
                                                        Get<IPullFaceFromExtrusionHandler>(),
                                                        Get<IPullFaceFromBrepHandler>());
            var drawer = Get<IDrawer>();
            var pullFaceDynamic = new PullFaceDynamic(new PullFaceMouseMoveInteractionIteration0(pullFaceCreation, new BrepFaceGetterFactory(Get<IRhinoTolerance>()).Create()),
                                                      new PullFaceDynamicDrawInteraction(pullFaceCreation,
                                                                                         Get<IDrawer>(),
                                                                                         new PullFaceDynamicDrawInteractionIteration0(pullFaceCreation, drawer),
                                                                                         new PullFaceDynamicDrawInteractionIteration1(pullFaceCreation, drawer)),
                                                      pullFaceCreation
                                                      );
            
            // 
            Iteration0(pullFaceCreation, pullFaceDynamic);
            if (pullFaceDynamic.CommandResult() != Result.Success)
            {
                return pullFaceDynamic.CommandResult();
            }


            pullFaceDynamic.Get(true);

            if (pullFaceDynamic.CommandResult() == Result.Success)
            {
                if (ReplaceObj(pullFaceCreation))
                {
                    return Result.Success;
                }
            }
            return Result.Cancel;

        }

        private void Iteration0(IPullFaceCreation extrusionCreation, PullFaceDynamic pullFaceDynamic)
        {
            pullFaceDynamic.SetCommandPrompt("Pull face");
            var getResult = pullFaceDynamic.Get(true);  // Get the face to pull

            ActiveDoc.SetActiveViewConstructionPlane(extrusionCreation.CPlane);
            Plane cplane = extrusionCreation.CPlane;
            pullFaceDynamic.Constrain(new Line(cplane.Origin, cplane.ZAxis));

            pullFaceDynamic.Iteration += 1;
        }

        private bool ReplaceObj(IPullFaceCreation pullFaceCreation)
        {
            var resultantBrep = pullFaceCreation.GetResultantBrep(ActiveDoc);
            if (resultantBrep != null)
            {
                return ActiveDoc.Objects.Replace(pullFaceCreation.ObjId, resultantBrep);
            }
            return false;
        }

        public override string EnglishName => "PullFace";
    }
}
