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
using ScintillaNET;
using System;

#endregion

namespace GZDoomIDE.Editor {
    [AttributeUsage (AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SyntaxHighlighterAttribute : Attribute {
        public string Language { get; private set; }

        internal SyntaxHighlighterAttribute () { }

        /// <summary>
        /// Used to mark a class as an usable project type. (So you can have base classes and such)
        /// </summary>
        public SyntaxHighlighterAttribute (string lang) {
            Language = lang;
        }
    }

    public abstract class SyntaxHighlighter {
        public abstract void DoSetup (Scintilla editor, ProjectData project);

        public abstract void ApplyTheme (Scintilla editor, ScintillaTheme theme, ProjectData project);

        public abstract void DoHighlighting (Scintilla editor, int startPos, int endPos, ProjectData project);

        public abstract void Finalize (Scintilla editor, ProjectData project);
    }
}
