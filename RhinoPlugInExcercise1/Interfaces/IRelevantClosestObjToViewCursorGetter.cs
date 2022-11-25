using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IRelevantClosestObjToViewCursorGetter
    {
        RhinoObject Get(RhinoViewport viewport, Line viewLine);
    }
}