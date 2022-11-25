using Rhino;
using Rhino.Geometry;
using System;

namespace RhinoPlugInExcercise1
{
    public interface IPullFaceCreation
    {
        Line ViewLine { get; set; }
        BrepFace SelectedFaceToPull { get; set; }
        Plane CPlane { get; set; }
        Vector3d ExtrusionVector { get; set; }
        Guid ObjId { get; set; }

        Brep GetResultantBrep(RhinoDoc rhinoDoc);
        Brep GetDynamicExtrusion();
    }
}