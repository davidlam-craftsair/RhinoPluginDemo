using DL.RhinoExcercise1.Core;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IRailingAssemblyBrepUpdateSubscriber
    {
        void SubscribeCompositionalElementsChanges(ICompositeObjId compositeObjIds);
    }
}