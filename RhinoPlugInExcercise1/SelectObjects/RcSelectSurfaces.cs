using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("55C1676E-1AC9-4049-8130-1EA3C32DFBA4")]
    public class RcSelectSurfaces : AbsCommand
    {
        public RcSelectSurfaces()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            return new ObjGetterV2("surface", new SurfaceSelectionFilter()).Get(ActiveDoc);
        }

        public override string EnglishName => "SelSurfaces";

        public static RcSelectSurfaces Instance { get; private set; }
    }
}
