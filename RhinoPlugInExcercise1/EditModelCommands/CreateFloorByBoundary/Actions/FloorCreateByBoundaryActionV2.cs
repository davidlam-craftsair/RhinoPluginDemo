using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System.Collections.Generic;
using System.Linq;

namespace RhinoPlugInExcercise1
{
    public class FloorCreateByBoundaryActionV2 : IFloorCreateByBoundaryAction
    {
        public IEdgesSorter EdgesSorter { get; }
        public IBrepFactoryExt BrepFloorFactory { get; }

        public FloorCreateByBoundaryActionV2(
                                           IEdgesSorter edgesSorter,
                                           IBrepFactoryExt brepFloorFactory
                                           )
        {
            EdgesSorter = edgesSorter;
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
                if (EdgesSorter.Find(edges, out Curve outerEdge, out IEnumerable<Curve> innerEdges))
                {
                    {
                        var floorHeight = gn.Number();
                        if (CreateFloor(doc, outerEdge, innerEdges, floorHeight))
                        {
                            return Result.Success;
                        }
                    }
                }

            }

            return Result.Cancel;


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
            var breps1 = BrepFloorFactory.Create(new Curve[]{ outerEdge}, floorHeight);
            var breps2 = BrepFloorFactory.Create(innerEdges, floorHeight);
            var ts = breps1.Concat(breps2);
            if (ts == null)
            {
                return false;
            }
            ts.Select(x=> AddToRhinoObjectTable(doc, x)).ToArray();
            return true;
        }

        private static System.Guid AddToRhinoObjectTable(IDocManager doc, Brep brep)
        {
            return doc.RhinoDoc.Add(brep);
        }
    }
}
