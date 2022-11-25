using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepCombiner: IBrepCombiner
    {
        public IBrepAdditionOrSubstractionChecker BrepAdditionOrSubstractionChecker { get; }

        public BrepCombiner(IBrepAdditionOrSubstractionChecker brepAdditionOrSubstractionChecker)
        {
            BrepAdditionOrSubstractionChecker = brepAdditionOrSubstractionChecker;
        }
        public Brep Combine(Brep originalBrep, Brep dynamicExtrusion, double tol)
        {
            Brep brepWholeTmp;
            var r = CheckBrepAdditionOrSubtraction(originalBrep, dynamicExtrusion);
            IEnumerable<Brep> brepsResult = Enumerable.Empty<Brep>();
            if (r == 1)
            {
                brepsResult = Union(originalBrep, dynamicExtrusion, tol);
            }
            else
            {
                brepsResult = Difference(originalBrep, dynamicExtrusion, tol);
            }

            if (brepsResult.Count() > 1)
            {
                throw new NotImplementedException();
            }
            brepWholeTmp = brepsResult.FirstOrDefault();
            return brepWholeTmp;
        }

        private int CheckBrepAdditionOrSubtraction(Brep originalBrep, Brep dynamicExtrusion)
        {
            return BrepAdditionOrSubstractionChecker.Check(originalBrep, dynamicExtrusion);
        }

        private static Brep[] Difference(Brep originalBrep, Brep dynamicExtrusion, double tol)
        {
            return Brep.CreateBooleanDifference(originalBrep, dynamicExtrusion, tol, true);
        }

        private static Brep[] Union(Brep originalBrep, Brep dynamicExtrusion, double tol)
        {
            return Brep.CreateBooleanUnion(new Brep[2] { originalBrep, dynamicExtrusion }, tol);
        }
    }
}
