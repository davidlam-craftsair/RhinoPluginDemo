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
    public class RcCloseEditRailing : AbsCommand
    {
        private IRailingCloseEditAction _action;

        protected override Result DoRunCommand()
        {
            return Action.Do();
        }

        public IRailingCloseEditAction Action 
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IRailingCloseEditAction>();
                }
                return _action;
            }
            
        }

        public override string EnglishName => "CloseRailingEdit";
    }
}
