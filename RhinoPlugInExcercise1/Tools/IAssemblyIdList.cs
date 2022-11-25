using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IAssemblyIdList
    {
        void Add(IAssemblyIdV2 assemblyId);
        int Count();
        IAssemblyIdV2 GetByComponentId(Guid componentId);
    }
}