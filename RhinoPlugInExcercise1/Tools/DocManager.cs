using DL.RhinoExcercise1.Core;
using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class DocManager : IDocManager
    {
        public DocManager(IRhinoDoc rhinoDoc, ISessionDoc sessionDoc, IRhinoDocContainer rhinoDocContainer)
        {
            RhinoDoc = rhinoDoc;
            SessionDoc = sessionDoc;
            RhinoDocContainer = rhinoDocContainer;
        }
        
        public IRhinoDocContainer RhinoDocContainer { get; private set; }

        public void Update(RhinoDoc rhinoDoc)
        {
            RhinoDocContainer.SetDoc(rhinoDoc);
        }

        public void Update(ISessionDoc sessionDoc)
        {
            SessionDoc = sessionDoc;
        }

        public ISessionDoc SessionDoc { get; private set; }

        public IRhinoDoc RhinoDoc { get; private set; }

    }
}
    