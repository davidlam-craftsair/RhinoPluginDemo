using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public interface IRailingAssemblyId: IAssemblyId
    {
        IEnumerable<Guid> RailId { get; }
        IEnumerable<Guid> ProfileId { get; }

    }
}
