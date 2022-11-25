using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RhBuildingComponentIdFactory : IRhBuildingComponentIdFactory
    {
        public IBuildingComponentId Create(Guid id, string buildingComponentName, IEnumerable<IBuildingComponentId> constituentIds)
        {
            return new RhBuildingComponentId(id, buildingComponentName, constituentIds);
        }

        /// <summary>
        /// Create the lowest level building component which consists only 1 rhino object, 
        /// which mainly is brep
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buildingComponentName"></param>
        /// <param name="rhinoObjectId"></param>
        /// <returns></returns>
        public IBuildingComponentId CreateSingle(Guid id, string buildingComponentName, Guid rhinoObjectId)
        {
            return Create(rhinoObjectId, buildingComponentName, new IBuildingComponentId[] { });
        }
    }
}
