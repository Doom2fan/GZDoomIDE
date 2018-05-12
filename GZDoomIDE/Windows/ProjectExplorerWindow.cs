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

using Aga.Controls.Tree;
using GZDoomIDE.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class ProjectExplorerWindow : DockContent {
        MainWindow parentForm;

        protected ProjectExplorerWindow () {
            InitializeComponent ();
            
            Program.Data.Themer.ApplyTheme (this);
        }

        public ProjectExplorerWindow (MainWindow parentForm) : this () {
            this.parentForm = parentForm ?? throw new ArgumentNullException ("parentForm");
            treeView.Model = new ProjectExplorerTreeModel (parentForm, treeView, this.parentForm.CurWorkspace);

            this.parentForm.WorkspaceChanged += WorkspaceChanged;
        }

        private void WorkspaceChanged (object sender, EventArgs e) {
            if (!(treeView.Model is null) && treeView.Model is ProjectExplorerTreeModel)
                (treeView.Model as ProjectExplorerTreeModel).Dispose ();

            treeView.Model = new ProjectExplorerTreeModel (parentForm, treeView, parentForm.CurWorkspace);
        }

        private void ProjectExplorerForm_FormClosed (object sender, FormClosedEventArgs e) {
            if (!(parentForm is null))
                parentForm.WorkspaceChanged -= WorkspaceChanged;
        }

        private void TreeView_NodeMouseDoubleClick (object sender, TreeNodeAdvMouseEventArgs e) {
            if (e.Node.Tag == null)
                return;

            var node = e.Node.Tag;

            if (node is PExp_BaseNode)
                (node as PExp_BaseNode).DoubleClicked (parentForm, this);
        }
    }
}
