using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DL.RhinoExcercise1.Core
{
    public interface ICompositeObjId
    {
        string TypeName { get; }
        int GroupId { get; }
        IDictionary<string, IEnumerable<ICompositionalElementId>> CompositionalElementIdsByName { get; }
        IDictionary<string, IEnumerable<IConstituentId>> ConstituentsByName { get; }
    }

    public interface IConstituentId
    {
        Guid Id { get; }
        string TypeName { get; }
    }

    public interface ICompositionalElementId
    {
        Guid ObjId { get; }
        string TypeName { get; }

        GeometryBase GetGeometry();
        void SetObjId(Guid objId);
    }

}