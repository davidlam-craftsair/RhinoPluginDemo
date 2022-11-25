using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class ClosedPlanarCurveAdditionBag : IClosedPlanarCurveAdditionBag
    {
        public ClosedPlanarCurveAdditionBag(IClosedPlanarCurveAdditionListener closedPlanarCurveAdditionListener)
        {
            ClosedPlanarCurveAdditionListener = closedPlanarCurveAdditionListener;
            ClosedPlanarCurveAdditionListener.AbsolutelyNewClosedPlanarCurveAdded += OnNewClosedPlanarCurveAdded;
            ClosedPlanarCurves = new List<Guid>();
        }

        private void OnNewClosedPlanarCurveAdded(Guid obj)
        {
            ClosedPlanarCurves.Add(obj);
        }

        public IClosedPlanarCurveAdditionListener ClosedPlanarCurveAdditionListener { get; }
        public List<Guid> ClosedPlanarCurves { get; }

        public IEnumerable<Guid> GetAll()
        {
            return ClosedPlanarCurves;
        }

        public void Clear()
        {
            ClosedPlanarCurves.Clear();
        }

        public void EnableListening()
        {
            ClosedPlanarCurveAdditionListener.Enable();

        }
    }
}
