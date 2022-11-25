using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    [Guid("EE477E43-D7E0-4F9C-B7C8-57EAA2A0BE37")]
    public class RcCleanBrep : AbsCommand
    {
        public IBrepCleaner BrepCleaner => IoC.Get<IBrepCleaner>();
        protected override Result DoRunCommand()
        {
            var go = new GetObject();
            go.SetCommandPrompt("Select brep to clean");
            go.EnablePreSelect(true, false);
            go.GetMultiple(0, 0);
            go.GeometryFilter = ObjectType.Brep | ObjectType.PolysrfFilter;
            var getResult = go.Get();
            if (go.CommandResult() != Result.Success)
            {
                return go.CommandResult();
            }
            ObjRef[] objRefs = go.Objects();
            foreach (var item in objRefs)
            {
                var t = item.Brep();
                if (t != null)
                {
                    BrepCleaner.Clean(t);
                    var objId = item.ObjectId;
                    if (ActiveDoc.Objects.Replace(objId, t))
                    {
                    }
                }
            }

            return Result.Success;
        }

        public override string EnglishName => "CleanBrep";
    }
}
