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
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Support.Themes {
    public class WF_VS2017DarkTheme : WinFormsThemeBase {
        public override ThemeBase DockTheme { get; } = new VS2015DarkTheme ();
        public override VisualStudioToolStripExtender ToolStripExtender { get; } = new VisualStudioToolStripExtender ();
        public override VisualStudioToolStripExtender.VsVersion TSEVsVersion { get; } = VisualStudioToolStripExtender.VsVersion.Vs2015;

        #region Windows

        public override BasicPalette MainWindowActive   { get; } = new BasicPalette (FromHex ("FF2D2D30"), FromHex ("FFF1F1F1"), FromHex ("FF007ACC"));
        public override BasicPalette MainWindowInactive { get; } = new BasicPalette (FromHex ("FF2D2D30"), FromHex ("FFF1F1F1"), FromHex ("FFF1F1F1"));

        public override BasicPalette CommonDocument { get; } = new BasicPalette (FromHex ("FF252526"), FromHex ("FFF1F1F1"), FromHex ("FFFFFFFF"));

        #endregion

        #region Common controls

        public override BasicPalette ButtonActive   { get; } = new BasicPalette (FromHex ("FF2D2D30"), FromHex ("FFF1F1F1"), FromHex ("FF3F3F46"));
        public override BasicPalette ButtonInactive { get; } = new BasicPalette (FromHex ("FF2D2D30"), FromHex ("FFF1F1F1"), FromHex ("FF3F3F46"));
        public override BasicPalette ButtonPressed  { get; } = new BasicPalette (FromHex ("FF007ACC"), FromHex ("FFF1F1F1"), FromHex ("FF007ACC"));
        public override BasicPalette ButtonHovered  { get; } = new BasicPalette (FromHex ("7F555555"), FromHex ("FFFFFFFF"), FromHex ("72555555"));

        public override BasicPalette TextBoxActive   { get; } = new BasicPalette (FromHex ("FF3F3F46"), FromHex ("FFFFFFFF"), FromHex ("FF3F3F46"));
        public override BasicPalette TextBoxInactive { get; } = new BasicPalette (FromHex ("FF333337"), FromHex ("FFF1F1F1"), FromHex ("FF3F3F46"));
        public override BasicPalette TextBoxHovered  { get; } = new BasicPalette (FromHex ("FF3F3F46"), FromHex ("FFFFFFFF"), FromHex ("FF007ACC"));
        public override BasicPalette TextBoxDisabled { get; } = new BasicPalette (FromHex ("FF2D2D30"), FromHex ("FF656565"), FromHex ("FF3F3F46"));

        #endregion
    }
}
