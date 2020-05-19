namespace WUD
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TriStateTreeView : TreeView
    {
        private IContainer components;
        private System.Windows.Forms.ImageList m_TriStateImages;
        private System.Windows.Forms.ImageList m_TriStateImagesXP;

        public TriStateTreeView()
        {
            this.InitializeComponent();
            if (this.VisualStylesEnabled())
            {
                this.ImageList = this.m_TriStateImagesXP;
            }
            else
            {
                this.ImageList = this.m_TriStateImages;
            }
            this.ImageIndex = 1;
            this.SelectedImageIndex = 1;
        }

        private void BuildTagDataList(TreeNode node, ArrayList list)
        {
            if ((this.GetChecked(node) == CheckState.Checked) && (node.Tag != null))
            {
                list.Add(node.Tag);
            }
            foreach (TreeNode node2 in node.Nodes)
            {
                this.BuildTagDataList(node2, list);
            }
        }

        protected void ChangeNodeState(TreeNode node)
        {
            CheckState @unchecked;
            base.BeginUpdate();
            if ((node.ImageIndex == 1) || (node.ImageIndex < 0))
            {
                @unchecked = CheckState.Checked;
            }
            else
            {
                @unchecked = CheckState.Unchecked;
            }
            this.CheckNode(node, @unchecked);
            this.ChangeParent(node.Parent);
            this.OnAfterCheck(new TreeViewEventArgs(node));
            base.EndUpdate();
        }

        private void ChangeParent(TreeNode node)
        {
            if (node != null)
            {
                CheckState state = this.GetChecked(node.FirstNode);
                foreach (TreeNode node2 in node.Nodes)
                {
                    state &= this.GetChecked(node2);
                }
                if (this.InternalSetChecked(node, state))
                {
                    this.ChangeParent(node.Parent);
                }
            }
        }

        private void CheckNode(TreeNode node, CheckState state)
        {
            this.InternalSetChecked(node, state);
            foreach (TreeNode node2 in node.Nodes)
            {
                this.CheckNode(node2, state);
            }
        }

        public void CheckNodeByTag(object tag, CheckState state)
        {
            if (tag != null)
            {
                foreach (TreeNode node in base.Nodes)
                {
                    this.FindAndCheckNode(node, tag, state);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DllImport("comctl32.dll", CharSet=CharSet.Auto)]
        private static extern int DllGetVersion(ref DLLVersionInfo version);
        private void FindAndCheckNode(TreeNode node, object tag, CheckState state)
        {
            if ((node.Tag != null) && node.Tag.Equals(tag))
            {
                this.SetChecked(node, state);
            }
            else
            {
                foreach (TreeNode node2 in node.Nodes)
                {
                    this.FindAndCheckNode(node2, tag, state);
                }
            }
        }

        public CheckState GetChecked(TreeNode node)
        {
            if (node.ImageIndex < 0)
            {
                return CheckState.Unchecked;
            }
            return (CheckState) node.ImageIndex;
        }

        public ArrayList GetCheckedTagData()
        {
            ArrayList list = new ArrayList();
            foreach (TreeNode node in base.Nodes)
            {
                this.BuildTagDataList(node, list);
            }
            return list;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TriStateTreeView));
            this.m_TriStateImages = new System.Windows.Forms.ImageList(this.components);
            this.m_TriStateImagesXP = new System.Windows.Forms.ImageList(this.components);
            base.SuspendLayout();
            this.m_TriStateImages.ImageStream = (ImageListStreamer) manager.GetObject("m_TriStateImages.ImageStream");
            this.m_TriStateImages.TransparentColor = Color.Magenta;
            this.m_TriStateImages.Images.SetKeyName(0, "");
            this.m_TriStateImages.Images.SetKeyName(1, "");
            this.m_TriStateImages.Images.SetKeyName(2, "");
            this.m_TriStateImagesXP.ImageStream = (ImageListStreamer) manager.GetObject("m_TriStateImagesXP.ImageStream");
            this.m_TriStateImagesXP.TransparentColor = Color.Fuchsia;
            this.m_TriStateImagesXP.Images.SetKeyName(0, "CheckBoxCheckedDisabled.bmp");
            this.m_TriStateImagesXP.Images.SetKeyName(1, "CheckBox.bmp");
            this.m_TriStateImagesXP.Images.SetKeyName(2, "CheckBoxChecked.bmp");
            base.ResumeLayout(false);
        }

        private bool InternalSetChecked(TreeNode node, CheckState state)
        {
            TreeViewCancelEventArgs e = new TreeViewCancelEventArgs(node, false, TreeViewAction.Unknown);
            this.OnBeforeCheck(e);
            if (e.Cancel)
            {
                return false;
            }
            node.ImageIndex = (int) state;
            node.SelectedImageIndex = (int) state;
            this.OnAfterCheck(new TreeViewEventArgs(node, TreeViewAction.Unknown));
            return true;
        }

        [DllImport("UxTheme.dll", CharSet=CharSet.Auto)]
        private static extern bool IsAppThemed();
        [DllImport("UxTheme.dll", CharSet=CharSet.Auto)]
        private static extern bool IsThemeActive();
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            TV_HITTESTINFO lParam = new TV_HITTESTINFO {
                pt = base.PointToClient(Control.MousePosition)
            };
            SendMessage(base.Handle, TreeViewMessages.TVM_HITTEST, 0, ref lParam);
            if ((lParam.flags & TVHit.OnItemIcon) == TVHit.OnItemIcon)
            {
                TreeNode nodeAt = base.GetNodeAt(lParam.pt);
                if (nodeAt != null)
                {
                    this.ChangeNodeState(nodeAt);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                this.ChangeNodeState(base.SelectedNode);
            }
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, TreeViewMessages msg, int wParam, ref TV_HITTESTINFO lParam);
        public void SetChecked(TreeNode node, CheckState state)
        {
            if (this.InternalSetChecked(node, state))
            {
                this.CheckNode(node, state);
                this.ChangeParent(node.Parent);
            }
        }

        private bool VisualStylesEnabled()
        {
            OperatingSystem oSVersion = Environment.OSVersion;
            bool flag = (oSVersion.Platform == PlatformID.Win32NT) && (((oSVersion.Version.Major == 5) && (oSVersion.Version.Minor >= 1)) || (oSVersion.Version.Major > 5));
            bool flag2 = false;
            bool flag3 = false;
            if (flag)
            {
                flag2 = OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null;
                DLLVersionInfo version = new DLLVersionInfo {
                    cbSize = Marshal.SizeOf(typeof(DLLVersionInfo))
                };
                DllGetVersion(ref version);
                flag3 = version.dwMajorVersion >= 6;
            }
            return (((flag && flag2) && (flag3 && IsAppThemed())) && IsThemeActive());
        }

        [Browsable(false)]
        public bool CheckBoxes
        {
            get => 
                base.CheckBoxes;
            set
            {
                base.CheckBoxes = value;
            }
        }

        [Browsable(false)]
        public int ImageIndex
        {
            get => 
                base.ImageIndex;
            set
            {
                base.ImageIndex = value;
            }
        }

        [Browsable(false)]
        public System.Windows.Forms.ImageList ImageList
        {
            get => 
                base.ImageList;
            set
            {
                base.ImageList = value;
            }
        }

        [Browsable(false)]
        public int SelectedImageIndex
        {
            get => 
                base.SelectedImageIndex;
            set
            {
                base.SelectedImageIndex = value;
            }
        }

        public enum CheckState
        {
            GreyChecked,
            Unchecked,
            Checked
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DLLVersionInfo
        {
            public int cbSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformID;
        }

        public enum TreeViewMessages
        {
            TV_FIRST = 0x1100,
            TVM_HITTEST = 0x1111
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TV_HITTESTINFO
        {
            public Point pt;
            public TriStateTreeView.TVHit flags;
            public IntPtr hItem;
        }

        [Flags]
        public enum TVHit
        {
            Above = 0x100,
            Below = 0x200,
            NoWhere = 1,
            OnItem = 70,
            OnItemButton = 0x10,
            OnItemIcon = 2,
            OnItemIndent = 8,
            OnItemLabel = 4,
            OnItemRight = 0x20,
            OnItemStateIcon = 0x40,
            ToLeft = 0x800,
            ToRight = 0x400
        }
    }
}

