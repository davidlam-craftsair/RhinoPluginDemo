using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("9E2E7AE0-77D6-417E-A826-26D84742C4FD")]
    public class RcSelectCurves : AbsCommand
    {
        public RcSelectCurves()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            return new ObjGetterV2("curve", new TypicalSelectionFilter<Curve>()).Get(ActiveDoc);
        }

        public override string EnglishName => "SelCurves";

        public static RcSelectCurves Instance { get; private set; }
    }
}
