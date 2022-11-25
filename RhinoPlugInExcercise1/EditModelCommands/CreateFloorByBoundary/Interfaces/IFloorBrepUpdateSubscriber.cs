using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IFloorBrepUpdateSubscriber
    {
        void SubscribeUpdate(IFloorUpdateInfo rhinoFloorUpdateInfo);
        void UnsubscribeUpdate(IFloorUpdateInfo rhinoFloorUpdateInfo);
    }
}