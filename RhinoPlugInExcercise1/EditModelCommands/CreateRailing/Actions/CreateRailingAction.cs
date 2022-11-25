using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class CreateRailingAction 
    {
        public Result Create(ObjRef railObj, IEnumerable<ObjRef> profileObjs)
        {
            var railCurve = railObj.Curve();
            if (railCurve != null)
            {
                var profileCurves = profileObjs.Select(x => x.Curve());
                
                
            }

            return Result.Cancel;
        }
    }
}
