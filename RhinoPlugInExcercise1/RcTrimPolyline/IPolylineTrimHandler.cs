using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IPolylineTrimHandler
    {
        IRhinoTolerance RhinoTolerance { get; }
        double Tol { get; }

        void Trim(Polyline polyline, IPolylineTrimInfo polylineTrimInfo, out IEnumerable<Point3d> polyline1, out IEnumerable<Point3d> polyline2);
    }

    public interface IPolylineTrimHandlerV2
    {
        IRhinoTolerance RhinoTolerance { get; }
        double Tol { get; }

        void Trim(Polyline polyline, IPolylineTrimInfoV2 polylineTrimInfo, out IEnumerable<Point3d> polyline1, out IEnumerable<Point3d> polyline2);
    }
}