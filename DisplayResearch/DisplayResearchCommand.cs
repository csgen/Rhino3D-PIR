using App;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Windows.Interop;

namespace DisplayResearch
{
    public class DisplayResearchCommand : Command
    {
        public DisplayResearchCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static DisplayResearchCommand Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "DisplayResearch";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---
            //RhinoApp.WriteLine("The {0} command will add a line right now.", EnglishName);
            MainWindow window = new MainWindow();
            window.Show();

            IntPtr rhinoMainWindowHandle = RhinoApp.MainWindowHandle();
            WindowInteropHelper wpfHelper = new WindowInteropHelper(window);
            wpfHelper.Owner = rhinoMainWindowHandle;

            RhinoApp.WriteLine("The {0} command added a Displayer to the document.", EnglishName);

            // ---
            return Result.Success;
        }
    }
}
