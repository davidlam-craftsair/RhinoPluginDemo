using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RhinoObjChangeManager : IRhinoObjChangeManager
    {
        private Guid rhinoObjAboutToReplaceId;
        private bool isReplacing;
        private Guid rhinoObjIdChanged;
        private bool isEnabled;

        public RhinoObjChangeManager()
        {
            WatchSet = new HashSet<Guid>();
        }

        public HashSet<Guid> WatchSet { get; }

        public void Enable()
        {
            if (isEnabled)
            {
                return;
            }
            RhinoDoc.ReplaceRhinoObject += OnReplaceRhinoObj;
            RhinoDoc.DeleteRhinoObject += OnDeleteRhinoObj;
            RhinoDoc.UndeleteRhinoObject += OnUndeleteRhinoObj;
            RhinoDoc.AddRhinoObject += OnAddRhinoObj;
            isEnabled = true;
        }

        public void Disable()
        {
            RhinoDoc.ReplaceRhinoObject -= OnReplaceRhinoObj;
            RhinoDoc.DeleteRhinoObject -= OnDeleteRhinoObj;
            RhinoDoc.UndeleteRhinoObject -= OnUndeleteRhinoObj;
            RhinoDoc.AddRhinoObject -= OnAddRhinoObj;
            isEnabled = false;
        }

        private void OnUndeleteRhinoObj(object sender, RhinoObjectEventArgs e)
        {
            if (isReplacing)
            {
                rhinoObjIdChanged = rhinoObjAboutToReplaceId;
                ObjChangedWhenUndo?.Invoke(rhinoObjIdChanged);
                Reset();

            }
        }

        public event Action<Guid> ObjChanged;
        public event Action<Guid> ObjChangedWhenUndo;
        public event Action<Guid> AbsolutelyNewObjAdded;

        private void OnAddRhinoObj(object sender, RhinoObjectEventArgs e)
        {
            if (isReplacing)
            {
                rhinoObjIdChanged = rhinoObjAboutToReplaceId;
                OnRhinoObjChangeDetected();
            }
            else
            {
                OnAbsolutelyNewObjectAdded(e.ObjectId);
            }
        }

        private void OnAbsolutelyNewObjectAdded(Guid id)
        {
            AbsolutelyNewObjAdded?.Invoke(id);
        }

        private void OnRhinoObjChangeDetected()
        {
            ObjChanged?.Invoke(rhinoObjIdChanged);
            Reset();
        }

        private void Reset()
        {
            rhinoObjIdChanged = Guid.Empty;
            isReplacing = false;
        }

        private void OnDeleteRhinoObj(object sender, RhinoObjectEventArgs e)
        {
            if (rhinoObjAboutToReplaceId == e.ObjectId)
            {
                isReplacing = true;
            }
        }

        private void OnReplaceRhinoObj(object sender, RhinoReplaceObjectEventArgs e)
        {
            if (WatchSet.Contains(e.ObjectId))
            {
                rhinoObjAboutToReplaceId = e.ObjectId;
            }
        }

        public void Add(Guid rhinoObjId)
        {
            WatchSet.Add(rhinoObjId);
        }

        public void Remove(Guid rhinoObjId)
        {
            WatchSet.Remove(rhinoObjId);
        }
    }
}
