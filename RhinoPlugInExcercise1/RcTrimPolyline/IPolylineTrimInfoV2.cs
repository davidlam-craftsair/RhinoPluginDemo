using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IPolylineTrimInfoV2
    {
        int LineCutterIndex { get; }
        int LineToBeCutIndex { get; }
        Point3d Selector { get; }
    }
}