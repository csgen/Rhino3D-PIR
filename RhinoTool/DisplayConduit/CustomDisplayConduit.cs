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
using Rhino.DocObjects;
using Rhino.Geometry;

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

        /// <summary>
        /// 将视窗设置为左右并列
        /// </summary>
        /// <param name="doc"></param>
        public void SetTwoViewLayout(RhinoDoc doc)
        {
            // 调整视窗布局
            RhinoView[] views = doc.Views.GetViewList(true, false);
            
            if (views.Length > 0)
            {
                var view = views[0];
                view.Maximized = true;
                int activeViewWidth = view.Size.Width;
                int activeViewHeight = view.Size.Height;

                var standardViews = doc.Views.GetStandardRhinoViews();
                for (int i = standardViews.Count() - 1; i >= 0; i--)
                {
                    var curView = standardViews[i];
                    var viewInfo = new ViewInfo(curView.ActiveViewport);
                    if (viewInfo.Name == "Top" || viewInfo.Name == "Perspective")
                    {
                        curView.Size = new Size(activeViewWidth / 2, activeViewHeight);
                    }
                }
            }

            // 缩放视角
            BoundingBox boundingBox = BoundingBox.Empty;
            foreach (var module in Modules)
            {
                boundingBox.Union(module.BrepObj.GetBoundingBox(true));
            }
            foreach (var view in views)
            {
                view.ActiveViewport.ZoomBoundingBox(boundingBox);
            }
        }
    }
}
