using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorBrepUpdateSubscriber : IFloorBrepUpdateSubscriber
    {
        public FloorBrepUpdateSubscriber(IRhinoObjChangeManager rhinoObjChangeManager, IDocManager docManager)
        {
            RhinoObjChangeManager = rhinoObjChangeManager;
            DocManager = docManager;
            RhinoObjChangeManager.ObjChanged += OnObjChanged;
        }

        private void OnObjChanged(Guid edgeId)
        {
            if (DocManager.SessionDoc.Get(edgeId, out IRhinoFloorUpdateInfo rhinoFloorUpdateInfo))
            {
                rhinoFloorUpdateInfo.SetNeedToUpdate();
            }
        }

        public IRhinoObjChangeManager RhinoObjChangeManager { get; }
        public IDocManager DocManager { get; }

        public void SubscribeUpdate(IDocManager docManager, IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            var rhinoDoc = docManager.RhinoDoc;
            var sessionDoc = docManager.SessionDoc;
            foreach (var edgeId in rhinoFloorUpdateInfo.Edges)
            {
                // add to rhinoDoc ListenObjChange
                rhinoDoc.ListenObjChange(edgeId);
                sessionDoc.Add(edgeId, rhinoFloorUpdateInfo);
            }

        }


        public void UnsubscribeUpdate(IDocManager docManager, IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            IRhinoDoc rhinoDoc = docManager.RhinoDoc;


            foreach (var edgeId in rhinoFloorUpdateInfo.Edges)
            {
                // remove from rhinoDoc ListenObjChange
                rhinoDoc.RemoveListenObjChange(edgeId);
                docManager.SessionDoc.Remove(edgeId, rhinoFloorUpdateInfo);
            }
        }

    }
}
