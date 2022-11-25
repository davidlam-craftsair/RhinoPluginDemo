using DL.RhinoExcercise1.Core;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class CompositionalId: ICompositionalElementId
    {
        public CompositionalId(Guid objId, string typeName)
        {
            ObjId = objId;
            TypeName = typeName;
        }

        public Guid ObjId { get; private set; }
        public string TypeName { get; }

        public GeometryBase GetGeometry()
        {
            return new ObjRef(ObjId).Geometry();
        }

        public void SetObjId(Guid objId)
        {
            ObjId = objId;
        }
    }
}
