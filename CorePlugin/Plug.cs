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

using CorePlugin.ZScript;
using GZDoomIDE;
using GZDoomIDE.Data;
using GZDoomIDE.Plugin;
using GZDoomIDE.Windows;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        Thread parserThread;
        Thread starterThread;
        public override void TextEditor_Insert (TextEditorWindow window, Scintilla editor, ModificationEventArgs e) {
            if (!CheckIfZScript (window))
                return;

            if (e.LinesAdded > 1 || e.Text.Length > 1)
                StartParse (window, editor);
        }

        internal void StartParse (TextEditorWindow window, Scintilla editor) {
            var parser = (window.Highlighter as ZScriptHighlighter).Parser;

            if (starterThread != null && starterThread.IsAlive)
                starterThread.Abort ();

            var state = new ParseState {
                parser = parser,
                window = window,
                editor = editor,
            };

            var ts = new ThreadStart (() => StartParsingThread (state));
            starterThread = new Thread (ts);
            starterThread.Name = "ZScript parser starter thread";

            starterThread.Start ();
        }

        internal class ParseState {
            internal Parsing parser;
            internal TextEditorWindow window;
            internal Scintilla editor;
        }

        System.Timers.Timer parseTimer;
        internal void StartParsingThread (ParseState state) {
            if (parserThread != null) {
                if (parserThread.IsAlive) {
                    parserThread.Abort ();
                    parserThread.Join ();
                }
            }

            if (parseTimer == null) {
                parseTimer = new System.Timers.Timer ();
                parseTimer.Elapsed += ParseTimer_Elapsed;
            }
        }

        private void ParseTimer_Elapsed (object sender, System.Timers.ElapsedEventArgs e) {
            var ts = new ThreadStart (() => ParseZScript (state.parser, GetText (), state.window, state.editor));
            parserThread = new Thread (ts);
            parserThread.Name = "ZScript parser thread";

            parserThread.Start ();
        }

        delegate void AddErrorsDelegate (MainWindow mainWindow, Scintilla editor, List<(IDEError, int, int)> error);
        internal void AddErrors (MainWindow mainWindow, Scintilla editor, List<(IDEError err, int pos, int len)> errors) {
            if (mainWindow.InvokeRequired) {
                mainWindow.Invoke (new AddErrorsDelegate (AddErrors), new object [] { mainWindow, editor, errors });
                return;
            }

            editor.IndicatorCurrent = ZScriptHighlighter.Indicators_SyntaxError;
            editor.IndicatorClearRange (0, editor.TextLength);

            PrepareErrorList ();
            foreach (var error in errors) {
                mainWindow.CurFileErrors.Errors.Add (error.err);
                editor.IndicatorCurrent = ZScriptHighlighter.Indicators_SyntaxError;
                editor.IndicatorFillRange (error.pos, error.len);
            }
            UpdateErrorList ();
        }

        internal bool ParseZScript (ZScript.Parsing parser, string code, TextEditorWindow window, Scintilla editor) {
            ParseResult result = null;
            try {
                parser.Reset ();
                result = parser.Parse (code);

                var errors = new List<(IDEError err, int pos, int len)> ();

                foreach (var error in result.Errors) {
                    string failMessage = null;

                    switch (error.Type) {
                        case ParseError.ErrorType.UnexpectedToken:
                            failMessage = String.Format ("Unexpected token \"{0}\". Expected {1}.", error.TokenText, error.ExpectedTokens);
                            break;

                        case ParseError.ErrorType.UnexpectedEOF:
                            failMessage = "Unexpected EOF";
                            break;

                        case ParseError.ErrorType.UnknownError:
                        default:
                            failMessage = "Unknown error";
                            break;
                    }

                    var err = new IDEError (ErrorType.Error, failMessage, (window.Project?.Name ?? ""));
                    err.LineNum = error.Line;
                    err.ColumnNum = error.Column;
                    err.Position = error.Position;
                    err.File = window.FilePath;
                    err.Window = window;

                    errors.Add ((err, error.Position, error.TokenText.Length));
                }

                AddErrors (MainWindow, editor, errors);
            } catch (ThreadAbortException) {
                return false;
            }

            return true;
        }

        delegate string GetTextDelegate (Scintilla editor);
        internal string GetText (Scintilla editor) {
            if (editor.InvokeRequired) {
                return (string) editor.Invoke (new GetTextDelegate (GetText), new object [] { editor });
            }

            return editor.Text;
        }

        internal void PrepareErrorList () {
            if (Program.MainWindow.InvokeRequired) {
                Program.MainWindow.Invoke (new Action (PrepareErrorList));
                return;
            }

            Program.MainWindow.CurFileErrors.Errors.DisableChangeCallback ();
            Program.MainWindow.CurFileErrors.Errors.Clear ();
        }

        internal void UpdateErrorList () {
            if (Program.MainWindow.InvokeRequired) {
                Program.MainWindow.Invoke (new Action (UpdateErrorList));
                return;
            }

            Program.MainWindow.CurFileErrors.Errors.EnableChangeCallback ();
            //Program.MainWindow.CurFileErrors.Errors.CallChangeCallback ();
        }
    }
}
