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

#endregion

namespace GZDoomIDE.Windows {
    public class ProjectExplorerTreeModel : ITreeModel, IDisposable {
        #region ================== Instance members

        protected MainWindow parentForm;
        protected WorkspaceData wsp;
        protected TreeViewAdv treeView; // Oh god, the hackery...

        #endregion

        #region ================== Constructors

        public ProjectExplorerTreeModel (MainWindow parentForm, TreeViewAdv treeView, WorkspaceData workspace) {
            this.parentForm = parentForm ?? throw new ArgumentNullException ("parentForm");
            this.treeView = treeView;
            wsp = workspace;

            parentForm.ProjectFolderModified += ProjectFolderModified;
        }

        #endregion

        #region ================== IDispose

        public void Dispose () {
            if (!(parentForm is null))
                parentForm.ProjectFolderModified -= ProjectFolderModified;
        }

        #endregion

        #region ================== Events

        public event EventHandler<TreeModelEventArgs> NodesChanged;
        public event EventHandler<TreeModelEventArgs> NodesInserted;
        public event EventHandler<TreeModelEventArgs> NodesRemoved;
        public event EventHandler<TreePathEventArgs> StructureChanged;

        private TreeNodeAdv FindNodeWithPath (string path) {
            foreach (var treeNode in treeView.AllNodes) {
                if (!(treeNode.Tag is PExp_BaseNode))
                    continue;

                var node = (treeNode.Tag as PExp_BaseNode);

                if (String.Equals (path, node.ItemPath, StringComparison.CurrentCultureIgnoreCase))
                    return treeNode;
            }

            return null;
        }

        private class PExp_FileNodeComparer : IComparer<PExp_BaseNode> {
            public int Compare (PExp_BaseNode x, PExp_BaseNode y) {
                if (x.GetType () == y.GetType ()) {
                    using (var comparer = new NaturalComparer ())
                        return comparer.Compare (x.Name, y.Name);
                } else if (x is PExp_FolderNode && y is PExp_FileNode)
                    return -1;
                else
                    return 1;
            }
        }
        private void ProjectFolderModified (object sender, MainWindow.ProjFolderEventArgs e) {
            if (e.EventArgs.ChangeType.HasFlag (WatcherChangeTypes.Created)) {
                var node = FindNodeWithPath (Path.GetDirectoryName (e.EventArgs.FullPath));
                if (node == null)
                    return;

                var treePath = treeView.GetPath (node);

                var attr = File.GetAttributes (e.EventArgs.FullPath);
                PExp_BaseNode newNode = null;

                if (attr.HasFlag (FileAttributes.Directory))
                    newNode = new PExp_FolderNode (e.EventArgs.FullPath, (node.Tag as PExp_BaseNode));
                else
                    newNode = new PExp_FileNode (e.EventArgs.FullPath, (node.Tag as PExp_BaseNode));

                int index;
                {
                    List<PExp_BaseNode> nodes = new List<PExp_BaseNode> (node.Children.Count + 1);
                    for (int i = 0; i < node.Children.Count; i++) {
                        var n = (node.Children [i]?.Tag as PExp_BaseNode);

                        if (n != null)
                            nodes.Add (n);
                    }

                    index = nodes.FindSortedAddIndex (newNode, new PExp_FileNodeComparer ());
                }

                if (treePath != null && newNode != null)
                    NodesInserted?.Invoke (this, new TreeModelEventArgs (treePath, new int [] { index }, new object [] { newNode }));
            }

            if (e.EventArgs.ChangeType.HasFlag (WatcherChangeTypes.Deleted)) {
                var oldNode = FindNodeWithPath (e.EventArgs.FullPath);

                if (oldNode == null)
                    return;

                var parentNode = oldNode.Parent;
                var treePath = treeView.GetPath (parentNode);

                if (treePath != null)
                    NodesRemoved?.Invoke (this, new TreeModelEventArgs (treePath, new object [] { oldNode }));
            }

            if (e.EventArgs.ChangeType.HasFlag (WatcherChangeTypes.Renamed)) {
                var oldPath = (e.EventArgs as RenamedEventArgs).OldFullPath;
                var oldNode = FindNodeWithPath (oldPath);
                
                if (oldNode == null)
                    return;

                var parentNode = oldNode.Parent;
                var treePath = treeView.GetPath (parentNode);

                var attr = File.GetAttributes (e.EventArgs.FullPath);
                PExp_BaseNode newNode = null;

                if (attr.HasFlag (FileAttributes.Directory))
                    newNode = new PExp_FolderNode (e.EventArgs.FullPath, (parentNode.Tag as PExp_BaseNode));
                else
                    newNode = new PExp_FileNode (e.EventArgs.FullPath, (parentNode.Tag as PExp_BaseNode));

                int index;
                {
                    List<PExp_BaseNode> nodes = new List<PExp_BaseNode> (parentNode.Children.Count + 1);
                    for (int i = 0; i < parentNode.Children.Count; i++) {
                        var n = (parentNode.Children [i]?.Tag as PExp_BaseNode);

                        if (n != null && n != oldNode.Tag)
                            nodes.Add (n);
                    }

                    index = nodes.FindSortedAddIndex (newNode, new PExp_FileNodeComparer ());
                }

                if (treePath != null && newNode != null) {
                    NodesRemoved?.Invoke (this, new TreeModelEventArgs (treePath, new object [] { oldNode }));
                    NodesInserted?.Invoke (this, new TreeModelEventArgs (treePath, new int [] { index }, new object [] { newNode }));
                }
            }
        }

        #endregion

        #region ================== Instance methods

        public IEnumerable GetChildren (TreePath treePath) {
            if (treePath.IsEmpty ()) {
                if (!(wsp is null)) {
                    PExp_WorkspaceNode item = new PExp_WorkspaceNode (wsp);
                    yield return item;
                } else
                    yield break;
            } else {
                List<PExp_BaseNode> items = new List<PExp_BaseNode> ();
                PExp_BaseNode parent = treePath.LastNode as PExp_BaseNode;

                if (!(parent is null)) {
                    if (parent is PExp_WorkspaceNode) {
                        var wspNode = parent as PExp_WorkspaceNode;

                        if (!(wspNode.Workspace is null)) {
                            foreach (var proj in wspNode.Workspace.ProjectFiles)
                                items.Add (new PExp_ProjectNode (proj, parent));

                            items.Sort ((a, b) => {
                                using (var comparer = new NaturalComparer ())
                                    return comparer.Compare (a.Name, b.Name);
                            });
                        }
                    } else if (parent is PExp_ProjectNode) {
                        var projNode = parent as PExp_ProjectNode;

                        if (!(projNode.Project is null) && !projNode.Project.IsInvalid && projNode.Project.IsLoaded) {
                            string projPath = Utils.GetAbsolutePath (projNode.Project.ProjectFilePath, Path.GetDirectoryName (wsp.WorkspaceFilePath));
                            string path = Utils.GetAbsolutePath (projNode.Project.SourcePath, Path.GetDirectoryName (projPath));

                            foreach (string str in Directory.GetDirectories (path))
                                items.Add (new PExp_FolderNode (str, parent));
                            foreach (string str in Directory.GetFiles (path))
                                items.Add (new PExp_FileNode (str, parent));

                            items.Sort (new PExp_FileNodeComparer ());
                        }
                    } else if (parent is PExp_FolderNode) {
                        foreach (string str in Directory.GetDirectories (parent.ItemPath))
                            items.Add (new PExp_FolderNode (str, parent));
                        foreach (string str in Directory.GetFiles (parent.ItemPath))
                            items.Add (new PExp_FileNode (str, parent));

                        items.Sort (new PExp_FileNodeComparer ());
                    }

                    foreach (PExp_BaseNode item in items)
                        yield return item;
                } else
                    yield break;
            }
        }

        public bool IsLeaf (TreePath treePath) {
            if (treePath.LastNode is PExp_FileNode)
                return true;
            else if (treePath.LastNode is PExp_WorkspaceNode)
                return (treePath.LastNode as PExp_WorkspaceNode).Workspace is null;
            else if (treePath.LastNode is PExp_ProjectNode) {
                var project = (treePath.LastNode as PExp_ProjectNode).Project;

                return (project is null) || project.IsInvalid || !project.IsLoaded;
            }

            return false;
        }

        #endregion
    }

    public abstract class PExp_BaseNode {
        #region ================== Instance members

        /// <summary>
        /// The path to the file.
        /// </summary>
        public virtual string ItemPath { get; set; }

        /// <summary>
        /// The item's icon.
        /// </summary>
        public virtual Image Icon { get; set; }

        /// <summary>
        /// The item's name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The item's parent node.
        /// </summary>
        public PExp_BaseNode Parent { get; set; }

        /// <summary>
        /// Called when the node is double-clicked
        /// </summary>
        public virtual void DoubleClicked (MainWindow mainWindow, ProjectExplorerWindow projExplWindow) { }

        #endregion
    }

    public class PExp_WorkspaceNode : PExp_BaseNode {
        #region ================== Constructors

        /// <summary>
        /// Creates a new workspace node.
        /// </summary>
        /// <param name="wsp">The workspace this node is connected to.</param>
        public PExp_WorkspaceNode (WorkspaceData wsp) {
            Workspace = wsp;
        }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The workspace this node is connected to.
        /// </summary>
        public WorkspaceData Workspace { get; set; }

        /// <summary>
        /// The item's name.
        /// </summary>
        public override string Name {
            get { return Workspace.Name; }
            set { Workspace.Name = value; }
        }

        #endregion
    }

    public class PExp_ProjectNode : PExp_BaseNode {
        #region ================== Constructors

        /// <summary>
        /// Creates a new project node.
        /// </summary>
        /// <param name="proj">The project this node is connected to.</param>
        /// <param name="parent">The node's parent node.</param>
        public PExp_ProjectNode (ProjectData proj, PExp_BaseNode parent) {
            Project = proj;
            Parent = parent;
        }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The project this node is connected to.
        /// </summary>
        public ProjectData Project { get; set; }

        /// <summary>
        /// The item's name.
        /// </summary>
        public override string Name {
            get { return Project.Name; }
            set { Project.Name = value; }
        }

        #endregion
    }

    public class PExp_FolderNode : PExp_BaseNode {
        #region ================== Constructors

        /// <summary>
        /// Creates a new folder node.
        /// </summary>
        /// <param name="path">The path to the folder.</param>
        /// <param name="parent">The node's parent node.</param>
        public PExp_FolderNode (string path, PExp_BaseNode parent) {
            ItemPath = path;
            Parent = parent;
        }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The file's name.
        /// </summary>
        public override string Name {
            get { return Path.GetFileName (ItemPath); }
            set {
                /*string dir = Path.GetDirectoryName (ItemPath);
                string destination = Path.Combine (dir, value);
                Directory.Move (ItemPath, destination);
                ItemPath = destination;*/
            }
        }

        #endregion
    }

    public class PExp_FileNode : PExp_BaseNode {
        #region ================== Constructors

        /// <summary>
        /// Creates a new file node.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="parent">The node's parent node.</param>
        public PExp_FileNode (string path, PExp_BaseNode parent) {
            ItemPath = path;
            Parent = parent;
        }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The file's name.
        /// </summary>
        public override string Name {
            get { return Path.GetFileName (ItemPath); }
            set {
                string dir = Path.GetDirectoryName (ItemPath);
                string destination = Path.Combine (dir, value);
                File.Move (ItemPath, destination);
                ItemPath = destination;
            }
        }

        public override void DoubleClicked (MainWindow mainWindow, ProjectExplorerWindow projExplWindow) {
            var node = Parent;
            while (node != null && !(node is PExp_ProjectNode))
                node = node.Parent;

            ProjectData proj = null;

            if (node != null && node is PExp_ProjectNode)
                proj = (node as PExp_ProjectNode).Project;

            mainWindow.OpenFileWindow (ItemPath, proj, out _);
        }

        #endregion
    }
}
