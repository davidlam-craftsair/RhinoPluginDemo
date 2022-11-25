using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceFromBrepHandlerV2 : IPullFaceFromBrepHandler
    {
        public IBrepCleaner BrepCleaner { get; }
        public IBrepCombiner BrepCombiner { get; }

        public PullFaceFromBrepHandlerV2(IBrepCleaner brepCleaner, IBrepCombiner brepCombiner)
        {
            BrepCleaner = brepCleaner;
            BrepCombiner = brepCombiner;
        }
        public bool Pull(Brep originalBrep, Brep dynamicExtrusion, out Brep brepWholeTmp)
        {
            // Determine whether to use Union or Difference
            brepWholeTmp = GetBrep(originalBrep, dynamicExtrusion);
            CleanBrep(brepWholeTmp);
            return brepWholeTmp != null;
        }

        private Brep GetBrep(Brep originalBrep, Brep dynamicExtrusion)
        {
            return BrepCombiner.Combine(originalBrep, dynamicExtrusion);
        }

        private void CleanBrep(Brep brepWholeTmp)
        {
            BrepCleaner.Clean(brepWholeTmp);
        }
    }
}
