using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace RhinoTool
{
    public class RhinoService: IRhinoService
    {
        public void Create()
        {
            RhinoCommand.Create();
        }
        public void Hide()
        {
            RhinoCommand.Hide();
        }
        public void SetView()
        {
            RhinoCommand.SetView();
        }
    }
}
