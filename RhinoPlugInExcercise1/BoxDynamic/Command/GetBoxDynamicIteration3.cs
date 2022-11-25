using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamicIteration3: IGetBoxDynamicIteration3
    {
        public GetBoxDynamicIteration3(IRhinoDocContainer rhinoDocContainer)
        {
            RhinoDocContainer = rhinoDocContainer;
        }

        public IRhinoDocContainer RhinoDocContainer { get; }
        public RhinoDoc ActiveDoc => RhinoDocContainer.RhinoDoc;

        public void Do(GetBoxDynamic planeDynamic, BoxDynamicParameters boxDynamicParameters, PersistentSettings settings)
        {
            var boxCreation = planeDynamic.BoxCreation;
            planeDynamic.SetCommandPrompt("Set Top Corner of the Box");
            // settings for planeDynamic for top corner of the box
            ActiveDoc.SetActiveViewConstructionPlane(boxCreation.CPlane2);
            planeDynamic.SetBasePoint(boxCreation.BaseCorner2, true);
            planeDynamic.ConstrainToConstructionPlane(true);
            // Constrain to a line from BaseCorner2 and CPlane1 ZAxis
            planeDynamic.Constrain(new Line(boxCreation.BaseCorner2, boxCreation.CPlane1.ZAxis));

            planeDynamic.Get(true);
            planeDynamic.Iteration += 1;
            SaveBoxDynamicParameters(boxDynamicParameters, settings);
        }

        private void SaveBoxDynamicParameters(BoxDynamicParameters boxDynamicParameters, PersistentSettings settings)
        {
            settings.SetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetX, boxDynamicParameters.BoxCorner1OffsetX);
            settings.SetDouble(BoxDynamicSettingKeys.BoxCorner1OffsetY, boxDynamicParameters.BoxCorner1OffsetY);
            settings.SetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetX, boxDynamicParameters.BoxCorner2OffsetX);
            settings.SetDouble(BoxDynamicSettingKeys.BoxCorner2OffsetY, boxDynamicParameters.BoxCorner2OffsetY);
        }
    }
}
