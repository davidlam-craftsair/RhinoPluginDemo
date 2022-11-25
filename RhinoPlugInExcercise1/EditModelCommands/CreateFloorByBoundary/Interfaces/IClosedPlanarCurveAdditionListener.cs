using System;

namespace RhinoPlugInExcercise1
{
    public interface IClosedPlanarCurveAdditionListener
    {

        event Action<Guid> AbsolutelyNewClosedPlanarCurveAdded;

        void Enable();
        void Disable();
    }

    public interface IRhinoCurveObjectAdditionListener
    {
        event Action<Guid> NewClosedPlanarCurveAdded;
        event Action<Guid> CurveAdded;
        void Enable();
        void Disable();
    }

}