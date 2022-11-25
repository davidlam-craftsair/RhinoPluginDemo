using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public class FloorBoundaryOpenEditAction : IFloorBoundaryOpenEditAction
    {
        public FloorBoundaryOpenEditAction(IFloorBrepUpdateSubscriber floorBrepUpdater)
        {
            FloorBrepUpdater = floorBrepUpdater;
        }

        public IFloorBrepUpdateSubscriber FloorBrepUpdater { get; }

        public Result Do(ObjRef objRef, IDocManager docManager)
        {
            // Retrieve floor component info
            var floorComponentInfo = docManager.SessionDoc.GetFloorComponentInfoById(objRef.ObjectId);

            // subscribe change in edge change
            FloorBrepUpdater.SubscribeUpdate(CreateRhinoFloorUpdateInfo(docManager, floorComponentInfo));

            //
            return Result.Success;

        }

        private static IRhinoFloorUpdateInfo CreateRhinoFloorUpdateInfo(IDocManager docManager, IFloorComponentInfo floorComponentInfo)
        {
            // Add curves to Rhino to get rhino object ids
            var outerEdgeId = docManager.RhinoDoc.Add(floorComponentInfo.OuterEdge);
            var innerEdgeIds = docManager.RhinoDoc.Add(floorComponentInfo.InnerEdges);
            var ts = new List<Guid>(innerEdgeIds)
            {
                outerEdgeId
            };

            return new RhinoFloorUpdateInfo(floorComponentInfo, ts, floorComponentInfo.Thickness);
        }
    }
}
