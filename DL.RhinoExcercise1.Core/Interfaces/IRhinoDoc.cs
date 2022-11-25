using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DL.RhinoExcercise1.Core
{
    public interface IRhinoDoc
    {
        event Action<Guid> ObjChanged;
        event Action<Guid> ObjChangedWhenUndo;
        event Action<Guid> AbsolutelyNewObjectAdded;

        RhinoDoc RhinoDocForDev { get; }
        Guid Add(Curve curve);
        Guid Add(Brep brep);
        Guid Add(GeometryBase geometryBase);
        Guid AddToGeometryDoc(Brep brep);
        IEnumerable<Guid> Add(IEnumerable<Brep> breps);
        IEnumerable<Guid> Add(IEnumerable<Curve> curves);
        void ListenObjChange(Guid rhinoObjId);
        IRhinoTolerance RhinoTolerance { get; }
        void RemoveListenObjChange(Guid edgeId);
        void Redraw();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="ts"></param>
        /// <returns>integer index</returns>
        int AddGroup(string v, IEnumerable<Guid> ts);
        IEnumerable<RhinoObject> GetGroupMemebers(int groupId);

        /// <summary>
        /// Get curves from rhino object ids
        /// </summary>
        /// <param name="innerEdges1"></param>
        /// <param name="curves"></param>
        /// <returns></returns>
        bool Get(IEnumerable<Guid> rhinoObjIds, out IEnumerable<Curve> curves);
        bool Delete(IEnumerable<ObjRef> objRefs);
        bool Delete(Guid floorRhinoId);
    }
}