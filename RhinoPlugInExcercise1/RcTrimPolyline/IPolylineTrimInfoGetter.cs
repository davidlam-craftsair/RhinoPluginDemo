using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IPolylineTrimInfoGetter
    {
        bool Get(Point3d selectorPt, Polyline polyline, out IPolylineTrimInfoV2 polylineTrimInfo, out Point3d intersectPt, out Line lineSelectedToTrim);
    }
}