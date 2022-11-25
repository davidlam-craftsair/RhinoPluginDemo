using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class PolylineTrimInfo : IPolylineTrimInfo
    {
        public PolylineTrimInfo(int lineSegIndex, Point3d trimPt1, Point3d trimPt2)
        {
            LineSegIndex = lineSegIndex;
            TrimPt1 = trimPt1;
            TrimPt2 = trimPt2;
        }
        public int LineSegIndex { get; }
        public Point3d TrimPt1 { get; }
        public Point3d TrimPt2 { get; }
    }

    public class PolylineTrimInfoV2 : IPolylineTrimInfoV2
    {
        public PolylineTrimInfoV2(int lineToBeCutIndex, int lineCutterIndex, Point3d selector)
        {
            LineToBeCutIndex = lineToBeCutIndex;
            LineCutterIndex = lineCutterIndex;
            Selector = selector;
        }
        public int LineToBeCutIndex { get; }
        public int LineCutterIndex { get; }
        public Point3d Selector { get; }
    }
}
