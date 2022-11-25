using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRhinoObjChangeManager
    {

        event Action<Guid> ObjChanged;
        event Action<Guid> ObjChangedWhenUndo;
        event Action<Guid> AbsolutelyNewObjAdded;

        void Enable();
        void Disable();
        void Add(Guid rhinoObjId);
        void Remove(Guid rhinoObjId);
    }
}