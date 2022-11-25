using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FloorBrepUpdater : IFloorBrepUpdateSubscriber
    {
        public FloorBrepUpdater(IRhinoObjChangeManager rhinoObjChangeManager)
        {
            RhinoObjChangeManager = rhinoObjChangeManager;
            RhinoObjChangeManager.ObjChanged += OnObjChanged;
            RhinoFloorUpdateInfos = new Dictionary<Guid, IRhinoFloorUpdateInfo>();
        }

        private void OnObjChanged(Guid obj)
        {
            if (RhinoFloorUpdateInfos.TryGetValue(obj, out IRhinoFloorUpdateInfo rhinoFloorUpdateInfo))
            {
                rhinoFloorUpdateInfo.SetNeedToUpdate();
            }
        }

        public IRhinoObjChangeManager RhinoObjChangeManager { get; }
        public IDictionary<Guid, IRhinoFloorUpdateInfo> RhinoFloorUpdateInfos { get; }


        public void SubscribeUpdate(IDocManager docManager, IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            IRhinoDoc rhinoDoc = docManager.RhinoDoc;
            Update(rhinoFloorUpdateInfo, rhinoDoc);

        }

        public void UnsubscribeUpdate(IDocManager docManager, IRhinoFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            IRhinoDoc rhinoDoc = docManager.RhinoDoc;


            foreach (var edgeId in rhinoFloorUpdateInfo.Edges)
            {
                // add to rhinoDoc ListenObjChange
                rhinoDoc.RemoveListenObjChange(edgeId);
                RhinoFloorUpdateInfos.Add(edgeId, rhinoFloorUpdateInfo);
            }
        }

        private void Update(IRhinoFloorUpdateInfo rhinoFloorUpdateInfo, IRhinoDoc rhinoDoc)
        {

            foreach (var edgeId in rhinoFloorUpdateInfo.Edges)
            {
                // add to rhinoDoc ListenObjChange
                rhinoDoc.ListenObjChange(edgeId);
                RhinoFloorUpdateInfos.Add(edgeId, rhinoFloorUpdateInfo);
            }

        }
    }
}
