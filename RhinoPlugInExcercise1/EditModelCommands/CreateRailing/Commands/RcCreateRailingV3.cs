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
    public class RcCreateRailingV2: AbsCommand
    {
        private IEditableSweepCreateAction _action;

        protected override Result DoRunCommand()
        {
            // Get the rail curve
            if (Result.Success == GetRailCurve(out ObjRef railPathObj))
            {
                if (Result.Success == GetProfile(out IEnumerable<ObjRef> profileObjs))
                {
                    return EditableSweepCreateAction.Create(railPathObj, profileObjs);
                }
            }
            return Result.Cancel;
        }

        private Result GetProfile(out IEnumerable<ObjRef> profileObjs)
        {
            return new RhinoGetCustom().SetCommandPrompt("Get top profile curves")
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
