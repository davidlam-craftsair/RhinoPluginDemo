using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RcFloorCloseEdit : AbsCommand
    {
        private IFloorBoundaryCloseEditAction _action;

        protected override Result DoRunCommand()
        {
            return FloorBoundaryCloseEditAction.Do();
        }

        public IFloorBoundaryCloseEditAction FloorBoundaryCloseEditAction
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IFloorBoundaryCloseEditAction>();
                }
                return _action;
            }
        }

        public override string EnglishName => "CloseFloorEdit";
    }
}
