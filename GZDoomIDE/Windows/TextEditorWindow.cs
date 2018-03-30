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

using ScintillaNET;
using System;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class TextEditorWindow : DockContent {
        public string FilePath { get; internal set; }

        protected TextEditorWindow () {
            InitializeComponent ();
        }

        /// <summary>
        /// Creates a new FileForm.
        /// </summary>
        /// <param name="filePath">The path to the file opened in this form. Null means a file form that doesn't have a file yet.</param>
        public static TextEditorWindow OpenFile (string filePath) {
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
            ff.FilePath = filePath;
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
    }
}
