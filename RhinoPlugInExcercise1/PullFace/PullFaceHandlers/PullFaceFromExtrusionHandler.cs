using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceFromExtrusionHandler : IPullFaceFromExtrusionHandler
    {
        public IBrepCleaner BrepCleaner { get; }
        public IRhinoTolerance RhinoTolerance { get; }
        public double Tol => RhinoTolerance.Tol;
        public PullFaceFromExtrusionHandler(IBrepCleaner brepCleaner, IRhinoTolerance rhinoTolerance)
        {
            BrepCleaner = brepCleaner;
            RhinoTolerance = rhinoTolerance;
        }
        public bool Pull(Extrusion originalExtrusion,
                        Vector3d extrusionVector,
                        Brep dynamicExtrusion,
                        out Brep brepWholeTmp)
        {
            int parallelInfo = extrusionVector.IsParallelTo(originalExtrusion.PathTangent, Tol);
            if (parallelInfo == -1) // anti - parallel
            {
                brepWholeTmp = dynamicExtrusion;
                return true;
            }
            else if (parallelInfo == 1)
            {
                if (originalExtrusion.ProfileCount == 1)
                {
                    var t = GetResultExtrusionForExtrusionVectorAlignedWithXAxis(originalExtrusion, extrusionVector);
                    brepWholeTmp = t.ToBrep();
                    return true;
                }
            }
            else // not parallel
            {
                return GetBrepWholeAfterExtrusion(originalExtrusion, dynamicExtrusion, out brepWholeTmp);

            }

            brepWholeTmp = default;
            return false;
        }

        private static Extrusion GetResultExtrusionForExtrusionVectorAlignedWithXAxis(Extrusion originalExtrusion, Vector3d extrusionVector)
        {
            var originalOuterProfile = originalExtrusion.Profile3d(0, 0);

            Point3d t = originalExtrusion.PathEnd + extrusionVector;
            Vector3d newVector = t - originalExtrusion.PathStart;
            return Extrusion.Create(originalOuterProfile, newVector.Length, true);
        }

        private bool GetBrepWholeAfterExtrusion(Extrusion originalExtrusion, Brep dynamicExtrusion, out Brep brepWholeTmp)
        {
            var brep = originalExtrusion.ToBrep();
            var brepTmp = Brep.CreateBooleanUnion(new Brep[] { brep, dynamicExtrusion }, Tol).FirstOrDefault();
            if (brepTmp != null)
            {
                CleanBrep(brepTmp);
                brepWholeTmp = brepTmp;
                return true;
            }
            brepWholeTmp = default;
            return false;
        }

        private void CleanBrep(Brep brepTmp)
        {
            BrepCleaner.Clean(brepTmp);
        }
    }
}
