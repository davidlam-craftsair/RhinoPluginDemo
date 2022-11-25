using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// For rhino geometry
    /// </summary>
    public class RhGeometryDoc : IRhGeometryDoc
    {
        public RhGeometryDoc()
        {
            BrepDict = new Dictionary<Guid, Brep>();

        }

        public Dictionary<Guid, Brep> BrepDict { get; }

        public Guid Add(Brep brep)
        {
            var t = Guid.NewGuid();
            BrepDict.Add(t, brep);
            return t;
        }
    }

}
