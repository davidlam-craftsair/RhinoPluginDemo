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
    /// Ask for multiple times for profiles
    /// </summary>
    public class RcCreateRailingV4: AbsCommand
    {
        private IEditableSweepCreateAction _action;

        public IDictionary<int, string> NamesByInteger
        {
            get
            {
                if (namesByInteger)
            }
        }
        protected override Result DoRunCommand()
        {
            // Get the rail curve
            if (Result.Success == GetRailCurve(out ObjRef railPathObj))
            {
                var ts = new List<IEnumerable<ObjRef>>();
                var r = Result.Success;
                var count = 0;
                while (r == Result.Success)
                {
                    r = GetProfile(out IEnumerable<ObjRef> profileObjs, GetIterationName(count));
                }
                if (Result.Success == GetProfile(out IEnumerable<ObjRef> profileObjs1, "1st"))
                {
                    ts.Add(profileObjs1);
                    if (Result.Success == GetProfile(out IEnumerable<ObjRef> profileObjs2, "1st"))
                    {

                    }
                }
                return EditableSweepCreateAction.Create(railPathObj, profileObjs1);
            }
            return Result.Cancel;
        }

        private string GetIterationName(int count)
        {
            var t = DocManager.SessionDoc.GetAssemblyCount(AssemblyTypeNames.Railing);

            return $"{t + count}";
        }


        private Result GetProfile(out IEnumerable<ObjRef> profileObjs, string iteration)
        {
            return new RhinoGetCustom().SetCommandPrompt($"Get {iteration} profile curves")
                                                            .SelectMultiple()
                                                            .SetFilter(GetRailProfileCurves)
                                                            .Get(out profileObjs);
        }

        private Result GetRailCurve(out ObjRef railObj)
        {
            return new RhinoGetCustom().SetCommandPrompt("Get rail curve")
                                                            .SelectOnlyOne()
                                                            .SetFilter(GetRailCurve)
                                                            .Get(out railObj);
        }

        private bool GetRailProfileCurves(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Curve t)
            {
                return t.IsPlanar() && t.IsClosed;
            }
            return false;
        }

        private bool GetRailCurve(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Curve t)
            {
                return true;
            }
            return false;
        }

        public override string EnglishName => "CreateRailing";

        public IEditableSweepCreateAction EditableSweepCreateAction
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IEditableSweepCreateActionFactory>().CreateForRailingComponents();
                }
                return _action;
            }
        }
    }
}
