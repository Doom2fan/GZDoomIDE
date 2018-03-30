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

#endregion

namespace GZDoomIDE.Plugin {
    public class Plug : IDisposable {
        #region ================== Properties

        internal PluginData Plugin { get; set; }

        /// <summary>
		/// Indicates if the plugin has been disposed.
		/// </summary>
		public bool IsDisposed { get; protected set; }

        /// <summary>
        /// Override this to return a more descriptive name for your plugin.
        /// Default is the library filename without extension.
        /// </summary>
        public virtual string Name { get { return Plugin.Name; } }
        
        /// <summary>
        /// The plugin's name.
        /// </summary>
        public virtual SemVer Version { get; }

        /// <summary>
        /// The lowest version this plugin works in
        /// </summary>
        public virtual SemVer MinimumIDEVersion { get; }

        /// <summary>
        /// The highest version this plugin works in
        /// </summary>
        public virtual SemVer MaximumIDEVersion { get; } = null;

        #endregion

        #region ================== Constructor / Disposer

        /// <summary>
        /// This is the key link between the IDE core and the plugin.
        /// Every plugin must expose a single class that inherits this class.
        /// <para>
        /// NOTE: Some methods cannot be used in this constructor, because the plugin
        /// is not yet fully initialized. Instead, use the Initialize method to do
        /// your initializations.
        /// </para>
        /// </summary>
        public Plug () {
            // Initialize

            // We have no destructor
            GC.SuppressFinalize (this);
        }

        /// <summary>
        /// This is called by the IDE core when the plugin is being disposed.
        /// </summary>
        public virtual void Dispose () {
            // Not already disposed?
            if (!IsDisposed) {
                // Clean up
                Plugin = null;

                // Done
                IsDisposed = true;
            }
        }

        #endregion

        public virtual void OnInitialize () { }
    }
}
