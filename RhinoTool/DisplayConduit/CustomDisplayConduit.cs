using RhinoTool.Def;
using RhinoTool.MouseClick;
using Rhino;
using Rhino.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoTool.DisplayConduit
{
    internal class CustomDisplayConduit: Rhino.Display.DisplayConduit
    {
        bool _showModule;
        public bool ShowModule
        {
            get
            {
                return _showModule;
            }
            set
            {
                _showModule = value;
            }
        }

        public List<Module> Modules { get; set; }

        /// <summary>
        /// 默认的
        /// </summary>
        public DisplayMaterial DefaultMaterial { get; set; }
        /// <summary>
        /// 选中的
        /// </summary>
        public DisplayMaterial PickedMaterial { get; set; }
        /// <summary>
        /// 没选中的（如果有其他的被选中的话）
        /// </summary>
        public DisplayMaterial UnpickedMaterial { get; set; }

        public CustomDisplayConduit(List<Module> modules)
        {
            Modules = modules;
            ShowModule = true;
            DefaultMaterial = GetDefaultMaterial();
            PickedMaterial = GetPickedMaterial();
            UnpickedMaterial = GetUnpickedMaterial();
        }
        private DisplayMaterial GetDefaultMaterial()
        {
            DisplayMaterial defaultMaterial = new DisplayMaterial();
            defaultMaterial.Emission = Color.White;
            return defaultMaterial;
        }
        private DisplayMaterial GetPickedMaterial()
        {
            DisplayMaterial pickedMaterial = new DisplayMaterial();
            //pickedMaterial.Emission = Color.Pink;
            pickedMaterial.Diffuse = Color.Pink;
            pickedMaterial.Transparency = 0.4;
            pickedMaterial.BackTransparency = 0.4;
            return pickedMaterial;
        }
        private DisplayMaterial GetUnpickedMaterial()
        {
            DisplayMaterial unpickedMaterial = new DisplayMaterial();
            //unpickedMaterial.Emission = Color.Gray;
            unpickedMaterial.Diffuse = Color.Gray;
            unpickedMaterial.Transparency = 0.4;
            unpickedMaterial.Transparency = 0.4;
            return unpickedMaterial;
        }

        protected override void PostDrawObjects(DrawEventArgs e)
        {
            base.PostDrawObjects(e);
            if (ShowModule)
            {
                foreach (var module in Modules)
                {
                    if (module.IsPicked)
                    {
                        e.Display.DrawBrepShaded(module.BrepObj, PickedMaterial);
                        e.Display.DrawBrepWires(module.BrepObj, Color.Pink, 2);
                    }
                    else
                    {
                        if (CustomMouseCallback.Picking)
                        {
                            e.Display.DrawBrepShaded(module.BrepObj, UnpickedMaterial);
                            e.Display.DrawBrepWires(module.BrepObj, Color.Gray, 0);
                        }
                        else
                        {
                            e.Display.DrawBrepShaded(module.BrepObj, DefaultMaterial);
                            e.Display.DrawBrepWires(module.BrepObj, Color.Gray, 0);
                        }
                    }
                }
            }
        }

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            foreach(var module in Modules)
            {
                e.IncludeBoundingBox(module.BrepObj.GetBoundingBox(false));
            }
            base.CalculateBoundingBox(e);
        }

        public void SetTwoViewLayout(RhinoDoc doc)
        {
            Rhino.Display.RhinoView[] views = doc.Views.GetViewList(true, false);

            doc.Views.ActiveView.Maximized = true;

            int activeViewWidth = doc.Views.ActiveView.Size.Width;
            int activeViewHeight = doc.Views.ActiveView.Size.Height;

            foreach(var view in views)
            {
                view.Close();
            }

            //string filePath = "D:/华东院/自动强排/测试/ViewportTemplate.3dm";
            //var file = Rhino.FileIO.File3dm.Read(filePath);

            //// 遍历文件中的所有视图
            //foreach (var viewInfo in file.Views)
            //{
            //    // 创建或找到一个与文件中相同名称的视图
            //    Rhino.Display.RhinoView rhinoView = doc.Views.Find(viewInfo.Name, true);
            //    if (rhinoView == null)
            //    {
            //        // 如果视图不存在，则创建一个新的视图
            //        rhinoView = doc.Views.Add(viewInfo.Name, viewInfo.Viewport.Projection, viewInfo.Viewport.Bounds, true);
            //    }

            //    // 应用视图设置
            //    rhinoView.ActiveViewport.SetProjection(viewInfo.Viewport.Projection, viewInfo.Viewport.CameraDirection, viewInfo.Viewport.CameraLocation);
            //    rhinoView.Redraw();
            //}


            var view1 = doc.Views.Add("1", DefinedViewportProjection.Top, new System.Drawing.Rectangle(0, 0, activeViewWidth/2, activeViewHeight), false);
            
            var view2 = doc.Views.Add("2", DefinedViewportProjection.Perspective, new System.Drawing.Rectangle(activeViewWidth / 2, 0, activeViewWidth / 2, activeViewHeight), false);
        }
    }
}
