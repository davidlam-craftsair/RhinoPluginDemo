using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// for Rhino Object
    /// </summary>
    public class RhBuildingComponentId : IBuildingComponentId
    {
        public RhBuildingComponentId(Guid id, string buildingComponentName, IEnumerable<IBuildingComponentId> constituentIds)
        {
            Id = id;
            BuildingComponentName = buildingComponentName;
            ConstituentIds = constituentIds;
        }
        

        public IEnumerable<IBuildingComponentId> ConstituentIds
        {
            get;set;
        }

        public string BuildingComponentName
        {
            get;set;
        }

        public Guid Id { get; set; }

        public bool IsRoot => ConstituentIds.Any();
    }
}
