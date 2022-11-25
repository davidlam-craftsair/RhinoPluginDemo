using DL.RhinoExcercise1.Core;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class EdgesFromFloorUpdateInfoGetter : IEdgesFromFloorUpdateInfoGetter
    {
        public EdgesFromFloorUpdateInfoGetter(IDocManager docManager,
            IClosedPlanarCurveAdditionBag closedPlanarCurveAdditionBag)
        {
            DocManager = docManager;
            ClosedPlanarCurveAdditionBag = closedPlanarCurveAdditionBag;
        }

        public IDocManager DocManager { get; }
        public IClosedPlanarCurveAdditionBag ClosedPlanarCurveAdditionBag { get; }

        public bool Get(IFloorUpdateInfo floorUpdateInfo, out IEnumerable<Curve> edges)
        {
            var b = DocManager.RhinoDoc.Get(GetEdgeIds(floorUpdateInfo), out IEnumerable<Curve> existingEdges);
            if (b)
            {
                var newClosedPlanarCurves = GetNewClosedPlanarCurves();
                var ts = existingEdges.ToList();
                ts.AddRange(newClosedPlanarCurves);
                edges = ts;
                ClosedPlanarCurveAdditionBag.Clear();
                return true;
            }
            edges = Enumerable.Empty<Curve>();
            return false;
        }

        private static IEnumerable<Guid> GetEdgeIds(IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            return rhinoFloorUpdateInfo.EdgeIds;
        }

        private IEnumerable<Curve> GetNewClosedPlanarCurves()
        {
            return ClosedPlanarCurveAdditionBag.GetAll().Select(x =>
            {
                var objRef = new ObjRef(x);
                return objRef.Curve();
            });
        }

    }
}
