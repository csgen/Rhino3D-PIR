using RhinoTool.Managers;
using RhinoTool.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoTool
{
    internal class RhinoCommand
    {
        
        public static void Create()
        {
            if(CreatorManager.moduleCreators.Count == 0)
            {
                ModuleCreator moduleCreator = new ModuleCreator();
                CreatorManager.moduleCreators.Add(moduleCreator);
            }
        }
        public static void Hide()
        {
            if (CreatorManager.moduleCreators.Count != 0)
            {
                foreach(var creator in CreatorManager.moduleCreators)
                {
                    creator.Displayer.Enabled = false;
                    creator.MouseCaller.Enabled = false;
                }
            }
        }
        public static void SetView()
        {
            if (CreatorManager.moduleCreators.Count != 0)
            {
                var creator = CreatorManager.moduleCreators[0];
                creator.Displayer.SetTwoViewLayout(Rhino.RhinoDoc.ActiveDoc);
            }
                
        }
    }
}
