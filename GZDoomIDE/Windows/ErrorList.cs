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
using GZDoomIDE.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

#endregion

namespace GZDoomIDE.Windows {
    public partial class ErrorList : DockContent {
        MainWindow mainWindow;

        Dictionary<IDEErrorProvider, List<IDEError>> errorItems = new Dictionary<IDEErrorProvider, List<IDEError>> ();

        protected ErrorList () {
            InitializeComponent ();

            Utils.ThemeToolstrips (vsToolStripExtender, new ToolStrip [] { toolStrip }, vs2015DarkTheme);
        }

        public ErrorList (MainWindow parentForm) : this () {
            mainWindow = parentForm;

            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = ideErrorListBindingSource;

            ideErrorListBindingSource.Clear ();
            if (!(mainWindow is null)) {
                errorItems.Add (Program.PluginErrors, new List<IDEError> ());
                Program.PluginErrors.OnChanged += ErrorProvider_OnChanged;

                errorItems.Add (mainWindow.WorkspaceErrors, new List<IDEError> ());
                mainWindow.WorkspaceErrors.OnChanged += ErrorProvider_OnChanged;

                errorItems.Add (mainWindow.CurFileErrors, new List<IDEError> ());
                mainWindow.CurFileErrors.OnChanged += ErrorProvider_OnChanged;

                mainWindow.PreWorkspaceChanged += MainWindow_PreWorkspaceChanged;
                mainWindow.WorkspaceChanged += MainWindow_WorkspaceChanged;

                MainWindow_WorkspaceChanged (null, null); // lazy hax

                foreach (var prov in errorItems.Keys) {
                    ErrorProvider_OnChanged (prov, null);
                }
            }
        }

        private void MainWindow_PreWorkspaceChanged (object sender, EventArgs e) {
            if (mainWindow.ProjectErrors is null)
                return;

            foreach (KeyValuePair<ProjectData, IDEErrorProvider> kvp in mainWindow.ProjectErrors) {
                if (!(kvp.Value is null)) {
                    kvp.Value.OnChanged -= ErrorProvider_OnChanged;
                    errorItems.Remove (kvp.Value);
                }
            }
        }

        private void MainWindow_WorkspaceChanged (object sender, EventArgs e) {
            if (mainWindow.ProjectErrors is null)
                return;

            foreach (KeyValuePair<ProjectData, IDEErrorProvider> kvp in mainWindow.ProjectErrors) {
                if (!(kvp.Value is null)) {
                    kvp.Value.OnChanged += ErrorProvider_OnChanged;
                    errorItems.Add (kvp.Value, new List<IDEError> ());
                }
            }
        }

        private void ErrorProvider_OnChanged (object sender, EventArgs e) {
            if (sender.GetType () != typeof (IDEErrorProvider))
                throw new Exception ("ErrorList: WTF? This isn't an IDEErrorProvider.");

            var errProv = (sender as IDEErrorProvider);

            if (!errorItems.ContainsKey (errProv))
                throw new Exception ("ErrorProvider_OnChanged was called on an unrecognized provider.");

            var errorList = errorItems [errProv];
            
            foreach (var item in errorList) {
                ideErrorListBindingSource.Remove (item);
            }
            errorList.Clear ();

            foreach (var err in errProv.Errors) {
                errorList.Add (err);
                ideErrorListBindingSource.Add (err);
            }
        }

        private void ErrorList_FormClosed (object sender, FormClosedEventArgs e) {
            mainWindow.PreWorkspaceChanged -= MainWindow_PreWorkspaceChanged;
            mainWindow.WorkspaceChanged -= MainWindow_WorkspaceChanged;

            foreach (var prov in errorItems.Keys) {
                prov.OnChanged -= ErrorProvider_OnChanged;
            }
            errorItems.Clear ();
        }

        private void DataGridView_CellFormatting (object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.ColumnIndex == dataGridView.Columns ["icon"].Index) {
                if (e.Value.GetType () != typeof (ErrorType))
                    throw new Exception ("Invalid value for icon cell");

                e.FormattingApplied = true;
                switch ((ErrorType) e.Value) {
                    case ErrorType.Error: e.Value = Resources.ExclamationIcon; break;
                    case ErrorType.Warning: e.Value = Resources.ErrorIcon; break;
                    case ErrorType.Information: e.Value = Resources.InfoIcon; break;
                }
            } else if (e.ColumnIndex == dataGridView.Columns ["line"].Index) {
                if (e.Value.GetType () != typeof (int))
                    throw new Exception ("Invalid value for line cell");

                if (((int) e.Value) <= 0) {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
