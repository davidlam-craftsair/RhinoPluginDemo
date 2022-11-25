using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class TypicalDrawer : IDrawer
    {
        public TypicalDrawer(IPlaneDrawer planeDrawer, IDefaultDisplayMaterials displayMaterials)
        {
            PlaneDrawer = planeDrawer;
            DisplayMaterials = displayMaterials;
        }

        public IPlaneDrawer PlaneDrawer { get; }
        public IDefaultDisplayMaterials DisplayMaterials { get; }

        public void DrawDefaultShaded(DisplayPipeline display, BrepFace brepFace)
        {
            Draw(display, brepFace, DisplayMaterials.BrepFace);
        }
        public void Draw(DisplayPipeline display, BrepFace brepFace, DisplayMaterial displayMaterial)
        {
            if (brepFace != null)
            {
                var b = brepFace.DuplicateFace(false);
                if (b != null)
                {
                    display.DrawBrepShaded(b, displayMaterial);
                }
            }
        }

        public void Draw(DisplayPipeline display, Plane plane)
        {
            PlaneDrawer.Draw(display, plane);
        }

        public void DrawDefaultWires(DisplayPipeline display, Brep brep)
        {
            display.DrawBrepWires(brep, System.Drawing.Color.DarkRed);
        }

        public void Draw(DisplayPipeline display, Brep brep)
        {
            throw new NotImplementedException();
        }
    }
}
