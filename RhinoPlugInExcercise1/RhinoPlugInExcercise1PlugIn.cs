using DL.Framework;
using Rhino.PlugIns;
using Rhino.Render.DataSources;

namespace RhinoPlugInExcercise1
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class RhinoPlugInExcercise1PlugIn : Rhino.PlugIns.PlugIn

    {
        public RhinoPlugInExcercise1PlugIn()
        {
            Instance = this;
            CreateFramework();
        }

        ///<summary>Gets the only instance of the RhinoPlugInExcercise1PlugIn plug-in.</summary>
        public static RhinoPlugInExcercise1PlugIn Instance
        {
            get; private set;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            return base.OnLoad(ref errorMessage);
        }

        private static void CreateFramework()
        {
            var frameworkConstruction = new FrameworkConstruction();
            frameworkConstruction.ConfigureIoC(new IoCConfiguration());
        }

        protected override void OnShutdown()
        {
            
            base.OnShutdown();
        }
    }
}
