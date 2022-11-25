using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using System;
using System.Runtime.InteropServices;

namespace RhinoPlugInExcercise1
{
    [Guid("B28615A6-F43A-4EDA-99C1-E5BC2795B33C")]
    public class RcFilletInteractive : AbsCommand
    {
        public RcFilletInteractive()
        {
            Instance = this;
            FilletInteractive = new FilletInteractiveRhAction();
        }
        protected override Result DoRunCommand()
        {
            return FilletInteractive.Get();
        }

        public override string EnglishName => "FilletInteractive";

        public static RcFilletInteractive Instance { get; private set; }
        public IRhinoAction FilletInteractive { get; }
    }
}
