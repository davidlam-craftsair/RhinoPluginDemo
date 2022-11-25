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
    class RcSweepObjV2 : AbsCommand
    {
        private IEditableSweepCreateAction _action;

        protected override Result DoRunCommand()
        {
            if (Result.Success == GetRailObj(out ObjRef railObj))
            {
                if (Result.Success == GetProfileObjs(out IEnumerable<ObjRef> profileObjs))
                {
                    return EditableSweepCreateAction.Create(railObj, profileObjs);
                }
            }

            return Result.Cancel;

        }

        private Result GetProfileObjs(out IEnumerable<ObjRef> profileObjs)
        {
            return new RhinoGetCustom().SetCommandPrompt("Select profiles")
                                       .SetFilter(SelectProfileCurve)
                                       .SelectMultiple()
                                       .Get(out profileObjs);
        }

        private Result GetRailObj(out ObjRef railObj)
        {
            return new RhinoGetCustom().SetCommandPrompt("Select rail")
                                       .SelectOnlyOne()
                                       .SetFilter(SelectRailCurve)
                                       .SelectSubObject()
                                       .DisablePreSelect()
                                       .Get(out railObj);
        }

        private bool SelectProfileCurve(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Curve t)
            {
                return t.IsPlanar();
            }
            return false;
        }

        private bool SelectRailCurve(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Curve)
            {
                return true;
            }
            return false;
        }

        public override string EnglishName => "SweepObj";

        public IEditableSweepCreateAction EditableSweepCreateAction
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IEditableSweepCreateAction>();
                }
                return _action;
            }
        }

    }
}
