using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System.Linq;

namespace RhinoPlugInExcercise1
{
    public class BrepCombinerV2 : IBrepCombiner
    {
        public IRhinoTolerance RhinoTolerance { get; }

        public BrepCombinerV2(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }
        public Brep Combine(Brep originalBrep, Brep dynamicExtrusion)
        {
            return Combine(originalBrep, dynamicExtrusion, RhinoTolerance.Tol);

        }
        public Brep Combine(Brep originalBrep, Brep dynamicExtrusion, double tol)
        {
            Brep[] breps = new Brep[2] { originalBrep, dynamicExtrusion };
            var unionResult = Brep.CreateBooleanUnion(breps, tol);
            if (unionResult != null)
            {
                var union = unionResult.FirstOrDefault();
                // if volumne of the union is the same as the original brep, then it should use difference rather union
                if (union.GetVolume() == originalBrep.GetVolume())
                {
                    var diffResult = Brep.CreateBooleanDifference(originalBrep, dynamicExtrusion, tol);
                    if (diffResult != null)
                    {
                        var diff = diffResult.FirstOrDefault();
                        return diff;
                    }
                }
                else
                {
                    return union;
                }
            }
            return default;
        }
    }
}
