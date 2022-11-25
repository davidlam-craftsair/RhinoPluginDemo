using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingComponentIdsByNameFactory : IRailingComponentIdsByNameFactory
    {
        /// <summary>
        /// Create a dictionary of component name with component objectId
        /// </summary>
        /// <param name="rails"></param>
        /// <param name="profiles"></param>
        /// <returns></returns>
        public IDictionary<string, IEnumerable<Guid>> Create(IEnumerable<Guid> rails, IEnumerable<Guid> profiles)
        {
            return new Dictionary<string, IEnumerable<Guid>>
            {
                { RailingCompositionalElementNames.Rail, rails},
                { RailingCompositionalElementNames.TopProfiles, profiles}
            };
        }

        public IDictionary<string, IEnumerable<GeometryBase>> Create(IEnumerable<Curve> rails, IEnumerable<Curve> profiles)
        {
            return new Dictionary<string, IEnumerable<GeometryBase>>
            {
                {RailingCompositionalElementNames.Rail, rails },
                {RailingCompositionalElementNames.TopProfiles, profiles }
            };
        }
    }
}
