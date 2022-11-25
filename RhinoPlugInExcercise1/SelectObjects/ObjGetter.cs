using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class ObjGetter<T>
    {
        public ObjGetter(string objName)
        {
            ObjName = objName;
        }

        public string ObjName { get; }


        public Result Get(RhinoDoc doc)
        {
            // Select only curves
            var go = new GetObject();
            go.SetCommandPrompt($"Select {ObjName}s");
            go.SetCustomGeometryFilter(Filter);
            go.EnablePreSelect(false, false); 
            go.EnablePostSelect(true);  // true means pre-selection is enabled
            go.GetMultiple(0, -1);  // -1 means selection stops once select objs is larger than minimum 
            if (go.CommandResult() == Result.Success)
            {
                ObjRef[] objs = go.Objects();
                doc.Objects.Select(objs);
                RhinoApp.WriteLine($"{objs.Count()} {ObjName}s are selected");
                return Result.Success;
            }
            return Result.Cancel;
        }

        private bool Filter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is T)
            {
                return true;
            }
            return false;
        }

    }
}
