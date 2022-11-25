using DL.RhinoExcercise1.Core;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class EditableSweepCreateAction : IEditableSweepCreateAction
    {
        public EditableSweepCreateAction(IDocManager docManager, IBrepSweepsFactory brepSweepsFactory, IComponentsToSessionAdder sweepComponentsToSessionAdder)
        {
            DocManager = docManager;
            BrepSweepsFactory = brepSweepsFactory;
            SweepComponentsToSessionAdder = sweepComponentsToSessionAdder;
        }

        public IDocManager DocManager { get; }
        public IRhinoDoc RhinoDoc => DocManager.RhinoDoc;
        public IBrepSweepsFactory BrepSweepsFactory { get; }
        public IComponentsToSessionAdder SweepComponentsToSessionAdder { get; }

        /// <summary>
        /// Create sweep objects in rhino
        /// </summary>
        /// <param name="railObj">the path curve </param>
        /// <param name="profileObjs"></param>
        /// <returns></returns>
        public Result Create(ObjRef railObj, IEnumerable<ObjRef> profileObjs)
        {
            var rail = railObj.Curve();
            if (rail == null)
            {
                return Result.Cancel;
            }
            var profiles = profileObjs.Select(x => x.Curve()).ToList();
            // assign the relationship
            var sweepObjs = GetSweepObjs(rail, profiles);
            var ts = new List<Guid>();

            if (ts.Any())
            {
                AddSweepComponentsToRhinoObjectTable(sweepObjs, ts);
                AddSweepComponentsToSessionDoc(ts);
                return Result.Success;
            }
            return Result.Cancel;
        }

        private void AddSweepComponentsToSessionDoc(List<Guid> ts)
        {
            SweepComponentsToSessionAdder.AddToSessionDoc(ts);
        }

        private IEnumerable<Brep> GetSweepObjs(Curve rail, List<Curve> profiles)
        {
            return BrepSweepsFactory.Create(rail, profiles);
        }

        private void AddSweepComponentsToRhinoObjectTable(IEnumerable<Brep> sweepObjs, List<Guid> ts)
        {
            foreach (var item in sweepObjs)
            {
                var brepId = RhinoDoc.Add(item);
                ts.Add(brepId);
            }
        }
    }
}
