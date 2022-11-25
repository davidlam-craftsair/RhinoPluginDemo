using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public class RhinoTolerance : IRhinoTolerance
    {
        public double Tol { get; private set; }

        public double RadAngleTol { get; private set; }

        public void SetTol(double tol, double angleTol)
        {
            Tol = tol;
            RadAngleTol = angleTol;
        }
    }
}
