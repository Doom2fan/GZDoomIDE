//
// BaseDirectory.cs
//
// Copyright (c) 2015 David Lechner <david@lechnology.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Linq;

namespace GZDoomIDE.Freedesktop.Xdg
{
    /// <summary>
    /// XDG Base directories.
    /// </summary>
    /// <remarks>
    /// Based on http://standards.freedesktop.org/basedir-spec/basedir-spec-0.8.html.
    /// </remarks>
    static class BaseDirectory
    {
        /// <summary>
        /// Gets the directory where user-specific data files are stored.
        /// </summary>
        /// <value>The full path of the directory.</value>
        /// <remarks>
        /// Uses the <c>XDG_DATA_HOME</c> environment variable if set and
        /// non-empty or <c>$HOME/.local/share</c> otherwise.
        /// </remarks>
        public static string DataHome {
            get {
                return GetDir ("XDG_DATA_HOME",Path.Combine (
                    Environment.GetEnvironmentVariable ("HOME"),
                    ".local",
                    "share"));
            }
        }

        /// <summary>
        /// Gets the directory where user-specific config files are stored.
        /// </summary>
        /// <value>The the full path of the directory.</value>
        /// <remarks>
        /// Uses the <c>XDG_CONFIG_HOME</c> environmet variable if set and
        /// non-empty or <c>$HOME/.config</c> otherwise.
        /// </remarks>
        public static string ConfigHome {
            get {
                return GetDir ("XDG_CONFIG_HOME", Path.Combine (
                    Environment.GetEnvironmentVariable ("HOME"),
                    ".config"));
            }
        }

        /// <summary>
        /// Gets a list of directories to search for data files in addition to
        /// <see cref="DataHome"/>.
        /// </summary>
        /// <value>The data directories.</value>
        /// <remarks>
        /// Uses the <c>XDG_DATA_DIRS</c> environment variable if set and
        /// non-empty or <c>/usr/local/share</c>, <c>/usr/share</c> otherwise;
        public static string[] DataDirs {
            get {
                return GetDirs ("XDG_DATA_DIRS", new [] {
                    Path.Combine (
                        Path.DirectorySeparatorChar.ToString (),
                        "usr", "local", "share"),
                    Path.Combine (
                        Path.DirectorySeparatorChar.ToString (),
                        "usr", "share")
                });
            }
        }

        /// <summary>
        /// Gets a list of directories to search for configuration files in
        /// addition to <see cref="ConfigHome"/>.
        /// </summary>
        /// <value>The configuration directories.</value>
        /// <remarks>
        /// Uses the <c>XDG_CONFIG_DIRS</c> environment variable if set and
        /// non-empty or <c>/etc/xdg</c> otherwise.
        /// </remarks>
        public static string[] ConfigDirs {
            get {
                return GetDirs ("XDG_CONFIG_DIRS", new [] {
                    Path.Combine (
                        Path.DirectorySeparatorChar.ToString (),
                        "etc", "xdg")
                });
            }
        }

        /// <summary>
        /// Gets the path for user-specific non-essential data files.
        /// </summary>
        /// <value>The full path of the directory.</value>
        /// <remarks>
        /// Uses the <c>XDG_CACHE_HOME</c> environment variable if set and
        /// non-empty or <c>$HOME/.cache</c> otherwise.
        /// </remarks>
        public static string CacheHome {
            get {
                return GetDir ("XDG_CACHE_HOME", Path.Combine (
                    Environment.GetEnvironmentVariable ("HOME"),
                    ".cache"));
            }
        }

        public static string RuntimeDir {
            get {
                // TODO: need to figure out default directory.
                return GetDir ("XDG_RUNTIME_DIR", null);
            }
        }

        /// <summary>
        /// Searches for the file specified by <paramref name="relativePath"/>
        /// in <see cref="DataHome"/> and <see cref="DataDirs"/>.
        /// </summary>
        /// <returns>The absolute path if the file exists or <c>null</c> otherwise.</returns>
        /// <param name="relativePath">The path of the file relative to the base data directory.</param>
        public static string FindDataFile (string relativePath)
        {
            foreach (var dir in new [] { DataHome }.Union (DataDirs)) {
                var path = Path.Combine (dir, relativePath);
                if (File.Exists (path)) {
                    return Path.GetFullPath (path);
                }
            }
            return null;
        }

        /// <summary>
        /// Searches for the directory specified by <paramref name="relativePath"/>
        /// in <see cref="DataHome"/> and <see cref="DataDirs"/>.
        /// </summary>
        /// <returns>The absolute path if the directory exists or <c>null</c> otherwise.</returns>
        /// <param name="relativePath">The path of the directory relative to the base data directory.</param>
        public static string FindDataDirectory (string relativePath)
        {
            foreach (var dir in new [] { DataHome }.Union (DataDirs)) {
                var path = Path.Combine (dir, relativePath);
                if (Directory.Exists (path)) {
                    return Path.GetFullPath (path);
                }
            }
            return null;
        }

        /// <summary>
        /// Searches for the file specified by <paramref name="relativePath"/>
        /// in <see cref="ConfigHome"/> and <see cref="ConfigDirs"/>.
        /// </summary>
        /// <returns>The absolute path if the file exists or <c>null</c> otherwise.</returns>
        /// <param name="relativePath">The path of the file relative to the base data directory.</param>
        public static string FindConfigFile (string relativePath)
        {
            foreach (var dir in new [] { ConfigHome }.Union (ConfigDirs)) {
                var path = Path.Combine (dir, relativePath);
                if (File.Exists (path)) {
                    return Path.GetFullPath (path);
                }
            }
            return null;
        }

        /// <summary>
        /// Searches for the directory specified by <paramref name="relativePath"/>
        /// in <see cref="ConfigHome"/> and <see cref="ConfigDirs"/>.
        /// </summary>
        /// <returns>The absolute path if the directory exists or <c>null</c> otherwise.</returns>
        /// <param name="relativePath">The path of the directory relative to the base data directory.</param>
        public static string FindConfigDirectory (string relativePath)
        {
            foreach (var dir in new [] { ConfigHome }.Union (ConfigDirs)) {
                var path = Path.Combine (dir, relativePath);
                if (Directory.Exists (path)) {
                    return Path.GetFullPath (path);
                }
            }
            return null;
        }

        static string GetDir (string envVar, string defaultValue)
        {
            var dir = Environment.GetEnvironmentVariable (envVar);
            if (string.IsNullOrWhiteSpace (dir) || !Path.IsPathRooted (dir)) {
                return defaultValue;
            }
            return dir;
        }

        static string[] GetDirs (string envVar, params string[] defaultValues)
        {
            var dirs = Environment.GetEnvironmentVariable (envVar)
                                  ?.Split (Path.PathSeparator).ToList ();
            if (dirs != null) {
                foreach (var dir in dirs.ToArray ()) {
                    if (!Path.IsPathRooted (dir)) {
                        dirs.Remove (dir);
                    }
                }
            }
            if (dirs == null || !dirs.Any ()) {
                return defaultValues;
            }
            return dirs?.ToArray ();
        }
    }
}
