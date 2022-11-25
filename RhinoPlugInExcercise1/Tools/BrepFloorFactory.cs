using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepFloorFactory : IBrepFloorFactory
    {
        public BrepFloorFactory(IRhinoTolerance rhinoTolerance, IReporter reporter)
        {
            RhinoTolerance = rhinoTolerance;
            Reporter = reporter;
        }

        public IRhinoTolerance RhinoTolerance { get; }
        public IReporter Reporter { get; }

        public Brep Create(Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight)
        {
            var extrudeDir = Plane.WorldXY.ZAxis;
            var extrudeVector = extrudeDir * floorHeight;
            var outerMass = Surface.CreateExtrusion(outerEdge, extrudeVector).ToBrep();
            if (!innerEdges.Any())
            {
                return outerMass;
            }
            var innerVoids = new List<Brep>();
            foreach (var innerEdge in innerEdges)
            {
                var innerVoid = Surface.CreateExtrusion(innerEdge, extrudeVector).ToBrep();
                innerVoids.Add(innerVoid);
            }
            
            var breps = Brep.CreateBooleanDifference(outerMass, innerVoids.FirstOrDefault(), RhinoTolerance.Tol);
            //var breps = Brep.CreateBooleanDifference(new Brep[] { outerMass }, innerVoids, RhinoTolerance.Tol);
            if (breps.Count() > 1)
            {
                Reporter.Report("floor creation have more than 1 brep, only 1 brep is created, other breps are ignored");
            }
            if (breps.Count() == 0)
            {
                Reporter.Report("no floor is created");
                return null;
            }
            return breps.First();
        }
    }
}
