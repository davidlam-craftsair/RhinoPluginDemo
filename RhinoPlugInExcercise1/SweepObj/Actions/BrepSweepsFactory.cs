using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepSweepsFactory : IBrepSweepsFactory
    {
        public BrepSweepsFactory(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }
        public double Tol => RhinoTolerance.Tol;

        public IEnumerable<Brep> Create(Curve rail, IEnumerable<Curve> shapes)
        {
            var ts = new List<Brep>();
            foreach (var item in shapes)
            {
                var breps = Brep.CreateFromSweep(rail, item, false, Tol);
                ts.AddRange(breps);
            }

            return ts;
        }
    }
}
