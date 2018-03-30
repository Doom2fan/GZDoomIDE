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

using GZDoomIDE.Data;

namespace GZDoomIDE {
    public static class Constants {
        public const byte MajorVersion = 0;
        public const byte MinorVersion = 1;
        public const byte PatchVersion = 0;
        public static readonly SemVer Version = new SemVer (MajorVersion, MinorVersion, PatchVersion);

        /// <summary>
        /// The filetypes supported by the IDE.
        /// </summary>
        public const string FileTypes = "ZScript files|*.zs;*.zsc|Text files|*.txt|All files|*.*";

        public static string ProgDir { get; private set; }
        public static string DataDir { get; private set; }
        public static string PluginsDir { get; private set; }

        static Constants () {
            ProgDir = System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location);
            DataDir = ProgDir;
            PluginsDir = System.IO.Path.Combine (DataDir, "/Plugins/");
        }
    }
}
