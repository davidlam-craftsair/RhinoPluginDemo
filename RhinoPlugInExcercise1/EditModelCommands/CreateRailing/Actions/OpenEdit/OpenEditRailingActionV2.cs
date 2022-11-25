using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// Get the selected railing assembly (a group of breps), 
    /// create the compositional geometries, ie. rail curve and profile curves 
    /// and subscribe changes to these geometries
    /// </summary>
    class OpenEditRailingActionV2: IOpenEditRailingAction
    {
        public OpenEditRailingActionV2(IDocManager docManager, 
                                    IRailingAssemblyBrepUpdateSubscriber sweepAssemblyBrepUpdateSubscriber,
                                    IRailsCurveAdditionBag railsCurveAdditionBag)
        {
            DocManager = docManager;
            RailingAssemblyBrepUpdateSubscriber = sweepAssemblyBrepUpdateSubscriber;
            RailsCurveAdditionBag = railsCurveAdditionBag;
        }

        public IDocManager DocManager { get; }
        public IRailingAssemblyBrepUpdateSubscriber RailingAssemblyBrepUpdateSubscriber { get; }
        public IRailsCurveAdditionBag RailsCurveAdditionBag { get; }

        public Result Do(ObjRef railComponent)
        {
            var railComponentId = railComponent.ObjectId;

            // Get SweepAssemblyId from SessionDoc
            if (GetBuildingComponentId(railComponentId, out ICompositeObjId compositeObjId))
            {
                // Add to SessionDoc
                DocManager.SessionDoc.ObjsInEditingMode.Add(compositeObjId);
                // Get the compositional geometries and add to rhino for editing
                var compositionalElementIds = compositeObjId.CompositionalElementIdsByName.Values.SelectMany(x=>x).ToArray();

                // Add compositional geometries to rhino
                
                foreach (ICompositionalElementId item in compositionalElementIds)
                {
                    // Add to rhino doc for every compositional element
                    var objId = DocManager.RhinoDoc.Add(item.GetGeometry());
                    item.SetObjId(objId);
                }

                // Subscribe changes of those compositional Objs
                SubscribeCompositionalElementsChanges(compositeObjId);

                EnableListeningToRhinoAddRailsCurveAddition();


                // Use the group index to get the object ids of the rails, and subscribe change
                //IEnumerable<RhinoObject> groupMembers = GetRhinoGroupMembers(compositeObjId);

                // 
                // Subscribe changes of the components in railing assembly
                //SubscribeChanges(sweepAssemblyId, groupMembers);

                EnableListeningToRhinoAddRailsCurveAddition();
                return Result.Success;
            }
            return Result.Cancel;
        }

        

        // Activate listening to rhino adding rails curve
        private void EnableListeningToRhinoAddRailsCurveAddition()
        {
            RailsCurveAdditionBag.EnableListening();
        }

        //private IEnumerable<RhinoObject> GetRhinoGroupMembers(ICompositeObjId sweepAssemblyId)
        //{
        //    return DocManager.RhinoDoc.GetGroupMemebers(sweepAssemblyId.GroupId);
        //}

        private bool GetBuildingComponentId(Guid constitutentId, out ICompositeObjId compositeObjId)
        {
            return DocManager.SessionDoc.GetCompositeIdByConstitutentId(constitutentId, out compositeObjId);
        }

        private void SubscribeCompositionalElementsChanges(ICompositeObjId compositeObjId)
        {
            RailingAssemblyBrepUpdateSubscriber.SubscribeCompositionalElementsChanges(compositeObjId);
        }

        //private void SubscribeChanges(ISweepAssemblyId sweepAssemblyId, IEnumerable<RhinoObject> groupMembers)
        //{
        //    IRailingUpdateInfo rhinoRailingUpdateInfo = CreateRhinoRailingUpdateInfo(sweepAssemblyId,
        //                                                                             groupMembers.Select(x => x.Id).ToArray());
        //    RailingAssemblyBrepUpdateSubscriber.SubscribeUpdate(rhinoRailingUpdateInfo);
        //}

        private IRailingUpdateInfo CreateRhinoRailingUpdateInfo(ISweepAssemblyId sweepAssemblyId, IEnumerable<Guid> groupMembers)
        {
            return new RhinoRailingUpdateInfo(sweepAssemblyId, groupMembers);
        }
    }
}
