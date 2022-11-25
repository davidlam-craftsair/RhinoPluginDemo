using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("B9CC6988-5E37-4745-9DB4-947E2663C0CA")]
    public class RcSelectPoints : AbsCommand
    {
        public RcSelectPoints()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            return new ObjGetterV2("point", new TypicalSelectionFilter<Point>()).Get(ActiveDoc);
        }

        public override string EnglishName => "SelPoints";

        public static RcSelectPoints Instance { get; private set; }
    }
}
