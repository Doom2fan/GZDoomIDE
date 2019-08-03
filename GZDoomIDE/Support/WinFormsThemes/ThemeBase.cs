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

        public abstract BasicPalette MainWindow_Active { get; }
        public abstract BasicPalette MainWindow_Inactive { get; }

        public abstract BasicPalette CommonDocument { get; }

        #endregion

        #region Common controls

        public abstract BasicPalette Button_Active { get; }
        public abstract BasicPalette Button_Inactive { get; }
        public abstract BasicPalette Button_Pressed { get; }
        public abstract BasicPalette Button_Hovered { get; }

        public abstract BasicPalette TextBox_Active { get; }
        public abstract BasicPalette TextBox_Inactive { get; }
        public abstract BasicPalette TextBox_Hovered { get; }
        public abstract BasicPalette TextBox_Disabled { get; }

        public abstract BasicPalette DataGridView_Active { get; }
        public abstract BasicPalette DataGridView_Inactive { get; }

        public abstract BasicPalette DataGridView_Header { get; }
        public abstract BasicPalette DataGridView_Header_Clicked { get; }

        public abstract BasicPalette DataGridView_Row { get; }
        public abstract BasicPalette DataGridView_Row_Selected { get; }
        public virtual BasicPalette DataGridView_AlternateRow { get => DataGridView_Row; }
        public virtual BasicPalette DataGridView_AlternateRow_Selected { get => DataGridView_Row_Selected; }

        #endregion
    }
}
