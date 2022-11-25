using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRelevantObjsGetter
    {
        IEnumerable<RhinoObject> Get(RhinoViewport viewport, Line viewLine);
    }
}