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
    public partial class MainWindow : Form {
        private ProjectExplorerWindow projExpl;
        private ErrorList errorList;
        private List<TextEditorWindow> fileForms = new List<TextEditorWindow> ();
        private Dictionary<ProjectData, System.IO.FileSystemWatcher> projFolderWatchers = new Dictionary<ProjectData, System.IO.FileSystemWatcher> ();

        #region ================== ErrorProviders

        public IDEErrorProvider WorkspaceErrors { get; protected set; } = new IDEErrorProvider ();
        public Dictionary<ProjectData, IDEErrorProvider> ProjectErrors { get; protected set; } = new Dictionary<ProjectData, IDEErrorProvider> ();
        public IDEErrorProvider CurFileErrors { get; protected set; } = new IDEErrorProvider ();

        #endregion

        #region ================== Fields

        private WorkspaceData _curWorkspace;
        protected internal WorkspaceData CurWorkspace {
            get => _curWorkspace;
            protected set {
                PreWorkspaceChange (EventArgs.Empty);
                _curWorkspace = value;
                OnWorkspaceChanged (EventArgs.Empty);
            }
        }

        #endregion

        #region ================== Events

        public event EventHandler<EventArgs> WorkspaceChanged;
        protected virtual void OnWorkspaceChanged (EventArgs e) {
            WorkspaceChanged?.Invoke (this, e);
        }

        public event EventHandler<EventArgs> PreWorkspaceChanged;
        protected virtual void PreWorkspaceChange (EventArgs e) {
            PreWorkspaceChanged?.Invoke (this, e);
        }

        #endregion

        #region ================== Constructors

        public MainWindow () {
            InitializeComponent ();

            InitializeMainForm ();

            Program.Data.PluginManager.LoadAllPlugins ();
        }

        private void InitializeMainForm () {
            openFileDialog.Filter = Constants.FileTypes;
            saveFileDialog.Filter = Constants.FileTypes;

            projExpl = new ProjectExplorerWindow (this);
            projExpl.Show (mainDockPanel, DockState.DockRightAutoHide);

            errorList = new ErrorList (this);
            errorList.Show (mainDockPanel, DockState.DockBottomAutoHide);

            Utils.ThemeToolstrips (vsToolStripExtender, new ToolStrip [] { mainMenuStrip, mainStatusStrip }, vs2015DarkTheme);
        }

        #endregion

        #region ================== Functions
        /// <summary>
        /// Gets the currently active document window/panel/tab.
        /// </summary>
        /// <returns>The active document as a DockContent -or- null.</returns>
        private DockContent GetActiveDocument () {
            if (mainDockPanel.ActiveDocument is null || mainDockPanel.ActiveDocument.DockHandler is null || mainDockPanel.ActiveDocument.DockHandler.Content is null)
                return null;

            return (DockContent) mainDockPanel.ActiveDocument.DockHandler.Content;
        }

        #region Workspace loading
        /// <summary>
        /// Opens a workspace.
        /// </summary>
        /// <param name="wspPath">The workspace to be opened.</param>
        /// <returns>Returns true if the workspace was opened successfully.</returns>
        private bool OpenWorkspace (string wspPath) {
            if (String.IsNullOrWhiteSpace (wspPath))
                return false;
            //ProjectData FileSystemWatcher projFolderWatchers

            CurWorkspace = WorkspaceData.Load (wspPath);
            
            return !(CurWorkspace is null);
        }
        #endregion

        #region Workspace saving



        #endregion

        /// <summary>
        /// Opens the specified file in a window
        /// </summary>
        /// <param name="filePath">The file to open</param>
        /// <returns>Returns a bool indicating whether the file was opened successfully. Returns false if it failed to read the file or the file is already open.</returns>
        private bool OpenFileWindow (string filePath) {
            if (!(filePath is null)) {
                foreach (TextEditorWindow ff in fileForms) {
                    if (ff.FilePath == filePath)
                        return false;
                }

                filePath = System.IO.Path.GetFullPath (filePath);
            }

            TextEditorWindow newFile = TextEditorWindow.OpenFile (filePath);

            if (newFile is null)
                return false;

            if (!String.IsNullOrWhiteSpace (filePath)) {
                newFile.TabText = System.IO.Path.GetFileName (filePath);
                newFile.Text = filePath;
            } else
                newFile.TabText = newFile.Text = "New file";
            newFile.FormClosed += FileForm_FormClosed;

            fileForms.Add (newFile);

            newFile.Show (mainDockPanel, DockState.Document);
            newFile.Focus ();

            return true;
        }

        #region File saving
        private bool SaveFileInternal (TextEditorWindow doc, string path) {
            if (String.IsNullOrWhiteSpace (path))
                return false;

            try {
                System.IO.File.WriteAllText (path, doc.scintillaControl.Text);
            } catch {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves the specified file window, opening a Save As prompt only if the file wasn't saved to disk yet.
        /// </summary>
        /// <param name="doc">The file window to save.</param>
        /// <returns>Returns a bool indicating whether or not the file was saved successfully</returns>
        private bool SaveFile (TextEditorWindow doc) {
            if (doc is null)
                return false;

            if (!String.IsNullOrWhiteSpace (doc.FilePath)) {
                if (SaveFileInternal (doc, doc.FilePath)) {
                    doc.scintillaControl.SetSavePoint ();
                    return true;
                } else
                    return false;
            } else
                return SaveFileAs (doc);
        }

        /// <summary>
        /// Saves the specified file window, opening a Save As prompt.
        /// </summary>
        /// <param name="doc">The file window to save.</param>
        /// <returns>Returns a bool indicating whether or not the file was saved successfully</returns>
        private bool SaveFileAs (TextEditorWindow doc) {
            if (doc is null)
                return false;

            if (saveFileDialog.ShowDialog () == DialogResult.OK) {
                string path = saveFileDialog.FileName;

                if (SaveFileInternal (doc, path)) {
                    doc.FilePath = path;
                    doc.scintillaControl.SetSavePoint ();

                    doc.Text = path;
                    doc.TabText = System.IO.Path.GetFileName (path);

                    return true;
                } else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Saves all the open files.
        /// </summary>
        private void SaveAllFiles () {
            foreach (TextEditorWindow doc in mainDockPanel.Documents) {
                if (doc is null)
                    continue;

                if (!SaveFile (doc))
                    return;
            }
        }
        #endregion
        #endregion

        #region ================== Menu items

        #region File menu
        private void FileNewFile_MenuItem_Click (object sender, EventArgs e) {
            OpenFileWindow (null);
        }

        private void FileOpenFile_MenuItem_Click (object sender, EventArgs e) {
            openFileDialog.Title = "Open file";

            if (openFileDialog.ShowDialog () == DialogResult.OK) {
                string path = openFileDialog.FileName;

                if (String.IsNullOrWhiteSpace (path))
                    return;

                OpenFileWindow (path);
            }
        }

        private void FileSave_MenuItem_Click (object sender, EventArgs e) {
            SaveFile (GetActiveDocument () as TextEditorWindow);
        }

        private void FileSaveAs_MenuItem_Click (object sender, EventArgs e) {
            SaveFileAs (GetActiveDocument () as TextEditorWindow);
        }

        private void FileClose_MenuItem_Click (object sender, EventArgs e) {
            var doc = GetActiveDocument ();

            if (!(doc is null))
                doc.Close ();
        }

        private void FileExit_MenuItem_Click (object sender, EventArgs e) {
            this.Close ();
        }
        #endregion

        #region View menu
        private void ViewProjExpl_MenuItem_Click (object sender, EventArgs e) {
            if (projExpl.Visible)
                projExpl.Hide ();
            else
                projExpl.Show ();

            ((ToolStripMenuItem) sender).Checked = projExpl.Visible;
        }

        private void viewErrorList_MenuItem_Click (object sender, EventArgs e) {
            if (errorList.Visible)
                errorList.Hide ();
            else
                errorList.Show ();

            ((ToolStripMenuItem) sender).Checked = errorList.Visible;
        }
        #endregion

        #region Help menu

        private void HelpAbout_MenuItem_Click (object sender, EventArgs e) {
            AboutBox aboutBox = new AboutBox ();
            aboutBox.ShowDialog ();
            aboutBox.Dispose ();
        }

        #endregion

        #endregion

        #region ================== File form events
        private void FileForm_FormClosed (object sender, FormClosedEventArgs e) {
            if (sender.GetType () != typeof (TextEditorWindow))
                throw new ArgumentException ("FileForm_FormClosed was called with a non-FileForm sender");

            var fileForm = sender as TextEditorWindow;

            fileForms.Remove (fileForm);
        }
        #endregion

        private void FileNewProject_MenuItem_Click (object sender, EventArgs e) {
            //NewProjectWindow newProjWin = new NewProjectWindow ();
            //newProjWin.ShowDialog ();
        }
    }
}
