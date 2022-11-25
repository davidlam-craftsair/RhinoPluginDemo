using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorComponentInfo : IFloorComponentInfo
    {
        public FloorComponentInfo(Guid floorRhinoId, Curve outerEdge, IEnumerable<Curve> innerEdges, double thickness)
        {
            FloorRhinoId = floorRhinoId;
            OuterEdge = outerEdge;
            InnerEdges = innerEdges;
            Thickness = thickness;
        }

        public Guid FloorRhinoId { get; }
        public Curve OuterEdge { get; }
        public IEnumerable<Curve> InnerEdges { get; }
        public double Thickness { get; }

        public override bool Equals(object obj)
        {
            return obj is FloorComponentInfo info &&
                   FloorRhinoId.Equals(info.FloorRhinoId);
        }

        public override int GetHashCode()
        {
            return 1334392852 + FloorRhinoId.GetHashCode();
        }

    }
}
