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
    public class RcCreateFloorByBoundary : AbsCommand
    {
        private IFloorCreateByBoundaryActionV2 _action;

        protected override Result DoRunCommand()
        {
            var go = new GetObject();
            go.SetCommandPrompt("Get floor edges");
            go.GetMultiple(0, 0);
            go.SetCustomGeometryFilter(CustomFilter);
            if (go.CommandResult() == Result.Success)
            {
                return FloorCreateByBoundaryAction.Create(go.Objects());
            }

            return Result.Cancel;
        }

        private bool CustomFilter(RhinoObject arg1, GeometryBase arg2, ComponentIndex arg3)
        {
            if (arg2 is Curve t)
            {
                return t.IsClosed;
            }
            return false;
        }

        public IFloorCreateByBoundaryActionV2 FloorCreateByBoundaryAction
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IFloorCreateByBoundaryActionV2>();  
                    return _action;
                }
                return _action;
            }
        }
        public override string EnglishName => "CreateFloor";
    }
}
