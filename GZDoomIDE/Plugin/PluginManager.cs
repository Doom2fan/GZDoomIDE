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

#endregion

namespace GZDoomIDE.Plugin {
    internal sealed class PluginManager {
        #region ================== Properties

        public List<PluginData> Plugins { get; private set; }

        public bool IsDisposed { get; private set; }

        #endregion

        #region ================== Constructor / Disposer

        public PluginManager () {
            Plugins = new List<PluginData> ();

            // We have no destructor
            GC.SuppressFinalize (this);
        }

        public void Dispose () {
            if (!IsDisposed) {
                foreach (PluginData p in Plugins)
                    p.Dispose ();
                
                IsDisposed = true;
            }
        }

        #endregion

        #region ================== Instance methods

        public void LoadAllPlugins () {
            // Load the core "plugin" first
            LoadPlugin (Path.Combine (Constants.ProgDir, "CorePlugin.DLL"));

            if (!Directory.Exists (Constants.PluginsDir))
                return;

            // Load the actual plugins
            List<string> files = new List<string> (Directory.GetFiles (Constants.PluginsDir, "*.dll", SearchOption.TopDirectoryOnly));

            foreach (string file in files) {
                LoadPlugin (file);
            }
        }

        /// <summary>
        /// Loads the specified DLL as a plugin.
        /// </summary>
        /// <param name="file">The DLL to load.</param>
        public void LoadPlugin (string file) {
            PluginData p;

            try {
                p = new PluginData (file);
            } catch (InvalidProgramException) {
                p = null;
            }

            // Continue if no errors
            if (!(p is null) && (!p.IsDisposed)) {
                // Add to plugins list
                Plugins.Add (p);

                Program.Data.LoadProjectTypes (p);

                // Plugin is now initialized
                p.Plug.OnInitialize ();
            }
        }

        #endregion
    }
}
