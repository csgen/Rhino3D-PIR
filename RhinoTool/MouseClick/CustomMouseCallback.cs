using RhinoTool.Def;
using Rhino.Geometry;
using Rhino.Input.Custom;
using Rhino;
using Rhino.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoTool.MouseClick
{
    internal class CustomMouseCallback : MouseCallback
    {
        public static bool Picking {  get; set; }
        public List<Module> Modules {  get; set; }
        /// <summary>
        /// 最后一次鼠标选中的索引
        /// </summary>
        public int PickIndex { get; set; }
        /// <summary>
        /// 多选：当前选择的物件索引列表
        /// </summary>
        public List<int> CurIndexes { get; set; }
        /// <summary>
        /// 当前（或最后一次）被选中的物件索引
        /// </summary>
        public int CurIndex { get; set; }

        public CustomMouseCallback(List<Module> modules)
        {
            Modules = modules;
            PickIndex = -1;
            CurIndex = -1;
            CurIndexes = new List<int>();
        }

        protected override void OnMouseDown(Rhino.UI.MouseCallbackEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.MouseButton != Rhino.UI.MouseButton.Left) { return; }

            #region 测试多选
            this.Picker(e);
            ToggleSelectionDisplay();
            #endregion

            e.Cancel = true;
        }

        private void Picker(Rhino.UI.MouseCallbackEventArgs e)
        {
            var picker = new PickContext();
            picker.View = e.View;
            picker.PickStyle = PickStyle.PointPick;
            var xform = e.View.ActiveViewport.GetPickTransform(e.ViewportPoint);
            picker.SetPickTransform(xform);

            Point3d hitPoint;
            double depth;
            double distance;
            PickContext.MeshHitFlag hitFlag;
            int hitIndex;

            List<double> depths = new List<double>();
            int pickIndex = -1;

            for (int i = 0; i < Modules.Count; i++)
            {
                Brep brep = Modules[i].BrepObj;
                Mesh[] meshes = Mesh.CreateFromBrep(brep, new MeshingParameters());
                Mesh mesh = new Mesh();
                mesh.Append(meshes);

                var result = picker.PickFrustumTest(mesh, PickContext.MeshPickStyle.ShadedModePicking,
                                out hitPoint,
                                out depth,
                                out distance,
                                out hitFlag,
                                out hitIndex);
                depths.Add(depth);
            }

            pickIndex = depths.Max() > -1 ? depths.IndexOf(depths.Max()) : -1;
            RhinoApp.WriteLine("Selected: " + pickIndex.ToString());

            this.PickIndex = pickIndex;

            if (pickIndex == -1)
            {
                this.CurIndexes.Clear();
                Picking = false;
                this.CurIndex = pickIndex;
                return;
            }
            else
            {
                //添加至多选列表
                if (!e.ShiftKeyDown)
                {
                    this.CurIndexes.Clear();
                    this.CurIndexes.Add(pickIndex);
                }
                else
                {
                    if (CurIndexes.Contains(pickIndex))
                    {
                        CurIndexes.Remove(pickIndex);
                        this.CurIndex = -1;
                    }
                    else
                    {
                        this.CurIndexes.Add(pickIndex);
                        this.CurIndex = pickIndex;
                    }
                }
            }
            if (CurIndexes.Count > 0)
            {
                Picking = true;
            }
            else
            {
                Picking = false;
            }
        }

        /// <summary>
        /// 切换Gumball显示/或更多其他显示
        /// </summary>
        private void ToggleSelectionDisplay()
        {
            Module m;

            for (int i = 0; i < Modules.Count; i++)
            {
                m = Modules[i];
                if (!CurIndexes.Contains(i) && m.IsPicked)
                {
                    // TODO: 显示改变
                    m.IsPicked = false;
                    RhinoDoc.ActiveDoc.Views.Redraw();
                }
            }
            for (int i = 0; i < Modules.Count; i++)
            {
                m = Modules[i];
                if (CurIndexes.Contains(i) && !m.IsPicked)
                {
                    m.IsPicked = true;
                    RhinoDoc.ActiveDoc.Views.Redraw();
                }
            }
        }
    }
}
