using Rhino;
using Rhino.Geometry;
using System;

namespace DL.RhinoExcercise1.Core
{
    public interface IRhinoReplaceManager
    {
        bool Replace(Guid id, Line line);
        bool Replace(Guid objectId, Curve curve);
    }
}