using DL.Framework;
using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    class RcSweepObj : AbsCommand
    {
        private IEditableSweepCreateAction _action;
        private IBrepSweepsFactory brepSweepsFactory;
        private ISweepAssemblyIdFactory sweepAssemblyIdFactory;

        protected override Result DoRunCommand()
        {
            // Get a rail with railId
            var go = new GetObject();
            go.SetCommandPrompt("Select rail");
            go.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            go.SubObjectSelect = true;
            go.EnablePreSelect(false, false);
            go.GetMultiple(0, 1);
            if (go.CommandResult() != Result.Success)
            {
                return go.CommandResult();
            }

            var objRef = go.Object(0);
            var rail = objRef.Curve();

            // Get a profile with profileIds (containing multiple curves)
            var go1 = new GetObject();
            go1.SetCommandPrompt("Select profiles");
            go1.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            go1.SubObjectSelect = true;
            go1.EnablePreSelect(false, false);
            go1.GetMultiple(0, 0);
            if (go1.CommandResult() != Result.Success)
            {
                return go1.CommandResult();
            }

            var objs = go.Objects();
            var profiles = objs.Select(x => x.Curve());

            // assign the relationship
            var sweepObjs = GetSweepObjs(rail, profiles);
            var ts = new List<Guid>();

            AddSweepObjToRhinoObjectTable(sweepObjs, ts);
            if (ts.Any())
            {
                var sweepObjId = Guid.NewGuid();
                var groupIdx = ActiveDoc.Groups.Add(sweepObjId.ToString(), ts);
                DocManager.SessionDoc.Add(CreateSweepComponentId(ts, groupIdx));

                // save the rail geometry and profile geometry, and create have a Rhino Command EditSweepObj to retrieve those curves for edit
                return Result.Success;
            }

            return Result.Cancel;

        }

        private IEnumerable<Brep> GetSweepObjs(Curve rail, IEnumerable<Curve> profiles)
        {
            return BrepSweepsFactory.Create(rail, profiles);
        }

        private void AddSweepObjToRhinoObjectTable(IEnumerable<Brep> sweepObjs, List<Guid> ts)
        {
            foreach (var item in sweepObjs)
            {
                var brepId = ActiveDoc.Objects.AddBrep(item);
                ts.Add(brepId);
            }
        }

        private ISweepAssemblyId CreateSweepComponentId(IEnumerable<Guid> ts, int groupIdx)
        {
            return SweepAssemblyIdFactory.Create(ts, groupIdx);
        }

        public override string EnglishName => "SweepObj";

        public IEditableSweepCreateAction Action
        {
            get
            {
                if (_action == null)
                {
                    _action = IoC.Get<IEditableSweepCreateAction>();
                }
                return _action;
            }
        }

        public IBrepSweepsFactory BrepSweepsFactory
        {
            get
            {
                if (brepSweepsFactory == null)
                {
                    brepSweepsFactory = IoC.Get<IBrepSweepsFactory>();
                }
                return brepSweepsFactory;
            }
        }

        public ISweepAssemblyIdFactory SweepAssemblyIdFactory
        {
            get
            {
                if (sweepAssemblyIdFactory == null)
                {
                    sweepAssemblyIdFactory = IoC.Get<ISweepAssemblyIdFactory>();
                }
                return sweepAssemblyIdFactory;
            }
        }
    }
}
