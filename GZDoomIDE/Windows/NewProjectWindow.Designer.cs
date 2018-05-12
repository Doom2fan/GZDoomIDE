namespace GZDoomIDE.Windows {
    partial class NewProjectWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TableLayoutPanel mainTLP;
            System.Windows.Forms.TableLayoutPanel okCancelTLP;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectWindow));
            this.templateDataLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.templateDescTextBox = new GZDoomIDE.Windows.Controls.IDETextBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.controlsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.solutionNewDirCheckBox = new System.Windows.Forms.CheckBox();
            this.nameTextBox = new GZDoomIDE.Windows.Controls.IDETextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelPath = new System.Windows.Forms.Label();
            this.pathTextBox = new GZDoomIDE.Windows.Controls.IDETextBox();
            this.pathBrowseButton = new System.Windows.Forms.Button();
            this.labelSolution = new System.Windows.Forms.Label();
            this.solutionComboBox = new System.Windows.Forms.ComboBox();
            this.labelSolutionName = new System.Windows.Forms.Label();
            this.solutionNameTextBox = new GZDoomIDE.Windows.Controls.IDETextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.searchButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.vs2015DarkTheme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            mainTLP = new System.Windows.Forms.TableLayoutPanel();
            okCancelTLP = new System.Windows.Forms.TableLayoutPanel();
            mainTLP.SuspendLayout();
            this.templateDataLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.controlsLayoutPanel.SuspendLayout();
            okCancelTLP.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTLP
            // 
            mainTLP.ColumnCount = 1;
            mainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTLP.Controls.Add(this.templateDataLayoutPanel, 0, 0);
            mainTLP.Controls.Add(this.bottomPanel, 0, 1);
            mainTLP.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTLP.Location = new System.Drawing.Point(0, 25);
            mainTLP.Name = "mainTLP";
            mainTLP.RowCount = 2;
            mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.38197F));
            mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.61803F));
            mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            mainTLP.Size = new System.Drawing.Size(784, 458);
            mainTLP.TabIndex = 0;
            // 
            // templateDataLayoutPanel
            // 
            this.templateDataLayoutPanel.ColumnCount = 2;
            this.templateDataLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.templateDataLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.templateDataLayoutPanel.Controls.Add(this.dataGridView, 0, 0);
            this.templateDataLayoutPanel.Controls.Add(this.templateDescTextBox, 1, 0);
            this.templateDataLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateDataLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.templateDataLayoutPanel.Name = "templateDataLayoutPanel";
            this.templateDataLayoutPanel.RowCount = 1;
            this.templateDataLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.templateDataLayoutPanel.Size = new System.Drawing.Size(778, 302);
            this.templateDataLayoutPanel.TabIndex = 5;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnHeadersVisible = false;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icon,
            this.name,
            this.tag});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ShowCellErrors = false;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.ShowRowErrors = false;
            this.dataGridView.Size = new System.Drawing.Size(577, 296);
            this.dataGridView.TabIndex = 3;
            this.dataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_RowEnter);
            // 
            // icon
            // 
            this.icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.icon.DataPropertyName = "IconImage";
            this.icon.FillWeight = 5F;
            this.icon.HeaderText = "Icon";
            this.icon.Name = "icon";
            this.icon.ReadOnly = true;
            this.icon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "Name";
            this.name.FillWeight = 95F;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // tag
            // 
            this.tag.HeaderText = "Tag";
            this.tag.Name = "tag";
            this.tag.ReadOnly = true;
            this.tag.Visible = false;
            // 
            // templateDescTextBox
            // 
            this.templateDescTextBox.BorderColor = System.Drawing.Color.Empty;
            this.templateDescTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateDescTextBox.Enabled = false;
            this.templateDescTextBox.Location = new System.Drawing.Point(586, 3);
            this.templateDescTextBox.MaxLength = 2147483647;
            this.templateDescTextBox.Multiline = true;
            this.templateDescTextBox.Name = "templateDescTextBox";
            this.templateDescTextBox.ReadOnly = true;
            this.templateDescTextBox.Size = new System.Drawing.Size(189, 296);
            this.templateDescTextBox.TabIndex = 2;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.controlsLayoutPanel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.Location = new System.Drawing.Point(3, 311);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(778, 144);
            this.bottomPanel.TabIndex = 1;
            // 
            // controlsLayoutPanel
            // 
            this.controlsLayoutPanel.ColumnCount = 3;
            this.controlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.controlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.controlsLayoutPanel.Controls.Add(this.solutionNewDirCheckBox, 2, 3);
            this.controlsLayoutPanel.Controls.Add(this.nameTextBox, 1, 0);
            this.controlsLayoutPanel.Controls.Add(this.labelName, 0, 0);
            this.controlsLayoutPanel.Controls.Add(this.labelPath, 0, 1);
            this.controlsLayoutPanel.Controls.Add(this.pathTextBox, 1, 1);
            this.controlsLayoutPanel.Controls.Add(this.pathBrowseButton, 2, 1);
            this.controlsLayoutPanel.Controls.Add(this.labelSolution, 0, 2);
            this.controlsLayoutPanel.Controls.Add(this.solutionComboBox, 1, 2);
            this.controlsLayoutPanel.Controls.Add(this.labelSolutionName, 0, 3);
            this.controlsLayoutPanel.Controls.Add(this.solutionNameTextBox, 1, 3);
            this.controlsLayoutPanel.Controls.Add(okCancelTLP, 2, 4);
            this.controlsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlsLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.controlsLayoutPanel.Name = "controlsLayoutPanel";
            this.controlsLayoutPanel.RowCount = 5;
            this.controlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.controlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.controlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.controlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.controlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.controlsLayoutPanel.Size = new System.Drawing.Size(778, 144);
            this.controlsLayoutPanel.TabIndex = 0;
            // 
            // solutionNewDirCheckBox
            // 
            this.solutionNewDirCheckBox.AutoSize = true;
            this.solutionNewDirCheckBox.Checked = true;
            this.solutionNewDirCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.solutionNewDirCheckBox.Location = new System.Drawing.Point(585, 85);
            this.solutionNewDirCheckBox.Name = "solutionNewDirCheckBox";
            this.solutionNewDirCheckBox.Size = new System.Drawing.Size(154, 17);
            this.solutionNewDirCheckBox.TabIndex = 0;
            this.solutionNewDirCheckBox.Text = "Create directory for solution";
            this.solutionNewDirCheckBox.UseVisualStyleBackColor = true;
            // 
            // nameTextBox
            // 
            this.nameTextBox.BorderColor = System.Drawing.Color.Empty;
            this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameTextBox.Location = new System.Drawing.Point(86, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(493, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Leave += new System.EventHandler(this.NameTextBox_Leave);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(77, 26);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name:";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPath.Location = new System.Drawing.Point(3, 26);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(77, 29);
            this.labelPath.TabIndex = 3;
            this.labelPath.Text = "Location:";
            // 
            // pathTextBox
            // 
            this.pathTextBox.BorderColor = System.Drawing.Color.Empty;
            this.pathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathTextBox.Location = new System.Drawing.Point(86, 29);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(493, 20);
            this.pathTextBox.TabIndex = 4;
            // 
            // pathBrowseButton
            // 
            this.pathBrowseButton.Location = new System.Drawing.Point(585, 29);
            this.pathBrowseButton.Name = "pathBrowseButton";
            this.pathBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.pathBrowseButton.TabIndex = 5;
            this.pathBrowseButton.Text = "Browse";
            this.pathBrowseButton.UseVisualStyleBackColor = true;
            // 
            // labelSolution
            // 
            this.labelSolution.AutoSize = true;
            this.labelSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSolution.Location = new System.Drawing.Point(3, 55);
            this.labelSolution.Name = "labelSolution";
            this.labelSolution.Size = new System.Drawing.Size(77, 27);
            this.labelSolution.TabIndex = 6;
            this.labelSolution.Text = "Solution:";
            // 
            // solutionComboBox
            // 
            this.solutionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.solutionComboBox.FormattingEnabled = true;
            this.solutionComboBox.Items.AddRange(new object[] {
            "Create new solution",
            "Add to solution"});
            this.solutionComboBox.Location = new System.Drawing.Point(86, 58);
            this.solutionComboBox.Name = "solutionComboBox";
            this.solutionComboBox.Size = new System.Drawing.Size(493, 21);
            this.solutionComboBox.TabIndex = 7;
            // 
            // labelSolutionName
            // 
            this.labelSolutionName.AutoSize = true;
            this.labelSolutionName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSolutionName.Location = new System.Drawing.Point(3, 82);
            this.labelSolutionName.Name = "labelSolutionName";
            this.labelSolutionName.Size = new System.Drawing.Size(77, 26);
            this.labelSolutionName.TabIndex = 8;
            this.labelSolutionName.Text = "Solution name:";
            // 
            // solutionNameTextBox
            // 
            this.solutionNameTextBox.BorderColor = System.Drawing.Color.Empty;
            this.solutionNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionNameTextBox.Location = new System.Drawing.Point(86, 85);
            this.solutionNameTextBox.Name = "solutionNameTextBox";
            this.solutionNameTextBox.Size = new System.Drawing.Size(493, 20);
            this.solutionNameTextBox.TabIndex = 9;
            this.solutionNameTextBox.Leave += new System.EventHandler(this.NameTextBox_Leave);
            // 
            // okCancelTLP
            // 
            okCancelTLP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            okCancelTLP.ColumnCount = 2;
            okCancelTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            okCancelTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            okCancelTLP.Controls.Add(this.okButton, 0, 0);
            okCancelTLP.Controls.Add(this.cancelButton, 1, 0);
            okCancelTLP.Location = new System.Drawing.Point(585, 111);
            okCancelTLP.Name = "okCancelTLP";
            okCancelTLP.RowCount = 1;
            okCancelTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            okCancelTLP.Size = new System.Drawing.Size(190, 30);
            okCancelTLP.TabIndex = 10;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(3, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(89, 24);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(98, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(89, 24);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchButton,
            this.toolStripTextBox1});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(784, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // searchButton
            // 
            this.searchButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.searchButton.AutoToolTip = false;
            this.searchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
            this.searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(23, 22);
            this.searchButton.ToolTipText = "Search";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(140, 25);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // NewProjectWindow
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 483);
            this.Controls.Add(mainTLP);
            this.Controls.Add(this.mainToolStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "New Project";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewProjectWindow_FormClosing);
            mainTLP.ResumeLayout(false);
            this.templateDataLayoutPanel.ResumeLayout(false);
            this.templateDataLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.controlsLayoutPanel.ResumeLayout(false);
            this.controlsLayoutPanel.PerformLayout();
            okCancelTLP.ResumeLayout(false);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton searchButton;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vs2015DarkTheme;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TableLayoutPanel controlsLayoutPanel;
        private System.Windows.Forms.CheckBox solutionNewDirCheckBox;
        private Controls.IDETextBox nameTextBox;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelPath;
        private Controls.IDETextBox pathTextBox;
        private System.Windows.Forms.Button pathBrowseButton;
        private System.Windows.Forms.Label labelSolution;
        private System.Windows.Forms.ComboBox solutionComboBox;
        private System.Windows.Forms.Label labelSolutionName;
        private Controls.IDETextBox solutionNameTextBox;
        private System.Windows.Forms.TableLayoutPanel templateDataLayoutPanel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewImageColumn icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn tag;
        private Controls.IDETextBox templateDescTextBox;
    }
}