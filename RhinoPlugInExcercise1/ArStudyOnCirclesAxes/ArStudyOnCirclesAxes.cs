using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("87D92FE4-BEA8-434A-A5DA-CE611F7BF2D0")]
    public class ArStudyOnCirclesAxes : AbsCommand
    {
        public ArStudyOnCirclesAxes()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            GetCurve("Get Fixed Curve 1 where cirlces can only", out Curve curve1, out Guid objectId1);
            GetCurve("Get Curve 2", out Curve curve2, out Guid objectId2);

            var t = new GetCircleClosestPt();
            t.ConstrainToConstructionPlane(false);
            t.Curve1 = curve1;
            t.Curve2 = curve2;
            t.Get();
            return Result.Success;
        }

        
        private bool GetCurve(string promptMessage, out Curve curve, out Guid objectId)
        {
            return RhinoHelper.GetCurve(promptMessage, filter, out curve, out objectId);
        }

        private bool filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            return geometry.ObjectType == ObjectType.Curve;
        }

        public override string EnglishName => "ArStudyOnCirclesAxes";

        public static ArStudyOnCirclesAxes Instance { get; private set; }
    }
}
