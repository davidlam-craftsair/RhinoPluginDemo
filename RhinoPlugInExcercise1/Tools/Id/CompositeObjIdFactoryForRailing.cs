using DL.RhinoExcercise1.Core;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class CompositeObjIdFactoryForRailing
    {
        public IBrepSweepsFactory BrepSweepsFactory { get; }
        public IDocManager DocManager { get; }

        public CompositeObjIdFactoryForRailing(IBrepSweepsFactory brepSweepsFactory, IDocManager docManager)
        {
            BrepSweepsFactory = brepSweepsFactory;
            DocManager = docManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="railProfilesPerPosition">rail profile rhino objs per position, e.g. 1st item group is top rail profile, 2nd item group is right below and so on</param>
        /// <param name="railCurve">the path that profiles sweep along</param>
        /// <returns></returns>
        public ICompositeObjId Create(IEnumerable<IEnumerable<ObjRef>> railProfilesPerPosition, ObjRef railCurve)
        {
            var compositionalElementIdsByTypeName = GetCompositionalElementIds(railProfilesPerPosition);

            var contituentsByTypeName = GetConstituentsByTypeName(railProfilesPerPosition, railCurve, out int groupId);
            
            return new CompositeObjId(AssemblyTypeNames.Railing,
                contituentsByTypeName,
                compositionalElementIdsByTypeName,
                groupId
                );
        }

        private IDictionary<string, IEnumerable<IConstituentId>> GetConstituentsByTypeName(IEnumerable<IEnumerable<ObjRef>> railProfilesPerPosition, ObjRef railCurve, out int groupId)
        {
            // Create breps and add to rhino and group them in rhino
            var posCount = 0;
            var constituentIds = new List<IConstituentId>();
            var brepRhinoObjIds = new List<Guid>();
            foreach (var railProfiles in railProfilesPerPosition)
            {
                var profileBreps = BrepSweepsFactory.Create(railCurve.Curve(), railProfiles.Select(x => x.Curve()));
                foreach (var brep in profileBreps)
                {
                    var objId = DocManager.RhinoDoc.Add(brep);
                    brepRhinoObjIds.Add(objId);
                    var constituentId = new ConstituentId(objId, $"{RailingConstituentNames.Rails}{posCount}");
                    constituentIds.Add(constituentId);
                }
                posCount += 1;
            }
            var railAssemblyCount = DocManager.SessionDoc.GetRailAssemblyCount();
            groupId = DocManager.RhinoDoc.AddGroup($"{AssemblyTypeNames.Railing}{railAssemblyCount + 1}", brepRhinoObjIds);

            var constituentIdsByTypeName = new Dictionary<string, IEnumerable<IConstituentId>>();
            foreach (var item in constituentIds)
            {
                if (constituentIdsByTypeName.TryGetValue(item.TypeName, out IEnumerable<IConstituentId> ts))
                {
                    // Cast to List
                    var listts = (IList<IConstituentId>)ts;
                    listts.Add(item);
                }
                else
                {
                    // Create List 
                    constituentIdsByTypeName.Add(item.TypeName, new List<IConstituentId> { item });
                }
            }

            return constituentIdsByTypeName;
        }

        private Dictionary<string, IEnumerable<ICompositionalElementId>> GetCompositionalElementIds(IEnumerable<IEnumerable<ObjRef>> railProfilesPerPosition)
        {
            var ts = new Dictionary<string, IEnumerable<ICompositionalElementId>>();
            var posCount = 0;
            foreach (var railProfiles in railProfilesPerPosition)
            {
                var componentCount = 0;
                var compositionalElementIds = new List<ICompositionalElementId>();
                // Give rail profiles a shared type name, called "RailProfiles0"
                var typeName = $"{RailingCompositionalElementNames.RailProfiles}{posCount}"; 
                foreach (var railProfile in railProfiles)
                {
                    ICompositionalElementId compositionalElementId = GetCompositionalElementId(typeName, railProfile);
                    compositionalElementIds.Add(compositionalElementId);
                    componentCount += 1;
                }
                ts.Add(typeName, compositionalElementIds);

                posCount += 1;
            }

            return ts;
        }

        private ICompositionalElementId GetCompositionalElementId(string v, ObjRef railProfile)
        {
            return new CompositionalId(railProfile.ObjectId, v);
        }
    }
}
