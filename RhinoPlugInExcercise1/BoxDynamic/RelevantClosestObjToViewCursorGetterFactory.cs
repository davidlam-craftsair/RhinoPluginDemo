using DL.Framework;
using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RelevantClosestObjToViewCursorGetterFactory
    {
        public RelevantClosestObjToViewCursorGetterFactory()
        {

        }
        public IRelevantClosestObjToViewCursorGetter Create()
        {
            return new RelevantClosestObjToViewCursorGetter(IoC.Get<IRelevantObjsGetter>(), IoC.Get<IDistanceBtwObjAndViewGetter>());
        }
    }
}
