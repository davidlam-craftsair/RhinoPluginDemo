using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface ISweepAssemblyIdFactory
    {
        ISweepAssemblyId Create(IEnumerable<Guid> sweepComponentIds, int groupIdx);
    }
}