using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingAssemblyBrepUpdateSubscriber : IRailingAssemblyBrepUpdateSubscriber
    {
        public RailingAssemblyBrepUpdateSubscriber(IRhinoObjChangeManager rhinoObjChangeManager, IDocManager docManager)
        {
            RhinoObjChangeManager = rhinoObjChangeManager;
            DocManager = docManager;
        }

        public IRhinoObjChangeManager RhinoObjChangeManager { get; }
        public IDocManager DocManager { get; }

        public void SubscribeCompositionalElementsChanges(IEnumerable<ICompositeObjId> compositeObjIds)
        {
            foreach (ICompositeObjId item in compositeObjIds)
            {
                DocManager.SessionDoc.RailingUpdateInfos.Add(item);
            }
        }
    }
}
