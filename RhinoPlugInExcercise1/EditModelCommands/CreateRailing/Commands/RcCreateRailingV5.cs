using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// Ask for multiple times for profiles, asking indefinite number of 
    /// horizontal railings
    /// </summary>
    public class RcCreateRailingV5: AbsCommand
    {
        private IEditableSweepCreateAction _action;
        private IRhinoObjectFilter _rhinoObjectPicker;
        private RhBuildingComponentIdFactory _rhBuildingComponentIdFactory;

        public IRhinoObjectFilter RailProfileCurveFilter
        {
            get 
            {
                if (_rhinoObjectPicker == null)
                {
                    _rhinoObjectPicker = new RailProfileCurveFilter();
                }
                return _rhinoObjectPicker;
            }
        }

        public IRhBuildingComponentIdFactory RhBuildingComponentIdFactory
        {
            get
            {
                if (_rhBuildingComponentIdFactory == null)
                {
                    _rhBuildingComponentIdFactory = new RhBuildingComponentIdFactory();
                }
                return _rhBuildingComponentIdFactory;
            }
        }
        public override string EnglishName => "CreateRailing";

        protected override Result DoRunCommand()
        {
            // Get the rail curve
            if (Result.Success == GetRailCurveByPrompt(out ObjRef railPathObj))
            {
                var ts = new List<IBuildingComponentId>();
                var r = Result.Success;

                // Ask for profiles for different positions
                var count = 0;
                while (r == Result.Success)
                {
                    r = AddProfilesByPrompt(ts, count.ToString());
                    count += 1;
                }

                // Create Breps from profiles and curves
                IEnumerable<IBuildingComponentId> railComponentsPerPrompt = ts.Select(x => CreateRailBrepFromProfiles(railPathObj.Curve(), ts));  
            }

        }

        private IBuildingComponentId CreateRailBrepFromProfiles(Curve rail, List<IBuildingComponentId> profileStructure)
        {
            profileStructure
                .Where(x => x.BuildingComponentName == "ProfilesForRail")
                .Select(profile => CreateSweep(rail, profile));
        }   

        private IBuildingComponentId CreateSweep(Curve rail, IBuildingComponentId profileComponent)
        {
            if (profileComponent.IsRoot)
            {
                // if it is root, it has single curve profile, sweep with rail
                var profileCurve = new ObjRef(profileComponent.Id).Curve();
                var brep = Brep.CreateFromSweep(rail, profileCurve, false, RhinoTolerance.Tol).FirstOrDefault();
                Guid t = GetIdForBrep(brep);
                return RhBuildingComponentIdFactory.CreateSingle(profileComponent.Id, "BrepForRail", t);
            }
            else
            {
                // retain the organization of profile, some profile curve are under the building component
                IEnumerable<IBuildingComponentId> ts = profileComponent.ConstituentIds.Select(x => CreateSweep(rail, x));
                return RhBuildingComponentIdFactory.Create(Guid.NewGuid(), "BrepsForRail", ts);
            }
        }

        private Guid GetIdForBrep(Brep brep)
        {
            return DocManager.RhinoDoc.AddToGeometryDoc(brep); // may use DocAdder to control in which way geometry is added to RhinoDoc or just self built collection 
        }

        private Result AddProfilesByPrompt(List<IBuildingComponentId> ts, string v)
        {
            // Get profile curves object
            new RhinoGetCustom().SetCommandPrompt($"Get {v} profile curves")
                                                            .SelectMultiple()
                                                            .SetFilter(GetRailProfileCurves)
                                                            .Get(out IEnumerable<ObjRef> objRefs);
            if (objRefs.Any())
            {
                // Create RhBuidlingComponentId for this collection of ObjRef
                var objIds = objRefs.Select(x => x.ObjectId).ToList();
                var buildingComponentIds = objIds.Select(x => RhBuildingComponentIdFactory.CreateSingle(x, "profileForRail", x));
                ts.Add(RhBuildingComponentIdFactory.Create(Guid.NewGuid(),
                                                    "ProfilesForRail",
                                                    buildingComponentIds));

                return Result.Success;
            }
            return Result.Cancel;
        }

        private bool GetRailProfileCurves(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            return RailProfileCurveFilter.Filter(rhObject, geometry, componentIndex);
        }

        private Result GetRailCurveByPrompt(out ObjRef railObj)
        {
            return new RhinoGetCustom().SetCommandPrompt("Get rail curve")
                                                            .SelectOnlyOne()
                                                            .SetFilter(GetRailCurve)
                                                            .Get(out railObj);
        }
    }
}
