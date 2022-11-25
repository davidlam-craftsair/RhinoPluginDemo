using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class EdgesSorter : IEdgesSorter
    {
        /// <summary>
        /// Find the outer edge from curves
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="outerEdge"></param>
        /// <param name="remaining"></param>
        /// <returns></returns>
        public bool Find(IEnumerable<Curve> curves, out Curve outerEdge, out IEnumerable<Curve> remaining)
        {
            // to find a curve its start pt doesnt lie within other curves
            IOrderedEnumerable<Curve> ts = curves.OrderByDescending(GetArea);
            // Curve with max area is the outer edge
            outerEdge = ts.FirstOrDefault();
            // Remove the first one, is the remaining curve
            var tstmp = ts.ToList();
            tstmp.RemoveAt(0);
            remaining = tstmp;
            return true;

        }

        private double GetArea(Curve arg)
        {
            if (arg == null)
            {
                return 0;
            }
            return AreaMassProperties.Compute(arg)?.Area??0;
        }
    }
}
