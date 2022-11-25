using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface IClosedPlanarCurveAdditionBag: IRhinoObjectAdditionBag
    {
    }

    public interface IRhinoObjectAdditionBag
    {
        void Clear();
        IEnumerable<Guid> GetAll();
        void EnableListening();

    }

    public interface IRailsCurveAdditionBag:IRhinoObjectAdditionBag
    {
        IEnumerable<Guid> GetQualifiedRails();
        IEnumerable<Guid> GetQualifedProfiles();
    }
}