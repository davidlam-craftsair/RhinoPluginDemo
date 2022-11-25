using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorBoundaryCloseEditActionV4 : IFloorBoundaryCloseEditAction
    {
        public FloorBoundaryCloseEditActionV4(IDocManager docManager,
                                            IBrepFloorFactoryV2 brepFloorFactoryV2,
                                            IFloorBrepUpdateSubscriber floorBrepUpdateSubscriber,
                                            IClosedPlanarCurveAdditionBag closedPlanarCurveAdditionBag,
                                            IEdgesFromFloorUpdateInfoGetter edgesFromFloorUpdateInfoGetter)
        {
            DocManager = docManager;
            BrepFloorFactoryV2 = brepFloorFactoryV2;
            FloorBrepUpdateSubscriber = floorBrepUpdateSubscriber;
            ClosedPlanarCurveAdditionBag = closedPlanarCurveAdditionBag;
            EdgesFromFloorUpdateInfoGetter = edgesFromFloorUpdateInfoGetter;
            RhinoFloorUpdateInfos = new List<IFloorUpdateInfo>();
        }

        public IDocManager DocManager { get; }
        public IBrepFloorFactoryV2 BrepFloorFactoryV2 { get; }
        public IFloorBrepUpdateSubscriber FloorBrepUpdateSubscriber { get; }
        public IClosedPlanarCurveAdditionBag ClosedPlanarCurveAdditionBag { get; }
        public IEdgesFromFloorUpdateInfoGetter EdgesFromFloorUpdateInfoGetter { get; }
        public List<IFloorUpdateInfo> RhinoFloorUpdateInfos { get; }

        public Result Do()
        {
            GetRhinoFloorUpdateInfos();
            CommitUpdate();
            OnUpdateCommited();
            return Result.Success;
        }

        private void GetRhinoFloorUpdateInfos()
        {
            DocManager.SessionDoc.ActionOnRhinoFloorUpdateInfo(GetRhinoFloorUpdateInfos);
        }

        private void OnUpdateCommited()
        {
            DocManager.RhinoDoc.Redraw();
            {
                foreach (var item in RhinoFloorUpdateInfos)
                {
                    FloorBrepUpdateSubscriber.UnsubscribeUpdate(item);
                }
            }

            RhinoFloorUpdateInfos.Clear();
        }

        private void CommitUpdate()
        {
            foreach (var rhinoFloorUpdateInfo in RhinoFloorUpdateInfos)
            {
                // Retrieve curve geometry class
                if (GetAllEdges(rhinoFloorUpdateInfo, out IEnumerable<Curve> edges))
                {
                    CreateFloor(rhinoFloorUpdateInfo, edges);
                    DeleteOldFloor(rhinoFloorUpdateInfo.FloorComponentInfo.FloorRhinoId);
                    DeleteEdges(rhinoFloorUpdateInfo.EdgeIds);
                }
            }
        }

        /// <summary>
        /// Get all the edges, edges from the existing floor and also newly created ones 
        /// </summary>
        /// <param name="rhinoFloorUpdateInfo"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        private bool GetAllEdges(IFloorUpdateInfo rhinoFloorUpdateInfo, out IEnumerable<Curve> edges)
        {
            if (EdgesFromFloorUpdateInfoGetter.Get(rhinoFloorUpdateInfo, out IEnumerable<Curve> edgesTmp))
            {

            }
            edges = Enumerable.Empty<Curve>();
            return false;
        }

        private IEnumerable<Curve> GetNewClosedPlanarCurves()
        {
            return ClosedPlanarCurveAdditionBag.GetAll().Select(x =>
            {
                var objRef = new ObjRef(x);
                return objRef.Curve();
            });
        }

        /// <summary>
        /// Execute all updates that are recorded in IRhinoFloorUpdateInfo
        /// </summary>
        /// <param name="rhinoFloorUpdateInfo"></param>
        private void GetRhinoFloorUpdateInfos(IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            if (rhinoFloorUpdateInfo.NeedToUpdate)
            {
                RhinoFloorUpdateInfos.Add(rhinoFloorUpdateInfo);
            }
        }

        private static IEnumerable<Guid> GetEdgeIds(IFloorUpdateInfo rhinoFloorUpdateInfo)
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

        private void CreateFloor(IFloorUpdateInfo rhinoFloorUpdateInfo, IEnumerable<Curve> edges)
        {
            BrepFloorFactoryV2.CreateFloor(DocManager, edges, rhinoFloorUpdateInfo.Thickness);
        }
    }
}
