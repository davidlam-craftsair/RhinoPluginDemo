using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PullFaceCreation : IPullFaceCreation
    {
        public PullFaceCreation(IExtrusionCreation dynamicExtrusionCreation,
                                IPullFaceFromExtrusionHandler pullFaceFromExtrusionHandler,
                                IPullFaceFromBrepHandler pullFaceFromBrepHandler)
        {
            DynamicExtrusionCreation = dynamicExtrusionCreation;
            PullFaceFromExtrusionHandler = pullFaceFromExtrusionHandler;
            PullFaceFromBrepHandler = pullFaceFromBrepHandler;
        }

        #region Properties
        public IExtrusionCreation DynamicExtrusionCreation { get; }
        public IPullFaceFromExtrusionHandler PullFaceFromExtrusionHandler { get; }
        public IPullFaceFromBrepHandler PullFaceFromBrepHandler { get; }

        public Line ViewLine { get => DynamicExtrusionCreation.ViewLine; set => DynamicExtrusionCreation.ViewLine = value; }
        public BrepFace SelectedFaceToPull { get => DynamicExtrusionCreation.SelectedFaceToPull; set => DynamicExtrusionCreation.SelectedFaceToPull = value; }
        public Plane CPlane { get => DynamicExtrusionCreation.CPlane; set => DynamicExtrusionCreation.CPlane = value; }
        public Vector3d ExtrusionVector { get => DynamicExtrusionCreation.ExtrusionVector; set => DynamicExtrusionCreation.ExtrusionVector = value; }
        public Guid ObjId { get; set; }

        #endregion

        public Brep GetResultantBrep(RhinoDoc doc)
        {
            var dynamicExtrusion = DynamicExtrusionCreation.Get();
            if (dynamicExtrusion != null)
            {
                RhinoObject obj = GetObjById(ObjId);

                if (obj.Geometry is Extrusion originalExtrusion)
                {
                    if (PullFaceFromExtrusion(originalExtrusion,
                                              ExtrusionVector,
                                              dynamicExtrusion,
                                              out Brep brepWholeTmp))
                    {
                        return brepWholeTmp;
                    }
                }
                else if (obj.Geometry is Brep originalBrep)
                {
                    if (PullFaceFromBrep(originalBrep, dynamicExtrusion, out Brep brepWholeTmp))
                    {
                        return brepWholeTmp;

                    }
                }
            }
            return default;
        }

        private bool PullFaceFromBrep(Brep originalBrep, Brep dynamicExtrusion, out Brep brepWholeTmp)
        {
            return PullFaceFromBrepHandler.Pull(originalBrep, dynamicExtrusion, out brepWholeTmp);
        }

        private bool PullFaceFromExtrusion(Extrusion originalExtrusion,
                                   Vector3d extrusionVector,
                                   Brep dynamicExtrusion,
                                   out Brep brepWholeTmp)
        {
            return PullFaceFromExtrusionHandler.Pull(originalExtrusion, extrusionVector, dynamicExtrusion, out brepWholeTmp);
        }


        private RhinoObject GetObjById(Guid id)
        {
            var objectTable = RhinoDoc.ActiveDoc.Objects;
            var obj = objectTable.Find(id);
            return obj;
        }



        public Brep GetDynamicExtrusion()
        {
            return DynamicExtrusionCreation.Get();
        }
    }
}
