using Rhino.DocObjects;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IRhinoObjectFilter
    {
        bool Filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex);
    }
}