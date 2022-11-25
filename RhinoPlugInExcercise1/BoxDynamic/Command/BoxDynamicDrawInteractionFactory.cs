using DL.Framework;
using Rhino.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicDrawInteractionFactory
    {
        public IPlaneDrawer PlaneDrawer { get; } = IoC.Get<IPlaneDrawer>();
        public IDrawer TypicalDrawer { get; } = IoC.Get<IDrawer>();
        public IDynamicDrawInteraction Create(IBoxCreationV2 boxCreation)
        {
            return new BoxDynamicDrawInteraction(new BoxDynamicDrawInteractionIteration0(boxCreation,
                                                                                         TypicalDrawer),
                                                 new BoxDynamicDrawInteractionIteration1(boxCreation, TypicalDrawer),
                                                 new BoxDynamicDrawInteractionIteration2(boxCreation, TypicalDrawer),
                                                 new BoxDynamicDrawInteractionIteration3(boxCreation),
                                                 boxCreation);
        }
    }
}
