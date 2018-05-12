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
using System.IO;

#endregion

namespace GZDoomIDE {
    public sealed class PathsDefs {
        public string ProgDir { get; private set; }
        public string DataDir { get; private set; }
        public string PluginsDir { get; private set; }

        public PathsDefs () {
            ProgDir = Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location);
            DataDir = ProgDir;
            PluginsDir = Path.Combine (DataDir, "/Plugins/");
        }
    }

    public sealed class ProgramData {
        #region ================== Instance members

        /// <summary>
        /// The plugin manager.
        /// </summary>
        internal PluginManager PluginManager { get; } = new PluginManager ();

        /// <summary>
        /// Gets the program's paths.
        /// </summary>
        public PathsDefs Paths { get; } = new PathsDefs ();

        /// <summary>
        /// The loaded project types.
        /// </summary>
        public Dictionary<string, Type> ProjectTypes { get; private set; } = new Dictionary<string, Type> (StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The loaded project templates
        /// </summary>
        public List<ProjectTemplate> ProjectTemplates { get; } = new List<ProjectTemplate> ();

        public Support.WinFormsThemer Themer { get; } = new Support.WinFormsThemer (new Support.Themes.WF_VS2017DarkTheme ());

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Loads the project types defined in a plugin.
        /// </summary>
        /// <param name="plugin">The plugin to load the project types from.</param>
        public void LoadProjectTypes (PluginData plugin) {
            var types = new Dictionary<string, Type> (ProjectTypes, StringComparer.OrdinalIgnoreCase);

            var classes = plugin.FindClasses (typeof (ProjectType));
            foreach (var newType in classes) {
                var attributes = newType.GetCustomAttributes (typeof (ProjectTypeAttribute), false);

                if (attributes.Length == 1)
                    types.Add ((attributes [0] as ProjectTypeAttribute).Name, newType);
            }

            ProjectTypes = types;
        }

        /// <summary>
        /// Loads all of the installed templates.
        /// </summary>
        public void LoadAllTemplates () {
            string [] files = Directory.GetFiles (Path.Combine (Paths.DataDir, @".\Templates"), "*.gzidetemplate", SearchOption.TopDirectoryOnly);

            foreach (string file in files) {
                string filename = Path.GetFileNameWithoutExtension (file);
                ProjectTemplate pt;

                try {
                    pt = LoadTemplate (new FileSource (file));
                } catch (Exception e) {
                    Program.Logger.WriteLine ("Could not load template \"{0}\".", filename);
                    Program.DebugLogger.WriteLine ("Could not load template \"{0}\".\n  {1}: {2}", filename, e.GetType ().Name, e.Message);
                    continue;
                }
                
                if (pt is null) {
                    Program.Logger.WriteLine ("Could not load template \"{0}\".", filename);
                    continue;
                }
                
                ProjectTemplates.Add (pt);

                Program.DebugLogger.WriteLine ("Template \"{0}\" loaded successfully.", filename);
            }
        }

        public ProjectTemplate LoadTemplate (StreamSource source) {
            ProjectTemplate ret;

            try {
                ret = ProjectTemplate.FromStreamSource (source);
            } catch (IDETemplateException) {
                throw;
            }

            return ret;
        }

        #endregion
    }
}
