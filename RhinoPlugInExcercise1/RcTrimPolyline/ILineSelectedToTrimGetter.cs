using DL.RhinoExcercise1.Core;
using Rhino.Geometry;

namespace RhinoPlugInExcercise1
{
    public interface ILineSelectedToTrimGetter
    {
        IRhinoTolerance RhinoTolerance { get; }

        bool Get(Line lineToBeCut, Line lineCutter, Point3d selectorPt, out Line lineSelectedToTrim, out Point3d intersectPt);
    }
}