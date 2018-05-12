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

using System.Drawing;
using System.Globalization;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Support.Themes {
    public abstract class WinFormsThemeBase {
        #region Utility functions

        protected static Color FromHex (string hexCode) {
            int a, r, g, b;

            if (!int.TryParse (hexCode.Substring (0, 2), NumberStyles.HexNumber, null, out a))
                return Color.Empty;
            if (!int.TryParse (hexCode.Substring (2, 2), NumberStyles.HexNumber, null, out r))
                return Color.Empty;
            if (!int.TryParse (hexCode.Substring (4, 2), NumberStyles.HexNumber, null, out g))
                return Color.Empty;
            if (!int.TryParse (hexCode.Substring (6, 2), NumberStyles.HexNumber, null, out b))
                return Color.Empty;

            return Color.FromArgb (a, r, g, b);
        }

        #endregion

        public abstract ThemeBase DockTheme { get; }
        public abstract VisualStudioToolStripExtender ToolStripExtender { get; }
        public abstract VisualStudioToolStripExtender.VsVersion TSEVsVersion { get; }

        public class BasicPalette {
            public virtual Color Background { get; protected set; }
            public virtual Color Foreground { get; protected set; }
            public virtual Color Border { get; protected set; }

            public BasicPalette (Color bg, Color fg, Color border) {
                Background = bg;
                Foreground = fg;
                Border = border;
            }
        }

        #region Windows

        public abstract BasicPalette MainWindowActive { get; }
        public abstract BasicPalette MainWindowInactive { get; }

        public abstract BasicPalette CommonDocument { get; }

        #endregion

        #region Common controls

        public abstract BasicPalette ButtonActive { get; }
        public abstract BasicPalette ButtonInactive { get; }
        public abstract BasicPalette ButtonPressed { get; }
        public abstract BasicPalette ButtonHovered { get; }

        public abstract BasicPalette TextBoxActive { get; }
        public abstract BasicPalette TextBoxInactive { get; }
        public abstract BasicPalette TextBoxHovered { get; }
        public abstract BasicPalette TextBoxDisabled { get; }

        #endregion
    }
}
