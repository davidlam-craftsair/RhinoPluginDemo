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
    public class FloorBoundaryCloseEditActionV2 : IFloorBoundaryCloseEditAction
    {
        public FloorBoundaryCloseEditActionV2(IDocManager docManager,
                                            IBrepFloorFactoryV2 brepFloorFactoryV2,
                                            IFloorBrepUpdateSubscriber floorBrepUpdateSubscriber)
        {
            DocManager = docManager;
            BrepFloorFactoryV2 = brepFloorFactoryV2;
            FloorBrepUpdateSubscriber = floorBrepUpdateSubscriber;
            RhinoFloorUpdateInfos = new List<IRhinoFloorUpdateInfo>();
        }

        public IDocManager DocManager { get; }
        public IBrepFloorFactoryV2 BrepFloorFactoryV2 { get; }
        public IFloorBrepUpdateSubscriber FloorBrepUpdateSubscriber { get; }
        public List<IRhinoFloorUpdateInfo> RhinoFloorUpdateInfos { get; }

        public Result Do()
        {
            DocManager.SessionDoc.ActionOnRhinoFloorUpdateInfo(GetRhinoFloorUpdateInfos);
            CommitUpdate();
            OnUpdateCommited();
            return Result.Success;
        }

        private void OnUpdateCommited()
        {
            DocManager.RhinoDoc.Redraw();
            foreach (var item in RhinoFloorUpdateInfos)
            {
                FloorBrepUpdateSubscriber.UnsubscribeUpdate(item);
            }
            RhinoFloorUpdateInfos.Clear();
        }

        private void CommitUpdate()
        {
            foreach (var rhinoFloorUpdateInfo in RhinoFloorUpdateInfos)
            {
                // Retrieve curve geometry class
                if (DocManager.RhinoDoc.Get(GetEdgeIds(rhinoFloorUpdateInfo), out IEnumerable<Curve> edges))
                {
                    CreateFloor(rhinoFloorUpdateInfo, edges);
                    DeleteOldFloor(rhinoFloorUpdateInfo.FloorComponentInfo.FloorRhinoId);
                    DeleteEdges(rhinoFloorUpdateInfo.EdgeIds);
                }
            }
        }

        /// <summary>
        /// Execute all updates that are recorded in IRhinoFloorUpdateInfo
        /// </summary>
        /// <param name="rhinoFloorUpdateInfo"></param>
        private void GetRhinoFloorUpdateInfos(IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            if (rhinoFloorUpdateInfo.NeedToUpdate)
            {
                RhinoFloorUpdateInfos.Add(rhinoFloorUpdateInfo);

            }
        }

        private static IEnumerable<Guid> GetEdgeIds(IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            return rhinoFloorUpdateInfo.EdgeIds;
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
