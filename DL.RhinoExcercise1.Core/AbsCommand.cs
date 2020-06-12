using Rhino;
using Rhino.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public abstract class AbsCommand : Command
    {
        public RhinoDoc ActiveDoc { get; private set; }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            ActiveDoc = doc;
            return DoRunCommand();
        }

        protected abstract Result DoRunCommand();
    }
}
