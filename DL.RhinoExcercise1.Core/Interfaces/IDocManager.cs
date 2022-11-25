using DL.RhinoExcercise1.Core;
using Rhino;

namespace RhinoPlugInExcercise1
{
    public interface IDocManager
    {
        ISessionDoc SessionDoc { get; }

        IRhinoDoc RhinoDoc { get; }

        void Update(RhinoDoc rhinoDoc);
    }
}