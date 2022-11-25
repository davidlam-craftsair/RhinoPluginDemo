using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class ClosedPlanarCurveAdditionListener : IClosedPlanarCurveAdditionListener
    {
        private bool enabled;

        public ClosedPlanarCurveAdditionListener(IDocManager docManager)
        {
            DocManager = docManager;
        }

        public IDocManager DocManager { get; }

        public void Enable()
        {
            if (enabled)
            {
                return;
            }
            DocManager.RhinoDoc.AbsolutelyNewObjectAdded += OnAbsolutelyNewObjectAdded;
            enabled = true;
        }

        public event Action<Guid> AbsolutelyNewClosedPlanarCurveAdded;

        private void OnAbsolutelyNewObjectAdded(Guid obj)
        {
            var objRef = new ObjRef(obj);
            if (objRef.Curve() is Curve t)
            {
                if (t.IsClosed && t.IsPlanar())
                {
                    AbsolutelyNewClosedPlanarCurveAdded?.Invoke(obj);
                }
            }
        }

        public void Disable()
        {
            DocManager.RhinoDoc.AbsolutelyNewObjectAdded -= OnAbsolutelyNewObjectAdded;
            enabled = false;
        }
    }
}
