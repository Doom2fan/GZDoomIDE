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
using GZDoomIDE.Plugin;
using System;
using System.Collections.Generic;

#endregion

namespace GZDoomIDE {
    public sealed class ProgramData {
        /// <summary>
        /// The plugin manager.
        /// </summary>
        internal PluginManager PluginManager { get; } = new PluginManager ();
        /// <summary>
        /// The loaded project types.
        /// </summary>
        public Type [] ProjectTypes { get; private set; }

        /// <summary>
        /// Loads the project types defined in a plugin.
        /// </summary>
        /// <param name="plugin">The plugin to load the project types from.</param>
        public void LoadProjectTypes (PluginData plugin) {
            List<Type> types;

            if (!(ProjectTypes is null))
                types = new List<Type> (ProjectTypes);
            else
                types = new List<Type> ();

            var classes = plugin.FindClasses (typeof (ProjectType));
            foreach (var newType in classes) {
                if (newType.IsDefined (typeof (ProjectTypeAttribute), false))
                    types.Add (newType);
            }

            ProjectTypes = types.ToArray ();
        }
    }
}
