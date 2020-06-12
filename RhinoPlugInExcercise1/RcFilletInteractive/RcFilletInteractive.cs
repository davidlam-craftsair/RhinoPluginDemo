using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("B28615A6-F43A-4EDA-99C1-E5BC2795B33C")]
    public class RcFilletInteractive : AbsCommand
    {
        public RcFilletInteractive()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            if (GetCurve("Get Curve 1", out Line curve1, out Guid objectId1))
            {
                if (GetCurve("Get Curve 2", out Line curve2, out Guid objectId2))
                {
                    var t = new GetCircleV3();
                    t.ConstrainToConstructionPlane(false);
                    t.Line1 = curve1;
                    t.Line2 = curve2;
                    if (GetResult.Point == t.Get())
                    {
                        t.Arc
                    }

                }

            }

            return Result.Success;
        }

        
        private bool GetCurve(string promptMessage, out Line line, out Guid objectId)
        {
            if (RhinoHelper.GetCurve(promptMessage, filter, out Curve curve, out objectId))
            {
                if (curve.TryGetPolyline(out Polyline polyline))
                {
                    line = polyline.SegmentAt(0);
                    return true;
                }
            }
            line = default;
            return false;
        }

        private bool filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry.ObjectType == ObjectType.Curve)
            {
                if (geometry is Curve t)
                {
                    return t.IsLinear(RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
                }
            }
            return false;
        }

        public override string EnglishName => "RcFilletInteractive";

        public static RcFilletInteractive Instance { get; private set; }
    }
}
