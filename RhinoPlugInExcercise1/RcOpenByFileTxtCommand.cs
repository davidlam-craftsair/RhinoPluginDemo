using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("3DD39AD6-7FA7-4E2E-B458-73C6E2258D19")]
    public class RcOpenByFileTxtCommand: Command
    {
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var ts = File.ReadAllLines(@"D:\01 Project\19 Rhino Modelling Training\ProjectManifest.txt");
            var gs = new GetInteger();
            gs.SetCommandPrompt("Input the integer");
            gs.AcceptNothing(true);
            gs.SetDefaultInteger(0);
            gs.Get();
            if (gs.CommandResult() != Result.Success)
            {
                return gs.CommandResult();
            }
            var idx = gs.Number();
            var filename = ts.ElementAtOrDefault(idx);
            RhinoDoc.Open(filename, out bool wasAlreadyOpen);
            return Result.Success;
        }

        public override string EnglishName => "NOpen";
    }
}
