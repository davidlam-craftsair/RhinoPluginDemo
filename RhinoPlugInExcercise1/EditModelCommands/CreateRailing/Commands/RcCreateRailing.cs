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
    public class RcCreateRailing: AbsCommand
    {
        private ICreateEditableSweepAction _action;

        protected override Result DoRunCommand()
        {
            // Get the rail curve
            var go = new GetObject();
            go.SetCommandPrompt("Get rail curve");
            go.GetMultiple(1, 1);
            go.SetCustomGeometryFilter(GetRailCurve);
            go.Get();
            if (go.CommandResult() == Result.Success)
            {
                var railObj = go.Objects().FirstOrDefault();
                // Get the profile curves  

                var gp = new GetObject();
                gp.SetCommandPrompt("Get profile curves");
                gp.GetMultiple(1, 0);
                gp.SetCustomGeometryFilter(GetProfileCurves);
                go.Get();
                if (gp.CommandResult() == Result.Success)
                {
                    var profileCurveObjs = gp.Objects();
                // Sweep the profile curves along the rail curve
                    return Action.Create(railObj, profileCurveObjs);
                }
            }

            return Result.Cancel;
        }

        private bool GetProfileCurves(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
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

        public ICreateEditableSweepAction Action
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<ICreateEditableSweepAction>();
                }
                return _action;
            }
        }
    }
}
