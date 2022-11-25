using Rhino;

namespace DL.RhinoExcercise1.Core
{
    public interface IRhinoDocContainer
    {
        RhinoDoc RhinoDoc { get; }

        void SetDoc(RhinoDoc doc);
    }
}