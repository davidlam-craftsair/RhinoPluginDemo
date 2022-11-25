using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class BoxDynamicParameters
    {
        public BoxDynamicParameters(double boxCorner1OffsetX, double boxCorner1OffsetY, double boxCorner2OffsetX, double boxCorner2OffsetY)
        {
            BoxCorner1OffsetX = boxCorner1OffsetX;
            BoxCorner1OffsetY = boxCorner1OffsetY;
            BoxCorner2OffsetX = boxCorner2OffsetX;
            BoxCorner2OffsetY = boxCorner2OffsetY;
        }
        public BoxDynamicParameters(double t)
        {
            BoxCorner1OffsetX = t;
            BoxCorner1OffsetY = -t;
            BoxCorner2OffsetX = -t;
            BoxCorner2OffsetY = t;
        }
        public double BoxCorner1OffsetX { get; set; }
        public double BoxCorner1OffsetY { get; set; }
        public double BoxCorner2OffsetX { get; set; }
        public double BoxCorner2OffsetY { get; set; }
    }

    public class BoxDynamicSettingKeys
    {
        public static string BoxCorner1OffsetX = "BoxCorner1OffsetX";
        public static string BoxCorner1OffsetY = "BoxCorner1OffsetY";
        public static string BoxCorner2OffsetX = "BoxCorner2OffsetX";
        public static string BoxCorner2OffsetY = "BoxCorner2OffsetY";
    }
}
