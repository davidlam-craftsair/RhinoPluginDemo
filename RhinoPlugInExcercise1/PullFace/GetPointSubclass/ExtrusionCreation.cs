using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class ExtrusionCreation : IExtrusionCreation
    {
        public Line ViewLine { get; set; }
        public BrepFace SelectedFaceToPull { get; set; }
        public Plane CPlane { get; set; }
        public Vector3d ExtrusionVector { get; set; }


        public Brep Get()
        {
            var constructionPlane = CPlane;
            var extrusionVector = ExtrusionVector;
            Curve profile = GetProfile(SelectedFaceToPull);
            int parallelInfo;
            if (profile.TryGetPlane(out Plane curvePlane))
            {
                parallelInfo = curvePlane.ZAxis.IsParallelTo(extrusionVector);
            }
            else
            {
                parallelInfo = extrusionVector.IsParallelTo(constructionPlane.ZAxis);
            }
            if (profile != null)
            {
                if (parallelInfo == 1)
                {
                    double height = extrusionVector.Length * (1);
                    return GetBrep(profile, height);

                }
                else if (parallelInfo == -1)  // anti-parallel
                {
                    double height = extrusionVector.Length * (-1);
                    return GetBrep(profile, height);
                }
                else
                {
                    if (!extrusionVector.IsZero)
                    {
                        // extrusionVector is perpendicular to ZAxis of construction plane
                        //var t = extrusionVector.IsPerpendicularTo(constructionPlane.ZAxis);
                        //if (t)
                        //{

                        //}
                        //throw new NotImplementedException();
                    }

                }
            }
            return default;
        }

        private static Brep GetBrep(Curve profile, double height)
        {
            var t = Extrusion.Create(profile, height, true);
            if (t != null)
            {
                return t.ToBrep();
            }
            return default;
        }

        private Curve GetProfile(BrepFace brepFace)
        {
            var brep = brepFace.DuplicateFace(false);
            var edges = brep.Edges;
            var joinedCurves = Curve.JoinCurves(edges);
            var profile = joinedCurves.FirstOrDefault();
            return profile;
        }

    }
}
