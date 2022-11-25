using DL.Framework;
using DL.RhinoExcercise1.Core;
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
    public class RcOpenEditRailing : AbsCommand
    {
        private IOpenEditRailingAction _action;

        public RcOpenEditRailing()
        {

        }
        protected override Result DoRunCommand()
        {
            // Select a railing
            if (Result.Success == new RhinoGetCustom().SetCommandPrompt("Select railing to edit")
                                                      .SelectOnlyOne()
                                                      .SetFilter(GetRailingObj)
                                                      .Get(out IEnumerable<ObjRef> railingComponentIds))
            {
                

            }

            // Search through doc for the sweep obj

            // 
            return Result.Cancel;
        }

        private bool GetRailingObj(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            // Search the id of rhinoObject if it is in a group of railing components
            SearchInRailingsByConstituentId(rhObject.Id);
            return true;
        }

        private void SearchInRailingsByConstituentId(Guid constituentGuid)
        {
            DocManager.SessionDoc.GetCompositeIdByConstitutentId(constitutentId)
        }

        public IOpenEditRailingAction OpenEditRailingAction
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IOpenEditRailingAction>();
                }
                return _action;
            }
        }

        public override string EnglishName => "EditRailing";
    }
}
