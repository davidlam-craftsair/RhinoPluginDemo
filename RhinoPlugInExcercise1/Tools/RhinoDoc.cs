using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.DocObjects;
using Rhino.DocObjects.Tables;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RhinoDocImp : IRhinoDoc
    {
        public RhinoDocImp(IRhinoDocContainer rhinoDocContainer,
                           IRhinoObjChangeManager rhinoObjChangeManager,
                           IRhinoTolerance rhinoTolerance,
                           IRhGeometryDoc geometryDoc,
                           IReporter reporter)
        {
            RhinoDocContainer = rhinoDocContainer;
            RhinoObjChangeManager = rhinoObjChangeManager;
            RhinoTolerance = rhinoTolerance;
            GeometryDoc = geometryDoc;
            Reporter = reporter;
            RhinoObjChangeManager.ObjChanged += OnRhinoObjChanged;
            RhinoObjChangeManager.AbsolutelyNewObjAdded += OnAbsolutelyNewObjAdded;
            RhinoObjChangeManager.Enable();
        }

        private void OnAbsolutelyNewObjAdded(Guid obj)
        {
            AbsolutelyNewObjectAdded?.Invoke(obj);
        }

        private void OnRhinoObjChanged(Guid obj)
        {
            ObjChanged?.Invoke(obj);
        }

        public IRhinoDocContainer RhinoDocContainer { get; }
        public IRhinoObjChangeManager RhinoObjChangeManager { get; }
        public IRhinoTolerance RhinoTolerance { get; }
        public IRhGeometryDoc GeometryDoc { get; }
        public IReporter Reporter { get; }

        public event Action<Guid> ObjChanged;
        public event Action<Guid> ObjChangedWhenUndo;
        public event Action<Guid> AbsolutelyNewObjectAdded;

        public RhinoDoc RhinoDocForDev 
        {
            get { return RhinoDocContainer.RhinoDoc; }
        }

        RhinoDoc RhinoDocT => RhinoDocContainer.RhinoDoc;
        ObjectTable Objects => RhinoDocT.Objects;

        public Guid Add(Curve curve)
        {
            return Objects.AddCurve(curve);
        }

        public IEnumerable<Guid> Add(IEnumerable<Curve> curves)
        {
            foreach (var item in curves)
            {
                var guid = RhinoDocContainer.RhinoDoc.Objects.AddCurve(item);
                yield return guid;
            }
        }

        public Guid Add(Brep brep)
        {
            return Objects.Add(brep);
        }

        public void ListenObjChange(Guid rhinoObjId)
        {
            RhinoObjChangeManager.Add(rhinoObjId);
        }

        public void RemoveListenObjChange(Guid rhinoObjId)
        {
            RhinoObjChangeManager.Remove(rhinoObjId);
        }

        public bool Get(IEnumerable<Guid> rhinoObjIds, out IEnumerable<Curve> curves)
        {
            curves = rhinoObjIds.Select(x => GetCurve(x));
            return true;
        }

        private Curve GetCurve(Guid x)
        {
            var objRef = new ObjRef(x);
            var t = objRef.Curve();
            if (t == null)
            {
                Reporter.Report("curve is null");
            }
            return t;
        }

        public bool Delete(IEnumerable<ObjRef> objRefs)
        {
            var objects = RhinoDocT.Objects;
            var ts = new List<bool>();
            foreach (var item in objRefs)
            {
                ts.Add(objects.Delete(item, false));
            }
            return ts.Any(x=>x);
        }

        public bool Delete(Guid floorRhinoId)
        {
            return RhinoDocT.Objects.Delete(floorRhinoId, false);
        }

        public void Redraw()
        {
            RhinoDocT.Views.RedrawEnabled = true;
            RhinoDocT.Views.Redraw();
        }

        public int AddGroup(string v, IEnumerable<Guid> ts)
        {
            return RhinoDocT.Groups.Add(v, ts);
        }

        public IEnumerable<RhinoObject> GetGroupMemebers(int groupId)
        {
            return RhinoDocT.Groups.GroupMembers(groupId);
        }

        public Guid Add(GeometryBase geometryBase)
        {
            return RhinoDocT.Objects.Add(geometryBase);
        }

        public Guid AddToGeometryDoc(Brep brep)
        {
            return GeometryDoc.Add(brep);
        }

        public IEnumerable<Guid> Add(IEnumerable<Brep> breps)
        {
            throw new NotImplementedException();
        }
    }
}
