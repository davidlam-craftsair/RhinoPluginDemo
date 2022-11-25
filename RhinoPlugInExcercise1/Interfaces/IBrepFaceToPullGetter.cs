using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using System;

namespace RhinoPlugInExcercise1
{
    public interface IBrepFaceGetter
    {
        bool Get(Line viewLine, RhinoViewport viewport, out BrepFace brepFaceToPull, out Plane frame, out Guid objId);
    }
}