using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class CloseEditRailingAction: IRailingCloseEditAction
    {
        public CloseEditRailingAction(IDocManager docManager,
            IRailingComponentIdsByNameFactory railingComponentIdsByNameFactory,
            IBrepSweepsFactory brepSweepsFactory,
            IBuildingComponentsToSessionAdder buildingComponentsToSessionAdder)
        {
            DocManager = docManager;
            RailingComponentIdsByNameFactory = railingComponentIdsByNameFactory;
            BrepSweepsFactory = brepSweepsFactory;
            
        }

        public IDocManager DocManager { get; }
        public IRailingComponentIdsByNameFactory RailingComponentIdsByNameFactory { get; }
        public IBrepSweepsFactory BrepSweepsFactory { get; }
        public IList<ICompositeObjId> ObjsInEditingMode => DocManager.SessionDoc.ObjsInEditingMode;

        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;

        public Result Do()
        {
            // Commit update
            if (CommitUpdate())
            {
                // On update committed
                OnUpdateCommitted();
                return Result.Success;
            }
            return Result.Cancel;
        }

        private void OnUpdateCommitted()
        {
            throw new NotImplementedException();
        }

        private bool CommitUpdate()
        {
            var railingObjs = ObjsInEditingMode.Where(x=>x.TypeName == AssemblyTypeNames.Railing);

            foreach (ICompositeObjId railingUpdateInfo in railingObjs)
            {
                if (GetProfilesAndRail(railingUpdateInfo, out IEnumerable<IEnumerable<Curve>> profilesPerPosition, out Curve rail))
                {
                    var railBrepsPerPosition = CreateRailingProfileBreps(profilesPerPosition, rail);
                    if (railBrepsPerPosition.Any())
                    {
                        var profileBrepIds = AddRailingProfileBrepsToRhinoObjectTable(railBrepsPerPosition);

                        // Create dictionary railingComponentIdsByName
                        railingUpdateInfo.ConstituentsByName.GetOrDefault(RailingComponentNames);

                        AddRailingComponentIdsByNameToSessionDoc(railingComponentIdsByName);
                    }


                }
            }
            return true;
        }

        private IDictionary<string, IEnumerable<Guid>> GetRailingComponentIdsByName(Guid railId, IEnumerable<Guid> profileIds)
        {
            return RailingComponentIdsByNameFactory.Create(new Guid[] { railId },
                                                          profileIds);
        }

        private void AddRailingComponentIdsByNameToSessionDoc(IDictionary<string, IEnumerable<Guid>> ts)
        {
            SweepComponentsToSessionAdder.AddToSessionDoc(ts);
        }

        /// <summary>
        /// Add railingComponentBreps to Rhino ObjectTable, and returns the objectIds
        /// </summary>
        /// <param name="profileBreps"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<Guid>> AddRailingProfileBrepsToRhinoObjectTable(IEnumerable<IEnumerable<Brep>> profileBreps)
        {
            var brepObjIds = new List<Guid>();
            foreach (var item in profileBreps)
            {
                var brepId = RhinoDoc.Add(item);
                brepObjIds.Add(brepId);
            }
            return brepObjIds;
        }

        private IEnumerable<IEnumerable<Brep>> CreateRailingProfileBreps(IEnumerable<IEnumerable<Curve>> profiles, Curve rail)
        {
            return profiles.Select(x=>BrepSweepsFactory.Create(rail, x));
        }

        private IEnumerable<Brep> CreateRailingRail(IEnumerable<Curve> rail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="railingObj"></param>
        /// <param name="profilesPerPosition"> profiles </param>
        /// <param name="rail"></param>
        /// <returns></returns>
        private bool GetProfilesAndRail(ICompositeObjId railingObj, 
                                        out IEnumerable<IEnumerable<Curve>> profilesPerPosition, 
                                        out Curve rail)
        {
            var compositionalElementIdsByName = railingObj.CompositionalElementIdsByName;
            compositionalElementIdsByName.GetOrDefault


            IEnumerable<ICompositionalElementId> railPathId = compositionalElementIdsByName.GetOrDefault(RailingCompositionalElementNames.Rail);
            IEnumerable<ICompositionalElementId> topProfileIds = compositionalElementIdsByName.GetOrDefault(RailingCompositionalElementNames.TopProfiles);
            IEnumerable<ICompositionalElementId> bottomProfileIds = compositionalElementIdsByName.GetOrDefault(RailingCompositionalElementNames.BottomProfiles);

            if (railPathId.FirstOrDefault().GetGeometry() is Curve t)
            {
                rail = t;
            }
            else
            {
                rail = default;
            }

            //topProfileIds.


        }

    }
}
