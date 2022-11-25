using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetCircle: GetPoint
    {

        public double Radius { get; set; }
        public Point3d Center { get; set; }
        public Plane Plane { get; set; }
        public Curve Curve1 { get; set; }
        public Curve Curve2 { get; set; }

        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            var tol = e.RhinoDoc.ModelAbsoluteTolerance;
            var movingPt = e.CurrentPoint;

            Radius = GetRadius(movingPt);
            if (Radius < tol)
            {
                return;
            }
            Center = movingPt;
            Plane = e.RhinoDoc.Views.ActiveView.ActiveViewport.ConstructionPlane();
            DrawCircle(e.Display);
        }

        private double GetRadius(Point3d movingPt)
        {
            Point3d ptOnCircle = FindPtOnCircle(movingPt);

            return ptOnCircle.DistanceTo(movingPt);
        }

        private Point3d FindPtOnCircle(Point3d movingPt)
        {
            var ts = new (Point3d, double)[4]{

                (Curve1.PointAtStart, movingPt.DistanceToSquared(Curve1.PointAtStart)),
                (Curve1.PointAtEnd, movingPt.DistanceToSquared(Curve1.PointAtEnd)),
                (Curve2.PointAtStart, movingPt.DistanceToSquared(Curve2.PointAtStart)),
                (Curve2.PointAtEnd, movingPt.DistanceToSquared(Curve2.PointAtEnd)),
            };

            var ptOnCircle = ts.OrderBy(x => x.Item2).First().Item1;
            return ptOnCircle;
        }

        private void DrawCircle(Rhino.Display.DisplayPipeline display)
        {
            display.DrawCircle(new Circle(Plane, Center, Radius), System.Drawing.Color.DarkRed);
        }
    }
}
