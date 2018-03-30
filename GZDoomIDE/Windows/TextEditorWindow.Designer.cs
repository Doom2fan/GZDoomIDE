namespace GZDoomIDE.Windows {
    partial class TextEditorWindow {
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
            this.scintillaControl = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // scintillaControl
            // 
            this.scintillaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaControl.Location = new System.Drawing.Point(0, 0);
            this.scintillaControl.MultipleSelection = true;
            this.scintillaControl.Name = "scintillaControl";
            this.scintillaControl.Size = new System.Drawing.Size(284, 261);
            this.scintillaControl.TabIndex = 0;
            this.scintillaControl.UseTabs = true;
            this.scintillaControl.SavePointLeft += new System.EventHandler<System.EventArgs>(this.ScintillaControl_SavePointLeft);
            this.scintillaControl.SavePointReached += new System.EventHandler<System.EventArgs>(this.ScintillaControl_SavePointReached);
            this.scintillaControl.TextChanged += new System.EventHandler(this.ScintillaControl_TextChanged);
            // 
            // FileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.scintillaControl);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Name = "FileForm";
            this.ResumeLayout(false);

        }

        #endregion

        protected internal ScintillaNET.Scintilla scintillaControl;
    }
}