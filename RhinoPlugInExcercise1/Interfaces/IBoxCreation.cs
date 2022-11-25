using Rhino.Geometry;
using System;

namespace RhinoPlugInExcercise1
{
    public interface IBoxCreation
    {
        Point3d BaseCorner1 { get; set; }
        Point3d BaseCorner2 { get; set; }
        Plane CPlane1 { get; set; }
        Plane CPlane2 { get;}
        Point3d TopCorner { get; set; }

        Box Get();
    }

    public interface IBoxCreationV2: IBoxCreation
    {
        Line ViewLine { get; set; }
        BrepFace SelectedBrepFaceHovered { get; set; }
        //BrepFace SelectedBrepFaceLocked { get; set; }

        void OffsetBaseCorner1(double dx, double dy);
        void OffsetBaseCorner2(double dx, double dy);
    }

    public interface IExtrusionCreation
    {
        Line ViewLine { get; set; }
        BrepFace SelectedFaceToPull { get; set; }
        Plane CPlane { get; set; }
        Vector3d ExtrusionVector { get; set; }
        
        Brep Get();
    }

}