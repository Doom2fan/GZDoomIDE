namespace GZDoomIDE.Windows {
    partial class ErrorList {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.vs2015DarkTheme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.proj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.file = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ideErrorListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ideErrorListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(684, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icon,
            this.desc,
            this.proj,
            this.file,
            this.line,
            this.column});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(0, 25);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.Size = new System.Drawing.Size(684, 186);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellContentDoubleClick);
            this.dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView_CellFormatting);
            // 
            // icon
            // 
            this.icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.icon.DataPropertyName = "Type";
            this.icon.FillWeight = 5F;
            this.icon.HeaderText = "";
            this.icon.Name = "icon";
            this.icon.ReadOnly = true;
            this.icon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.icon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.icon.Width = 5;
            // 
            // desc
            // 
            this.desc.DataPropertyName = "Message";
            this.desc.FillWeight = 55F;
            this.desc.HeaderText = "Description";
            this.desc.MaxInputLength = 2147483647;
            this.desc.Name = "desc";
            this.desc.ReadOnly = true;
            // 
            // proj
            // 
            this.proj.DataPropertyName = "Project";
            this.proj.FillWeight = 15F;
            this.proj.HeaderText = "Project";
            this.proj.Name = "proj";
            this.proj.ReadOnly = true;
            // 
            // file
            // 
            this.file.DataPropertyName = "File";
            this.file.FillWeight = 17F;
            this.file.HeaderText = "File";
            this.file.Name = "file";
            this.file.ReadOnly = true;
            // 
            // line
            // 
            this.line.DataPropertyName = "LineNum";
            this.line.FillWeight = 8F;
            this.line.HeaderText = "Line";
            this.line.Name = "line";
            this.line.ReadOnly = true;
            // 
            // column
            // 
            this.column.DataPropertyName = "ColumnNum";
            this.column.FillWeight = 5F;
            this.column.HeaderText = "Column";
            this.column.Name = "column";
            this.column.ReadOnly = true;
            // 
            // ideErrorListBindingSource
            // 
            this.ideErrorListBindingSource.AllowNew = true;
            this.ideErrorListBindingSource.DataSource = typeof(GZDoomIDE.Data.IDEError);
            // 
            // ErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 211);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Name = "ErrorList";
            this.Text = "Error List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ErrorList_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ideErrorListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vs2015DarkTheme;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource ideErrorListBindingSource;
        private System.Windows.Forms.DataGridViewImageColumn icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn proj;
        private System.Windows.Forms.DataGridViewTextBoxColumn file;
        private System.Windows.Forms.DataGridViewTextBoxColumn line;
        private System.Windows.Forms.DataGridViewTextBoxColumn column;
    }
}