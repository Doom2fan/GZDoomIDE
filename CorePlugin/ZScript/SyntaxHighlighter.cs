using GZDoomIDE.Data;
using GZDoomIDE.Editor;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.ZScript {
    [SyntaxHighlighter ("ZScript")]
    public class ZScriptHighlighter : SyntaxHighlighter {
        public override void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project) { }

        public override void DoSetup (Scintilla editor, ProjectData project) {
            editor.Lexer = Lexer.Null;
            editor.UpdateUI += Editor_UpdateUI;
        }

        public override void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project) {

        }

        public override void Finalize (Scintilla editor, ProjectData project) {
            editor.UpdateUI -= Editor_UpdateUI;
        }

        protected static bool IsBrace (int c) {
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

        int lastCaretPos = 0;

        private void Editor_UpdateUI (object sender, UpdateUIEventArgs e) {
            var editor = (sender as Scintilla);

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
}
