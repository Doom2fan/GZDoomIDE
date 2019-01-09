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
using GZDoomIDE.Windows;
using ScintillaNET;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace CorePlugin {
    public class IDEPlug : Plug {
        public override SemVer MinimumIDEVersion => new SemVer (Constants.MajorVersion, Constants.MinorVersion, Constants.PatchVersion);
        public override SemVer MaximumIDEVersion => new SemVer (Constants.MajorVersion, Constants.MinorVersion, Constants.PatchVersion);

        public IDEPlug () : base () {
        }

        public bool CheckIfZScript (TextEditorWindow editor) {
            return editor.Highlighter is ZScript.ZScriptHighlighter;
        }

        Thread parseThread;
        public override void TextEditor_Insert (TextEditorWindow window, Scintilla editor, ModificationEventArgs e) {
            if (CheckIfZScript (window)) {
                var parser = (window.Highlighter as ZScript.ZScriptHighlighter).Parser;

                Program.MainWindow.CurFileErrors.Errors.DisableChangeCallback ();

                editor.IndicatorCurrent = ZScript.ZScriptHighlighter.Indicators_SyntaxError;
                editor.IndicatorClearRange (0, editor.TextLength);
                Program.MainWindow.CurFileErrors.Errors.Clear ();

                if (parseThread != null) {
                    if (parseThread.IsAlive) {
                        parseThread.Abort ();
                        parseThread.Join ();
                    }
                }

                var ts = new ThreadStart (() => ParseZScript (parser, window, editor));
                parseThread = new Thread (ts);

                parseThread.Start ();
            }
        }

        internal bool ParseZScript (ZScript.Parsing parser, TextEditorWindow window, Scintilla editor) {
            try {
                parser.Reset ();
                parser.Parse (window, editor, GetText (editor), window.Project, window.FilePath);

                UpdateErrorList ();
            } catch (ThreadAbortException) { }

            return true;
        }

        delegate string GetTextDelegate (Scintilla editor);
        internal string GetText (Scintilla editor) {
            if (editor.InvokeRequired) {
                return (string) editor.Invoke (new GetTextDelegate (GetText), new object [] { editor });
            }

            return editor.Text;
        }

        internal void UpdateErrorList () {
            if (Program.MainWindow.InvokeRequired) {
                Program.MainWindow.Invoke (new Action (UpdateErrorList));
                return;
            }

            Program.MainWindow.CurFileErrors.Errors.EnableChangeCallback ();
            Program.MainWindow.CurFileErrors.Errors.CallChangeCallback ();
        }
    }
}
