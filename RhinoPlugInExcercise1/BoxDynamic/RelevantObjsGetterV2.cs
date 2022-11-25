
using DL.RhinoExcercise1.Core;
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
    public class RelevantObjsGetterV2 : IRelevantObjsGetter
    {
        public IRhinoTolerance RhinoTolerance { get; }
        public IRhinoDocContainer RhinoDocContainer { get; }

        public RelevantObjsGetterV2(IRhinoTolerance rhinoTolerance, IRhinoDocContainer rhinoDocContainer)
        {
            RhinoTolerance = rhinoTolerance;
            RhinoDocContainer = rhinoDocContainer;
        }
        public IEnumerable<RhinoObject> Get(RhinoViewport viewport, Line viewLine)
        {
            return GetRelevantObjs(GetViewportVisibleObjs(viewport,
                                                          RhinoDocContainer.RhinoDoc), 
                                    viewLine, 
                                    RhinoTolerance.Tol);
        }

        private IEnumerable<RhinoObject> GetRelevantObjs(IEnumerable<RhinoObject> viewportVisibleObjs, Line viewLine, double tol)
        {
            var ts = viewportVisibleObjs.Where(x => CheckLineIntersectObj(x, viewLine, tol));
            // A more accurate screening 
            var rhinoObjs = new List<RhinoObject>();
            foreach (var item in ts)
            {
                if (item.Geometry is Brep brep)
                {
                    if (CheckLineBrepIntersect(viewLine, brep, tol))
                    {
                        rhinoObjs.Add(item);
                    }
                }
                else if (item.Geometry is Surface surface)
                {
                    if (CheckLineBrepIntersect(viewLine, surface.ToBrep(), tol))
                    {
                        rhinoObjs.Add(item);
                    }
                }
                else
                {
                    // for other unidentified objs, just add
                    rhinoObjs.Add(item);
                }
            }
            return rhinoObjs;
        }

        private bool CheckLineBrepIntersect(Line viewLine, Brep brep, double tol)
        {
            return RhinoHelper.CurveBrepIntersection(viewLine.ToNurbsCurve(), brep, tol, out Curve[] overlapCurves, out Point3d[] intersectPts);
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
