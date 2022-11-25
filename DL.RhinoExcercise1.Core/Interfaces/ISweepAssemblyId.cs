using System.Collections;

namespace DL.RhinoExcercise1.Core
{
    public interface ISweepAssemblyId: IAssemblyId
    {
    }

    public interface ISweepAssemblyUpdateInfo
    {
        ISweepAssemblyId SweepAssemblyId { get; }
        
    }


}