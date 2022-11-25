using DL.RhinoExcercise1.Core;
using System;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public interface ISessionDoc
    {
        IRailingUpdateInfos RailingUpdateInfos { get; }
        IList<ICompositeObjId> ObjsInEditingMode { get; }

        #region Add
        void Add(ISweepAssemblyId componentId);
        void Add(IAssemblyId assemblyId);
        void Add(IAssemblyIdV2 assemblyId);
        void Add(IFloorComponentInfo componentInfo);
        void Add(IFloorUpdateInfo rhinoFloorUpdateInfo);
        void Add(ICompositeObjId buildingComponentId);
        void Add(IRailingUpdateInfo rhinoRailingUpdateInfo);
        #endregion

        #region Get
        bool Get(Guid id, out IFloorComponentInfo floorComponentInfo);
        bool GetSweepAssemblyIdByComponentId(Guid railComponentId, out ISweepAssemblyId sweepComponentId);
        bool GetCompositeIdByConstitutentId(Guid constitutentGuid, out ICompositeObjId compositeObjId);

        int GetGenericAssemblyCount();
        int GetRailAssemblyCount();
        int GetAssemblyCount(string typeName);
        #endregion

        IFloorComponentInfo GetFloorComponentInfoById(Guid id);
        bool CheckIfContainsEdge(Guid edgeId, out IFloorUpdateInfo rhinoFloorUpdateInfo);

        #region Action on customized building component update info
        void ActionOnRhinoFloorUpdateInfo(Action<IFloorUpdateInfo> action);

        #endregion

        void Remove(IFloorUpdateInfo rhinoFloorUpdateInfo);
    }
}