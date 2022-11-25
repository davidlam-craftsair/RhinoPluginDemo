using DL.Framework;
using Rhino.Commands;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorCreateByBoundaryActionV3: IFloorCreateByBoundaryActionV2
    {
        public FloorCreateByBoundaryActionV3(IDocManager docManager, IFloorCreateByBoundaryAction floorCreateByBoundaryAction)
        {
            DocManager = docManager;
            FloorCreateByBoundaryAction = floorCreateByBoundaryAction;
        }


        public IDocManager DocManager { get; }
        public IFloorCreateByBoundaryAction FloorCreateByBoundaryAction { get; }

        public Result Create(IEnumerable<ObjRef> objRefs)
        {
            var curves = objRefs.Select(x => x.Curve()).Where(x => x != null);
            if (FloorCreateByBoundaryAction.Create(curves, DocManager) == Result.Success)
            {
                // Delete the sketch curves
                DocManager.RhinoDoc.Delete(objRefs);
                return Result.Success;
            }
            return Result.Cancel;
        }
    }
}
