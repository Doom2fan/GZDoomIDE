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

#endregion

namespace CorePlugin {
    [ProjectType]
    public class GZDoomMod : ProjectType {
        #region ================== Compilation
        /// <summary>
        /// Indicates whether the project can be compiled.
        /// </summary>
        public override bool CanCompile {
            get { return true; }
            protected set { }
        }
        /// <summary>
        /// Compile the project.
        /// </summary>
        /// <param name="projData">The project to compile.</param>
        /// <returns>Returns a bool indicating whether the project was compiled successfully.</returns>
        public override bool Compile (ProjectData projData) { return false; }
        #endregion

        #region ================== Running
        /// <summary>
        /// Indicates whether the project can be run.
        /// </summary>
        public override bool CanRun {
            get { return true; }
            protected set { }
        }
        /// <summary>
        /// Run the project.
        /// </summary>
        /// <param name="projData">The project to run.</param>
        /// <returns>Returns a bool indicating whether the project ran successfully.</returns>
        public override bool Run (ProjectData projData) { return false; }
        #endregion
    }
}
