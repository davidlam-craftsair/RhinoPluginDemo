using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public static class DI
    {
        public static IRhinoReplaceManager RhinoReplaceManager => IoC.Get<IRhinoReplaceManager>();
        public static IRhinoGetManager RhinoGetManager => IoC.Get<IRhinoGetManager>();
        public static RhinoDoc ActiveDoc => RhinoDoc.ActiveDoc;

        public static IReporter Reporter => IoC.Get<IReporter>();

        public static IDocManager DocManager => IoC.Get<IDocManager>();
    }
}
