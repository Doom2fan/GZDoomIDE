#region ================== Copyright (C) 2018 Chronos Ouroboros

/*
 *  GZDoom IDE - An IDE for GZDoom modding/game-making.
 *  Copyright (C) 2018 Chronos Ouroboros
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
**/

#endregion


#region ================== Namespaces

using GZDoomIDE.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class ProjectExplorerWindow : DockContent {
        MainWindow parentForm;

        protected ProjectExplorerWindow () {
            InitializeComponent ();
        }

        public ProjectExplorerWindow (MainWindow parentForm) : this () {
            this.parentForm = parentForm ?? throw new ArgumentNullException ("parentForm");
            this.parentForm.WorkspaceChanged += ParentForm_WorkspaceChanged; ;
        }
        private void ProjectExplorerForm_FormClosed (object sender, FormClosedEventArgs e) {
            this.parentForm.WorkspaceChanged -= ParentForm_WorkspaceChanged;
        }

        private void ParentForm_WorkspaceChanged (object sender, EventArgs e) {
            var wsp = this.parentForm.CurWorkspace;

            this.SuspendLayout ();

            treeView.Nodes.Clear ();

            if (!(wsp is null)) {
                List<TreeNode> projects = new List<TreeNode> ();

                foreach (ProjectData proj in wsp.ProjectFiles)
                    projects.Add (new TreeNode (proj.Name));

                treeView.Nodes.Add (new TreeNode (wsp.Name, projects.ToArray ()));
            }

            this.ResumeLayout ();
        }
    }
}
