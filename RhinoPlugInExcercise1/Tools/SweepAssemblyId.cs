using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class SweepAssemblyId: ISweepAssemblyId
    {
        private Guid sweepObjId;
        

        public SweepAssemblyId(Guid sweepObjId, int groupIdx, IEnumerable<Guid> componentIds)
        {
            this.sweepObjId = sweepObjId;
            GroupId = groupIdx;
            ComponentIds = componentIds;
        }

        public Guid Id => sweepObjId;


        public int GroupId { get; }
        public IEnumerable<Guid> ComponentIds { get; }

        public string AssemblyTypeName => "Generic Sweeps";
    }

}
