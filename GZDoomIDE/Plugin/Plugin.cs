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

#endregion

namespace GZDoomIDE.Plugin {
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

        public PluginData (string path) {
            Name = Path.GetFileNameWithoutExtension (path);
            Program.Logger.WriteLine ("Loading plugin \"{0}\" from \"{1}\"...", Name, Path.GetFileName (path));

            try {
                // Load assembly
                Asm = Assembly.LoadFrom (path);
            } catch (Exception) {
                string err = String.Format ("Could not load plugin \"{0}\", the DLL file could not be read.", Name);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
                Program.Logger.WriteLine (err);

                throw new InvalidProgramException ();
            }

            Type plugClass = FindSingleClass (typeof (Plug));

            if (plugClass is null) {
                string err = String.Format ("Invalid plugin \"{0}\".", Name);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
                Program.Logger.WriteLine ("Plugin \"{0}\" is invalid, the DLL file has no Plug.", Name);

                throw new InvalidProgramException ();
            }

            if (FindClasses (typeof (Plug)).Length > 1) {
                string err = String.Format ("Invalid plugin \"{0}\".", Name);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
                Program.Logger.WriteLine ("Plugin \"{0}\" is invalid, the DLL file has more than one Plug.", Name);

                throw new InvalidProgramException ();
            }

            // Create an instance of the plug
            Plug = CreateObject<Plug> (plugClass);
            Plug.Plugin = this;

            if (!(Plug.MinimumIDEVersion is null) && Plug.MinimumIDEVersion > Constants.Version) {
                string err = String.Format ("Could not load plugin \"{0}\". The plugin's minimum version is {1}, you are using {2}.", Name, Plug.MinimumIDEVersion, Constants.Version);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
                Program.Logger.WriteLine (err);

                throw new InvalidProgramException ();
            } else if (!(Plug.MaximumIDEVersion is null) && Plug.MaximumIDEVersion < Constants.Version) {
                string err = String.Format ("Could not load plugin \"{0}\". The plugin's maximum version is {1}, you are using {2}.", Name, Plug.MaximumIDEVersion, Constants.Version);

                Program.PluginErrors.Errors.Add (new IDEError (ErrorType.Error, err));
                Program.Logger.WriteLine (err);

                throw new InvalidProgramException ();
            }

            GC.SuppressFinalize (this);
        }

        public void Dispose () {
            if (!IsDisposed) {
                Plug.Dispose ();
                Plug = null;
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
