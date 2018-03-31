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

namespace GZDoomIDE.Data {
    [AttributeUsage (AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ProjectTypeAttribute : Attribute {
        public string Name { get; protected set; }

        internal ProjectTypeAttribute () { }

        /// <summary>
        /// Used to mark a class as an usable project type. (So you can have base classes and such)
        /// </summary>
        public ProjectTypeAttribute (string name) {
            Name = name;
        }
    }

    public class ProjectType {
        #region ================== Compilation

        /// <summary>
        /// Indicates whether the project can be compiled.
        /// </summary>
        public virtual bool CanCompile { get; protected internal set; } = false;
        /// <summary>
        /// Compile the project.
        /// </summary>
        /// <param name="projData">The project to compile.</param>
        /// <returns>Returns a bool indicating whether the project was compiled successfully.</returns>
        public virtual bool Compile (ProjectData projData) { return false; }

        #endregion

        #region ================== Running

        /// <summary>
        /// Indicates whether the project can be run.
        /// </summary>
        public virtual bool CanRun { get; protected internal set; } = false;
        /// <summary>
        /// Indicates whether the project needs to be compiled before running when changes are made.
        /// </summary>
        public virtual bool NeedsCompilePreRun { get; protected internal set; } = false;
        /// <summary>
        /// Run the project.
        /// </summary>
        /// <param name="projData">The project to run.</param>
        /// <returns>Returns a bool indicating whether the project ran successfully.</returns>
        public virtual bool Run (ProjectData projData) { return false; }

        #endregion
    }

    public sealed class UnknownProjectType : ProjectType {
        public string TypeName { get; private set; }

        public UnknownProjectType (string name) {
            TypeName = name;
        }
    }
}
