using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BrepCleaner : IBrepCleaner
    {
        public BrepCleaner(IRhinoTolerance rhinoTolerance)
        {
            RhinoTolerance = rhinoTolerance;
        }

        public IRhinoTolerance RhinoTolerance { get; }

        public void Clean(Brep brep)
        {
            if (brep.MergeCoplanarFaces(RhinoTolerance.Tol, RhinoTolerance.RadAngleTol))
            {
            }
            brep.Standardize();
            brep.Compact();
        }
    }
}
