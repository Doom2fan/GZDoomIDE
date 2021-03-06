﻿#region ================== Copyright (C) 2018 Chronos Ouroboros

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
using GZDoomIDE.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class MainWindow : Form {
        #region ================== ErrorProviders

        public IDEErrorProvider WorkspaceErrors { get; protected set; } = new IDEErrorProvider ();
        public Dictionary<ProjectData, IDEErrorProvider> ProjectErrors { get; protected set; } = new Dictionary<ProjectData, IDEErrorProvider> ();
        public IDEErrorProvider CurFileErrors { get; protected set; } = new IDEErrorProvider ();

        #endregion

        #region ================== Events

        #region Workspaces

        public event EventHandler<EventArgs> WorkspaceChanged;
        protected virtual void OnWorkspaceChanged (EventArgs e) {
            foreach (var watcher in projFolderWatchers.Values) {
                watcher.Changed -= Watcher_Changed;
                watcher.Dispose ();
            }
            projFolderWatchers.Clear ();

            if (!(CurWorkspace is null) && !(CurWorkspace.ProjectFiles is null)) {
                foreach (var proj in CurWorkspace.ProjectFiles) {
                    if (!proj.IsLoaded) {
                        string path = Utils.GetAbsolutePath (proj.ProjectFilePath, Path.GetDirectoryName (CurWorkspace.WorkspaceFilePath));
                        if (!File.Exists (path)) {
                            proj.IsInvalid = true;

                            WorkspaceErrors.Errors.Add (new IDEError (ErrorType.Warning, String.Format ("Could not load project {0}.", proj.Name)));
                            continue;
                        }

                        var loadedProj = ProjectData.Load (path);
                        if (!(loadedProj is null))
                            loadedProj.ProjectFilePath = proj.ProjectFilePath;

                        proj.Copy (loadedProj);
                    }

                    string projPath = Utils.GetAbsolutePath (proj.ProjectFilePath, Path.GetDirectoryName (CurWorkspace.WorkspaceFilePath));
                    if (proj.IsLoaded && !proj.IsInvalid) {
                        var watcher = new FileSystemWatcher (Utils.GetAbsolutePath (proj.SourcePath, Path.GetDirectoryName (projPath)));
                        watcher.Filter = "";
                        watcher.IncludeSubdirectories = true;
                        watcher.EnableRaisingEvents = true;
                        watcher.Created += Watcher_Changed;
                        watcher.Deleted += Watcher_Changed;
                        watcher.Renamed += Watcher_Changed;
                        watcher.Changed += Watcher_Changed;

                        projFolderWatchers.Add (proj, watcher);
                    }
                }

                if (CurWorkspace.ProjectFiles.Count > 0) // Just to make sure...
                    CurWorkspace.ProjectFiles.Move (0, 0); // durrrrrr
            }

            WorkspaceChanged?.Invoke (this, e);
        }

        public event EventHandler<EventArgs> PreWorkspaceChanged;
        protected virtual void PreWorkspaceChange (EventArgs e) {
            PreWorkspaceChanged?.Invoke (this, e);
        }

        public class ProjFolderEventArgs : EventArgs {
            public FileSystemWatcher Watcher { get; }
            public FileSystemEventArgs EventArgs { get; }

            public ProjFolderEventArgs (FileSystemWatcher watcher, FileSystemEventArgs args) {
                Watcher = watcher;
                EventArgs = args;
            }
        }
        public event EventHandler<ProjFolderEventArgs> ProjectFolderModified;
        protected virtual void OnProjectFolderModified (ProjectData proj, ProjFolderEventArgs e) {
            ProjectFolderModified?.Invoke (proj, e);
        }
        private void Watcher_Changed (object sender, FileSystemEventArgs e) {
            var watcher = (sender as FileSystemWatcher);
            var kvp = projFolderWatchers.First (a => a.Value == watcher);

            if (kvp.Key is null || kvp.Value is null) // wtf?
                return;

            OnProjectFolderModified (kvp.Key, new ProjFolderEventArgs (watcher, e));
        }

        #endregion

        #region Themes

        public event EventHandler<EventArgs> EditorThemeChanged;
        protected virtual void OnEditorThemeChanged (EventArgs e) {
            EditorThemeChanged?.Invoke (this, e);
        }

        #endregion

        #endregion

        #region ================== Instance members

        private bool dataLoaded = false;

        private ProjectExplorerWindow projExpl;
        private ErrorList errorList;

        private List<TextEditorWindow> fileForms = new List<TextEditorWindow> ();
        private Dictionary<ProjectData, FileSystemWatcher> projFolderWatchers = new Dictionary<ProjectData, System.IO.FileSystemWatcher> ();

        private WorkspaceData curWorkspace;
        protected internal WorkspaceData CurWorkspace {
            get => curWorkspace;
            protected set {
                PreWorkspaceChange (EventArgs.Empty);
                curWorkspace = value;
                OnWorkspaceChanged (EventArgs.Empty);
            }
        }

        private ScintillaTheme editorTheme;
        protected internal ScintillaTheme EditorTheme {
            get => editorTheme;
            protected set {
                editorTheme = value;
                OnEditorThemeChanged (EventArgs.Empty);
            }
        }

        #endregion

        #region ================== Initialization

        public MainWindow () {
            InitializeComponent ();
        }

        public void Initialize () {
            if (!dataLoaded) {
                (Action, string) [] operations = {
                    (LoadPlugins, "Loading plugins"),
                    (LoadProjectTypes, "Loading project types"),
                    (LoadSyntaxHighlighters, "Loading syntax highlighters"),
                    (LoadTemplates, "Loading templates"),
                    (LoadIDEThemes, "Loading UI themes"),
                    (LoadEditorThemes, "Loading editor themes"),
                    (() => { Program.Data.Themer.ApplyTheme (this); }, "Applying theme"),
                    (InitializeMainForm, "Initializing main window"),
                };

                double a = 1000.0 / operations.Length;
                int count = 0;
                foreach (var op in operations) {
                    SplashScreen.SetOperationLabel (op.Item2);
                    SplashScreen.SetTotalBar ((int) (a * count));
                    op.Item1 ();
                    count++;
                }

                SplashScreen.SetOperationLabel ("Ready");
                SplashScreen.SetTotalBar (1000);

                dataLoaded = true;
            }
        }

        private void LoadPlugins () {
            var manager = Program.Data.PluginManager;
            
            // Load the actual plugins
            List<string> files;
            if (Directory.Exists (Program.Data.Paths.PluginsDir))
                files = new List<string> (Directory.GetFiles (Program.Data.Paths.PluginsDir, "*.gzideplugin", SearchOption.TopDirectoryOnly));
            else
                files = new List<string> ();

            // Load the core "plugin" first
            files.Insert (0, Path.Combine (Program.Data.Paths.ProgDir, "CorePlugin.gzideplugin"));

            double a = 1000.0 / files.Count;
            int count = 0;
            foreach (string file in files) {
                SplashScreen.SetWorkLabel (Path.GetFileNameWithoutExtension (file));
                SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, (int) (a * count));

                manager.LoadPlugin (file);

                count++;
            }

            SplashScreen.SetWorkLabel ("");
            SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, 0);
        }

        private void LoadProjectTypes () {
            var manager = Program.Data.PluginManager;

            double a = 1000.0 / manager.Plugins.Count;
            int count = 0;
            foreach (var plug in manager.Plugins) {
                SplashScreen.SetWorkLabel (String.Format ("From {0}", plug.Name));
                SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, (int) (a * count));

                Program.Data.LoadProjectTypes (plug);

                count++;
            }

            SplashScreen.SetWorkLabel ("");
            SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, 0);
        }

        private void LoadSyntaxHighlighters () {
            var manager = Program.Data.PluginManager;

            double a = 1000.0 / manager.Plugins.Count;
            int count = 0;
            foreach (var plug in manager.Plugins) {
                SplashScreen.SetWorkLabel (String.Format ("From {0}", plug.Name));
                SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, (int) (a * count));

                Program.Data.LoadSyntaxHighlighters (plug);

                count++;
            }

            SplashScreen.SetWorkLabel ("");
            SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, 0);
        }

        private void LoadTemplates () {
            string templatesFolder = Path.Combine (Program.Data.Paths.DataDir, @".\Templates");
            List<string> files;
            if (Directory.Exists (templatesFolder))
                files = new List<string> (Directory.GetFiles (templatesFolder, "*.gzidetemplate", SearchOption.TopDirectoryOnly));
            else
                files = new List<string> ();

            double a = 1000.0 / files.Count;
            int count = 0;
            foreach (string file in files) {
                SplashScreen.SetWorkLabel (Path.GetFileNameWithoutExtension (file));
                SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, (int) (a * count));

                Program.Data.LoadTemplate (file);

                count++;
            }

            SplashScreen.SetWorkLabel ("");
            SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, 0);
        }

        private void LoadIDEThemes () {

        }

        private void LoadEditorThemes () {
            using (var ms = new MemoryStream (Properties.Resources.ScintillaDarkTheme)) {
                var theme = ScintillaTheme.Load (ms);
                Program.Data.EditorThemes.Add (theme);
                EditorTheme = theme;
            }

            string themesFolder = Path.Combine (Program.Data.Paths.DataDir, @".\Themes\Editor");

            List<string> files;
            if (Directory.Exists (themesFolder))
                files = new List<string> (Directory.GetFiles (themesFolder, "*.gzideeditortheme", SearchOption.TopDirectoryOnly));
            else
                files = new List<string> ();

            double a = 1000.0 / files.Count;
            int count = 0;
            foreach (string file in files) {
                SplashScreen.SetWorkLabel (Path.GetFileNameWithoutExtension (file));
                SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, (int) (a * count));

                var theme = ScintillaTheme.Load (file);
                Program.Data.EditorThemes.Add (theme);

                count++;
            }

            SplashScreen.SetWorkLabel ("");
            SplashScreen.SetWorkBar (SplashScreen.WorkBarState.Continuous, 0);
        }

        private void InitializeMainForm () {
            openFileDialog.Filter = Constants.FileTypes;
            saveFileDialog.Filter = Constants.FileTypes;

            // Workspace explorer
            projExpl = new ProjectExplorerWindow (this);
            projExpl.Show (mainDockPanel, DockState.DockRightAutoHide);

            // Error list
            errorList = new ErrorList (this);
            errorList.Show (mainDockPanel, DockState.DockBottomAutoHide);

            // Syntax list
            if (Program.Data.SyntaxHighlighters.Count > 0) {
                List<ToolStripItem> menuItems = new List<ToolStripItem> (Program.Data.SyntaxHighlighters.Count);

                foreach (var kvp in Program.Data.SyntaxHighlighters) {
                    var lang = kvp.Key;

                    var menuItem = new ToolStripMenuItem (lang);
                    menuItem.Tag = kvp.Value;
                    menuItem.Click += SetSyntaxMenuItem_Click;
                    menuItems.Add (menuItem);
                }

                var comparer = new NaturalComparer ();
                menuItems.Sort (new Comparison<ToolStripItem> ((x, y) => comparer.Compare (x.Text, y.Text)));

                viewSyntax_MenuItem.DropDownItems.AddRange (menuItems.ToArray ());
            }
        }

        private void SetSyntaxMenuItem_Click (object sender, EventArgs e) {
            var menuItem = (sender as ToolStripItem);
            var document = (GetActiveDocument () as TextEditorWindow);

            if (menuItem == null || document == null)
                return;

            var highlighterType = (menuItem.Tag as Type);

            if (highlighterType == null || highlighterType.IsAssignableFrom (typeof (SyntaxHighlighter)))
                return;

            document.Highlighter = (Activator.CreateInstance ((menuItem.Tag as Type)) as SyntaxHighlighter);
        }

        #endregion

        #region ================== Functions

        /// <summary>
        /// Sets the status label's text
        /// </summary>
        /// <param name="newText">The text to set the label to.</param>
        public void SetStatusLabel (string newText) {
            if (newText is null)
                throw new ArgumentNullException ("newText");

            status_StatusLabel.Text = newText;
        }

        /// <summary>
        /// Gets the currently active document window/panel/tab.
        /// </summary>
        /// <returns>The active document as a DockContent -or- null.</returns>
        public DockContent GetActiveDocument () {
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
        public bool OpenWorkspace (string wspPath) {
            if (wspPath is null)
                throw new ArgumentNullException ("wspPath");
            if (String.IsNullOrWhiteSpace (wspPath))
                throw new ArgumentException ("Workspace path cannot be empty or whitespace.", "wspPath");

            CurWorkspace = WorkspaceData.Load (wspPath);
            
            return !(CurWorkspace is null);
        }

        /// <summary>
        /// Opens a workspace.
        /// </summary>
        /// <param name="wspPath">The workspace to be opened.</param>
        /// <returns>Returns true if the workspace was opened successfully.</returns>
        public bool OpenWorkspace (WorkspaceData wsp) {
            if (wsp is null)
                throw new ArgumentNullException ("wsp");

            CurWorkspace = wsp;

            return !(CurWorkspace is null);
        }

        #endregion

        #region Workspace saving



        #endregion

        /// <summary>
        /// Opens the specified file in a window
        /// </summary>
        /// <param name="filePath">The file to open</param>
        /// <param name="proj">The project containing the file</param>
        /// <returns>Returns a bool indicating whether the file was opened successfully. Returns false if it failed to read the file or the file is already open.</returns>
        public bool OpenFileWindow (string filePath, ProjectData proj, out TextEditorWindow fileForm) {
            if (!(filePath is null)) {
                filePath = System.IO.Path.GetFullPath (filePath);

                foreach (TextEditorWindow ff in fileForms) {
                    if (ff.FilePath == filePath) {
                        fileForm = ff;
                        return false;
                    }
                }
            }

            TextEditorWindow newFile = TextEditorWindow.OpenFile (this, filePath, proj);

            if (newFile is null) {
                fileForm = null;
                return false;
            }

            if (!String.IsNullOrWhiteSpace (filePath)) {
                newFile.TabText = System.IO.Path.GetFileName (filePath);
                newFile.Text = filePath;
            } else
                newFile.TabText = newFile.Text = "New file";
            newFile.FormClosed += FileForm_FormClosed;

            fileForms.Add (newFile);

            newFile.Show (mainDockPanel, DockState.Document);
            newFile.Focus ();

            fileForm = newFile;

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
            OpenFileWindow (null, null, out _);
        }

        private void FileNewProject_MenuItem_Click (object sender, EventArgs e) {
            var mode = NewProjectWindow.NewProjType.NewWorkspace;

            if (!(CurWorkspace is null))
                mode = NewProjectWindow.NewProjType.NewProject;

            using (var newProjWin = new NewProjectWindow (this, mode)) {
                newProjWin.ShowDialog ();

                if (newProjWin.DialogResult == DialogResult.OK) {
                    OpenWorkspace (newProjWin.NewWorkspace);
                }
            }
        }

        private void FileOpenFile_MenuItem_Click (object sender, EventArgs e) {
            openFileDialog.Title = "Open file";

            if (openFileDialog.ShowDialog () == DialogResult.OK) {
                foreach (string path in openFileDialog.FileNames) {

                    if (String.IsNullOrWhiteSpace (path))
                        continue;

                    OpenFileWindow (path, null, out _);
                }
            }
        }

        private void FileOpenProject_MenuItem_Click (object sender, EventArgs e) {
            if (openWorkspaceDialog.ShowDialog () == DialogResult.OK) {
                OpenWorkspace (openWorkspaceDialog.FileName);
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

            if (!(doc is null)) {
                doc.Close ();

                if (doc.GetType () == typeof (TextEditorWindow))
                    doc.Dispose ();
            }
        }

        private void FileExit_MenuItem_Click (object sender, EventArgs e) {
            this.Close ();
        }

        #endregion

        #region Edit menu

        private void SettingToolStripMenuItem_Click (object sender, EventArgs e) {
            var settingsWindow = new SettingsWindow ();
            settingsWindow.ShowDialog ();
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

        private void ViewErrorList_MenuItem_Click (object sender, EventArgs e) {
            if (errorList.Visible)
                errorList.Hide ();
            else
                errorList.Show ();

            ((ToolStripMenuItem) sender).Checked = errorList.Visible;
        }

        #endregion

        #region Help menu

        private void HelpAbout_MenuItem_Click (object sender, EventArgs e) {
            using (var aboutBox = new AboutBox ()) {
                aboutBox.ShowDialog ();
            }
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
    }
}
