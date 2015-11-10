namespace User_Merger
{
    partial class UserMerger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStackOverflow = new System.Windows.Forms.Button();
            this.buttonGoogleGroups = new System.Windows.Forms.Button();
            this.buttonGitHub = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonStats = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStackOverflow
            // 
            this.buttonStackOverflow.Location = new System.Drawing.Point(13, 13);
            this.buttonStackOverflow.Name = "buttonStackOverflow";
            this.buttonStackOverflow.Size = new System.Drawing.Size(100, 23);
            this.buttonStackOverflow.TabIndex = 0;
            this.buttonStackOverflow.Text = "Stack Overflow";
            this.buttonStackOverflow.UseVisualStyleBackColor = true;
            this.buttonStackOverflow.Click += new System.EventHandler(this.buttonStackOverflow_Click);
            // 
            // buttonGoogleGroups
            // 
            this.buttonGoogleGroups.Location = new System.Drawing.Point(13, 43);
            this.buttonGoogleGroups.Name = "buttonGoogleGroups";
            this.buttonGoogleGroups.Size = new System.Drawing.Size(100, 23);
            this.buttonGoogleGroups.TabIndex = 1;
            this.buttonGoogleGroups.Text = "Google Groups";
            this.buttonGoogleGroups.UseVisualStyleBackColor = true;
            this.buttonGoogleGroups.Click += new System.EventHandler(this.buttonGoogleGroups_Click);
            // 
            // buttonGitHub
            // 
            this.buttonGitHub.Location = new System.Drawing.Point(13, 73);
            this.buttonGitHub.Name = "buttonGitHub";
            this.buttonGitHub.Size = new System.Drawing.Size(100, 23);
            this.buttonGitHub.TabIndex = 2;
            this.buttonGitHub.Text = "GitHub";
            this.buttonGitHub.UseVisualStyleBackColor = true;
            this.buttonGitHub.Click += new System.EventHandler(this.buttonGitHub_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // buttonStats
            // 
            this.buttonStats.Location = new System.Drawing.Point(13, 102);
            this.buttonStats.Name = "buttonStats";
            this.buttonStats.Size = new System.Drawing.Size(100, 23);
            this.buttonStats.TabIndex = 4;
            this.buttonStats.Text = "Stats";
            this.buttonStats.UseVisualStyleBackColor = true;
            this.buttonStats.Click += new System.EventHandler(this.buttonStats_Click);
            // 
            // UserMerger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonStats);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonGitHub);
            this.Controls.Add(this.buttonGoogleGroups);
            this.Controls.Add(this.buttonStackOverflow);
            this.Name = "UserMerger";
            this.Text = "User Merger";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStackOverflow;
        private System.Windows.Forms.Button buttonGoogleGroups;
        private System.Windows.Forms.Button buttonGitHub;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button buttonStats;
    }
}

