using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FilletInteractiveRhAction : IRhinoAction
    {
        public RhinoDoc ActiveDoc => DI.ActiveDoc;
        public Result Get()
        {
            if (GetCurve("Get Curve 1", out Line curve1, out Guid objectId1))
            {
                if (GetCurve("Get Curve 2", out Line curve2, out Guid objectId2))
                {
                    var gp = new GetArcFilletDynamic
                    {
                        Line1 = curve1,
                        Line2 = curve2
                    };
                    
                    if (GetResult.Point == gp.Get())
                    {
                        if (gp.GetResultGeometries(out Line line1, out Line line2, out Arc filletArc))
                        {
                            ActiveDoc.Objects.AddArc(filletArc);
                            Replace(objectId1, line1);
                            Replace(objectId2, line2);
                            return Result.Success;
                        }
                    }
                }

            }

            return Result.Failure;
        }
        private void Replace(Guid id, Line line)
        {
            DI.RhinoReplaceManager.Replace(id, line);
        }

        private bool GetCurve(string promptMessage, out Line line, out Guid objectId)
        {
            return DI.RhinoGetManager.TryGetLine(promptMessage, out line, out objectId);
        }

    }
}
