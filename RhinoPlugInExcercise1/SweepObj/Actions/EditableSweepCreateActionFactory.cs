using DL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class EditableSweepCreateActionFactory : IEditableSweepCreateActionFactory
    {
        public IEditableSweepCreateAction Create(IComponentsToSessionAdderV2 componentsToSessionAdder)
        {
            return new EditableRailingCreateActionV2(IoC.Get<IDocManager>(), 
                                                     IoC.Get<IBrepSweepsFactory>(), 
                                                     componentsToSessionAdder,
                                                     
                                                     );
        }

        public IEditableSweepCreateAction CreateGeneric()
        {
            return Create(GetAssemblyComponetsToSessionAdderFactory().CreateGeneric());
        }

        private static IAssemblyComponentsToSessionAdderV2Factory GetAssemblyComponetsToSessionAdderFactory()
        {
            return IoC.Get<IAssemblyComponentsToSessionAdderV2Factory>();
        }

        public IEditableSweepCreateAction CreateForRailingComponents()
        {
            return Create(GetAssemblyComponetsToSessionAdderFactory().CreateForRailingComponentsAssembly());
        }
    }
}
