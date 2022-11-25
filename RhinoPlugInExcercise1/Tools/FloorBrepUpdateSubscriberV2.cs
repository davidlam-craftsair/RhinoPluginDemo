using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorBrepUpdateSubscriberV2 : IFloorBrepUpdateSubscriber
    {
        public FloorBrepUpdateSubscriberV2(IRhinoObjChangeManager rhinoObjChangeManager, IDocManager docManager)
        {
            RhinoObjChangeManager = rhinoObjChangeManager;
            DocManager = docManager;
            RhinoObjChangeManager.ObjChanged += OnObjChanged;
        }

        private void OnObjChanged(Guid edgeId)
        {
            if (DocManager.SessionDoc.CheckIfContainsEdge(edgeId, out IFloorUpdateInfo rhinoFloorUpdateInfo))
            {
                rhinoFloorUpdateInfo.SetNeedToUpdate();
            }
        }

        public IRhinoObjChangeManager RhinoObjChangeManager { get; }
        public IDocManager DocManager { get; }

        public void SubscribeUpdate(IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            var rhinoDoc = DocManager.RhinoDoc;
            var sessionDoc = DocManager.SessionDoc;
            sessionDoc.Add(rhinoFloorUpdateInfo);
            foreach (var edgeId in rhinoFloorUpdateInfo.EdgeIds)
            {
                // add to rhinoDoc ListenObjChange
                rhinoDoc.ListenObjChange(edgeId);
            }

        }


        public void UnsubscribeUpdate(IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            IRhinoDoc rhinoDoc = DocManager.RhinoDoc;

            DocManager.SessionDoc.Remove(rhinoFloorUpdateInfo);
            foreach (var edgeId in rhinoFloorUpdateInfo.EdgeIds)
            {
                // remove from rhinoDoc ListenObjChange
                rhinoDoc.RemoveListenObjChange(edgeId);
            }
        }
    }
}
