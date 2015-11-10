namespace Stack_Overflow_Parser
{
    partial class SOParser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SOParser));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxTag = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonParse = new System.Windows.Forms.Button();
            this.buttonParseUsers = new System.Windows.Forms.Button();
            this.buttonEmailHash = new System.Windows.Forms.Button();
            this.textBoxHelp = new System.Windows.Forms.TextBox();
            this.buttonStats = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonSplit = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxTag
            // 
            this.textBoxTag.Location = new System.Drawing.Point(10, 30);
            this.textBoxTag.Name = "textBoxTag";
            this.textBoxTag.Size = new System.Drawing.Size(318, 20);
            this.textBoxTag.TabIndex = 1;
            this.textBoxTag.Text = "<ggplot2>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filter:";
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(10, 56);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(75, 23);
            this.buttonParse.TabIndex = 3;
            this.buttonParse.Text = "Posts";
            this.buttonParse.UseVisualStyleBackColor = true;
            this.buttonParse.Click += new System.EventHandler(this.buttonParse_Click);
            // 
            // buttonParseUsers
            // 
            this.buttonParseUsers.Location = new System.Drawing.Point(91, 56);
            this.buttonParseUsers.Name = "buttonParseUsers";
            this.buttonParseUsers.Size = new System.Drawing.Size(75, 23);
            this.buttonParseUsers.TabIndex = 4;
            this.buttonParseUsers.Text = "Users";
            this.buttonParseUsers.UseVisualStyleBackColor = true;
            this.buttonParseUsers.Click += new System.EventHandler(this.buttonParseUsers_Click);
            // 
            // buttonEmailHash
            // 
            this.buttonEmailHash.Location = new System.Drawing.Point(172, 56);
            this.buttonEmailHash.Name = "buttonEmailHash";
            this.buttonEmailHash.Size = new System.Drawing.Size(75, 23);
            this.buttonEmailHash.TabIndex = 5;
            this.buttonEmailHash.Text = "Email Hash";
            this.buttonEmailHash.UseVisualStyleBackColor = true;
            this.buttonEmailHash.Click += new System.EventHandler(this.buttonEmailHash_Click);
            // 
            // textBoxHelp
            // 
            this.textBoxHelp.Location = new System.Drawing.Point(13, 114);
            this.textBoxHelp.Multiline = true;
            this.textBoxHelp.Name = "textBoxHelp";
            this.textBoxHelp.Size = new System.Drawing.Size(315, 246);
            this.textBoxHelp.TabIndex = 6;
            this.textBoxHelp.Text = resources.GetString("textBoxHelp.Text");
            // 
            // buttonStats
            // 
            this.buttonStats.Location = new System.Drawing.Point(253, 56);
            this.buttonStats.Name = "buttonStats";
            this.buttonStats.Size = new System.Drawing.Size(75, 23);
            this.buttonStats.TabIndex = 7;
            this.buttonStats.Text = "Stats";
            this.buttonStats.UseVisualStyleBackColor = true;
            this.buttonStats.Click += new System.EventHandler(this.buttonStats_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 363);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(339, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // buttonSplit
            // 
            this.buttonSplit.Location = new System.Drawing.Point(10, 85);
            this.buttonSplit.Name = "buttonSplit";
            this.buttonSplit.Size = new System.Drawing.Size(75, 23);
            this.buttonSplit.TabIndex = 9;
            this.buttonSplit.Text = "Split";
            this.buttonSplit.UseVisualStyleBackColor = true;
            this.buttonSplit.Click += new System.EventHandler(this.buttonSplit_Click);
            // 
            // SOParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 385);
            this.Controls.Add(this.buttonSplit);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonStats);
            this.Controls.Add(this.textBoxHelp);
            this.Controls.Add(this.buttonEmailHash);
            this.Controls.Add(this.buttonParseUsers);
            this.Controls.Add(this.buttonParse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTag);
            this.Name = "SOParser";
            this.Text = "Stack Overflow Parser";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonParse;
        private System.Windows.Forms.Button buttonParseUsers;
        private System.Windows.Forms.Button buttonEmailHash;
        private System.Windows.Forms.TextBox textBoxHelp;
        private System.Windows.Forms.Button buttonStats;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button buttonSplit;
    }
}

