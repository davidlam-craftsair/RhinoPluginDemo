using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorBoundaryCloseEditAction : IFloorBoundaryCloseEditAction
    {
        public FloorBoundaryCloseEditAction(IDocManager docManager,
                                            IBrepFloorFactoryV2 brepFloorFactoryV2,
                                            IFloorBrepUpdateSubscriber floorBrepUpdateSubscriber)
        {
            DocManager = docManager;
            BrepFloorFactoryV2 = brepFloorFactoryV2;
            FloorBrepUpdateSubscriber = floorBrepUpdateSubscriber;
        }

        public IDocManager DocManager { get; }
        public IBrepFloorFactoryV2 BrepFloorFactoryV2 { get; }
        public IFloorBrepUpdateSubscriber FloorBrepUpdateSubscriber { get; }

        public Result Do()
        {
            // Get updated rhinoFloorUpdateInfo   
            DocManager.SessionDoc.ActionOnRhinoFloorUpdateInfo(CommitUpdateSubscribed);
            DocManager.RhinoDoc.Redraw();
            return Result.Success;
        }

        /// <summary>
        /// Execute all updates that are recorded in IRhinoFloorUpdateInfo
        /// </summary>
        /// <param name="rhinoFloorUpdateInfo"></param>
        private void CommitUpdateSubscribed(IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            if (rhinoFloorUpdateInfo.NeedToUpdate)
            {
                // Retrieve curve geometry class
                if (DocManager.RhinoDoc.Get(GetEdges(rhinoFloorUpdateInfo), out IEnumerable<Curve> edges))
                {
                    CreateFloor(rhinoFloorUpdateInfo, edges);
                    DeleteOldFloor(rhinoFloorUpdateInfo.FloorComponentInfo.FloorRhinoId);
                    DeleteEdges(rhinoFloorUpdateInfo.Edges);
                    // Clear RhinoFloorUpdateInfos in SessionDoc
                    //FloorBrepUpdateSubscriber.UnsubscribeUpdate(rhinoFloorUpdateInfo);

                }
                //rhinoFloorUpdateInfo.ResetNeedToUpdateToFalse();
            }
        }

        private static IEnumerable<Guid> GetEdges(IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            return rhinoFloorUpdateInfo.Edges;
        }

        private void DeleteEdges(IEnumerable<Guid> edges)
        {
            var rhinoDoc = DocManager.RhinoDoc;
            foreach (var item in edges)
            {
                rhinoDoc.Delete(item);
            }
        }

        private bool DeleteOldFloor(Guid floorRhinoId)
        {
            return DocManager.RhinoDoc.Delete(floorRhinoId);
        }

        private void CreateFloor(IRhinoFloorUpdateInfo rhinoFloorUpdateInfo, IEnumerable<Curve> edges)
        {
            BrepFloorFactoryV2.CreateFloor(DocManager, edges, rhinoFloorUpdateInfo.Thickness);
        }
    }
}
