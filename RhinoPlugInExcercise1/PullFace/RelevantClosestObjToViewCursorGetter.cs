using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RelevantClosestObjToViewCursorGetter : IRelevantClosestObjToViewCursorGetter
    {
        public RelevantClosestObjToViewCursorGetter(IRelevantObjsGetter relevantObjsGetter,
                                                    IDistanceBtwObjAndViewGetter distanceBtwObjAndViewGetter)
        {
            RelevantObjsGetter = relevantObjsGetter;
            DistanceBtwObjAndViewGetter = distanceBtwObjAndViewGetter;
        }

        public IRelevantObjsGetter RelevantObjsGetter { get; }
        public IDistanceBtwObjAndViewGetter DistanceBtwObjAndViewGetter { get; }

        public RhinoObject Get(RhinoViewport viewport, Line viewLine)
        {
            // find out the visible objs
            var relevantObjs = GetRelevantObjs(viewport, viewLine);
#if Debug

            RhinoApp.WriteLine($"No of relevant objs = {relevantObjs.Count()}");
#endif
            var closestObj = relevantObjs.OrderBy(x => GetDistance(x, viewLine))
                                         .FirstOrDefault();
            return closestObj;
        }

        private IEnumerable<RhinoObject> GetRelevantObjs(RhinoViewport viewport, Line viewLine)
        {
            return RelevantObjsGetter.Get(viewport, viewLine);
        }

        private double GetDistance(RhinoObject x, Line viewLine)
        {
            return DistanceBtwObjAndViewGetter.Get(x, viewLine);
        }
    }
}
