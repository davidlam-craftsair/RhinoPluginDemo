using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("2EE72D28-B4B7-457F-9AFC-CBD63F537630")]
    public class RcBoxDynamic : AbsCommand
    {
        public RcBoxDynamic()
        {
        }
        protected override Result DoRunCommand()
        {
            BoxCreationV2 boxCreation = GetBoxCreation();
            var action = new GetBoxDynamicAction(GetBoxDynamic(boxCreation),
                GetBoxDynamicParameters(),
                Get<IRhinoDocContainer>(),
                boxCreation,
                new GetBoxDynamicIteration0(Get<IRhinoDocContainer>()),
                new GetBoxDynamicIteration1V2(new CornerPtIteration1(), Get<IRhinoDocContainer>()),
                new GetBoxDynamicIteration2(new CornerPtIteration2()),
                new GetBoxDynamicIteration3(Get<IRhinoDocContainer>())
                );
            return action.Do(Settings);

        }

        private GetBoxDynamic GetBoxDynamic(BoxCreationV2 boxCreation)
        {
            return new GetBoxDynamic(boxCreation,
                                     new BoxDynamicMouseMoveInteractionV2(Get<IPlaneDrawer>(),
                                                                         new BrepFaceGetterFactory(Get<IRhinoTolerance>()).Create(),
                                                                         new FrameAlignHandler(),
                                                                         boxCreation
                                                                         ),
                                     new BoxDynamicDrawInteractionFactory().Create(boxCreation)
                                     );
        }

        private static BoxCreationV2 GetBoxCreation()
        {
            return new BoxCreationV2(new BoxCreation());
        }


        private BoxDynamicParameters GetBoxDynamicParameters(double t=0.3)
        {
            return new BoxDynamicParameters(Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetX, t),
                                            Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetY, -t),
                                            Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetX, -t),
                                            Settings.GetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetY, t)
                                            );
        }



        public override string EnglishName => "BoxDynamic";

    }
}
