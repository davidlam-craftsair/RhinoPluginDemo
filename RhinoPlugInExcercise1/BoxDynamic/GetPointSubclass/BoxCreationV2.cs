using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BoxCreationV2: IBoxCreationV2
    {
        public BoxCreationV2(IBoxCreation boxCreation)
        {
            BoxCreation = boxCreation;
        }
        public Line ViewLine { get; set; }
        public BrepFace SelectedBrepFaceHovered { get; set; }
        //public BrepFace SelectedBrepFaceLocked { get; set; }
        public Point3d BaseCorner1 { get => BoxCreation.BaseCorner1; set => BoxCreation.BaseCorner1 = value; }
        public Point3d BaseCorner2 { get => BoxCreation.BaseCorner2; set => BoxCreation.BaseCorner2 = value; }
        public Plane CPlane1 { get => BoxCreation.CPlane1; set => BoxCreation.CPlane1 = value; }
        public Plane CPlane2 { get => BoxCreation.CPlane2;}
        public Point3d TopCorner { get => BoxCreation.TopCorner; set => BoxCreation.TopCorner = value; }
        public IBoxCreation BoxCreation { get; }

        public Box Get()
        {
            return new Box(CPlane1, new Point3d[] { BaseCorner1, BaseCorner2, TopCorner });
        }

        public void OffsetBaseCorner1(double dx, double dy)
        {
            var pt = BaseCorner1;
            BoxCreation.BaseCorner1 = GetTransformedPt(dx, dy, pt);
        }

        private Point3d GetTransformedPt(double t1, double t2, Point3d pt)
        {
            Transform transform = GetTransform(t1, t2, CPlane1);
            pt.Transform(transform);
            return pt;
        }

        public void OffsetBaseCorner2(double dx, double dy)
        {
            var pt = BaseCorner2;
            BoxCreation.BaseCorner2 = GetTransformedPt(dx, dy, pt);
        }

        private Transform GetTransform(double t1, double t2, Plane cplane)
        {
            var xVector = cplane.XAxis;
            xVector.Unitize();
            var yVector = cplane.YAxis;
            yVector.Unitize();
            var v = xVector * t1 + yVector * t2;
            var transform = Transform.Translation(v);
            return transform;
        }
    }
}
