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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Resources;

#endregion

namespace GZDoomIDE.Plugin {
    internal sealed class PluginCompileData {
        public string [] SourceFiles { get; set; }
        public string [] References { get; set; }
        public string [] ResXFiles { get; set; }

        private PluginCompileData () { }

        /// <summary>
        /// Load's a plugin's compilation data.
        /// </summary>
        /// <param name="path">The path to the json file containing the compilation data.</param>
        /// <returns>An instance of PluginCompileData.</returns>
        internal static PluginCompileData Load (string path) {
            PluginCompileData ret = null;

            using (var reader = new JsonTextReader (new StreamReader (path))) {
                ret = Program.Data.JsonSerializer.Deserialize<PluginCompileData> (reader);

                reader.CloseInput = true;
                reader.Close ();
            }

            return ret;
        }
    }

    public class PluginData : IDisposable {
        #region ================== Properties

        /// <summary>
        /// The plugin's assembly
        /// </summary>
        public Assembly Asm { get; protected set; }
        /// <summary>
        /// The plug
        /// </summary>
        public Plug Plug { get; protected set; }
        /// <summary>
        /// The plugin's name
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Disposing
        /// </summary>
        public bool IsDisposed { get; protected set; }

        #endregion

        #region ================== Constructor / Disposer

        protected PluginData () { }

        public static PluginData Load (string path) {
            var plugin = new PluginData ();

            plugin.Name = Path.GetFileNameWithoutExtension (path);
            Program.Logger.WriteLine ("Loading plugin \"{0}\" from \"{1}\"...", plugin.Name, Path.GetFileName (path));

            string compilationPath = null;
            try {
                compilationPath = Utils.GetTemporaryDirectory ();

                var fz = new ICSharpCode.SharpZipLib.Zip.FastZip ();
                fz.ExtractZip (path, compilationPath, "");

                if (!File.Exists (Path.Combine (compilationPath, "plugin.json"))) {
                    PrintError_CantLoadPlugin (plugin, " No plugin.json file.");
                    throw new InvalidProgramException ();
                }

                var compileData = PluginCompileData.Load (Path.Combine (compilationPath, "plugin.json"));
                if (compileData == null) {
                    PrintError_CantLoadPlugin (plugin, " Error loading plugin.json.");
                    throw new InvalidProgramException ();
                }

                List<SyntaxTree> syntaxTrees = new List<SyntaxTree> ();
                List<MetadataReference> references = new List<MetadataReference> ();
                List<ResourceDescription> manifestResources = new List<ResourceDescription> ();

                // Add source files
                foreach (string file in compileData.SourceFiles) {
                    string filePath = Path.Combine (compilationPath, file);

                    if (!File.Exists (filePath)) {
                        Program.DebugLogger.WriteLine ("Plugin compilation warning: Could not find source file \"{0}\" in plugin \"{1}\".", file, plugin.Name);
                        continue;
                    }

                    using (var stream = new FileStream (filePath, FileMode.Open))
                    using (var reader = new StreamReader (stream)) {
                        var tree = CSharpSyntaxTree.ParseText (Microsoft.CodeAnalysis.Text.SourceText.From (stream, canBeEmbedded: true), path: filePath);
                        syntaxTrees.Add (tree);
                    }
                }

                // Add references
                references.Add (MetadataReference.CreateFromFile (Assembly.GetEntryAssembly ().Location));
                foreach (var reference in Assembly.GetEntryAssembly ().GetReferencedAssemblies ()) {
                    references.Add (MetadataReference.CreateFromFile (Assembly.ReflectionOnlyLoad (reference.FullName).Location));
                }
                foreach (string reference in compileData.References) {
                    MetadataReference metaRef = null;

                    if (reference.StartsWith ("dll://")) {
                        string refPath = reference.Remove (0, "dll://".Length);

                        refPath = Path.ChangeExtension (refPath, !Utils.IsUnix ? ".dll" : ".so");

                        metaRef = MetadataReference.CreateFromFile (refPath);
                    } else
                        metaRef = MetadataReference.CreateFromFile (Assembly.ReflectionOnlyLoad (reference).Location);

                    if (metaRef != null)
                        references.Add (metaRef);
                }

                foreach (string file in compileData.ResXFiles) {
                    string filePath = Path.Combine (compilationPath, file);

                    if (!File.Exists (filePath)) {
                        Program.DebugLogger.WriteLine ("Plugin compilation warning: Could not find resource file \"{0}\" in plugin \"{1}\".", file, plugin.Name);
                        continue;
                    }

                    var reader = new ResXResourceReader (filePath);
                    reader.BasePath = Path.GetDirectoryName (filePath);
                    reader.UseResXDataNodes = true;

                    foreach (System.Collections.DictionaryEntry de in reader) {
                        //var res = new ResourceDescription ((de.Key as string), () => { return File.OpenRead ((de.Value as ResXDataNode).FileRef.FileName); }, true);
                        //manifestResources.Add (res);
                    }
                }

                var compilationOptions = new CSharpCompilationOptions (
                    OutputKind.DynamicallyLinkedLibrary,
                    moduleName: plugin.Name,
                    optimizationLevel: (Program.Data.IsDebugBuild ? OptimizationLevel.Debug : OptimizationLevel.Release),
                    platform: Program.Data.IDERoslynPlatform
                );
                var compilation = CSharpCompilation.Create (Path.ChangeExtension (plugin.Name, ".dll"), syntaxTrees, references, compilationOptions);

                string dllPath = Path.Combine (Program.Data.Paths.TempDir, Path.ChangeExtension (plugin.Name, ".dll"));
                string pdbPath = Path.Combine (Program.Data.Paths.TempDir, Path.ChangeExtension (plugin.Name, ".pdb"));

                var results = compilation.Emit (dllPath, pdbPath, manifestResources: manifestResources);

                if (!results.Success) {
                    Program.DebugLogger.WriteLine ("Could not compile plugin \"{0}\".", plugin.Name);
                    foreach (var err in results.Diagnostics)
                        Program.DebugLogger.WriteLine ("\t{0}", err.ToString ());

                    plugin.Dispose ();
                    plugin = null;

                    return null;
                }

                Program.DebugLogger.WriteLine ("Compiled plugin \"{0}\" successfully.", plugin.Name);
                foreach (var err in results.Diagnostics)
                    Program.DebugLogger.WriteLine ("\t{0}", err.ToString ());

                // Load assembly
                plugin.Asm = Assembly.LoadFile (dllPath);
            } catch (InvalidProgramException) {
                throw;
            } finally {
                if (compilationPath != null)
                    Directory.Delete (compilationPath, true);
            }

            Type plugClass = plugin.FindSingleClass (typeof (Plug));

            if (plugClass is null) {
                PrintError_InvalidPlugin (plugin, "The plugin has no class that inherits from Plug.");
                throw new InvalidProgramException ();
            }

            if (plugin.FindClasses (typeof (Plug)).Length > 1) {
                PrintError_InvalidPlugin (plugin, "The plugin has more than one class that inherits from Plug.");
                throw new InvalidProgramException ();
            }

            // Create an instance of the plug
            plugin.Plug = plugin.CreateObject<Plug> (plugClass);
            plugin.Plug.Plugin = plugin;

            if (!(plugin.Plug.MinimumIDEVersion is null) && plugin.Plug.MinimumIDEVersion > Constants.Version) {
                PrintError_CantLoadPlugin (plugin, String.Format (" The plugin's minimum version is {0}, you are using {1}.", plugin.Plug.MinimumIDEVersion, Constants.Version));
                throw new InvalidProgramException ();
            } else if (!(plugin.Plug.MaximumIDEVersion is null) && plugin.Plug.MaximumIDEVersion < Constants.Version) {
                PrintError_CantLoadPlugin (plugin, String.Format (" The plugin's maximum version is {0}, you are using {1}.", plugin.Plug.MaximumIDEVersion, Constants.Version));
                throw new InvalidProgramException ();
            }

            GC.SuppressFinalize (plugin);

            return plugin;
        }
        protected static void PrintError_CantLoadPlugin (PluginData plugin, string text) {
            string err = String.Format ("Could not load plugin \"{0}\".{1}", plugin.Name, text);

            Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
            Program.Logger.WriteLine (err);
        }
        protected static void PrintError_InvalidPlugin (PluginData plugin, string text) {
            string err = String.Format ("Invalid plugin \"{0}\".", plugin.Name);

            Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
            Program.Logger.WriteLine ("Plugin \"{0}\" is invalid: {1}", plugin.Name, text);
        }

        public void Dispose () {
            if (!IsDisposed) {
                if (Plug != null) {
                    Plug.Dispose ();
                    Plug = null;
                }
                Asm = null;

                IsDisposed = true;
            }
        }

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Creates an instance of the specified class with the specified arguments.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="t">The class to instantiate.</param>
        /// <param name="args">The arguments to pass to the constructor.</param>
        /// <returns>An instance of the class.</returns>
        public T CreateObject<T> (Type t, params object [] args) {
            try {
                return (T) Asm.CreateInstance (t.FullName, false, BindingFlags.Default, null, args, System.Globalization.CultureInfo.CurrentCulture, new object [0]);
            } catch (TargetInvocationException e) {
                string err = String.Format ("Failed to create class instance \"{0}\" from plugin \"{1}\".", t.Name, Name);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, String.Format ("{0} See the error log for more details", err)));
                Program.Logger.WriteLine ("{0} {1} at target: {2}\nStacktrace: {3}", err, e.InnerException.GetType ().Name, e.InnerException.Message, e.InnerException.StackTrace.Trim ());

                return default (T);
            } catch (Exception e) {
                string err = String.Format ("Failed to create class instance \"{0}\" from plugin \"{1}\".", t.Name, Name);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, String.Format ("{0} See the error log for more details", err)));
                Program.Logger.WriteLine ("{0} {1}: {2}\nStacktrace: {3}", err, e.GetType ().Name, e.Message, e.StackTrace.Trim ());

                return default (T);
            }
        }

        /// <summary>
        /// Find all the classes that inherit from a given type.
        /// </summary>
        /// <param name="t">The base type to search for.</param>
        /// <returns>An array of Types.</returns>
        public Type [] FindClasses (Type baseType) {
            List<Type> ret = new List<Type> ();

            Type [] types = Asm.GetExportedTypes ();

            foreach (Type t in types) {
                // Compare types
                if (baseType.IsAssignableFrom (t))
                    ret.Add (t);
            }

            return ret.ToArray ();
        }

        /// <summary>
        /// Finds a single class that inherits from a given type.
        /// </summary>
        /// <param name="t">The base type to search for.</param>
        /// <returns>A Type.</returns>
        public Type FindSingleClass (Type t) {
            Type [] types = FindClasses (t);
            return (types.Length > 0 ? types [0] : null);
        }

        #endregion
    }
}
