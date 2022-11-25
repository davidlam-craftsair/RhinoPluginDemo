using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System.Linq;

namespace RhinoPlugInExcercise1
{
    public class RcFloorOpenEdit : AbsCommand
    {
        private IFloorBoundaryOpenEditAction _action;

        protected override Result DoRunCommand()
        {
            var go = new GetObject();
            go.SetCommandPrompt("Select floor to edit");
            go.GetMultiple(0, 1);
            go.SetCustomGeometryFilter(CustomFilter);
            go.Get();
            if (go.CommandResult() == Result.Success)
            {
                var t = go.Objects().FirstOrDefault();
                if (t != null)
                {
                    return FloorBoundaryOpenEditAction.Do(t, DocManager);
                }
            }
            
            return Result.Cancel;
        }

        private bool CustomFilter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex componentIndex)
        {
            if (geometry is Brep t)
            {
                var id = rhObject.Id;
                return DocManager.SessionDoc.Get(id, out IFloorComponentInfo floorComponentInfo);
            }
            return false;
        }

        public IFloorBoundaryOpenEditAction FloorBoundaryOpenEditAction
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IFloorBoundaryOpenEditAction>();
                    return _action;
                }
                return _action;
            }
        } 

        public override string EnglishName => "EditFloor";
    }
}
