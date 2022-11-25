using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("FAF24E62-0D81-459A-B0CF-514D16932EED")]
    public class RcSelectBreps : AbsCommand
    {
        public RcSelectBreps()
        {
            Instance = this;
        }
        protected override Result DoRunCommand()
        {
            return new ObjGetterV2("brep", new TypicalSelectionFilter<Brep>()).Get(ActiveDoc);
        }

        public override string EnglishName => "SelBreps";

        public static RcSelectBreps Instance { get; private set; }
    }
}
