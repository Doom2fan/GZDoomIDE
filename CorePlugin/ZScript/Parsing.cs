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
    struct ParseError {
        public enum ErrorType {
            UnknownError,
            UnexpectedToken,
            UnexpectedEOF,
        }

        public ErrorType Type { get; set; }

        public string TokenText { get; set; }
        public string ExpectedTokens { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }
        public int Position { get; set; }
    }
    class ParseResult {
        public List<ParseError> Errors { get; private set; }

        public ParseResult () {
            Errors = new List<ParseError> ();
        }
    }

    class Parsing {
        internal ScintillaTheme theme;

        LALRParser parser;

        protected MainWindow MainWindow { get => GZDoomIDE.Program.MainWindow; }

        public Parsing () {
            Reset ();
        }

        public void Reset () {
            using (var stream = new MemoryStream (GZDoomIDE.Properties.Resources.ZScriptGrammar)) {
                var reader = new CGTReader (stream);
                parser = reader.CreateNewParser ();
            }

            parser.OnParseError += Parser_OnParseError;
            parser.OnTokenRead += Parser_OnTokenRead;
        }

        private void Parser_OnTokenRead (LALRParser parser, TokenReadEventArgs args) {
            switch (args.Token.Symbol.Name) {
                case "Identifier":
                    args.Token.UserObject = new ZSIdentifier (args.Token.Text);
                    break;

                case "DecLiteral":
                    args.Token.UserObject = new ZSIntLiteral (ZSIntLiteral.NumberType.DecLiteral, args.Token.Text);
                    break;
                case "HexLiteral":
                    args.Token.UserObject = new ZSIntLiteral (ZSIntLiteral.NumberType.HexLiteral, args.Token.Text);
                    break;

                case "RealLiteral":
                case "RealLiteralAlt":
                    args.Token.UserObject = new ZSFloatLiteral (args.Token.Text);
                    break;
            }
        }

        private ParseResult result;

        public ParseResult Parse (string code) {
            result = new ParseResult ();

            var parseTree = parser.Parse (code);



            var ret = result;
            result = null;

            return ret;
        }

        bool parsingState;
        private void Parser_OnParseError (LALRParser parser, ParseErrorEventArgs args) {
            ParseError err;

            if (args.UnexpectedToken.Symbol == SymbolCollection.EOF) {
                err = new ParseError ();

                err.Type = ParseError.ErrorType.UnexpectedEOF;

                err.TokenText = "";
                err.ExpectedTokens = "";

                err.Line = args.UnexpectedToken.Location.LineNr;
                err.Column = args.UnexpectedToken.Location.ColumnNr;
                err.Position = args.UnexpectedToken.Location.Position;

                result.Errors.Add (err);

                args.Continue = ContinueMode.Stop;

                return;
            }

            foreach (Symbol expTok in args.ExpectedTokens) {
                switch (expTok?.Name) {
                    case "SpriteName":
                        if (args.UnexpectedToken.Text.Length != 4)
                            break;

                        args.NextToken = new TerminalToken ((SymbolTerminal) expTok, args.UnexpectedToken.Text, args.UnexpectedToken.Location);
                        args.Continue = ContinueMode.Skip;
                        parsingState = true;

                        return;

                    case "StateFrame":
                        if (!parsingState)
                            break;

                        args.NextToken = new TerminalToken ((SymbolTerminal) expTok, args.UnexpectedToken.Text, args.UnexpectedToken.Location);
                        args.Continue = ContinueMode.Skip;
                        parsingState = false;

                        return;
                }
            }

            err = new ParseError ();

            err.Type = ParseError.ErrorType.UnexpectedToken;

            err.TokenText = args.UnexpectedToken.Text;
            err.ExpectedTokens = String.Format ("{0}", args.ExpectedTokens);

            err.Line = args.UnexpectedToken.Location.LineNr;
            err.Column = args.UnexpectedToken.Location.ColumnNr;
            err.Position = args.UnexpectedToken.Location.Position;

            result.Errors.Add (err);

            args.Continue = ContinueMode.Skip;
        }
    }
}
