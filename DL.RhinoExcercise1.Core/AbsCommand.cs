using DL.Framework;
using Rhino;
using Rhino.Commands;
using RhinoPlugInExcercise1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public abstract class AbsCommand : Command
    {
        public AbsCommand()
        {

        }
        public RhinoDoc ActiveDoc { get; private set; }
        public IRhinoTolerance RhinoTolerance { get; } = IoC.Get<IRhinoTolerance>();
        public IDocManager DocManager { get; } = IoC.Get<IDocManager>();


        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            DocManager.Update(doc);
            ActiveDoc = doc;
            RhinoTolerance.SetTol(doc.ModelAbsoluteTolerance, doc.ModelAngleToleranceRadians);
            return DoRunCommand();
        }   

        protected abstract Result DoRunCommand();

        protected T Get<T>()
        {
            return IoC.Get<T>();
        }
    }
}
