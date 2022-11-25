using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System.Collections.Generic;
using System.Linq;

namespace RhinoPlugInExcercise1
{
    public class FloorCreateByBoundaryActionOriginalV2 : IFloorCreateByBoundaryAction
    {
        public IDocManager DocManager { get; }
        public IBrepFloorFactoryV2 BrepFloorFactory { get; }

        public FloorCreateByBoundaryActionOriginalV2(
                                            IDocManager docManager,
                                           IBrepFloorFactoryV2 brepFloorFactory
                                           )
        {
            DocManager = docManager;
            BrepFloorFactory = brepFloorFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges">include 1 outer edge and inner edges</param>
        /// <returns></returns>
        public Result Create(IEnumerable<Curve> edges, IDocManager doc)
        {
            var gn = new GetNumber();
            gn.SetCommandPrompt("Input floor height");
            gn.SetDefaultNumber(0.15);
            gn.Get();
            if (gn.CommandResult() == Result.Success)
            {
                var floorHeight = gn.Number();
                if (CreateFloor(edges, floorHeight))
                {
                    return Result.Success;
                }
                return Result.Failure;

            }

            return Result.Cancel;


        }

        private bool CreateFloor(IEnumerable<Curve> innerEdges, double floorHeight)
        {
            return BrepFloorFactory.CreateFloor(DocManager, innerEdges, floorHeight);
        }

    }
}
