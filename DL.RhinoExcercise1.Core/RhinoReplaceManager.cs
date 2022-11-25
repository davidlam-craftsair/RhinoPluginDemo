using Rhino;
using Rhino.DocObjects.Tables;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.RhinoExcercise1.Core
{
    public class RhinoReplaceManager : IRhinoReplaceManager
    {
        public RhinoDoc ActiveDoc => RhinoDoc.ActiveDoc;
        public ObjectTable Objects => ActiveDoc.Objects;

        public bool Replace(Guid id, Line line)
        {
            var t = Objects.Replace(id, line);
            if (!t)
            {
                Debug.WriteLine("cant replace line");
            }
            return t;
        }

        public bool Replace(Guid id, Curve curve)
        {
            var t = Objects.Replace(id, curve);
            if (!t)
            {
                Debug.WriteLine("cant replace curve");
            }
            return t;
        }
    }
}
