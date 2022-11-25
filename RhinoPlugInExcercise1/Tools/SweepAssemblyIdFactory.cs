using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class SweepAssemblyIdFactory : ISweepAssemblyIdFactory
    {
        public SweepAssemblyIdFactory()
        {

        }
        public ISweepAssemblyId Create(IEnumerable<Guid> sweepComponentIds, int groupIdx)
        {
            return new SweepAssemblyId(Guid.NewGuid(), groupIdx, sweepComponentIds);

        }
    }
}
