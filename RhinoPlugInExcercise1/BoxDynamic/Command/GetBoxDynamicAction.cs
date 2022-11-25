using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetBoxDynamicAction
    {
        public GetBoxDynamicAction(GetBoxDynamic getBoxDynamic, 
            BoxDynamicParameters boxDynamicParameters,
            IRhinoDocContainer rhinoDocContainer,
            IBoxCreationV2 boxCreation,
            IGetBoxDynamicIteration0 getBoxDynamicIteration0,
            IGetBoxDynamicIteration1 getBoxDynamicIteration1,
            IGetBoxDynamicIteration2 getBoxDynamicIteration2,
            IGetBoxDynamicIteration3 getBoxDynamicIteration3
            )
        {
            GetBoxDynamic = getBoxDynamic;
            BoxDynamicParameters = boxDynamicParameters;
            RhinoDocContainer = rhinoDocContainer;
            BoxCreation = boxCreation;
            GetBoxDynamicIteration0 = getBoxDynamicIteration0;
            GetBoxDynamicIteration1 = getBoxDynamicIteration1;
            GetBoxDynamicIteration2 = getBoxDynamicIteration2;
            GetBoxDynamicIteration3 = getBoxDynamicIteration3;
        }

        public GetBoxDynamic GetBoxDynamic { get; }
        public BoxDynamicParameters BoxDynamicParameters { get; }
        public IRhinoDocContainer RhinoDocContainer { get; }
        public IBoxCreationV2 BoxCreation { get; }
        public IGetBoxDynamicIteration0 GetBoxDynamicIteration0 { get; }
        public IGetBoxDynamicIteration1 GetBoxDynamicIteration1 { get; }
        public IGetBoxDynamicIteration2 GetBoxDynamicIteration2 { get; }
        public IGetBoxDynamicIteration3 GetBoxDynamicIteration3 { get; }

        public RhinoDoc ActiveDoc => RhinoDocContainer.RhinoDoc;

        public Result Do(PersistentSettings persistentSettings)
        {
            // Iteration 0  Construction Plane for box
            Iteration0(BoxCreation, GetBoxDynamic);
            if (GetBoxDynamic.CommandResult() != Result.Success)
            {
                return GetBoxDynamic.CommandResult();
            }


            // Iteration 1 CornerPt1
            Iteration1(GetBoxDynamic, BoxDynamicParameters);
            if (GetBoxDynamic.CommandResult() != Result.Success)
            {
                return GetBoxDynamic.CommandResult();
            }


            GetBoxDynamic.ClearCommandOptions();  // Clear command options for planeDynamic before call Get() for corner 2

            // Iteration 2 CornerPt2
            Iteration2(GetBoxDynamic, BoxDynamicParameters);
            if (GetBoxDynamic.CommandResult() != Result.Success)
            {
                return GetBoxDynamic.CommandResult();
            }


            GetBoxDynamic.ClearCommandOptions();


            // Iteration 3 TopCorner
            Iteration3(GetBoxDynamic, BoxDynamicParameters, persistentSettings);

            if (GetBoxDynamic.CommandResult() == Result.Success)
            {
                Box box = BoxCreation.Get();
                if (box.IsValid)
                {
                    ActiveDoc.Objects.AddBox(box);
                    return Result.Success;
                }
            }
            return Result.Cancel;
        }

        private void Iteration3(GetBoxDynamic getBoxDynamic, BoxDynamicParameters boxDynamicParameters, PersistentSettings persistentSettings)
        {
            GetBoxDynamicIteration3.Do(getBoxDynamic, boxDynamicParameters, persistentSettings);
        }

        private void Iteration2(GetBoxDynamic getBoxDynamic, BoxDynamicParameters boxDynamicParameters)
        {
            GetBoxDynamicIteration2.Do(getBoxDynamic, boxDynamicParameters);
        }

        private void Iteration1(GetBoxDynamic getBoxDynamic, BoxDynamicParameters boxDynamicParameters)
        {
            GetBoxDynamicIteration1.Do(getBoxDynamic, boxDynamicParameters);
        }

        private void Iteration0(IBoxCreationV2 boxCreation, GetBoxDynamic planeDynamic)
        {
            GetBoxDynamicIteration0.Do(boxCreation, planeDynamic, RhinoDocContainer.RhinoDoc);
        }

    }
}
