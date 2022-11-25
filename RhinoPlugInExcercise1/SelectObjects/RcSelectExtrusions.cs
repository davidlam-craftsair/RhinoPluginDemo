using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("43FA7C31-A710-49E0-AADB-6DB04C1F5FDB")]
    public class RcSelectExtrusions : AbsCommand
    {
        public RcSelectExtrusions()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            return new ObjGetterV2("extrusion", new TypicalSelectionFilter<Extrusion>()).Get(ActiveDoc);
        }

        public override string EnglishName => "SelExtrusions";

        public static RcSelectExtrusions Instance { get; private set; }
    }
}
