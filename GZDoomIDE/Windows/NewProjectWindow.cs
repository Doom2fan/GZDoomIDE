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
using System.IO;
using System.Windows.Forms;

#endregion

namespace GZDoomIDE.Windows {
    internal partial class NewProjectWindow : Form {
        internal enum NewProjType {
            NewWorkspace,
            NewProject,
            AddProject,
        };

        #region ================== Instance members

        private MainWindow mainWindow;
        private NewProjType mode;
        private bool allowExit = true;

        private ProjectTemplate _template;
        protected ProjectTemplate Template {
            get => _template;
            set {
                _template = value;
                TemplateChanged ();
            }
        }

        public WorkspaceData NewWorkspace { get; protected set; }

        #endregion

        #region ================== Constructors

        protected NewProjectWindow () {
            InitializeComponent ();

            Program.Data.Themer.ApplyTheme (this);
        }

        public NewProjectWindow (MainWindow mainWindow, NewProjType mode) : this () {
            this.mainWindow = mainWindow;
            this.mode = mode;

            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = Program.Data.ProjectTemplates;

            solutionComboBox.Visible = labelSolution.Visible = (mode == NewProjType.NewProject);
            solutionNewDirCheckBox.Visible = solutionNameTextBox.Visible = labelSolutionName.Visible = (mode == NewProjType.NewWorkspace || mode == NewProjType.NewProject);
        }

        #endregion

        #region ================== Instance methods

        #region ================== Events

        private void NameTextBox_Leave (object sender, EventArgs e) {
            SetDefaultName ();
        }

        private void DataGridView_RowEnter (object sender, DataGridViewCellEventArgs e) {
            var row = dataGridView.Rows [e.RowIndex];
            Template = (ProjectTemplate) row.DataBoundItem;
        }

        #endregion

        private void SetDefaultName () {
            string name = String.Empty;

            if (!(Template is null)) {
                name = Template.DefaultProjectName;
                /*for (int i = 1; i < int.MaxValue; i++) {
                    if (!)
                    template.DefaultProjectName
                }*/
            }

            if (!nameTextBox.Modified || String.IsNullOrWhiteSpace (nameTextBox.Text)) {
                nameTextBox.Text = name;
                nameTextBox.Modified = false;
            }

            if (!solutionNameTextBox.Modified || String.IsNullOrWhiteSpace (solutionNameTextBox.Text)) {
                solutionNameTextBox.Text = name;
                solutionNameTextBox.Modified = false;
            }
        }

        protected void TemplateChanged () {
            SetDefaultName ();

            templateDescTextBox.Text = (!(Template is null) ? Template.Description : string.Empty);
        }

        #endregion

        private void OkButton_Click (object sender, EventArgs e) {
            string path = pathTextBox.Text;
            
            if (!Path.IsPathRooted (path)) {
                MessageBox.Show ("The path must be an absolute path.", "Invalid path");
                allowExit = false;
                return;
            } else if (!Directory.Exists (path)) {
                MessageBox.Show ("The path must be an existing directory.", "Invalid path");
                allowExit = false;
                return;
            }
            
            bool createSolution = (mode == NewProjType.NewWorkspace);
            if (mode == NewProjType.NewProject)
                createSolution = ((string) solutionComboBox.SelectedItem == "Create new solution");

            string projName = nameTextBox.Text;
            string solutionName = solutionNameTextBox.Text;
            
            if (createSolution && solutionNewDirCheckBox.Checked)
                path = Path.Combine (path, solutionName);

            string projPath = Path.Combine (path, projName);

            ProjectData projData;
            Template.CreateProject (projPath, projName, out projData);

            if (createSolution) {
                WorkspaceData solutionData = new WorkspaceData ();

                solutionData.Name = solutionName;
                solutionData.ProjectFiles.Add (projData);
                solutionData.WorkspaceFilePath = Path.Combine (path, solutionName + ".gzidewsp");

                projData.ProjectFilePath = Utils.GetRelativePath (projData.ProjectFilePath, Path.GetDirectoryName (solutionData.WorkspaceFilePath));

                solutionData.Save ();

                NewWorkspace = solutionData;
            } else {
                var curSolution = mainWindow.CurWorkspace;

                if (curSolution is null) // How the fuck did we even get here, then?
                    return;

                projData.ProjectFilePath = Utils.GetRelativePath (projData.ProjectFilePath, Path.GetDirectoryName (curSolution.WorkspaceFilePath));

                curSolution.ProjectFiles.Add (projData);
            }
        }

        private void NewProjectWindow_FormClosing (object sender, FormClosingEventArgs e) {
            e.Cancel = !allowExit;
            allowExit = true;
        }

        private void PathTextBox_Leave (object sender, EventArgs e) {
            if (!Utils.IsPathValid (pathTextBox.Text)) {
                MessageBox.Show ("The specified path is invalid", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pathTextBox.Focus ();
            }
        }

        private void PathBrowseButton_Click (object sender, EventArgs e) {
            if (Utils.IsPathValid (pathTextBox.Text))
                locationBrowserDialog.SelectedPath = pathTextBox.Text;
            else
                locationBrowserDialog.SelectedPath = "";

            if (locationBrowserDialog.ShowDialog () == DialogResult.OK) {
                pathTextBox.Text = locationBrowserDialog.SelectedPath;
            }
        }
    }
}
