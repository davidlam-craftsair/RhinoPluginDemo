using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public class RhinoHelper
    {
        public static bool GetCurve(string promptMessage,
            GetObjectGeometryFilter filter,
            out Curve curve, out Guid objectId)
        {
            var go = new GetObject();
            go.DisablePreSelect();
            go.SetCommandPrompt(promptMessage);
            go.SetCustomGeometryFilter(filter);
            go.Get();
            if (go.CommandResult() == Result.Success)
            {
                var objRefTmp = go.Object(0);
                var t = objRefTmp.Curve();
                if (t != null)
                {
                    curve = t;
                    objectId = objRefTmp.ObjectId;
                    return true;
                }
            }

            curve = default;
            objectId = default;
            return false;
        }

    }
}
