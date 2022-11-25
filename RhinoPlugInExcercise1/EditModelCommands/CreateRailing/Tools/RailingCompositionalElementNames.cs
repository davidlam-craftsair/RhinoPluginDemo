using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RailingCompositionalElementNames
    {
        public const string Rail = "Rail";
        public const string Profiles = "Profiles";
        public const string Profiles0 = "Profiles0";
        public const string Profiles1 = "Profiles1";
        public const string Profiles2 = "Profiles2";
        public const string Profiles3 = "Profiles3";
        public const string Profiles4 = "Profiles4";
        public const string Profiles5 = "Profiles5";
        public const string Profiles6 = "Profiles6";
        public const string Profiles7 = "Profiles7";
        public const string TopProfiles = "TopProfiles";
        public const string BottomProfiles = "BottomProfiles";
        public const string InBetweenProfiles1 = "InBetweenProfiles1";
        public const string RailProfiles = "RailProfiles";
        private static IEnumerable<string> _all;

        public static IEnumerable<string> All {
            get
            {
                if (_all == null)
                {
                    _all = new string[]
                    {
                        Rail,
                        TopProfiles,
                        BottomProfiles,
                        InBetweenProfiles1
                    };
                }
                return _all;
            }
        }
    }

    public class RailingConstituentNames
    {
        public const string TopRail = "TopRail";
        public const string BottomRail = "BottomRail";
        public const string InBetweenRail = "InBetweenRail1";
        public const string Rails = "Rails";

        public const string StartPost = "StartPost";
        public const string EndPost = "EndPost";
        public const string InBetweenPost = "InBetweenPost";
        private static IEnumerable<string> _all;

        public static IEnumerable<string> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new string[]
                    {
                        TopRail,
                        BottomRail,
                        InBetweenRail,
                        StartPost,
                        EndPost,
                        InBetweenPost,
                    };
                }
                return _all;
            }
        }
    }
}
