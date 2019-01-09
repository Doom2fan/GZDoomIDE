using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.calitha.goldparser;
using GZDoomIDE.Data;
using GZDoomIDE.Editor;
using GZDoomIDE.Windows;
using ScintillaNET;

namespace CorePlugin.ZScript {
    class Parsing {
        internal ScintillaTheme theme;

        LALRParser parser;
        TextEditorWindow window;
        Scintilla editor;
        string code;
        ProjectData project;
        string filename;

        protected MainWindow mainWindow { get => GZDoomIDE.Program.MainWindow; }

        public Parsing () {
            Reset ();
        }

        public void Reset () {
            code = null;
            project = null;
            filename = null;
            using (var stream = new MemoryStream (GZDoomIDE.Properties.Resources.ZScriptGrammar)) {
                var reader = new CGTReader (stream);
                parser = reader.CreateNewParser ();
            }

            parser.OnParseError += Parser_OnParseError;
        }

        public void Parse (TextEditorWindow window, Scintilla editor, string code, ProjectData project, string filename) {
            this.window = window; this.editor = editor;
            this.code = code; this.project = project; this.filename = filename;

            parser.Parse (code);
        }

        delegate void SetIndicatorDelegate (int startPos, int length);
        internal void SetIndicator (int startPos, int length) {
            if (editor.InvokeRequired) {
                editor.Invoke (new SetIndicatorDelegate (SetIndicator), new object [] { startPos, length });
                return;
            }

            editor.IndicatorCurrent = ZScriptHighlighter.Indicators_SyntaxError;
            editor.IndicatorFillRange (startPos, length);
        }

        private void Parser_OnParseError (LALRParser parser, ParseErrorEventArgs args) {
            string failMessage = null;
            var expTokenList = new List<Symbol> ();
            
            foreach (Symbol token in args.ExpectedTokens)
                expTokenList.Add (token);

            if (expTokenList.Count > 1)
                failMessage = String.Format ("Unexpected token \"{0}\". Expected one of \"{1}\".", args.UnexpectedToken, args.ExpectedTokens);
            else
                failMessage = String.Format ("Unexpected token \"{0}\". Expected \"{1}\".", args.UnexpectedToken, args.ExpectedTokens);

            AddError (new IDEError (ErrorType.Error, failMessage, (project != null ? project.Name : "")));

            SetIndicator (args.UnexpectedToken.Location.Position, args.UnexpectedToken.Text.Length);

            args.Continue = ContinueMode.Insert;
        }

        delegate void AddErrorDelegate (IDEError error);
        internal void AddError (IDEError error) {
            if (mainWindow.InvokeRequired) {
                mainWindow.Invoke (new AddErrorDelegate (AddError), new object [] { error });
                return;
            }

            mainWindow.CurFileErrors.Errors.Add (error);
        }
    }
}
