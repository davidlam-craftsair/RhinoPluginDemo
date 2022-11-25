using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingAssemblyId: IRailingAssemblyId
    {
        public RailingAssemblyId(Guid assemblyId, int groupId, IEnumerable<Guid> railId, IEnumerable<Guid> profileId)
        {
            AssemblyId = assemblyId;
            GroupId = groupId;
            RailId = railId;
            ProfileId = profileId;
            var componentIds = new List<Guid>();
            componentIds.AddRange(railId);
            componentIds.AddRange(profileId);
        }
        public string AssemblyTypeName => "Railing Sweeps";

        public Guid Id => AssemblyId;

        public Guid AssemblyId { get; }
        public int GroupId { get; }
        public IEnumerable<Guid> RailId { get; }
        public IEnumerable<Guid> ProfileId { get; }
        public IEnumerable<Guid> ComponentIds { get; }

    }
}
