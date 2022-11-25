using Rhino;
using Rhino.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RhinoPlugInExcercise1
{
    [Guid("CAF3517F-750D-4069-9933-FAECED4DCD4B")]
    public class RcOpenCommand : Rhino.Commands.Command
    {
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = @"Rhino Files (*.3dm;)|*.3dm;||";
            dialog.DefaultExt = "3dm";
            DialogResult rc = dialog.ShowDialog();
            if (rc != DialogResult.OK)
                return Result.Cancel;

            var filename = dialog.FileName;
            RhinoDoc.Open(filename, out bool wasAlreadyOpen);
            return Result.Success;
        }

        public override string EnglishName => "BOpen";
    }
}
