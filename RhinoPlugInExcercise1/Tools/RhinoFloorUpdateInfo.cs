using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RhinoFloorUpdateInfo : IFloorUpdateInfo
    {
        public RhinoFloorUpdateInfo(IFloorComponentInfo floorComponentInfo,
                                    IEnumerable<Guid> edges,
                                    double thickness)
        {
            FloorComponentInfo = floorComponentInfo;
            Thickness = thickness;

            EdgeIds = edges;
        }

        public IFloorComponentInfo FloorComponentInfo { get; }
        public double Thickness { get; }
        public IEnumerable<Guid> EdgeIds { get; }

        public bool NeedToUpdate { get; private set; }

        public void SetNeedToUpdate()
        {
            NeedToUpdate = true;
        }

        public void ResetNeedToUpdateToFalse()
        {
            NeedToUpdate = false;
        }

        public bool CheckIfContainEdgeId(Guid id)
        {
            foreach (var item in EdgeIds)
            {
                if (item == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
