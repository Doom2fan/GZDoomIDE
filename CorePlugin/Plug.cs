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

using GZDoomIDE;
using GZDoomIDE.Data;
using GZDoomIDE.Plugin;
using System;

#endregion

namespace CorePlugin {
    public class IDEPlug : Plug {
        public override SemVer MinimumIDEVersion => new SemVer (Constants.MajorVersion, Constants.MinorVersion, Constants.PatchVersion);
        public override SemVer MaximumIDEVersion => new SemVer (Constants.MajorVersion, Constants.MinorVersion, Constants.PatchVersion);

        public IDEPlug () : base () {
        }

        /*public override void TextEditor_Insert (TextEditorWindow window, Scintilla editor, ModificationEventArgs e) {
            if (CheckIfZScript (window.FilePath, window.Project)) {
                ParseZScript (editor.Text, window.FilePath, window.Project);
            }
        }*/

        bool CheckIfZScript (string filePath, ProjectData project) {
            if (String.IsNullOrWhiteSpace (filePath))
                return false;
            
            return System.IO.Path.GetExtension (filePath).Equals (".zs", System.StringComparison.OrdinalIgnoreCase);
        }

        void ParseZScript (string code, string filePath, ProjectData project) {
            /*parser.FailMessage = string.Empty;
            parser.Parse (code);

            MainWindow.CurFileErrors.Errors.Clear ();
            if (parser.FailMessage != string.Empty)
                MainWindow.CurFileErrors.Errors.Add (new IDEError (ErrorType.Error, parser.FailMessage, (project != null ? project.Name : "")));*/
        }
    }
}
