using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace DL.RhinoExcercise1.Core
{
    public interface IRailingUpdateInfo
    {
        ICompositeObjId CompositeObjId { get; }
    }

    public interface IRailingUpdateInfos
    {
        IEnumerable<ICompositeObjId> CompositeObjIds { get; }

        void Add(ICompositeObjId compositeObjId);
    }
}