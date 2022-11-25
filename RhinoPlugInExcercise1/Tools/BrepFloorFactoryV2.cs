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
    public class BrepFloorFactoryV2 : IBrepFloorFactory
    {
        public BrepFloorFactoryV2(IRhinoTolerance rhinoTolerance, IReporter reporter, IBrepFactoryExt brepFactoryExt)
        {
            RhinoTolerance = rhinoTolerance;
            Reporter = reporter;
            BrepFactoryExt = brepFactoryExt;
        }

        public IRhinoTolerance RhinoTolerance { get; }
        public IReporter Reporter { get; }
        public IBrepFactoryExt BrepFactoryExt { get; }

        public Brep Create(Curve outerEdge, IEnumerable<Curve> innerEdges, double floorHeight)
        {
            var outerMass = BrepFactoryExt.Create(new Curve[] { outerEdge }, floorHeight);

            if (!innerEdges.Any())
            {
                return outerMass.FirstOrDefault();
            }
            var innerVoids = BrepFactoryExt.Create(innerEdges, floorHeight);

            var breps = Brep.CreateBooleanDifference(outerMass, innerVoids, RhinoTolerance.Tol);
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
