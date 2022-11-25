using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public class RhinoGetManager : IRhinoGetManager
    {
        public RhinoDoc ActiveDoc => RhinoDoc.ActiveDoc;
        public bool TryGetLine(string promptMessage, out Line line, out Guid objectId)
        {
            if (RhinoHelper.GetCurve(promptMessage, lineFilter, out Curve curve, out objectId))
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

        
        private bool lineFilter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry.ObjectType == ObjectType.Curve)
            {
                if (geometry is Curve t)
                {
                    return t.IsLinear(ActiveDoc.ModelAbsoluteTolerance);
                }
            }
            return false;
        }

        public bool TryGetLine(string promptMessage, out Line line, out Curve curve, out Guid objectId)
        {
            if (RhinoHelper.GetCurve(promptMessage, lineFilter, out Curve curvetmp, out objectId))
            {
                if(RhinoHelper.TryGetPolylineFromCurveV2(curvetmp, out Polyline polyline))
                {
                    line = polyline.SegmentAt(0);
                    curve = curvetmp;
                    return true;
                }
            }
            line = default;
            curve = default;
            objectId = default;
            return false;
        }


    }
}
