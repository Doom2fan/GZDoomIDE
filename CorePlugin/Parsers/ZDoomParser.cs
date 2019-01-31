using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.Parsers.ZDoomParsers {
    /// <summary>
    /// Tokens used in (G)ZDoom's parser.
    /// </summary>
    public enum ZDoomTokens {
        Identifier,
        Number,
        String,
        Bang,
        Hash,
        Dollar,
        Percent,
        And,
        ParenO,
        ParenC,
        Asterisk,
        Plus,
        Comma,
        Minus,
        Period,
        Slash,
        Colon,
        Semicolon,
        LesserThan,
        Equals,
        GreaterThan,
        Question,
        At,
        BrackO,
        Backslash,
        BrackC,
        Caret,
        Backquote,
        BraceO,
        VerticalBar,
        BraceC,
        Tilde,
        EOF,
        Unknown,
    }

    public class ZDoomToken {
        public Dictionary<ZDoomTokens, string> Names { get; } = new Dictionary<ZDoomTokens, string> {
            { ZDoomTokens.Identifier,  "an identifier"    },
            { ZDoomTokens.Number,      "a number"         },
            { ZDoomTokens.String,      "a string"         },
            { ZDoomTokens.Bang,        "'!'"              },
            { ZDoomTokens.Hash,        "'#'"              },
            { ZDoomTokens.Dollar,      "'$'"              },
            { ZDoomTokens.Percent,     "'%'"              },
            { ZDoomTokens.And,         "'&'"              },
            { ZDoomTokens.ParenO,      "'('"              },
            { ZDoomTokens.ParenC,      "')'"              },
            { ZDoomTokens.Asterisk,    "'*'"              },
            { ZDoomTokens.Plus,        "'+'"              },
            { ZDoomTokens.Comma,       "','"              },
            { ZDoomTokens.Minus,       "'-'"              },
            { ZDoomTokens.Period,      "'.'"              },
            { ZDoomTokens.Slash,       "'/'"              },
            { ZDoomTokens.Colon,       "':'"              },
            { ZDoomTokens.Semicolon,   "';'"              },
            { ZDoomTokens.LesserThan,  "'<'"              },
            { ZDoomTokens.Equals,      "'='"              },
            { ZDoomTokens.GreaterThan, "'>'"              },
            { ZDoomTokens.Question,    "'?'"              },
            { ZDoomTokens.At,          "'@'"              },
            { ZDoomTokens.BrackO,      "'['"              },
            { ZDoomTokens.Backslash,   "'\\'"             },
            { ZDoomTokens.BrackC,      "']'"              },
            { ZDoomTokens.Caret,       "'^'"              },
            { ZDoomTokens.Backquote,   "'`'"              },
            { ZDoomTokens.BraceO,      "'{'"              },
            { ZDoomTokens.VerticalBar, "'|'"              },
            { ZDoomTokens.BraceC,      "'}'"              },
            { ZDoomTokens.Tilde,       "'~'"              },
            { ZDoomTokens.EOF,         "EOF"              },
            { ZDoomTokens.Unknown,     "an unknown token" },
        };
        public ZDoomTokens Type { get; set; }
        public int PosLine { get; set; }
        public int PosChar { get; set; }
        public string Text { get; set; }

        public string GetName () {
            return Names [Type];
        }
    }

    /// <summary>
    /// An exception in the (G)ZDoom lumps parser.
    /// </summary>
    public class ZDoomParserException : Exception {
        public ZDoomParserException (string message = "", Exception innerException = null) :
            base (message, innerException) {

        }
    }

    public static class StreamExtensions {
        internal static int Peek (this Stream stream) {
            var curPos = stream.Position;
            var ret = stream.ReadByte ();

            stream.Seek (curPos, SeekOrigin.Begin);

            return ret;
        }
    }

    /// <summary>
    /// A parser for (G)ZDoom's lumps
    /// </summary>
    public abstract class ZDoomParser {
        /// <summary>
        /// The data being parsed.
        /// </summary>
        protected Stream FileData { get; set; }

        /// <summary>
        /// Current character position.
        /// </summary>
        protected int PosChar { get; set; }

        /// <summary>
        /// Current line being read.
        /// </summary>
        protected int PosLine { get; set; }

        protected void Initialize (Stream data) {
            if (!data.CanRead)
                throw new ArgumentException ("Stream can't be read", "data");
            if (!data.CanSeek)
                throw new ArgumentException ("Stream can't be seeked", "data");

            FileData = data;
            PosChar = 0;
            PosLine = 0;
        }

        /// <summary>
        /// Checks if a character is whitespace.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>A bool indicating whether the character is a whitespace.</returns>
        protected bool IsWhitespace (char c) {
            switch (c) {
                case '\t': // Horizontal tab
                case '\n': // Line feed
                case '\v': // Vertical tab
                case '\f': // Form feed
                case '\r': // Carriage return
                case ' ' : // Space
                case '\0': // NUL
                    return true;
                default:
                    return false;
            }
        }

        protected char ReadChar () {
            char ret = (char) FileData.ReadByte ();

            if (ret == '\n') {
                PosLine++;
                PosChar = 1;
            } else
                PosChar++;

            return ret;
        }

        /// <summary>
        /// Skips whitespace characters.
        /// </summary>
        protected void SkipWhitespace () {
            while (IsWhitespace ((char) FileData.Peek ()))
                ReadChar ();
        }

        /// <summary>
        /// Checks if a character is a letter.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>A bool indicating whether the character is a letter.</returns>
        protected bool IsLetter (char c) {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        /// <summary>
        /// Checks if a character is a digit.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>A bool indicating whether the character is a digit.</returns>
        protected bool IsDigit (char c) {
            return (c >= '0' && c <= '9');
        }

        /// <summary>
        /// Checks if a character is a letter or digit.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>A bool indicating whether the character is a letter or digit.</returns>
        protected bool IsLetterOrDigit (char c) {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }

        /// <summary>
        /// Reads the next token without advancing the stream.
        /// </summary>
        /// <returns>The token read.</returns>
        protected ZDoomToken Peek () {
            var curLine = PosLine;
            var curChar = PosChar;
            var streamPos = FileData.Position;

            var ret = Next ();

            PosLine = curLine;
            PosChar = curChar;
            FileData.Seek (streamPos, SeekOrigin.Begin);

            return ret;
        }

        /// <summary>
        /// Reads the next token.
        /// </summary>
        /// <returns>The token read.</returns>
        protected virtual ZDoomToken Next () {
            SkipWhitespace ();
            var tk = new ZDoomToken ();

            // Are we at the end of the stream?
            if (FileData.Peek () == -1) {
                tk.Type = ZDoomTokens.EOF;
                return tk;
            }

            tk.PosLine = PosLine;
            tk.PosChar = PosChar;
            var c = ReadChar ();
            tk.Text = c.ToString ();
            switch (c) {
                case '"':
                case '\'': {
                        tk.Type = ZDoomTokens.String;
                        StringBuilder str = new StringBuilder ();

                        var end = c;

                        do {
                            c = ReadChar ();

                            // Handle escape sequences
                            if (c == '\\') {
                                var peek = (char) FileData.Peek ();

                                switch (peek) {
                                    case '"': // '"'
                                    case '\'': // '''
                                    case '\\': // '\'
                                        str.Append (peek);
                                        ReadChar ();
                                        break;
                                    case 'a': // 'a'
                                    case 'A': // 'a'
                                        str.Append ('\a');
                                        ReadChar ();
                                        break;
                                    case 'n': // 'n'
                                    case 'N': // 'n'
                                        str.Append ('\n');
                                        ReadChar ();
                                        break;
                                    case 'r': // 'r'
                                    case 'R': // 'r'
                                        str.Append ('\r');
                                        ReadChar ();
                                        break;
                                    default: // Ignore unknown sequences
                                        str.Append (c);
                                        break;
                                }
                            } else if (c != end)
                                str.Append (c);
                        } while (c != end);

                        tk.Text = str.ToString ();
                        str = null;
                        break;
                    }

                case '/': {
                        switch (FileData.Peek ()) {
                            case '*': // '/*' - multi-line comment
                                c = ReadChar ();
                                while (true) {
                                    c = ReadChar ();

                                    // Have we encountered "*/"?
                                    if (c == '*' && FileData.Peek () == '/') {
                                        ReadChar ();
                                        break;
                                    }

                                    // Are we at the end of the stream?
                                    if (FileData.Peek () == -1) break;
                                }

                                return Next ();
                            case '/': // '//' - single-line comment
                                do {
                                    // Keep reading until we reach a new line or the end of the stream.
                                    c = ReadChar ();
                                } while (c != '\n' && FileData.Peek () != -1);

                                return Next ();
                        }

                        tk.Type = ZDoomTokens.Slash;
                        break;
                    }

                case '_':
                default: {
                        // Identifiers are made up of alphanumeric characters and underscores.
                        bool readNonDigit = false;

                        StringBuilder str = new StringBuilder ();

                        do {
                            if (!readNonDigit && IsLetter (c) || c == '_')
                                readNonDigit = true;

                            str.Append (c);

                            var peek = (char) FileData.Peek ();
                            if (!IsLetterOrDigit (peek) && peek != '_')
                                break;

                            c = ReadChar ();
                        } while (IsLetterOrDigit (c) || c == '_');

                        tk.Type = readNonDigit ? ZDoomTokens.Identifier : ZDoomTokens.Number;
                        tk.Text = str.ToString ();
                        str = null;

                        break;
                    }

                case '!':  tk.Type = ZDoomTokens.Bang;        break;
                case '#':  tk.Type = ZDoomTokens.Hash;        break;
                case '$':  tk.Type = ZDoomTokens.Dollar;      break;
                case '%':  tk.Type = ZDoomTokens.Percent;     break;
                case '&':  tk.Type = ZDoomTokens.And;         break;
                case '(':  tk.Type = ZDoomTokens.ParenO;      break;
                case ')':  tk.Type = ZDoomTokens.ParenC;      break;
                case '*':  tk.Type = ZDoomTokens.Asterisk;    break;
                case '+':  tk.Type = ZDoomTokens.Plus;        break;
                case ',':  tk.Type = ZDoomTokens.Comma;       break;
                case '-':  tk.Type = ZDoomTokens.Minus;       break;
                case '.':  tk.Type = ZDoomTokens.Period;      break;
                case ':':  tk.Type = ZDoomTokens.Colon;       break;
                case ';':  tk.Type = ZDoomTokens.Semicolon;   break;
                case '<':  tk.Type = ZDoomTokens.LesserThan;    break;
                case '=':  tk.Type = ZDoomTokens.Equals;      break;
                case '>':  tk.Type = ZDoomTokens.GreaterThan;    break;
                case '?':  tk.Type = ZDoomTokens.Question;    break;
                case '@':  tk.Type = ZDoomTokens.At;          break;
                case '[':  tk.Type = ZDoomTokens.BrackO;      break;
                case '\\': tk.Type = ZDoomTokens.Backslash;   break;
                case ']':  tk.Type = ZDoomTokens.BrackC;      break;
                case '^':  tk.Type = ZDoomTokens.Caret;       break;
                case '`':  tk.Type = ZDoomTokens.Backquote;   break;
                case '{':  tk.Type = ZDoomTokens.BraceO;      break;
                case '|':  tk.Type = ZDoomTokens.VerticalBar; break;
                case '}':  tk.Type = ZDoomTokens.BraceC;      break;
                case '~':  tk.Type = ZDoomTokens.Tilde;       break;
            }

            return tk;
        }
    }
}
