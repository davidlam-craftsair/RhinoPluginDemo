using Rhino.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class DefaultDisplayMaterials : IDefaultDisplayMaterials
    {
        public DefaultDisplayMaterials()
        {
            BrepFace = new DisplayMaterial(Color.DarkRed, 0.5);
        }
        public DisplayMaterial BrepFace { get; set; }
    }
}
