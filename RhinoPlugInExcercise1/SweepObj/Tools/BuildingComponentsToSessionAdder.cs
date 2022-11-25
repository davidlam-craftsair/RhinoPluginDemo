using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BuildingComponentsToSessionAdder: IBuildingComponentsToSessionAdder
    {
        public BuildingComponentsToSessionAdder(IDocManager docManager)
        {
            DocManager = docManager;
        }

        public IDocManager DocManager { get; }

        public void AddToSessionDoc(ICompositeObjId buildingComponentId)
        {
            DocManager.SessionDoc.Add(buildingComponentId);
        }
    }
}
