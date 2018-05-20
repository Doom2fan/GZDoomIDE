namespace GZDoomIDE.Windows {
    partial class SplashScreen {
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
            this.totalProgressBar = new System.Windows.Forms.ProgressBar();
            this.operationLabel = new System.Windows.Forms.Label();
            this.workLabel = new System.Windows.Forms.Label();
            this.workProgressBar = new System.Windows.Forms.ProgressBar();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // totalProgressBar
            // 
            this.totalProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.totalProgressBar.Location = new System.Drawing.Point(12, 149);
            this.totalProgressBar.Maximum = 1000;
            this.totalProgressBar.Name = "totalProgressBar";
            this.totalProgressBar.Size = new System.Drawing.Size(326, 20);
            this.totalProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.totalProgressBar.TabIndex = 1;
            // 
            // operationLabel
            // 
            this.operationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operationLabel.Location = new System.Drawing.Point(12, 133);
            this.operationLabel.Name = "operationLabel";
            this.operationLabel.Size = new System.Drawing.Size(326, 13);
            this.operationLabel.TabIndex = 2;
            this.operationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // workLabel
            // 
            this.workLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workLabel.Location = new System.Drawing.Point(12, 172);
            this.workLabel.Name = "workLabel";
            this.workLabel.Size = new System.Drawing.Size(326, 13);
            this.workLabel.TabIndex = 4;
            this.workLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.workLabel.Visible = false;
            // 
            // workProgressBar
            // 
            this.workProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workProgressBar.Location = new System.Drawing.Point(12, 188);
            this.workProgressBar.Maximum = 1000;
            this.workProgressBar.Name = "workProgressBar";
            this.workProgressBar.Size = new System.Drawing.Size(326, 20);
            this.workProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.workProgressBar.TabIndex = 3;
            this.workProgressBar.Visible = false;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logoPictureBox.Image = global::GZDoomIDE.Properties.Resources.SplashScreen;
            this.logoPictureBox.Location = new System.Drawing.Point(12, 12);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(326, 118);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 220);
            this.ControlBox = false;
            this.Controls.Add(this.workLabel);
            this.Controls.Add(this.workProgressBar);
            this.Controls.Add(this.operationLabel);
            this.Controls.Add(this.totalProgressBar);
            this.Controls.Add(this.logoPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.ProgressBar totalProgressBar;
        private System.Windows.Forms.Label operationLabel;
        private System.Windows.Forms.Label workLabel;
        private System.Windows.Forms.ProgressBar workProgressBar;
    }
}