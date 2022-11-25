using Rhino.DocObjects;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IDistanceBtwObjAndViewGetter
    {
        double Get(RhinoObject x, Line viewLine);
    }
}