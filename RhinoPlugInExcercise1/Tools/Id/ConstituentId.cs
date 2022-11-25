using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    class ConstituentId: IConstituentId
    {
        public ConstituentId(Guid id, string typeName)
        {
            Id = id;
            TypeName = typeName;
        }

        public Guid Id { get; }
        public string TypeName { get; }
    }
}
