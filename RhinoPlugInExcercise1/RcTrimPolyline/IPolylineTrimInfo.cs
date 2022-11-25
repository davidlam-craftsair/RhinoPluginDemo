using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IPolylineTrimInfo
    {
        int LineSegIndex { get; }
        Point3d TrimPt1 { get; }
        Point3d TrimPt2 { get; }
    }
}