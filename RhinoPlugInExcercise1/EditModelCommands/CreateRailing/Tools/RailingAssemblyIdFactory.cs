using DL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingAssemblyIdFactory : IRailingAssemblyIdFactory
    {
        public IRailingAssemblyId Create(int groupId, IDictionary<string, IEnumerable<Guid>> guidsByComponentName)
        {
            return Create(Guid.NewGuid(), 
                          groupId, 
                          guidsByComponentName.GetOrDefault(RailingCompositionalElementNames.Rail),
                          guidsByComponentName.GetOrDefault(RailingCompositionalElementNames.TopProfiles));
        }
        public IRailingAssemblyId Create(int groupId, IEnumerable<Guid> railId, IEnumerable<Guid> profileId)
        {
            return Create(Guid.NewGuid(), groupId, railId, profileId);
        }

        public IRailingAssemblyId Create(Guid assemblyId, int groupId, IEnumerable<Guid> railId, IEnumerable<Guid> profileId)
        {
            return new RailingAssemblyId(assemblyId, groupId, railId, profileId);
        }
    }
}
