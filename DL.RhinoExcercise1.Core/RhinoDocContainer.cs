using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public class RhinoDocContainer : IRhinoDocContainer
    {
        public RhinoDocContainer()
        {
        }

        public RhinoDoc RhinoDoc { get; private set; }

        public void SetDoc(RhinoDoc doc)
        {
            RhinoDoc = doc;
        }
    }
}
