using DL.RhinoExcercise1.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    /// <summary>
    /// An entity to emulate building component e.g. railing or floor, which in view of modelling, 
    /// Compositional elements: need some geometries (curves) to, let say, extrude 
    /// Constituents elements: in the case of railing, top railing and panel and base railing
    /// </summary>
    public class CompositeObjId : ICompositeObjId
    {
        public CompositeObjId(string typeName,
                              IDictionary<string, IEnumerable<IConstituentId>> constituents, 
                              IDictionary<string, IEnumerable<ICompositionalElementId>> compositionalElementIds,
                              int groupId
                              )
        {
            TypeName = typeName;
            ConstituentsByName = constituents;
            CompositionalElementIdsByName = compositionalElementIds;
            GroupId = groupId;
        }

        public string TypeName { get; }
        public IDictionary<string, IEnumerable<IConstituentId>> ConstituentsByName { get; }
        public IDictionary<string, IEnumerable<ICompositionalElementId>> CompositionalElementIdsByName { get; }
        public int GroupId { get; }
    }
}
