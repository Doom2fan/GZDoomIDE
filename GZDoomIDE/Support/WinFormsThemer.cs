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

using GZDoomIDE.Support.Themes;
using GZDoomIDE.Windows.Controls;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Support {
    public sealed class WinFormsThemer {
        public WinFormsThemeBase Theme { get; set; }
        private WF_ControlThemer first;

        public WinFormsThemer () { }
        public WinFormsThemer (WinFormsThemeBase theme) {
            Theme = theme;
        }

        public void ApplyTheme (Form form) {
            form.BackColor = Theme.MainWindowActive.Background;
            form.ForeColor = Theme.MainWindowActive.Foreground;

            List<Control> ctrls = new List<Control> ();
            foreach (Control ctrl in form.Controls) {
                ctrls.Add (ctrl);
            }

            ApplyTheme (ctrls.ToArray ());
        }

        public void ApplyTheme (Control [] controls) {
            foreach (var ctrl in controls) {
                if (ctrl is DockPanel) {
                    (ctrl as DockPanel).Theme = Theme.DockTheme;
                    Theme.DockTheme.ApplyTo ((DockPanel) ctrl);
                } else if (ctrl is ToolStrip)
                    Theme.ToolStripExtender.SetStyle ((ToolStrip) ctrl, Theme.TSEVsVersion, Theme.DockTheme);
                else if (ctrl is Button)
                    ApplyTheme ((Button) ctrl);
                else if (ctrl is IDETextBox)
                    ApplyTheme ((IDETextBox) ctrl);

                List<Control> ctrls = new List<Control> ();
                foreach (Control subCtrl in ctrl.Controls) {
                    ctrls.Add (subCtrl);
                }
                ApplyTheme (ctrls.ToArray ());
            }
        }

        public void ApplyTheme (Button btn) { new WF_ButtonThemer (ref first, btn, Theme); }

        public void ApplyTheme (IDETextBox box) { new WF_IDETextBoxThemer (ref first, box, Theme); }
    }

    class WF_ControlThemer {
        protected WF_ControlThemer prev;
        protected WF_ControlThemer next;

        protected WF_ControlThemer (ref WF_ControlThemer prev) {
            this.prev = prev;

            if (!(prev is null))
                prev.next = this;
            else
                prev = this;
        }

        protected void Unlink () {
            if (!(prev is null))
                prev.next = next;
            if (!(next is null))
                next.prev = prev;

            prev = next = null;
        }
    }

    class WF_ButtonThemer : WF_ControlThemer {
        #region ================== Constructors

        public WF_ButtonThemer (ref WF_ControlThemer prev, Button btn, WinFormsThemeBase theme) : base (ref prev) {
            button = btn;
            this.theme = theme;
            
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;

            button.MouseDown += Button_MouseDown;
            button.MouseUp += Button_MouseUp;
            
            button.GotFocus += Button_MiscEvent;
            button.LostFocus += Button_MiscEvent;

            button.Disposed += Button_Disposed;

            SetStyle ();
        }

        #endregion

        private void SetStyle () {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;

            WinFormsThemeBase.BasicPalette colors;

            if (pressed)
                colors = theme.ButtonPressed;
            else if (button.Focused)
                colors = theme.ButtonActive;
            else if (hovered)
                colors = theme.ButtonHovered;
            else
                colors = theme.ButtonInactive;

            button.BackColor = colors.Background;
            button.ForeColor = colors.Foreground;
            button.FlatAppearance.BorderColor = colors.Border;

            button.FlatAppearance.CheckedBackColor = colors.Background;
            button.FlatAppearance.MouseDownBackColor = colors.Background;
            button.FlatAppearance.MouseOverBackColor = colors.Background;
        }
        
        private void Button_MouseEnter (object sender, System.EventArgs e) {
            hovered = true;
            SetStyle ();
        }
        private void Button_MouseLeave (object sender, System.EventArgs e) {
            hovered = false;
            SetStyle ();
        }

        private void Button_MouseDown (object sender, MouseEventArgs e) {
            pressed = true;
            SetStyle ();
        }
        private void Button_MouseUp (object sender, MouseEventArgs e) {
            pressed = false;
            SetStyle ();
        }

        private void Button_MiscEvent (object sender, System.EventArgs e) {
            SetStyle ();
        }

        private void Button_Disposed (object sender, System.EventArgs e) {
            button.MouseEnter -= Button_MouseEnter;
            button.MouseLeave -= Button_MouseLeave;

            button.MouseDown -= Button_MouseDown;
            button.MouseUp -= Button_MouseUp;

            button.GotFocus -= Button_MiscEvent;
            button.LostFocus -= Button_MiscEvent;

            button.Disposed -= Button_Disposed;

            Unlink ();
        }

        #region ================== Instance members

        protected bool pressed, hovered;
        protected Button button;
        private WinFormsThemeBase theme;

        #endregion
    }
    
    class WF_IDETextBoxThemer : WF_ControlThemer {
        #region ================== Constructors

        public WF_IDETextBoxThemer (ref WF_ControlThemer prev, IDETextBox box, WinFormsThemeBase theme) : base (ref prev) {
            textBox = box;
            this.theme = theme;
            
            textBox.MouseEnter += TextBox_MouseEnter;
            textBox.MouseLeave += TextBox_MouseLeave;

            textBox.MouseDown += TextBox_MiscEvent;
            textBox.MouseUp += TextBox_MiscEvent;

            textBox.GotFocus += TextBox_MiscEvent;
            textBox.LostFocus += TextBox_MiscEvent;

            textBox.Disposed += TextBox_Disposed;
            
            SetStyle ();
        }

        #endregion

        private void SetStyle () {
            textBox.BorderStyle = BorderStyle.FixedSingle;

            WinFormsThemeBase.BasicPalette colors;

            if (!textBox.Enabled)
                colors = theme.TextBoxDisabled;
            else if (textBox.Focused)
                colors = theme.TextBoxActive;
            else if (hovered)
                colors = theme.TextBoxHovered;
            else
                colors = theme.TextBoxInactive;

            textBox.BackColor = colors.Background;
            textBox.ForeColor = colors.Foreground;
            textBox.BorderColor = colors.Border;
        }

        private void TextBox_MouseEnter (object sender, System.EventArgs e) {
            hovered = true;
            SetStyle ();
        }
        private void TextBox_MouseLeave (object sender, System.EventArgs e) {
            hovered = false;
            SetStyle ();
        }

        private void TextBox_MiscEvent (object sender, System.EventArgs e) {
            SetStyle ();
        }

        private void TextBox_Disposed (object sender, System.EventArgs e) {
            textBox.MouseEnter -= TextBox_MouseEnter;
            textBox.MouseLeave -= TextBox_MouseLeave;

            textBox.GotFocus -= TextBox_MiscEvent;
            textBox.LostFocus -= TextBox_MiscEvent;

            textBox.Disposed -= TextBox_Disposed;

            Unlink ();
        }

        #region ================== Instance members

        protected bool hovered;
        protected IDETextBox textBox;
        private WinFormsThemeBase theme;

        #endregion
    }
}
