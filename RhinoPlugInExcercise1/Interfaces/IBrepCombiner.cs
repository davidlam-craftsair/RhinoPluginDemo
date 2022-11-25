using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface IBrepCombiner
    {
        /// <summary>
        /// Combine two breps either by union or difference, depends on dynamicextrusion is inside or out the original brep
        /// </summary>
        /// <param name="originalBrep"></param>
        /// <param name="dynamicExtrusion"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        Brep Combine(Brep originalBrep, Brep dynamicExtrusion, double tol);
        Brep Combine(Brep originalBrep, Brep dynamicExtrusion);
    }
}