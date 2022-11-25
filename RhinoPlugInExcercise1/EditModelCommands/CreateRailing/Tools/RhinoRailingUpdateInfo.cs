using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RhinoRailingUpdateInfo : IRailingUpdateInfo
    {
        public RhinoRailingUpdateInfo(ISweepAssemblyId sweepAssemblyId, IEnumerable<Guid> objIdsToWatch)
        {
            SweepAssemblyId = sweepAssemblyId;
            ComponentIdsToWatch = new HashSet<Guid>(objIdsToWatch);
        }

        public ISweepAssemblyId SweepAssemblyId { get; }
        public ISet<Guid> ComponentIdsToWatch { get; }

        public bool CheckIfContain(Guid id)
        {
            return ComponentIdsToWatch.Contains(id);
        }

        public bool NeedToUpdate => throw new NotImplementedException();
    }
}
