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
    public class BrepFloorFactoryV5 : IBrepFloorFactoryV2
    {
        public BrepFloorFactoryV5(IEdgesSorter edgesSorter,
                                  IBrepFloorFactory brepFloorFactory)
        {
            EdgesSorter = edgesSorter;
            BrepFloorFactory = brepFloorFactory;
        }

        public IEdgesSorter EdgesSorter { get; }
        public IBrepFloorFactory BrepFloorFactory { get; }

        public bool CreateFloor(IDocManager doc, IEnumerable<Curve> edges, double floorHeight)
        {
            if (EdgesSorter.Find(edges, out Curve outerEdge, out IEnumerable<Curve> innerEdges))
            {
                if (CreateFloor(doc, outerEdge, innerEdges, floorHeight))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Create floor from an outer edge and inner edges, and then add to rhino object table
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="outerEdge"></param>
        /// <param name="innerEdges"></param>
        /// <param name="floorHeight"></param>
        /// <returns></returns>
        private bool CreateFloor(IDocManager doc, Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight)
        {
            var brep = BrepFloorFactory.Create(outerEdge, innerEdges, floorHeight);
            if (brep == null)
            {
                return false;
            }
            var floorBrepId = AddToRhinoObjectTable(doc, brep);
            var componentInfo = CreateFloorComponentInfo(outerEdge, innerEdges, floorHeight, floorBrepId);
            doc.SessionDoc.Add(componentInfo);
            return true;
        }

        private static IFloorComponentInfo CreateFloorComponentInfo(Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight, System.Guid floorBrepId)
        {
            return new FloorComponentInfo(floorBrepId, outerEdge, innerEdges, floorHeight);
        }

        private static Guid AddToRhinoObjectTable(IDocManager doc, Brep brep)
        {
            return doc.RhinoDoc.Add(brep);
        }

    }
}
