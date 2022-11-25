using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class RhinoGetCustom
    {

        public RhinoGetCustom()
        {
            GetObject = new GetObject();
            GetObject.DisablePreSelect();
        }

        public GetObject GetObject { get; private set; }

        public RhinoGetCustom SetCommandPrompt(string t)
        {
            GetObject.SetCommandPrompt(t);
            return this;
        }

        public RhinoGetCustom SelectOnlyOne()
        {
            GetObject.GetMultiple(1, 1);
            return this;
        }

        public RhinoGetCustom SelectSubObject()
        {
            GetObject.SubObjectSelect=true;
            return this;
        }
        public RhinoGetCustom SelectMultiple()
        {
            GetObject.GetMultiple(1, 0);
            return this;
        }
        public RhinoGetCustom DisablePreSelect()
        {
            GetObject.EnablePreSelect(false, false);
            return this;
        }


        public RhinoGetCustom SetFilter(GetObjectGeometryFilter filter)
        {
            GetObject.SetCustomGeometryFilter(filter);
            return this;
        }
        public Result Get()
        {
            GetObject.Get();
            return GetObject.CommandResult();
        }

        public IEnumerable<ObjRef> GetObjects()
        {
            return GetObject.Objects();
        }

        public Result Get(out IEnumerable<ObjRef> objRefs)
        {
            objRefs = GetObject.Objects();
            return GetObject.CommandResult();
        }

        public Result Get(out ObjRef objRef)
        {
            objRef = GetObject.Objects().FirstOrDefault();
            return GetObject.CommandResult();
        }

    }
}
