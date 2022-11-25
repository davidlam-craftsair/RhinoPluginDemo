using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class CurveAdditionListenerV2 : IRhinoCurveObjectAdditionListener
    {
        private bool enabled;

        public CurveAdditionListenerV2(IDocManager docManager)
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
            DocManager.RhinoDoc.AbsolutelyNewObjectAdded += OnNewObjectAdded;
            enabled = true;
        }

        public event Action<Guid> NewClosedPlanarCurveAdded;

        private void OnNewObjectAdded(Guid obj)
        {
            var objRef = new ObjRef(obj);
            if (objRef.Curve() is Curve t)
            {
                OnNewCurveAdded(obj);
                if (t.IsClosed && t.IsPlanar())
                {
                    OnNewClosedPlanarCurveAdded(obj);
                }
            }
        }

        private void OnNewCurveAdded(Guid obj)
        {
            CurveAdded?.Invoke(obj);
        }

        private void OnNewClosedPlanarCurveAdded(Guid obj)
        {
            NewClosedPlanarCurveAdded?.Invoke(obj);
        }

        public void Disable()
        {
            DocManager.RhinoDoc.AbsolutelyNewObjectAdded -= OnNewObjectAdded;
            enabled = false;
        }

        public event Action<Guid> CurveAdded;
    }
}
