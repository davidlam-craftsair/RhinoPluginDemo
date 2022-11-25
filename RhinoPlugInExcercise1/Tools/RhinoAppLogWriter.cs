using DL.Framework;
using Rhino;
using System.Collections.Generic;

namespace RhinoPlugInExcercise1
{
    public class RhinoAppLogWriter :
    ILogWriter
    {
        public RhinoAppLogWriter()
        {
            RhinoLogLevels = new HashSet<LogLvl> { LogLvl.Informational, LogLvl.MajorError, LogLvl.MinorError};
        }

        public void LogWrite(string logMessage)
        {
            RhinoApp.WriteLine(logMessage);
        }

        public ISet<LogLvl> RhinoLogLevels { get; }

        public void LogWrite(LogLvl logLevel, string logMessage)
        {
            if (RhinoLogLevels.Contains(logLevel))
            {
                RhinoApp.WriteLine($"GH: {logMessage}");
            }
        }
    }
}
