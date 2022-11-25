using System;
using System.Collections.Generic;

namespace DL.RhinoExcercise1.Core
{
    public interface IAssemblyId
    {
        string AssemblyTypeName { get; }
        Guid Id { get; }

        int GroupId { get; }

        IEnumerable<Guid> ComponentIds { get; }
    }

    public interface IAssemblyIdV2: IAssemblyId
    {
        IDictionary<string, IEnumerable<Guid>> ComponentIdsByName { get; }
    }

}