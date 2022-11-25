using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepFloorFactoryV3 : IBrepFloorFactoryV2
    {
        public BrepFloorFactoryV3(IEdgesSorter edgesSorter,
                                  IBrepFactoryExt brepFloorFactory)
        {
            EdgesSorter = edgesSorter;
            BrepFactory = brepFloorFactory;
        }

        public IEdgesSorter EdgesSorter { get; }
        public IBrepFactoryExt BrepFactory { get; }

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
            var breps1 = BrepFactory.Create(new Curve[] { outerEdge }, floorHeight);
            var breps2 = BrepFactory.Create(innerEdges, floorHeight);
            var ts = breps1.Concat(breps2);
            if (ts == null)
            {
                return false;
            }
            ts.Select(x => AddToRhinoObjectTable(doc, x)).ToArray();
            return true;
        }

        private static Guid AddToRhinoObjectTable(IDocManager doc, Brep brep)
        {
            return doc.RhinoDoc.Add(brep);
        }

    }
}
