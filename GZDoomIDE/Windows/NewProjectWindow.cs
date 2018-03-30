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

using System;
using System.Windows.Forms;

#endregion

namespace GZDoomIDE.Windows {
    public partial class NewProjectWindow : Form {
        public NewProjectWindow () {
            InitializeComponent ();

            bigIconsButton.Tag = true;
            smallIconsButton.Tag = false;

            Utils.ThemeToolstrips (vsToolStripExtender, new ToolStrip [] { mainToolStrip }, vs2015DarkTheme);

            templatesListView.Items.Add (new ListViewItem ("lul"));
            templatesListView.Items.Add (new ListViewItem ("lol"));
            templatesListView.Items.Add (new ListViewItem ("lel"));
            templatesListView.Items.Add (new ListViewItem ("lawl"));
        }

        private bool _bigIcons = true;
        public bool BigIcons {
            get => _bigIcons;
            set {
                _bigIcons = value;
                OnViewSizeChanged (EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> ViewSizeChanged;
        protected virtual void OnViewSizeChanged (EventArgs e) { ViewSizeChanged?.Invoke (this, e); }

        private void ViewSizeButton_Click (object sender, EventArgs e) {
            if (sender.GetType () != typeof (ToolStripButton))
                throw new Exception ("Invalid sender");
            else if ((sender as ToolStripButton).Tag.GetType () != typeof (bool))
                throw new Exception ("Attempted to set an incorrect window size");

            bool bigIconsOn = (bool) ((sender as ToolStripButton).Tag);

            BigIcons = bigIconsOn;
            bigIconsButton.Checked = bigIconsOn;
            smallIconsButton.Checked = !bigIconsOn;

            templatesListView.View = (bigIconsOn ? View.LargeIcon : View.SmallIcon);
        }
    }
}
