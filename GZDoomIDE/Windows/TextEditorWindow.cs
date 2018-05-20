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
using GZDoomIDE.Plugin;
using ScintillaNET;
using System;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class TextEditorWindow : DockContent {
        protected MainWindow parentWindow;

        public string FilePath { get; internal set; }
        public ProjectData Project { get; internal set; }

        protected TextEditorWindow () {
            InitializeComponent ();

            Program.Data.Themer.ApplyTheme (this);
        }

        /// <summary>
        /// Creates a new FileForm.
        /// </summary>
        /// <param name="filePath">The path to the file opened in this form. Null means a file form that doesn't have a file yet.</param>
        public static TextEditorWindow OpenFile (MainWindow parentWindow, string filePath, ProjectData proj = null) {
            if (parentWindow is null)
                throw new ArgumentNullException ("parentWindow");
            if (filePath != null && String.IsNullOrWhiteSpace (filePath))
                throw new ArgumentException ("File path cannot be empty or whitespace.", "filePath");
            
            string fileText = "";

            if (!(filePath is null)) {
                if (String.IsNullOrWhiteSpace (filePath))
                    return null;

                try {
                    fileText = System.IO.File.ReadAllText (filePath);
                } catch {
                    return null;
                }
            }

            TextEditorWindow ff = new TextEditorWindow ();
            ff.parentWindow = parentWindow;
            ff.FilePath = filePath;
            ff.Project = proj;
            ff.scintillaControl.Text = fileText;
            ff.scintillaControl.EmptyUndoBuffer ();
            ff.ScintillaControl_TextChanged (ff.scintillaControl, EventArgs.Empty);

            return ff;
        }

        private int maxLineNumberCharLength;
        private void ScintillaControl_TextChanged (object sender, EventArgs e) {

            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = scintillaControl.Lines.Count.ToString ().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            scintillaControl.Margins [0].Width = scintillaControl.TextWidth (Style.LineNumber, new string ('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void ScintillaControl_SavePointLeft (object sender, EventArgs e) {
            if (!String.IsNullOrWhiteSpace (FilePath)) {
                this.Text = String.Concat (FilePath, '*');
                this.TabText = String.Concat (System.IO.Path.GetFileName (FilePath), '*');
            } else
                this.Text = this.TabText = "New file*";
        }

        private void ScintillaControl_SavePointReached (object sender, EventArgs e) {
            if (!String.IsNullOrWhiteSpace (FilePath)) {
                this.Text = FilePath;
                this.TabText = System.IO.Path.GetFileName (FilePath);
            } else
                this.Text = this.TabText = "New file";
        }

        #region ================== Events

        #region ================== Text modified

        private void ScintillaControl_BeforeDelete (object sender, BeforeModificationEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnBeforeDelete (this, scintillaControl, e);
        }

        private void ScintillaControl_BeforeInsert (object sender, BeforeModificationEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnBeforeInsert (this, scintillaControl, e);
        }

        private void ScintillaControl_InsertCheck (object sender, InsertCheckEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnInsertCheck (this, scintillaControl, e);
        }

        private void ScintillaControl_Insert (object sender, ModificationEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnInsert (this, scintillaControl, e);
        }

        private void ScintillaControl_Delete (object sender, ModificationEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnDelete (this, scintillaControl, e);
        }

        private void ScintillaControl_CharAdded (object sender, CharAddedEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnCharAdded (this, scintillaControl, e);
        }

        #endregion

        #region ================== Misc Events

        private void ScintillaControl_ChangeAnnotation (object sender, ChangeAnnotationEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnChangeAnnotation (this, scintillaControl, e);
        }

        private void ScintillaControl_DwellStart (object sender, DwellEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnDwellStart (this, scintillaControl, e);
        }

        private void ScintillaControl_DwellEnd (object sender, DwellEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnDwellEnd (this, scintillaControl, e);
        }

        private void ScintillaControl_UpdateUI (object sender, UpdateUIEventArgs e) {
            Program.Data.PluginManager.TextEditor_OnUpdateUI (this, scintillaControl, e);
        }

        private void ScintillaControl_ZoomChanged (object sender, EventArgs e) {
            Program.Data.PluginManager.TextEditor_OnZoomChanged (this, scintillaControl, e);
        }

        #endregion

        #endregion
    }
}
