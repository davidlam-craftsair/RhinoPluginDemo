using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IRelevantBrepFaceClosestToViewCursorGetter
    {
        bool Get(Brep brep, Line viewLine, out BrepFace brepFace, out Plane frame);
    }
}