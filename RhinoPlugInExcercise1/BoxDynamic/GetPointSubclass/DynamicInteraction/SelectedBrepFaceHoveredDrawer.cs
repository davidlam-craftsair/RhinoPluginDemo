using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class SelectedBrepFaceHoveredDrawer : ISelectedBrepFaceHoveredDrawer
    {
        public SelectedBrepFaceHoveredDrawer(IBoxCreationV2 boxCreation, DisplayMaterial defaultBrepMaterial, IDrawer drawer)
        {
            BoxCreation = boxCreation;
            DefaultBrepMaterial = defaultBrepMaterial;
            Drawer = drawer;
        }

        public IBoxCreationV2 BoxCreation { get; }
        public DisplayMaterial DefaultBrepMaterial { get; }
        public IDrawer Drawer { get; }

        public BrepFace SelectedBrepFaceHovered => BoxCreation.SelectedBrepFaceHovered;

        public void Draw(DisplayPipeline display)
        {
            Drawer.Draw(display, SelectedBrepFaceHovered, DefaultBrepMaterial);
        }
    }
}
