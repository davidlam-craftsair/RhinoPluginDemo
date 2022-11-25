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
    class OpenEditRailingAction: IOpenEditRailingAction
    {
        public OpenEditRailingAction(IDocManager docManager, 
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
            if (GetSweepAssemblyId(railComponentId, out ISweepAssemblyId sweepAssemblyId))
            {
                // use the group index to get the object ids of the rails, and subscribe change
                var groupMembers = GetRhinoGroupMembers(sweepAssemblyId);

                // Subscribe changes of the components in railing assembly
                SubscribeChanges(sweepAssemblyId, groupMembers);

                ListenToRhinoAddRailsCurveAddition();
                return Result.Success;
            }
            return Result.Cancel;
        }

        // Activate listening to rhino adding rails curve
        private void ListenToRhinoAddRailsCurveAddition()
        {
            RailsCurveAdditionBag.EnableListening();
        }

        private IEnumerable<RhinoObject> GetRhinoGroupMembers(ISweepAssemblyId sweepAssemblyId)
        {
            return DocManager.RhinoDoc.GetGroupMemebers(sweepAssemblyId.GroupId);
        }

        private bool GetSweepAssemblyId(Guid railComponentId, out ISweepAssemblyId sweepAssemblyId)
        {
            return DocManager.SessionDoc.GetSweepAssemblyIdByComponentId(railComponentId, out sweepAssemblyId);
        }

        private void SubscribeChanges(ISweepAssemblyId sweepAssemblyId, IEnumerable<RhinoObject> groupMembers)
        {
            RailingAssemblyBrepUpdateSubscriber.SubscribeUpdate(CreateRhinoRailingUpdateInfo(sweepAssemblyId, 
                                                                                             groupMembers.Select(x => x.Id).ToArray()));
        }

        private IRailingUpdateInfo CreateRhinoRailingUpdateInfo(ISweepAssemblyId sweepAssemblyId, IEnumerable<Guid> groupMembers)
        {
            return new RhinoRailingUpdateInfo(sweepAssemblyId, groupMembers);
        }
    }
}
