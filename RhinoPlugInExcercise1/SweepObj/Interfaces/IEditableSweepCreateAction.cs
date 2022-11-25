using Rhino.Commands;
using Rhino.DocObjects;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IEditableSweepCreateAction
    {
        Result Create(ObjRef railObj, IEnumerable<ObjRef> profileObjs);
    }

    
}