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
using GZDoomIDE.Editor;
using GZDoomIDE.Plugin;
using ScintillaNET;
using System;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class TextEditorWindow : DockContent {
        protected MainWindow parentWindow;

        public string FilePath { get; internal set; }
        public ProjectData Project { get; internal set; }

        private SyntaxHighlighter _highlighter = null;
        public SyntaxHighlighter Highlighter {
            get => _highlighter;
            internal set {
                if (_highlighter != null)
                    _highlighter.Finalize (scintillaControl, Project);

                _highlighter = value;

                if (_highlighter != null) {
                    _highlighter.DoSetup (scintillaControl, Project);
                    _highlighter.ApplyTheme (scintillaControl, parentWindow.EditorTheme, Project);
                }

                ApplyEditorTheme ();
            }
        }

        protected TextEditorWindow (MainWindow parent) {
            parentWindow = parent;
            MdiParent = parent;

            InitializeComponent ();

            Program.Data.Themer.ApplyTheme (this);

            parentWindow.EditorThemeChanged += EditorThemeChanged;

            if (Program.Data.SyntaxHighlighters.ContainsKey ("Plain text"))
                Highlighter = (SyntaxHighlighter) Activator.CreateInstance (Program.Data.SyntaxHighlighters ["Plain text"]);
            else
                Highlighter = null;
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

            TextEditorWindow ff = new TextEditorWindow (parentWindow);
            ff.FilePath = filePath;
            ff.Project = proj;
            ff.scintillaControl.Text = fileText;
            ff.scintillaControl.EmptyUndoBuffer ();
            ff.ScintillaControl_TextChanged (ff.scintillaControl, EventArgs.Empty);

            return ff;
        }

        private void EditorThemeChanged (object sender, EventArgs e) { ApplyEditorTheme (); }

        private void ApplyEditorTheme () {
            var theme = parentWindow.EditorTheme;

            // Line numbers
            scintillaControl.Margins [0].BackColor = SystemColors.ControlDarkDark;
            scintillaControl.Margins [0].Type = MarginType.Text;
            scintillaControl.Margins [0].Sensitive = true;
            scintillaControl.Margins [0].Width = 0;
            scintillaControl.Margins [0].Cursor = MarginCursor.ReverseArrow;

            // Line numbers
            scintillaControl.Margins [1].Type = MarginType.Number;
            scintillaControl.Margins [1].Cursor = MarginCursor.ReverseArrow;

            // Code folding
            scintillaControl.SetProperty ("fold", "1");
            scintillaControl.SetProperty ("fold.compact", "0");

            scintillaControl.Margins [2].Type = MarginType.Symbol;
            scintillaControl.Margins [2].Mask = Marker.MaskFolders;
            scintillaControl.Margins [2].Sensitive = true;
            scintillaControl.Margins [2].Width = 20;
            scintillaControl.Margins [2].Cursor = MarginCursor.ReverseArrow;
            scintillaControl.SetFoldMarginColor (true, theme.FoldMargin.Background.Value);
            scintillaControl.SetFoldMarginHighlightColor (true, theme.FoldMargin.Background.Value);

            //scintillaControl.SetFoldFlags (FoldFlags.LineAfterContracted);
            scintillaControl.FoldDisplayTextSetStyle (FoldDisplayText.Boxed);

            // Indentation guides
            scintillaControl.IndentationGuides = IndentView.LookBoth;
            scintillaControl.IndentWidth = 4;

            // Markers
            for (int i = 25; i <= 31; i++) {
                scintillaControl.Markers [i].SetBackColor (theme.FoldMarker.Background.Value);
                scintillaControl.Markers [i].SetForeColor (theme.FoldMarker.Foreground.Value);
            }

            // Configure folding markers with respective symbols
            scintillaControl.Markers [Marker.FolderOpen].Symbol = theme.FoldMarkerFold;
            scintillaControl.Markers [Marker.Folder].Symbol = theme.FoldMarkerUnfold;
            scintillaControl.Markers [Marker.FolderTail].Symbol = theme.FoldMarkerEnd;

            scintillaControl.Markers [Marker.FolderOpenMid].Symbol = theme.FoldMarkerNestedFold;
            scintillaControl.Markers [Marker.FolderEnd].Symbol = theme.FoldMarkerNestedUnfold;
            scintillaControl.Markers [Marker.FolderMidTail].Symbol = theme.FoldMarkerNestedEnd;

            scintillaControl.Markers [Marker.FolderSub].Symbol = theme.FoldMarkerLine;

            // Colors
            theme.Default.SetScintillaStyle          (scintillaControl.Styles [Style.Default]);
            theme.LineNumber?.SetScintillaStyle      (scintillaControl.Styles [Style.LineNumber]);
            theme.IndentGuide?.SetScintillaStyle     (scintillaControl.Styles [Style.IndentGuide]);
            theme.BraceLight?.SetScintillaStyle      (scintillaControl.Styles [Style.BraceLight]);
            theme.BraceBad?.SetScintillaStyle        (scintillaControl.Styles [Style.BraceBad]);
            theme.FoldDisplayText?.SetScintillaStyle (scintillaControl.Styles [Style.FoldDisplayText]);

            // Selection color
            if (theme.Selection != null) {
                scintillaControl.SetSelectionBackColor (true, theme.Selection.Background.Value);
                scintillaControl.SetSelectionForeColor (true, theme.Selection.Foreground.Value);
            } else {
                scintillaControl.SetSelectionBackColor (true, theme.Default.Foreground.Value);
                scintillaControl.SetSelectionForeColor (true, theme.Default.Background.Value);
            }

            // Whitespace color
            if (theme.Whitespace != null) {
                scintillaControl.SetWhitespaceBackColor (true, theme.Whitespace.Background.Value);
                scintillaControl.SetWhitespaceForeColor (true, theme.Whitespace.Foreground.Value);
            } else {
                scintillaControl.SetWhitespaceBackColor (false, Color.Empty);
                scintillaControl.SetWhitespaceForeColor (false, Color.Empty);
            }

            // Caret color
            scintillaControl.CaretForeColor = theme.Caret.Foreground.Value;
            scintillaControl.AdditionalCaretForeColor = theme.AdditionalCarets.Foreground.Value;
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
            scintillaControl.Margins [1].Width = scintillaControl.TextWidth (Style.LineNumber, new string ('9', maxLineNumberCharLength + 1)) + padding;
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

        private void ScintillaControl_StyleNeeded (object sender, StyleNeededEventArgs e) {
            if (Highlighter != null)
                Highlighter.DoHighlighting (scintillaControl, scintillaControl.GetEndStyled (), e.Position, Project);

            Program.Data.PluginManager.TextEditor_OnStyleNeeded (this, scintillaControl, e);
        }

        private void ScintillaControl_ZoomChanged (object sender, EventArgs e) {
            Program.Data.PluginManager.TextEditor_OnZoomChanged (this, scintillaControl, e);
        }

        #endregion

        #endregion
    }
}
