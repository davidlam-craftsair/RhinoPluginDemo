using DL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class AssemblyComponentsToSessionAdderFactory : IAssemblyComponentsToSessionAdderFactory
    {
        public IComponentsToSessionAdder Create()
        {
            return new GenericAssemblyComponentsToSessionAdder(DI.DocManager, IoC.Get<IAssemblyIdFactory>(), "Generic");
        }

        public IComponentsToSessionAdder CreateForRailingComponentsAssembly()
        {
            return new RailingComponentsToSessionAdder(DI.DocManager, IoC.Get<IRailingAssemblyIdFactory>());
        }
    }
}
