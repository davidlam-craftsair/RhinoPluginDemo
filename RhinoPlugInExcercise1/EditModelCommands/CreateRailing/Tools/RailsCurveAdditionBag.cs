using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailsCurveAdditionBag: IRailsCurveAdditionBag
    {
        public RailsCurveAdditionBag(IRhinoCurveObjectAdditionListener rhinoCurveObjectAdditionListener)
        {
            RhinoCurveObjectAdditionListener = rhinoCurveObjectAdditionListener;
            RhinoCurveObjectAdditionListener.CurveAdded += OnCurveAdded;
            Rails = new List<Guid>();
        }

        private void OnCurveAdded(Guid obj)
        {
            var objRef = new ObjRef(obj);
            if (objRef.Curve() is Curve t)
            {
                if (t.IsPlanar())
            }
        }

        public IRhinoCurveObjectAdditionListener RhinoCurveObjectAdditionListener { get; }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetAll()
        {
            throw new NotImplementedException();
        }

        public void EnableListening()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetQualifiedRails()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetQualifedProfiles()
        {
            throw new NotImplementedException();
        }
    }
}
