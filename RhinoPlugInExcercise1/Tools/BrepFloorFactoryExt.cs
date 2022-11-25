using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepFactoryExt: IBrepFactoryExt
    {
        public BrepFactoryExt(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }

        public IEnumerable<Brep> Create(IEnumerable<Curve> edges, double height)
        {
            var extrudeDir = Plane.WorldXY.ZAxis;
            var extrudeVector = extrudeDir * height;

            var breps = new List<Brep>() {};
            double tol = RhinoTolerance.Tol;
            foreach (var edge in edges)
            {
                if (edge == null)
                {
                    continue;
                }
                var brep = Surface.CreateExtrusion(edge, extrudeVector).ToBrep();
                var brepCapped = brep.CapPlanarHoles(tol);
                breps.Add(brepCapped);
            }

            return breps;
        }
    }
}
