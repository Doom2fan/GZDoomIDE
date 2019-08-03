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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static GZDoomIDE.Support.Themes.WinFormsThemeBase;

#endregion

namespace GZDoomIDE.Support {
    public sealed class WinFormsThemer {
        private WinFormsThemeBase theme;
        public WinFormsThemeBase Theme {
            get => theme;
            set {
                theme = value;

                while (first != null) {
                    var node = first;

                    first = node.next;
                    node.Dispose ();
                }

                OnThemeChanged (EventArgs.Empty);
            }
        }
        private WF_ControlThemer first;

        public event EventHandler<EventArgs> ThemeChanged;
        private void OnThemeChanged (EventArgs e) {
            ThemeChanged?.Invoke (this, e);
        }

        public WinFormsThemer () { }
        public WinFormsThemer (WinFormsThemeBase theme) {
            Theme = theme;
        }

        public void ApplyTheme (Form form) {
            form.BackColor = Theme.MainWindow_Active.Background;
            form.ForeColor = Theme.MainWindow_Active.Foreground;

            var ctrls = new List<Control> ();
            foreach (Control ctrl in form.Controls) {
                ctrls.Add (ctrl);
            }

            ApplyTheme (ctrls.ToArray ());
        }

        public void ApplyTheme (Control [] controls) {
            foreach (var ctrl in controls) {
                switch (ctrl) {
                    case DockPanel dock:
                        if (dock.Panes.Count == 0) {
                            dock.Theme = Theme.DockTheme;
                            Theme.DockTheme.ApplyTo (dock);
                        }
                        break;
                    case ToolStrip strip:
                        Theme.ToolStripExtender.SetStyle (strip, Theme.TSEVsVersion, Theme.DockTheme);
                        break;
                    case Button btn:
                        ApplyTheme (btn);
                        break;
                    case IDETextBox box:
                        ApplyTheme (box);
                        break;
                    case DataGridView gridView:
                        ApplyTheme (gridView);
                        break;
                }

                var ctrls = new List<Control> ();
                foreach (Control subCtrl in ctrl.Controls) {
                    ctrls.Add (subCtrl);
                }
                ApplyTheme (ctrls.ToArray ());
            }
        }

        public void ApplyTheme (Button btn) { new WF_ButtonThemer (ref first, btn, Theme); }

        public void ApplyTheme (IDETextBox box) { new WF_IDETextBoxThemer (ref first, box, Theme); }

        public void ApplyTheme (DataGridView gridView) { new WF_DataGridViewThemer (ref first, gridView, Theme); }
    }

    abstract class WF_ControlThemer : IDisposable {
        protected internal WF_ControlThemer prev;
        protected internal WF_ControlThemer next;

        protected bool isDisposed;
        public abstract void Dispose ();

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
        #region ================== Instance members

        protected bool pressed, hovered;
        protected Button button;
        private WinFormsThemeBase theme;

        #endregion

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
                colors = theme.Button_Pressed;
            else if (button.Focused)
                colors = theme.Button_Active;
            else if (hovered)
                colors = theme.Button_Hovered;
            else
                colors = theme.Button_Inactive;

            button.BackColor = colors.Background;
            button.ForeColor = colors.Foreground;
            button.FlatAppearance.BorderColor = colors.Border;

            button.FlatAppearance.CheckedBackColor = colors.Background;
            button.FlatAppearance.MouseDownBackColor = colors.Background;
            button.FlatAppearance.MouseOverBackColor = colors.Background;
        }

        public override void Dispose () {
            if (!isDisposed) {
                button.MouseEnter -= Button_MouseEnter;
                button.MouseLeave -= Button_MouseLeave;

                button.MouseDown -= Button_MouseDown;
                button.MouseUp -= Button_MouseUp;

                button.GotFocus -= Button_MiscEvent;
                button.LostFocus -= Button_MiscEvent;

                button.Disposed -= Button_Disposed;

                Unlink ();

                isDisposed = true;
            }
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
            Dispose ();
        }
    }
    
    class WF_IDETextBoxThemer : WF_ControlThemer {
        #region ================== Instance members

        protected bool hovered;
        protected IDETextBox textBox;
        protected WinFormsThemeBase theme;

        #endregion

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
                colors = theme.TextBox_Disabled;
            else if (textBox.Focused)
                colors = theme.TextBox_Active;
            else if (hovered)
                colors = theme.TextBox_Hovered;
            else
                colors = theme.TextBox_Inactive;

            textBox.BackColor = colors.Background;
            textBox.ForeColor = colors.Foreground;
            textBox.BorderColor = colors.Border;
        }

        public override void Dispose () {
            if (!isDisposed) {
                textBox.MouseEnter -= TextBox_MouseEnter;
                textBox.MouseLeave -= TextBox_MouseLeave;

                textBox.GotFocus -= TextBox_MiscEvent;
                textBox.LostFocus -= TextBox_MiscEvent;

                textBox.Disposed -= TextBox_Disposed;

                Unlink ();

                isDisposed = true;
            }
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
            Dispose ();
        }
    }

    class WF_DataGridViewThemer : WF_ControlThemer {
        #region ================== Instance members

        protected DataGridView gridView;
        protected WinFormsThemeBase theme;
        protected BasicPalette curPalette;

        #endregion

        #region ================== Constructors

        public WF_DataGridViewThemer (ref WF_ControlThemer prev, DataGridView btn, WinFormsThemeBase theme) : base (ref prev) {
            gridView = btn;
            this.theme = theme;

            gridView.GotFocus += GridView_FocusChanged;
            gridView.LostFocus += GridView_FocusChanged;

            gridView.Paint += GridView_Paint;

            gridView.Disposed += GridView_Disposed;

            SetStyle ();
        }

        private void GridView_Paint (object sender, PaintEventArgs e) {
            using (var p = new Pen (curPalette.Border))
                e.Graphics.DrawRectangle (p, new Rectangle (0, 0, gridView.Width - 1, gridView.Height - 1));
        }

        private void GridView_FocusChanged (object sender, EventArgs e) {
            SetStyle ();
        }

        #endregion

        private void SetStyle () {
            curPalette = (gridView.Focused ? theme.DataGridView_Active : theme.DataGridView_Inactive);

            gridView.BackgroundColor = curPalette.Background;
            gridView.ForeColor = curPalette.Foreground;
            gridView.GridColor = curPalette.Border;

            gridView.EnableHeadersVisualStyles = false;

            var defPadding = new Padding (2, 4, 2, 4);

            // Column headers
            gridView.AdvancedColumnHeadersBorderStyle.Top    = gridView.AdvancedColumnHeadersBorderStyle.Left  = DataGridViewAdvancedCellBorderStyle.None;
            gridView.AdvancedColumnHeadersBorderStyle.Bottom = gridView.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;

            gridView.ColumnHeadersDefaultCellStyle.Font = gridView.Font;
            gridView.ColumnHeadersDefaultCellStyle.Padding = defPadding;

            gridView.ColumnHeadersDefaultCellStyle.BackColor = theme.DataGridView_Header.Background;
            gridView.ColumnHeadersDefaultCellStyle.ForeColor = theme.DataGridView_Header.Foreground;
            // Column headers, selected
            gridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = theme.DataGridView_Header_Clicked.Background;
            gridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = theme.DataGridView_Header_Clicked.Foreground;

            // Row
            gridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gridView.RowsDefaultCellStyle.BackColor = theme.DataGridView_Row.Background;
            gridView.RowsDefaultCellStyle.ForeColor = theme.DataGridView_Row.Foreground;
            gridView.RowsDefaultCellStyle.Font = gridView.Font;
            gridView.RowsDefaultCellStyle.Padding = defPadding;
            // Row, selected
            gridView.RowsDefaultCellStyle.SelectionBackColor = theme.DataGridView_Row_Selected.Background;
            gridView.RowsDefaultCellStyle.SelectionForeColor = theme.DataGridView_Row_Selected.Foreground;
            // Alternate row
            gridView.AlternatingRowsDefaultCellStyle.BackColor = theme.DataGridView_AlternateRow.Background;
            gridView.AlternatingRowsDefaultCellStyle.ForeColor = theme.DataGridView_AlternateRow.Foreground;
            gridView.AlternatingRowsDefaultCellStyle.Font = gridView.Font;
            //gridView.AlternatingRowsDefaultCellStyle.Padding = defPadding;
            // Alternate row, selected
            gridView.AlternatingRowsDefaultCellStyle.SelectionBackColor = theme.DataGridView_AlternateRow_Selected.Background;
            gridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = theme.DataGridView_AlternateRow_Selected.Foreground;
        }

        public override void Dispose () {
            if (!isDisposed) {
                gridView.GotFocus -= GridView_FocusChanged;
                gridView.LostFocus -= GridView_FocusChanged;

                gridView.Disposed -= GridView_Disposed;

                Unlink ();

                isDisposed = true;
            }
        }

        private void GridView_Disposed (object sender, System.EventArgs e) {
            Dispose ();
        }
    }
}
