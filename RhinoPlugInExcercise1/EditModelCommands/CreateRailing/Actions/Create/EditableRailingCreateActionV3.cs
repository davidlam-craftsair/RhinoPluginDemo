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
    /// <summary>
    /// Compared to v2, differentiate top rail and bottom rail
    /// </summary>
    public class EditableRailingCreateActionV3
    {
        public EditableRailingCreateActionV3(IDocManager docManager,
                                           IBrepSweepsFactory brepSweepsFactory,
                                           IComponentsToSessionAdderV2 sweepComponentsToSessionAdder,
                                           IRailingComponentIdsByNameFactory railingComponentIdsByNameFactory)
        {
            DocManager = docManager;
            BrepSweepsFactory = brepSweepsFactory;
            SweepComponentsToSessionAdder = sweepComponentsToSessionAdder;
            RailingComponentIdsByNameFactory = railingComponentIdsByNameFactory;
        }

        public IDocManager DocManager { get; }
        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;
        public IBrepSweepsFactory BrepSweepsFactory { get; }
        public IComponentsToSessionAdderV2 SweepComponentsToSessionAdder { get; }
        public IRailingComponentIdsByNameFactory RailingComponentIdsByNameFactory { get; }

        /// <summary>
        /// Create sweep objects in rhino
        /// </summary>
        /// <param name="railObj">the path curve </param>
        /// <param name="profileObjs"></param>
        /// <returns></returns>
        public Result Create(ObjRef railObj, IEnumerable<ObjRef> profileObjs)
        {
            var rail = railObj.Curve();
            if (rail == null)
            {
                return Result.Cancel;
            }
            
            var profiles = profileObjs.Select(x => x.Curve()).ToList();
            IEnumerable<Brep> railingBreps = CreateRailingBreps(rail, profiles);

            if (railingBreps.Any())
            {
                var brepComponentIds = AddRailingComponentBrepsToRhinoObjectTable(railingBreps);

                // Create dictionary railingComponentIdsByName
                IDictionary<string, IEnumerable<Guid>> railingComponentIdsByName = GetRailingComponentIdsByName(railObj, profileObjs);
                AddRailingComponentIdsByNameToSessionDoc(railingComponentIdsByName);

                return Result.Success;
            }
            return Result.Cancel;
        }

        private IDictionary<string, IEnumerable<Guid>> GetRailingComponentIdsByName(ObjRef railObj, IEnumerable<ObjRef> profileObjs)
        {
            return RailingComponentIdsByNameFactory.Create(new Guid[] { railObj.ObjectId },
                                                          profileObjs.Select(x => x.ObjectId).ToArray());
        }

        private void AddRailingComponentIdsByNameToSessionDoc(IDictionary<string, IEnumerable<Guid>> ts)
        {
            SweepComponentsToSessionAdder.AddToSessionDoc(ts);
        }

        private IEnumerable<Brep> CreateRailingBreps(Curve rail, List<Curve> profiles)
        {
            return BrepSweepsFactory.Create(rail, profiles);
        }

        /// <summary>
        /// Add railingComponentBreps to Rhino ObjectTable, and returns the objectIds
        /// </summary>
        /// <param name="railingBreps"></param>
        /// <returns></returns>
        private IEnumerable<Guid> AddRailingComponentBrepsToRhinoObjectTable(IEnumerable<Brep> railingBreps)
        {
            var brepObjIds = new List<Guid>();
            foreach (var item in railingBreps)
            {
                var brepId = RhinoDoc.Add(item);
                brepObjIds.Add(brepId);
            }
            return brepObjIds;
        }
    }
}
