using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingUpdateInfos: IRailingUpdateInfos
    {
        public RailingUpdateInfos()
        {
            CompositeObjIdsToBeUpdated = new List<ICompositeObjId>();
        }
        public void Add(ICompositeObjId compositeObjId)
        {
            CompositeObjIdsToBeUpdated.Add(compositeObjId);
        }

        public void Clear()
        {
            CompositeObjIdsToBeUpdated.Clear();
        }

        public IEnumerable<ICompositeObjId> CompositeObjIds => CompositeObjIdsToBeUpdated;

        public List<ICompositeObjId> CompositeObjIdsToBeUpdated { get; }
    }
}
