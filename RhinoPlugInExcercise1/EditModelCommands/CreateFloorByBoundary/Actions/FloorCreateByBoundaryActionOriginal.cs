using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System.Collections.Generic;
using System.Linq;

namespace RhinoPlugInExcercise1
{
    public class FloorCreateByBoundaryActionOriginal : IFloorCreateByBoundaryAction
    {
        public IEdgesSorter EdgesSorter { get; }
        public IBrepFloorFactory BrepFloorFactory { get; }

        public FloorCreateByBoundaryActionOriginal(
                                           IEdgesSorter edgesSorter,
                                           IBrepFloorFactory brepFloorFactory
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
            if (EdgesSorter.Find(edges, out Curve outerEdge, out IEnumerable<Curve> innerEdges))
            {
                var gn = new GetNumber();
                gn.SetCommandPrompt("Input floor height");
                gn.SetDefaultNumber(0.15);
                gn.Get();
                if (gn.CommandResult() == Result.Success)
                {
                    var floorHeight = gn.Number();
                    if (CreateFloor(doc, outerEdge, innerEdges, floorHeight))
                    {
                        return Result.Success;
                    }
                    return Result.Failure;
                }
            }
            return Result.Cancel;
        }

        private bool CreateFloor(IDocManager doc, Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight)
        {
            var brep = BrepFloorFactory.Create(outerEdge, innerEdges, floorHeight);
            if (brep == null)
            {
                return false;
            }
            var floorBrepId = AddToRhinoObjectTable(doc, brep);
            // Extrusion added to object table and retrieve the guid of the floor extrusion
            // if it detects the change in objref, then it would change the floor extrusion
            var componentInfo = CreateFloorComponentInfo(outerEdge, innerEdges, floorHeight, floorBrepId);
            doc.SessionDoc.Add(componentInfo);
            return true;
        }

        private static IFloorComponentInfo CreateFloorComponentInfo(Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight, System.Guid floorBrepId)
        {
            return new FloorComponentInfo(floorBrepId, outerEdge, innerEdges, floorHeight);
        }

        private static System.Guid AddToRhinoObjectTable(IDocManager doc, Brep brep)
        {
            return doc.RhinoDoc.Add(brep);
        }
    }
}
