using Rhino;
using Rhino.Geometry;
using System;

namespace DL.RhinoExcercise1.Core
{
    public interface IRhinoGetManager
    {
        bool TryGetLine(string promptMessage, out Line line, out Curve curve, out Guid objectId);
        bool TryGetLine(string promptMessage, out Line line, out Guid objectId);
    }
}