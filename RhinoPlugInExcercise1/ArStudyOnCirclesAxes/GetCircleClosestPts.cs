using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class GetCircleClosestPt: GetPoint
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
            // Decide closesness of pt
            // curve 1
            // Find the pt on circle by selecting the pt of curve end pts which is closest to the mouse point
            Point3d ptOnCircle = FindPtOnCircle(movingPt);

            Radius = ptOnCircle.DistanceTo(movingPt);
            if (Radius < tol)
            {
                return;
            }
            Center = movingPt;
            Plane = e.RhinoDoc.Views.ActiveView.ActiveViewport.ConstructionPlane();
            DrawCircle(e.Display);
        }

        private Point3d FindPtOnCircle(Point3d movingPt)
        {
            var ts = new List<Point3d>();
            // Get the closest pt on curve 1
            if (Curve1.ClosestPoint(movingPt, out double parameter))
            {
                var ptOnCircleCandidate = Curve1.PointAt(parameter);
                ts.Add(ptOnCircleCandidate);
            }
            // Get the closest pt on curve 2
            if (Curve2.ClosestPoint(movingPt, out double parameter2))
            {
                var ptOnCircleCandidate2 = Curve2.PointAt(parameter2);
                ts.Add(ptOnCircleCandidate2);
            }

            IEnumerable<Point3d> t = ts.ToLookup(x => CalcuateDist(x, movingPt)).OrderBy(x=>x.Key).SelectMany(x=>x);
            return t.FirstOrDefault();
            
        }

        private double CalcuateDist(Point3d x, Point3d movingPt)
        {
            return x.DistanceToSquared(movingPt);
        }

        private void DrawCircle(Rhino.Display.DisplayPipeline display)
        {
            display.DrawCircle(new Circle(Plane, Center, Radius), System.Drawing.Color.DarkRed);
        }
    }
}
