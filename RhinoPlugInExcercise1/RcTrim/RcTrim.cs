using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("DD911578-09D4-40D7-911A-EE58F4FE7D23")]
    public class RcTrim : AbsCommand
    {
        public RcTrim()
        {
            Instance = this;
            TrimRcAction = new TrimRcAction();
        }
        protected override Result DoRunCommand()
        {
            return TrimRcAction.Get();
        }

        public override string EnglishName => "RcTrim";

        public static RcTrim Instance { get; private set; }
        public TrimRcAction TrimRcAction { get; }
    }
}
