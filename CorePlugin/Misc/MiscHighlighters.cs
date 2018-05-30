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
using ScintillaNET;
using System.Text.RegularExpressions;

#endregion

namespace CorePlugin.Misc {
    public abstract class BaseHighlighter : SyntaxHighlighter {
        /// <summary>
        /// Determines whether a character is a brace.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns>A bool indicating whether the character is a brace.</returns>
        protected virtual bool IsBrace (int c) { return false; }

        protected int lastCaretPos = 0;

        /// <summary>
        /// Matches braces, using IsBrace to check whether a character is a brace.
        /// </summary>
        /// <param name="editor">An instance of Scintilla.</param>
        protected void BraceMatching (Scintilla editor) {
            // Has the caret changed position?
            var caretPos = editor.CurrentPosition;
            if (lastCaretPos != caretPos) {
                lastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace (editor.GetCharAt (caretPos - 1)))
                    bracePos1 = (caretPos - 1);
                else if (IsBrace (editor.GetCharAt (caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0) {
                    // Find the matching brace
                    bracePos2 = editor.BraceMatch (bracePos1);

                    if (bracePos2 == Scintilla.InvalidPosition)
                        editor.BraceBadLight (bracePos1);
                    else
                        editor.BraceHighlight (bracePos1, bracePos2);
                } else {
                    // Turn off brace matching
                    editor.BraceHighlight (Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                }
            }
        }
    }

    [SyntaxHighlighter ("Plain text")] // Plain text
    public class PlainTextHighlighter : SyntaxHighlighter {
        public override void DoSetup (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Container;

            editor.StartStyling (0);
            editor.SetStyling (editor.TextLength, 0);
        }

        public override void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project) {
            theme.Default.SetScintillaStyle (editor.Styles [0]);
        }

        public override void Finalize (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Null;
        }

        public override void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project) { }
    }

    [SyntaxHighlighter ("C++")]
    public class CppHighlighter : BaseHighlighter {
        public override void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project) { }

        public override void DoSetup (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Cpp;
            editor.UpdateUI += Editor_UpdateUI;
            editor.InsertCheck += Editor_InsertCheck;
            editor.CharAdded += Editor_CharAdded;

            editor.SetKeywords (0, @"Primary keywords and identifiers alignas alignof and and_eq asm auto bitand bitor bool break case catch char char16_t char32_t class compl const constexpr const_cast continue decltype default delete do double dynamic_cast else enum explicit export extern false final float for friend goto if inline int long mutable namespace new noexcept not not_eq nullptr operator or or_eq override private protected public register reinterpret_cast return short signed sizeof static static_assert static_cast struct switch template this thread_local throw true try typedef typeid typename union unsigned using virtual void volatile wchar_t while xor xor_eq");
        }

        public override void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project) {
            theme.Default.SetScintillaStyle (editor.Styles [Style.Cpp.Default]);
            theme.Number.SetScintillaStyle (editor.Styles [Style.Cpp.Number]);

            theme.CommentBlock.SetScintillaStyle (editor.Styles [Style.Cpp.Comment]);
            theme.CommentLine.SetScintillaStyle (editor.Styles [Style.Cpp.CommentLine]);
            theme.CommentDocBlock.SetScintillaStyle (editor.Styles [Style.Cpp.CommentDoc]);
            theme.CommentDocLine.SetScintillaStyle (editor.Styles [Style.Cpp.CommentLineDoc]);

            theme.Literal.SetScintillaStyle (editor.Styles [Style.Cpp.UserLiteral]);

            theme.Keyword.SetScintillaStyle (editor.Styles [Style.Cpp.Word]);
            theme.Keyword.SetScintillaStyle (editor.Styles [Style.Cpp.Word2]);
            theme.Identifier.SetScintillaStyle (editor.Styles [Style.Cpp.Identifier]);
            theme.Operator.SetScintillaStyle (editor.Styles [Style.Cpp.Operator]);

            theme.String.SetScintillaStyle (editor.Styles [Style.Cpp.String]);
            theme.String.SetScintillaStyle (editor.Styles [Style.Cpp.StringEol]);
            theme.StringVerbatim.SetScintillaStyle (editor.Styles [Style.Cpp.Verbatim]);
            theme.Character.SetScintillaStyle (editor.Styles [Style.Cpp.Character]);
            theme.Regex.SetScintillaStyle (editor.Styles [Style.Cpp.Regex]);

            theme.Preprocessor.SetScintillaStyle (editor.Styles [Style.Cpp.Preprocessor]);

            theme.Default.SetScintillaStyle (editor.Styles [Style.Cpp.Uuid]);
            theme.EscapeSequence.SetScintillaStyle (editor.Styles [Style.Cpp.EscapeSequence]);
        }

        public override void Finalize (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Null;
            editor.UpdateUI -= Editor_UpdateUI;
            editor.InsertCheck -= Editor_InsertCheck;
            editor.CharAdded -= Editor_CharAdded;
        }

        protected override bool IsBrace (int c) {
            switch (c) {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }

        private void Editor_UpdateUI (object sender, UpdateUIEventArgs e) {
            BraceMatching (sender as Scintilla);
        }

        private void Editor_InsertCheck (object sender, InsertCheckEventArgs e) {
            var editor = (sender as Scintilla);

            if (e.Text.EndsWith ("\r") || e.Text.EndsWith ("\n")) {
                int startPos = editor.Lines [editor.LineFromPosition (editor.CurrentPosition)].Position;
                int endPos = e.Position;
                string curLineText = editor.GetTextRange (startPos, (endPos - startPos));

                Match indent = Regex.Match (curLineText, @"^[ \t]*");
                e.Text = (e.Text + indent.Value);
                if (Regex.IsMatch (curLineText, @"{\s*$"))
                    e.Text = (e.Text + "    ");
            }
        }

        private void Editor_CharAdded (object sender, CharAddedEventArgs e) {
            var editor = (sender as Scintilla);

            if (e.Char == 125) {
                int curLine = editor.LineFromPosition (editor.CurrentPosition);
                if (editor.Lines [curLine].Text.Trim () == "}")
                    editor.Lines [curLine].Indentation -= editor.TabWidth;
            }
        }
    }

    [SyntaxHighlighter ("INI")]
    public class IniHighlighter : BaseHighlighter {
        public override void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project) { }

        public override void DoSetup (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Properties;
            editor.UpdateUI += Editor_UpdateUI;
        }

        public override void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project) {
            theme.Default.SetScintillaStyle      (editor.Styles [Style.Properties.Default]);
            theme.CommentLine.SetScintillaStyle  (editor.Styles [Style.Properties.Comment]);
            theme.Preprocessor.SetScintillaStyle (editor.Styles [Style.Properties.Section]);
            theme.Operator.SetScintillaStyle     (editor.Styles [Style.Properties.Assignment]);
            theme.Keyword.SetScintillaStyle      (editor.Styles [Style.Properties.Key]);
        }

        public override void Finalize (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Null;
            editor.UpdateUI -= Editor_UpdateUI;
        }

        protected override bool IsBrace (int c) {
            switch (c) {
                case '[':
                case ']':
                    return true;
            }

            return false;
        }

        private void Editor_UpdateUI (object sender, UpdateUIEventArgs e) {
            BraceMatching (sender as Scintilla);
        }
    }

    [SyntaxHighlighter ("Json")]
    public class JsonHighlighter : BaseHighlighter {
        public override void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project) { }

        public override void DoSetup (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Json;
            editor.UpdateUI += Editor_UpdateUI;
        }

        public override void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project) {

        }

        public override void Finalize (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Null;
            editor.UpdateUI -= Editor_UpdateUI;
        }

        protected override bool IsBrace (int c) {
            switch (c) {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }

        private void Editor_UpdateUI (object sender, UpdateUIEventArgs e) {
            BraceMatching (sender as Scintilla);
        }
    }

    [SyntaxHighlighter ("Batch")]
    public class BatchHighlighter : SyntaxHighlighter {
        public override void DoSetup (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Batch;
        }

        public override void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project) {

        }

        public override void Finalize (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Null;
        }

        public override void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project) { }
    }
}
