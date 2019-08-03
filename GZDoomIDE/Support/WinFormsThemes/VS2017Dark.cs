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

        public override BasicPalette MainWindow_Active   { get; } = new BasicPalette (Utils.ColorFromHex ("#FF2D2D30"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF007ACC"));
        public override BasicPalette MainWindow_Inactive { get; } = new BasicPalette (Utils.ColorFromHex ("#FF2D2D30"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FFF1F1F1"));

        public override BasicPalette CommonDocument { get; } = new BasicPalette (Utils.ColorFromHex ("#FF252526"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FFFFFFFF"));

        #endregion

        #region Common controls

        public override BasicPalette Button_Active   { get; } = new BasicPalette (Utils.ColorFromHex ("#FF2D2D30"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF3F3F46"));
        public override BasicPalette Button_Inactive { get; } = new BasicPalette (Utils.ColorFromHex ("#FF2D2D30"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF3F3F46"));
        public override BasicPalette Button_Pressed  { get; } = new BasicPalette (Utils.ColorFromHex ("#FF007ACC"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF007ACC"));
        public override BasicPalette Button_Hovered  { get; } = new BasicPalette (Utils.ColorFromHex ("#7F555555"), Utils.ColorFromHex ("#FFFFFFFF"), Utils.ColorFromHex ("#72555555"));

        public override BasicPalette TextBox_Active   { get; } = new BasicPalette (Utils.ColorFromHex ("#FF3F3F46"), Utils.ColorFromHex ("#FFFFFFFF"), Utils.ColorFromHex ("#FF3F3F46"));
        public override BasicPalette TextBox_Inactive { get; } = new BasicPalette (Utils.ColorFromHex ("#FF333337"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF3F3F46"));
        public override BasicPalette TextBox_Hovered  { get; } = new BasicPalette (Utils.ColorFromHex ("#FF3F3F46"), Utils.ColorFromHex ("#FFFFFFFF"), Utils.ColorFromHex ("#FF007ACC"));
        public override BasicPalette TextBox_Disabled { get; } = new BasicPalette (Utils.ColorFromHex ("#FF2D2D30"), Utils.ColorFromHex ("#FF656565"), Utils.ColorFromHex ("#FF3F3F46"));

        public override BasicPalette DataGridView_Active { get; } = new BasicPalette (Utils.ColorFromHex ("#FF252526"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF3F3F46"));
        public override BasicPalette DataGridView_Inactive { get; } = new BasicPalette (Utils.ColorFromHex ("#FF252526"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF3F3F46"));

        public override BasicPalette DataGridView_Header { get; } = new BasicPalette (Utils.ColorFromHex ("#FF252526"), Utils.ColorFromHex ("#FFF1F1F1"), Utils.ColorFromHex ("#FF3F3F46"));
        public override BasicPalette DataGridView_Header_Clicked { get; } = new BasicPalette (Utils.ColorFromHex ("#FF007ACC"), Utils.ColorFromHex ("#FFFFFFFF"), Utils.ColorFromHex ("#FF3F3F46"));

        public override BasicPalette DataGridView_Row { get; } = new BasicPalette (Utils.ColorFromHex ("#FF252526"), Utils.ColorFromHex ("#FFD0D2D3"), Color.Transparent);
        public override BasicPalette DataGridView_Row_Selected { get; } = new BasicPalette (Utils.ColorFromHex ("#FF3399FF"), Utils.ColorFromHex ("#FFFFFFFF"), Color.Transparent);

        #endregion
    }
}
