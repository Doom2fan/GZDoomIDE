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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            System.Windows.Forms.Panel topPanel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectWindow));
            this.templateDescTextBox = new System.Windows.Forms.TextBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.smallIconsButton = new System.Windows.Forms.ToolStripButton();
            this.bigIconsButton = new System.Windows.Forms.ToolStripButton();
            this.searchButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.vs2015DarkTheme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.templatesListView = new System.Windows.Forms.ListView();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            topPanel = new System.Windows.Forms.Panel();
            tableLayoutPanel.SuspendLayout();
            topPanel.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(topPanel, 0, 0);
            tableLayoutPanel.Controls.Add(this.bottomPanel, 0, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 25);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanel.Size = new System.Drawing.Size(784, 436);
            tableLayoutPanel.TabIndex = 0;
            // 
            // topPanel
            // 
            topPanel.Controls.Add(this.templateDescTextBox);
            topPanel.Controls.Add(this.templatesListView);
            topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            topPanel.Location = new System.Drawing.Point(3, 3);
            topPanel.Name = "topPanel";
            topPanel.Size = new System.Drawing.Size(778, 299);
            topPanel.TabIndex = 0;
            // 
            // templateDescTextBox
            // 
            this.templateDescTextBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.templateDescTextBox.Location = new System.Drawing.Point(614, 0);
            this.templateDescTextBox.MaxLength = 2147483647;
            this.templateDescTextBox.Multiline = true;
            this.templateDescTextBox.Name = "templateDescTextBox";
            this.templateDescTextBox.ReadOnly = true;
            this.templateDescTextBox.Size = new System.Drawing.Size(164, 299);
            this.templateDescTextBox.TabIndex = 2;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.Location = new System.Drawing.Point(3, 308);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(778, 125);
            this.bottomPanel.TabIndex = 1;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smallIconsButton,
            this.bigIconsButton,
            this.searchButton,
            this.toolStripTextBox1});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(784, 25);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // smallIconsButton
            // 
            this.smallIconsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.smallIconsButton.Image = ((System.Drawing.Image)(resources.GetObject("smallIconsButton.Image")));
            this.smallIconsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.smallIconsButton.Name = "smallIconsButton";
            this.smallIconsButton.Size = new System.Drawing.Size(23, 22);
            this.smallIconsButton.ToolTipText = "Small icons";
            this.smallIconsButton.Click += new System.EventHandler(this.ViewSizeButton_Click);
            // 
            // bigIconsButton
            // 
            this.bigIconsButton.Checked = true;
            this.bigIconsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bigIconsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bigIconsButton.Image = ((System.Drawing.Image)(resources.GetObject("bigIconsButton.Image")));
            this.bigIconsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bigIconsButton.Name = "bigIconsButton";
            this.bigIconsButton.Size = new System.Drawing.Size(23, 22);
            this.bigIconsButton.Tag = "";
            this.bigIconsButton.ToolTipText = "Big icons";
            this.bigIconsButton.Click += new System.EventHandler(this.ViewSizeButton_Click);
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
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // templatesListView
            // 
            this.templatesListView.Dock = System.Windows.Forms.DockStyle.Left;
            this.templatesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.templatesListView.Location = new System.Drawing.Point(0, 0);
            this.templatesListView.Name = "templatesListView";
            this.templatesListView.Size = new System.Drawing.Size(614, 299);
            this.templatesListView.TabIndex = 1;
            this.templatesListView.UseCompatibleStateImageBehavior = false;
            // 
            // NewProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(tableLayoutPanel);
            this.Controls.Add(this.mainToolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "NewProjectWindow";
            this.Text = "New Project";
            tableLayoutPanel.ResumeLayout(false);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripButton smallIconsButton;
        private System.Windows.Forms.ToolStripButton bigIconsButton;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton searchButton;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vs2015DarkTheme;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.TextBox templateDescTextBox;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.ListView templatesListView;
    }
}