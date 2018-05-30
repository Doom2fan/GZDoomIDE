namespace GZDoomIDE.Windows {
    partial class MainWindow {
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
            this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.status_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.file_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNew_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNewProject_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNewFile_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOpen_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOpenProject_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOpenFile_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSave_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSaveAs_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSaveAll_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileClose_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileCloseProject_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileExit_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edit_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.find_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewProjExpl_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewErrorList_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSyntax_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goto_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.help_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpAbout_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openWorkspaceDialog = new System.Windows.Forms.OpenFileDialog();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mainStatusStrip.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(183, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // mainDockPanel
            // 
            this.mainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.mainDockPanel.Location = new System.Drawing.Point(0, 24);
            this.mainDockPanel.Name = "mainDockPanel";
            this.mainDockPanel.ShowAutoHideContentOnHover = false;
            this.mainDockPanel.Size = new System.Drawing.Size(624, 395);
            this.mainDockPanel.TabIndex = 0;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_StatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 419);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(624, 22);
            this.mainStatusStrip.TabIndex = 2;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // status_StatusLabel
            // 
            this.status_StatusLabel.Name = "status_StatusLabel";
            this.status_StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_MenuItem,
            this.edit_MenuItem,
            this.find_MenuItem,
            this.view_MenuItem,
            this.goto_MenuItem,
            this.help_MenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(624, 24);
            this.mainMenuStrip.TabIndex = 3;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // file_MenuItem
            // 
            this.file_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNew_MenuItem,
            this.fileOpen_MenuItem,
            toolStripSeparator2,
            this.fileSave_MenuItem,
            this.fileSaveAs_MenuItem,
            this.fileSaveAll_MenuItem,
            toolStripSeparator3,
            this.fileClose_MenuItem,
            this.fileCloseProject_MenuItem,
            toolStripSeparator1,
            this.fileExit_MenuItem});
            this.file_MenuItem.Name = "file_MenuItem";
            this.file_MenuItem.Size = new System.Drawing.Size(37, 20);
            this.file_MenuItem.Text = "&File";
            // 
            // fileNew_MenuItem
            // 
            this.fileNew_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNewProject_MenuItem,
            this.fileNewFile_MenuItem});
            this.fileNew_MenuItem.Name = "fileNew_MenuItem";
            this.fileNew_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileNew_MenuItem.Text = "&New";
            // 
            // fileNewProject_MenuItem
            // 
            this.fileNewProject_MenuItem.Name = "fileNewProject_MenuItem";
            this.fileNewProject_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.fileNewProject_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileNewProject_MenuItem.Text = "&Project";
            this.fileNewProject_MenuItem.Click += new System.EventHandler(this.FileNewProject_MenuItem_Click);
            // 
            // fileNewFile_MenuItem
            // 
            this.fileNewFile_MenuItem.Name = "fileNewFile_MenuItem";
            this.fileNewFile_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.fileNewFile_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileNewFile_MenuItem.Text = "&File";
            this.fileNewFile_MenuItem.Click += new System.EventHandler(this.FileNewFile_MenuItem_Click);
            // 
            // fileOpen_MenuItem
            // 
            this.fileOpen_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOpenProject_MenuItem,
            this.fileOpenFile_MenuItem});
            this.fileOpen_MenuItem.Name = "fileOpen_MenuItem";
            this.fileOpen_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileOpen_MenuItem.Text = "&Open";
            // 
            // fileOpenProject_MenuItem
            // 
            this.fileOpenProject_MenuItem.Name = "fileOpenProject_MenuItem";
            this.fileOpenProject_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.fileOpenProject_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileOpenProject_MenuItem.Text = "&Project";
            this.fileOpenProject_MenuItem.Click += new System.EventHandler(this.FileOpenProject_MenuItem_Click);
            // 
            // fileOpenFile_MenuItem
            // 
            this.fileOpenFile_MenuItem.Name = "fileOpenFile_MenuItem";
            this.fileOpenFile_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.fileOpenFile_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileOpenFile_MenuItem.Text = "&File";
            this.fileOpenFile_MenuItem.Click += new System.EventHandler(this.FileOpenFile_MenuItem_Click);
            // 
            // fileSave_MenuItem
            // 
            this.fileSave_MenuItem.Name = "fileSave_MenuItem";
            this.fileSave_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.fileSave_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileSave_MenuItem.Text = "&Save";
            this.fileSave_MenuItem.Click += new System.EventHandler(this.FileSave_MenuItem_Click);
            // 
            // fileSaveAs_MenuItem
            // 
            this.fileSaveAs_MenuItem.Name = "fileSaveAs_MenuItem";
            this.fileSaveAs_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.fileSaveAs_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileSaveAs_MenuItem.Text = "Save &As";
            this.fileSaveAs_MenuItem.Click += new System.EventHandler(this.FileSaveAs_MenuItem_Click);
            // 
            // fileSaveAll_MenuItem
            // 
            this.fileSaveAll_MenuItem.Name = "fileSaveAll_MenuItem";
            this.fileSaveAll_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileSaveAll_MenuItem.Text = "Save A&ll";
            // 
            // fileClose_MenuItem
            // 
            this.fileClose_MenuItem.Name = "fileClose_MenuItem";
            this.fileClose_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.fileClose_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileClose_MenuItem.Text = "&Close";
            this.fileClose_MenuItem.Click += new System.EventHandler(this.FileClose_MenuItem_Click);
            // 
            // fileCloseProject_MenuItem
            // 
            this.fileCloseProject_MenuItem.Name = "fileCloseProject_MenuItem";
            this.fileCloseProject_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileCloseProject_MenuItem.Text = "Close project";
            // 
            // fileExit_MenuItem
            // 
            this.fileExit_MenuItem.Name = "fileExit_MenuItem";
            this.fileExit_MenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.fileExit_MenuItem.Size = new System.Drawing.Size(186, 22);
            this.fileExit_MenuItem.Text = "E&xit";
            this.fileExit_MenuItem.Click += new System.EventHandler(this.FileExit_MenuItem_Click);
            // 
            // edit_MenuItem
            // 
            this.edit_MenuItem.Name = "edit_MenuItem";
            this.edit_MenuItem.Size = new System.Drawing.Size(39, 20);
            this.edit_MenuItem.Text = "&Edit";
            // 
            // find_MenuItem
            // 
            this.find_MenuItem.Name = "find_MenuItem";
            this.find_MenuItem.Size = new System.Drawing.Size(42, 20);
            this.find_MenuItem.Text = "&Find";
            // 
            // view_MenuItem
            // 
            this.view_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewProjExpl_MenuItem,
            this.viewErrorList_MenuItem,
            toolStripSeparator4,
            this.viewSyntax_MenuItem});
            this.view_MenuItem.Name = "view_MenuItem";
            this.view_MenuItem.Size = new System.Drawing.Size(44, 20);
            this.view_MenuItem.Text = "&View";
            // 
            // viewProjExpl_MenuItem
            // 
            this.viewProjExpl_MenuItem.Name = "viewProjExpl_MenuItem";
            this.viewProjExpl_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewProjExpl_MenuItem.Text = "&Project Explorer";
            this.viewProjExpl_MenuItem.Click += new System.EventHandler(this.ViewProjExpl_MenuItem_Click);
            // 
            // viewErrorList_MenuItem
            // 
            this.viewErrorList_MenuItem.Name = "viewErrorList_MenuItem";
            this.viewErrorList_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewErrorList_MenuItem.Text = "&Error List";
            this.viewErrorList_MenuItem.Click += new System.EventHandler(this.ViewErrorList_MenuItem_Click);
            // 
            // viewSyntax_MenuItem
            // 
            this.viewSyntax_MenuItem.Name = "viewSyntax_MenuItem";
            this.viewSyntax_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewSyntax_MenuItem.Text = "&Syntax";
            // 
            // goto_MenuItem
            // 
            this.goto_MenuItem.Name = "goto_MenuItem";
            this.goto_MenuItem.Size = new System.Drawing.Size(45, 20);
            this.goto_MenuItem.Text = "&Goto";
            // 
            // help_MenuItem
            // 
            this.help_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpAbout_MenuItem});
            this.help_MenuItem.Name = "help_MenuItem";
            this.help_MenuItem.Size = new System.Drawing.Size(44, 20);
            this.help_MenuItem.Text = "Help";
            // 
            // helpAbout_MenuItem
            // 
            this.helpAbout_MenuItem.Name = "helpAbout_MenuItem";
            this.helpAbout_MenuItem.Size = new System.Drawing.Size(107, 22);
            this.helpAbout_MenuItem.Text = "&About";
            this.helpAbout_MenuItem.Click += new System.EventHandler(this.HelpAbout_MenuItem_Click);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(150, 150);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.SupportMultiDottedExtensions = true;
            this.saveFileDialog.Title = "Save as";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.SupportMultiDottedExtensions = true;
            this.openFileDialog.Title = "Open file";
            // 
            // openWorkspaceDialog
            // 
            this.openWorkspaceDialog.Filter = "Workspace files|*.gzidewsp|All files|*.*";
            this.openWorkspaceDialog.SupportMultiDottedExtensions = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.mainDockPanel);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.mainStatusStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainWindow";
            this.Text = "GZDoom IDE";
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem file_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileNew_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem edit_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem view_MenuItem;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripMenuItem fileNewProject_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileNewFile_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewProjExpl_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileExit_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileClose_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileCloseProject_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileOpen_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileOpenProject_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileOpenFile_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileSave_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileSaveAs_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileSaveAll_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem find_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem goto_MenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel status_StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem viewErrorList_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem help_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpAbout_MenuItem;
        private System.Windows.Forms.OpenFileDialog openWorkspaceDialog;
        private System.Windows.Forms.ToolStripMenuItem viewSyntax_MenuItem;
    }
}

