using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// Subscribe changes in the existing boundaries of floor
    /// </summary>
    public class FloorBoundaryOpenEditActionV2 : IFloorBoundaryOpenEditAction
    {
        public FloorBoundaryOpenEditActionV2(IFloorBrepUpdateSubscriber floorBrepUpdater,
                                            IClosedPlanarCurveAdditionBag closedPlanarCurveAdditionBag)
        {
            FloorBrepUpdater = floorBrepUpdater;
            ClosedPlanarCurveAdditionBag = closedPlanarCurveAdditionBag;
        }

        #region Properties
        public IFloorBrepUpdateSubscriber FloorBrepUpdater { get; }
        public IClosedPlanarCurveAdditionBag ClosedPlanarCurveAdditionBag { get; }

        #endregion

        /// <summary>
        /// Search and find the record of this floor object from ObjRef
        /// </summary>
        public Result Do(ObjRef objRef, IDocManager docManager)
        {
            // Retrieve floor component info
            IFloorComponentInfo floorComponentInfo = GetFloorComponentInfo(objRef, docManager);

            // subscribe change in edge change
            SubscribeChanges(docManager, floorComponentInfo);

            //  subscribe adding closed planar curve
            ListenToRhinoAddingClosedPlanarCurveAddition();
            return Result.Success;

        }

        private void ListenToRhinoAddingClosedPlanarCurveAddition()
        {
            ClosedPlanarCurveAdditionBag.EnableListening();
        }

        private static IFloorComponentInfo GetFloorComponentInfo(ObjRef objRef, IDocManager docManager)
        {
            return docManager.SessionDoc.GetFloorComponentInfoById(objRef.ObjectId);
        }

        private void SubscribeChanges(IDocManager docManager, IFloorComponentInfo floorComponentInfo)
        {
            FloorBrepUpdater.SubscribeUpdate(CreateRhinoFloorUpdateInfo(docManager, floorComponentInfo));
        }

        private static IFloorUpdateInfo CreateRhinoFloorUpdateInfo(IDocManager docManager, IFloorComponentInfo floorComponentInfo)
        {
            // Add curves to Rhino to get rhino object ids
            var outerEdgeId = docManager.RhinoDoc.Add(floorComponentInfo.OuterEdge);
            var innerEdgeIds = docManager.RhinoDoc.Add(floorComponentInfo.InnerEdges);

            var ts = new List<Guid>(innerEdgeIds)
            {
                outerEdgeId
            };

            var t = new RhinoFloorUpdateInfo(floorComponentInfo, ts, floorComponentInfo.Thickness);
            t.SetNeedToUpdate();
            return t;
        }
    }
}
