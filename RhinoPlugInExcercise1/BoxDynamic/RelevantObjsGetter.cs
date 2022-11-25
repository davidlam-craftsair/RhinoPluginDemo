using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RelevantObjsGetter : IRelevantObjsGetter
    {
        public RelevantObjsGetter()
        {
        }
        public IEnumerable<RhinoObject> Get(RhinoViewport viewport, Line viewLine, RhinoDoc activeDoc)
        {
            return GetRelevantObjs(GetViewportVisibleObjs(viewport, activeDoc), viewLine, activeDoc.ModelAbsoluteTolerance);
        }

        private IEnumerable<RhinoObject> GetRelevantObjs(IEnumerable<RhinoObject> viewportVisibleObjs, Line viewLine, double tol)
        {
            return viewportVisibleObjs.Where(x => CheckLineIntersectObj(x, viewLine, tol));
        }

        private bool CheckLineIntersectObj(RhinoObject x, Line viewLine, double tol)
        {
            return Intersection.LineBox(viewLine, x.Geometry.GetBoundingBox(true), tol, out Interval lineParameters);
        }

        private static IEnumerable<RhinoObject> GetViewportVisibleObjs(RhinoViewport viewport, RhinoDoc activeDoc)
        {
            return activeDoc.Objects.Where(x => viewport.IsVisible(x.Geometry.GetBoundingBox(false)));
        }
    }
}
