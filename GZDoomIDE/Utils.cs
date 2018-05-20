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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

    public static class Utils {
        /// <summary>
        /// Adds an item to a sorted list.
        /// </summary>
        /// <typeparam name="T">The list's type.</typeparam>
        /// <param name="this">The list to add the item to.</param>
        /// <param name="item">The item to add.</param>
        /// <param name="comparer">The comparer to use.</param>
        public static void AddSorted<T> (this List<T> @this, T item, IComparer<T> comparer) {
            if (@this.Count == 0) {
                @this.Add (item);
                return;
            } else if (comparer.Compare (@this [@this.Count - 1], item) <= 0) {
                @this.Add (item);
                return;
            } else if (comparer.Compare (@this [0], item) >= 0) {
                @this.Insert (0, item);
                return;
            }

            int index = @this.BinarySearch (item, comparer);

            if (index < 0)
                index = ~index;

            @this.Insert (index, item);
        }

        /// <summary>
        /// Finds the correct index to insert an item into a List at while keeping it sorted.
        /// </summary>
        /// <typeparam name="T">The list's type.</typeparam>
        /// <param name="this">The list to use.</param>
        /// <param name="item">The item to be checked.</param>
        /// <param name="comparer">The comparer to use.</param>
        public static int FindSortedAddIndex<T> (this List<T> @this, T item, IComparer<T> comparer) {
            if (@this.Count == 0)
                return 0;
            else if (comparer.Compare (@this [@this.Count - 1], item) <= 0)
                return @this.Count;
            else if (comparer.Compare (@this [0], item) >= 0)
                return 0;

            int index = @this.BinarySearch (item, comparer);

            if (index < 0)
                index = ~index;

            return index;
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

        /// <summary>
        /// Converts a full path to a relative path.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="basePath">The base path for the relative path.</param>
        /// <returns>A relative path.</returns>
        public static string GetRelativePath (string fullPath, string basePath) {
            if (!IsFullPath (fullPath))
                return fullPath;

            if (!basePath.EndsWith (@"\"))
                basePath += @"\";

            var baseUri = new Uri (basePath);
            var fullUri = new Uri (fullPath);

            var relativeUri = baseUri.MakeRelativeUri (fullUri);

            return relativeUri.ToString ().Replace ('/', '\\');
        }

        /// <summary>
        /// Converts a relative path to a full path.
        /// </summary>
        /// <param name="relPath">The relative path.</param>
        /// <param name="basePath">The base path of the relative path</param>
        /// <returns>A full path.</returns>
        public static string GetAbsolutePath (string relPath, string basePath) {
            if (IsFullPath (relPath))
                return relPath;

            if (!basePath.EndsWith (@"\"))
                basePath += @"\";

            return System.IO.Path.Combine (basePath, relPath);
        }

        /// <summary>
        /// Checks if the specified path is a full path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>Returns true if the path is a full path.</returns>
        public static bool IsFullPath (string path) {
            return Path.IsPathRooted (path) &&
                !(Path.GetPathRoot (path).Equals (Path.DirectorySeparatorChar.ToString (), StringComparison.Ordinal));
        }

        /// <summary>
        /// Checks if the specified path is valid.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>Returns true if the path is valid.</returns>
        public static bool IsPathValid (string path) {
            return !String.IsNullOrWhiteSpace (path) && path.IndexOfAny (Path.GetInvalidPathChars ().ToArray ()) == -1;
        }

        /// <summary>
        /// Creates a uniquely named temporary directory and returns the full path to that directory.
        /// </summary>
        /// <returns>The full path of the temporary directory.</returns>
        public static string GetTemporaryDirectory () {
            string tempFolderPath = Path.GetTempPath ();

            string tempDir = null;

            do {
                tempDir = Path.Combine (tempFolderPath, Path.GetFileNameWithoutExtension (Path.GetRandomFileName ()));
            } while (Directory.Exists (tempDir));

            Directory.CreateDirectory (tempDir);

            return tempDir;
        }
    }

    /// <summary>
    /// String comparison using natural alphabetical ordering.
    /// </summary>
    public class NaturalComparer : Comparer<string>, IDisposable {
        private Dictionary<string, string []> table;

        public NaturalComparer () {
            table = new Dictionary<string, string []> ();
        }

        public void Dispose () {
            table.Clear ();
            table = null;
        }

        public override int Compare (string x, string y) {
            if (x == y)
                return 0;

            string [] x1, y1;
            if (!table.TryGetValue (x, out x1)) {
                x1 = Regex.Split (x.Replace (" ", ""), "([0-9]+)");
                table.Add (x, x1);
            }
            if (!table.TryGetValue (y, out y1)) {
                y1 = Regex.Split (y.Replace (" ", ""), "([0-9]+)");
                table.Add (y, y1);
            }

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
                if (x1 [i] != y1 [i])
                    return PartCompare (x1 [i], y1 [i]);

            if (y1.Length > x1.Length)
                return 1;
            else if (x1.Length > y1.Length)
                return -1;
            else
                return 0;
        }

        private static int PartCompare (string left, string right) {
            int x, y;
            if (!int.TryParse (left, out x))
                return left.CompareTo (right);

            if (!int.TryParse (right, out y))
                return left.CompareTo (right);

            return x.CompareTo (y);
        }
    }
}
