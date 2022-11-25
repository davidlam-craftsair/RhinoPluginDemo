using DL.Framework;
using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class SessionDoc : ISessionDoc
    {
        public SessionDoc()
        {
            SweepAssemblyIdsByComponentId = new Dictionary<Guid, ISweepAssemblyId>();
            FloorComponentsByRhinoFloorId = new Dictionary<Guid, IFloorComponentInfo>();
            AssemblyIdsByComponentId = new Dictionary<Guid, IAssemblyIdV2>();
            AssemblyIdListsByAssemblyType = new Dictionary<string, IAssemblyIdList>();
            RhinoFloorUpdateInfoSet = new HashSet<IFloorUpdateInfo>();
            BuildingComponentIds = new HashSet<ICompositeObjId>();
            CompositeObjIdsByConstitutentGuid = new Dictionary<Guid, ICompositeObjId>();


        }

        public IDictionary<Guid, IFloorComponentInfo> FloorComponentsByRhinoFloorId { get; }
        public IDictionary<Guid, IAssemblyIdV2> AssemblyIdsByComponentId { get; }
        public IDictionary<string, IAssemblyIdList> AssemblyIdListsByAssemblyType { get; }
        public IDictionary<Guid, ISweepAssemblyId> SweepAssemblyIdsByComponentId { get; }
        public HashSet<IFloorUpdateInfo> RhinoFloorUpdateInfoSet { get; }
        public HashSet<ICompositeObjId> BuildingComponentIds { get; }
        public IDictionary<Guid, ICompositeObjId> CompositeObjIdsByConstitutentGuid { get; }

        #region Add
        public void Add(ISweepAssemblyId sweepComponentId)
        {
            foreach (var item in sweepComponentId.ComponentIds)
            {
                SweepAssemblyIdsByComponentId.Add(item, sweepComponentId);
            }
        }

        public void Add(IFloorComponentInfo componentInfo)
        {
            FloorComponentsByRhinoFloorId.Add(componentInfo.FloorRhinoId, componentInfo);
        }

        public void Add(IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            RhinoFloorUpdateInfoSet.Add(rhinoFloorUpdateInfo);
        }
        
        public void Add(IRailingUpdateInfo rhinoRailingUpdateInfo)
        {
            throw new NotImplementedException();
        }

        public void Add(IAssemblyId assemblyId)
        {
            throw new NotImplementedException();
        }

        public void Add(IAssemblyIdV2 assemblyId)
        {
            var assemblyIdList = AssemblyIdListsByAssemblyType.GetOrDefault(assemblyId.AssemblyTypeName);
            if (assemblyIdList == null)
            {
                // If assemblyIdList is null, create AssemblyIdList and add assemblyId.AssemblyTypeName as key
                assemblyIdList = new AssemblyIdList();
                AssemblyIdListsByAssemblyType.Add(assemblyId.AssemblyTypeName, assemblyIdList);
            }
            assemblyIdList.Add(assemblyId);
        }

        public void Add(ICompositeObjId buildingComponentId)
        {
            BuildingComponentIds.Add(buildingComponentId);
            AddToCompositeObjIdsByConstituentGuid(buildingComponentId);
        }

        private void AddToCompositeObjIdsByConstituentGuid(ICompositeObjId buildingComponentId)
        {
            foreach (IEnumerable<IConstituentId> constituentIds in buildingComponentId.ConstituentsByName.Values)
            {
                foreach (IConstituentId constituentId in constituentIds)
                {
                    CompositeObjIdsByConstitutentGuid.Add(constituentId.Id, buildingComponentId);
                }
            }
        }
        #endregion

        #region Get
        public IFloorComponentInfo GetFloorComponentInfoById(Guid id)
        {
            if (FloorComponentsByRhinoFloorId.TryGetValue(id, out IFloorComponentInfo floorComponentInfo))
            {
                return floorComponentInfo;
            }
            return null;
        }
        
        public bool Get(Guid id, out IFloorComponentInfo floorComponentInfo)
        {
            return FloorComponentsByRhinoFloorId.TryGetValue(id, out floorComponentInfo);
        }
        
        public bool GetSweepAssemblyIdByComponentId(Guid componentId, out ISweepAssemblyId sweepComponentId)
        {
            return SweepAssemblyIdsByComponentId.TryGetValue(componentId, out sweepComponentId);
        }
        
        public bool GetCompositeIdByConstitutentId(Guid constitutentGuid, out ICompositeObjId compositeObjId)
        {
            return CompositeObjIdsByConstitutentGuid.TryGetValue(constitutentGuid, out compositeObjId);
        }

        public int GetRailAssemblyCount()
        {
            return GetGenericAssemblyIdList(AssemblyTypeNames.Railing).Count();
        }

        public int GetGenericAssemblyCount()
        {
            return GetGenericAssemblyIdList(AssemblyTypeNames.Generic).Count();
        }

        public int GetAssemblyCount(string typeName)
        {
            var t = AssemblyIdListsByAssemblyType.GetOrDefault(typeName);
            return t.Count();
        }

        #endregion

        #region Action
        public void ActionOnRhinoFloorUpdateInfo(Action<IFloorUpdateInfo> action)
        {
            foreach (var item in RhinoFloorUpdateInfoSet)
            {
                action.Invoke(item);
            }
        }


        #endregion

        public void Remove(IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            RhinoFloorUpdateInfoSet.Remove(rhinoFloorUpdateInfo);
        }

        public bool CheckIfContainsEdge(Guid edgeId, out IFloorUpdateInfo rhinoFloorUpdateInfo)
        {
            foreach (IFloorUpdateInfo item in RhinoFloorUpdateInfoSet)
            {
                if (item.CheckIfContainEdgeId(edgeId))
                {
                    rhinoFloorUpdateInfo = item;
                    return true;
                }
            }
            rhinoFloorUpdateInfo = null;
            return false;
        }

        private IAssemblyIdList GetGenericAssemblyIdList(string assemblyTypeName)
        {
            return AssemblyIdListsByAssemblyType.GetOrDefault(assemblyTypeName);
        }

        public IRailingUpdateInfos RailingUpdateInfos => throw new NotImplementedException();

        public IList<ICompositeObjId> ObjsInEditingMode => throw new NotImplementedException();


    }
}
