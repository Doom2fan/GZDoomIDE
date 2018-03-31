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

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE {
    public struct PreprocessorMacro {
        public string Input { get; set; }
        public string Output { get; set; }
        public char TokenChar { get; set; }
        public bool IsRegex { get; set; }

        /// <summary>
        /// Creates a preprocessor macro from a RegEx.
        /// </summary>
        /// <param name="input">The RegEx to match.</param>
        /// <param name="output">The string to replace matches with.</param>
        /// <returns>The new macro.</returns>
        public static PreprocessorMacro AsRegex (string input, string output) {
            var ret = new PreprocessorMacro ();

            ret.Input = input;
            ret.Output = output;
            ret.IsRegex = true;

            return ret;
        }

        /// <summary>
        /// Creates a preprocessor macro a string and a delimiter.
        /// </summary>
        /// <param name="input">The string to match.</param>
        /// <param name="output">The string to replace matches with.</param>
        /// <returns>The new macro.</returns>
        public static PreprocessorMacro AsToken (char tokenChar, string input, string output) {
            var ret = new PreprocessorMacro ();

            ret.TokenChar = tokenChar;
            ret.Input = input;
            ret.Output = output;
            ret.IsRegex = false;

            return ret;
        }
    }

    static class Utils {
        public static void ThemeToolstrips (VisualStudioToolStripExtender vsToolStripExtender, ToolStrip [] toolStrips, ThemeBase theme) {
            foreach (ToolStrip ts in toolStrips)
                vsToolStripExtender.SetStyle (ts, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);
        }

        /// <summary>
        /// Preprocesses a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="macros">The preprocessor macros.</param>
        /// <returns>The preprocessed macro.</returns>
        public static string Preprocess (string input, PreprocessorMacro [] macros) {
            if (input is null)
                throw new ArgumentNullException ("input");
            if (macros is null)
                throw new ArgumentNullException ("macros");

            string output = input;

            foreach (var macro in macros) {
                if (!macro.IsRegex) {
                    string regex = String.Format ("(?<!\\x{0:X2})\\x{0:X2}{1}", (byte) macro.TokenChar, macro.Input);

                    output = Regex.Replace (output, regex, macro.Output, RegexOptions.ECMAScript);
                } else
                    output = Regex.Replace (output, macro.Input, macro.Output, RegexOptions.ECMAScript);
            }

            return output;
        }
    }
}
