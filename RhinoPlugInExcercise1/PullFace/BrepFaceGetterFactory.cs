using DL.RhinoExcercise1.Core;
using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepFaceGetterFactory
    {
        public BrepFaceGetterFactory(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }

        public IBrepFaceGetter Create()
        {
            return new BrepFaceGetter(new RelevantClosestObjToViewCursorGetterFactory().Create(),
                                    new RelevantBrepFaceClosestToViewCursorGetterV3(RhinoTolerance)
                                    );
        }
    }
}
