using Rhino.Commands;
using Rhino.DocObjects;

namespace RhinoPlugInExcercise1
{
    public interface IFloorBoundaryOpenEditAction
    {
        Result Do(ObjRef objRef, IDocManager docManager);
    }
}