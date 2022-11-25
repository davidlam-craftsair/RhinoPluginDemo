using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class FrameAlignHandler : IFrameAlignHandler
    {
        public FrameAlignHandler()
        {
            Up = new Vector3d(0, 0, 1);
        }

        public Vector3d Up { get; }

        public Plane Align(Plane plane)
        {
            if (1 == plane.XAxis.IsParallelTo(Up))
            {
                var a = -Math.PI / 2;
                return RotateByZAxis(plane, a);
            }
            else if (plane.XAxis.IsParallelTo(Up) == -1)
            {
                var a = Math.PI / 2;
                return RotateByZAxis(plane, a);
            }
            else if (plane.YAxis.IsParallelTo(Up) == -1)  // y is oppsite to up
            {
                var a = Math.PI;
                return RotateByZAxis(plane, a);
            }
            else if (plane.ZAxis.IsParallelTo(Up) == 1)  // if normal is parallel to up, then check x axis 
            {
                if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.GetFrustumNearPlane(out Plane nearPlane))
                {
                    var refXAxis = nearPlane.XAxis;
                    var transform = Transform.PlanarProjection(plane);
                    refXAxis.Transform(transform);
                    double v = Vector3d.VectorAngle(plane.XAxis, refXAxis);
                    var crossproduct = Vector3d.CrossProduct(plane.XAxis, refXAxis);
                    crossproduct.Unitize();

                    RhinoApp.WriteLine($"Degree = {RhinoMath.ToDegrees(v)}; Crossproduct = {crossproduct}");
                    if (v > Math.PI / 4)
                    {
                        if (v > Math.PI / 2)
                        {
                            if (crossproduct.IsParallelTo(Up, RhinoDoc.ActiveDoc.ModelAngleToleranceRadians) == -1)
                            {
                                if (v < Math.PI * 3 / 4)
                                {
                                    return RotateByZAxis(plane, Math.PI+Math.PI/2);
                                }
                                else
                                {
                                    return RotateByZAxis(plane, Math.PI);

                                }
                            }
                            else
                            {
                                return RotateByZAxis(plane, Math.PI);
                            }
                        }
                        return RotateByZAxis(plane, Math.PI / 2);
                    }
                }
            }
            return plane;
        }

        private static Plane RotateByZAxis(Plane plane, double a)
        {
            var t2 = new Plane(plane);
            t2.Rotate(a, plane.ZAxis);
            return t2;
        }
    }
}
