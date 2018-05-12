namespace GZDoomIDE.Windows {
    partial class ProjectExplorerWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeIcon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.KeepNodesExpanded = true;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.nodeIcon);
            this.treeView.NodeControls.Add(this.nodeName);
            this.treeView.SelectedNode = null;
            this.treeView.Size = new System.Drawing.Size(209, 606);
            this.treeView.TabIndex = 0;
            this.treeView.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.TreeView_NodeMouseDoubleClick);
            // 
            // nodeIcon
            // 
            this.nodeIcon.DataPropertyName = "Icon";
            // 
            // nodeName
            // 
            this.nodeName.DataPropertyName = "Name";
            this.nodeName.EditEnabled = true;
            // 
            // ProjectExplorerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 606);
            this.Controls.Add(this.treeView);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.MinimizeBox = false;
            this.Name = "ProjectExplorerWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TabText = "Project Explorer";
            this.Text = "Project Explorer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectExplorerForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeView;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeName;
    }
}