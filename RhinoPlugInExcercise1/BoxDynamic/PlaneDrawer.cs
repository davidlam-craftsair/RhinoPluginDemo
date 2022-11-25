using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PlaneDrawer : IPlaneDrawer
    {
        public void Draw(DisplayPipeline display, Plane plane)
        {
            if (plane.IsValid)
            {
                display.DrawLine(GetLineFromPlaneX(plane, 4), System.Drawing.Color.Red, 5);
                display.DrawLine(GetLineFromPlaneY(plane, 4), System.Drawing.Color.Green, 5);
                display.DrawLine(GetLineFromPlaneZ(plane, 4), System.Drawing.Color.Blue, 5);
            }
        }

        private Line GetLineFromPlaneZ(Plane plane, int length)
        {
            Vector3d span = plane.ZAxis * length;
            return CreateLine(plane, span);
        }

        private Line GetLineFromPlaneY(Plane plane, int length)
        {
            Vector3d span = plane.YAxis * length;
            return CreateLine(plane, span);
        }

        private Line GetLineFromPlaneX(Plane plane, double length)
        {
            Vector3d span = plane.XAxis * length;
            return CreateLine(plane, span);
        }

        private static Line CreateLine(Plane plane, Vector3d span)
        {
            return new Line(plane.Origin, span);
        }

    }
}
