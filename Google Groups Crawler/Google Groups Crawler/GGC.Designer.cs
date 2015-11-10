namespace Google_Groups_Crawler
{
    partial class GGC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GGC));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEditPostsContent = new System.Windows.Forms.Button();
            this.buttonEditPostsEmail = new System.Windows.Forms.Button();
            this.buttonEditPostsID = new System.Windows.Forms.Button();
            this.groupBoxGoogleGroups = new System.Windows.Forms.GroupBox();
            this.buttonFile = new System.Windows.Forms.Button();
            this.numericUpDownStartAt = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.labelDelay = new System.Windows.Forms.Label();
            this.labelGroupID = new System.Windows.Forms.Label();
            this.textBoxGroupID = new System.Windows.Forms.TextBox();
            this.buttonReadTopics = new System.Windows.Forms.Button();
            this.buttonLoadList = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.webBrowserGoogleGroups = new System.Windows.Forms.WebBrowser();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonEditPostsDate = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxGoogleGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(498, 451);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBoxGoogleGroups);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(490, 425);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 149);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(477, 255);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonEditPostsDate);
            this.groupBox1.Controls.Add(this.buttonEditPostsContent);
            this.groupBox1.Controls.Add(this.buttonEditPostsEmail);
            this.groupBox1.Controls.Add(this.buttonEditPostsID);
            this.groupBox1.Location = new System.Drawing.Point(231, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 137);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Google Groups - Editor";
            // 
            // buttonEditPostsContent
            // 
            this.buttonEditPostsContent.Location = new System.Drawing.Point(18, 81);
            this.buttonEditPostsContent.Name = "buttonEditPostsContent";
            this.buttonEditPostsContent.Size = new System.Drawing.Size(113, 23);
            this.buttonEditPostsContent.TabIndex = 2;
            this.buttonEditPostsContent.Text = "Edit Posts - Content";
            this.buttonEditPostsContent.UseVisualStyleBackColor = true;
            this.buttonEditPostsContent.Click += new System.EventHandler(this.buttonEditPostsContent_Click);
            // 
            // buttonEditPostsEmail
            // 
            this.buttonEditPostsEmail.Location = new System.Drawing.Point(18, 52);
            this.buttonEditPostsEmail.Name = "buttonEditPostsEmail";
            this.buttonEditPostsEmail.Size = new System.Drawing.Size(113, 23);
            this.buttonEditPostsEmail.TabIndex = 1;
            this.buttonEditPostsEmail.Text = "Edit Posts - Email";
            this.buttonEditPostsEmail.UseVisualStyleBackColor = true;
            this.buttonEditPostsEmail.Click += new System.EventHandler(this.buttonEditPostsEmail_Click);
            // 
            // buttonEditPostsID
            // 
            this.buttonEditPostsID.Location = new System.Drawing.Point(18, 23);
            this.buttonEditPostsID.Name = "buttonEditPostsID";
            this.buttonEditPostsID.Size = new System.Drawing.Size(113, 23);
            this.buttonEditPostsID.TabIndex = 0;
            this.buttonEditPostsID.Text = "Edit Posts - ID";
            this.buttonEditPostsID.UseVisualStyleBackColor = true;
            this.buttonEditPostsID.Click += new System.EventHandler(this.buttonEditPostsID_Click);
            // 
            // groupBoxGoogleGroups
            // 
            this.groupBoxGoogleGroups.Controls.Add(this.buttonFile);
            this.groupBoxGoogleGroups.Controls.Add(this.numericUpDownStartAt);
            this.groupBoxGoogleGroups.Controls.Add(this.numericUpDownDelay);
            this.groupBoxGoogleGroups.Controls.Add(this.labelDelay);
            this.groupBoxGoogleGroups.Controls.Add(this.labelGroupID);
            this.groupBoxGoogleGroups.Controls.Add(this.textBoxGroupID);
            this.groupBoxGoogleGroups.Controls.Add(this.buttonReadTopics);
            this.groupBoxGoogleGroups.Controls.Add(this.buttonLoadList);
            this.groupBoxGoogleGroups.Location = new System.Drawing.Point(6, 6);
            this.groupBoxGoogleGroups.Name = "groupBoxGoogleGroups";
            this.groupBoxGoogleGroups.Size = new System.Drawing.Size(219, 137);
            this.groupBoxGoogleGroups.TabIndex = 2;
            this.groupBoxGoogleGroups.TabStop = false;
            this.groupBoxGoogleGroups.Text = "Google Groups - Crawler";
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(71, 75);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(33, 23);
            this.buttonFile.TabIndex = 11;
            this.buttonFile.Text = "File";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // numericUpDownStartAt
            // 
            this.numericUpDownStartAt.Location = new System.Drawing.Point(110, 100);
            this.numericUpDownStartAt.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownStartAt.Name = "numericUpDownStartAt";
            this.numericUpDownStartAt.Size = new System.Drawing.Size(97, 20);
            this.numericUpDownStartAt.TabIndex = 10;
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Location = new System.Drawing.Point(110, 50);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownDelay.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(97, 20);
            this.numericUpDownDelay.TabIndex = 6;
            this.numericUpDownDelay.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownDelay.ValueChanged += new System.EventHandler(this.numericUpDownDelay_ValueChanged);
            // 
            // labelDelay
            // 
            this.labelDelay.AutoSize = true;
            this.labelDelay.Location = new System.Drawing.Point(10, 50);
            this.labelDelay.Name = "labelDelay";
            this.labelDelay.Size = new System.Drawing.Size(34, 13);
            this.labelDelay.TabIndex = 5;
            this.labelDelay.Text = "Delay";
            // 
            // labelGroupID
            // 
            this.labelGroupID.AutoSize = true;
            this.labelGroupID.Location = new System.Drawing.Point(10, 25);
            this.labelGroupID.Name = "labelGroupID";
            this.labelGroupID.Size = new System.Drawing.Size(50, 13);
            this.labelGroupID.TabIndex = 2;
            this.labelGroupID.Text = "Group ID";
            // 
            // textBoxGroupID
            // 
            this.textBoxGroupID.Location = new System.Drawing.Point(110, 25);
            this.textBoxGroupID.Name = "textBoxGroupID";
            this.textBoxGroupID.Size = new System.Drawing.Size(97, 20);
            this.textBoxGroupID.TabIndex = 1;
            this.textBoxGroupID.Text = "ggplot2";
            // 
            // buttonReadTopics
            // 
            this.buttonReadTopics.Location = new System.Drawing.Point(13, 100);
            this.buttonReadTopics.Name = "buttonReadTopics";
            this.buttonReadTopics.Size = new System.Drawing.Size(91, 23);
            this.buttonReadTopics.TabIndex = 5;
            this.buttonReadTopics.Text = "Read Topics";
            this.buttonReadTopics.UseVisualStyleBackColor = true;
            this.buttonReadTopics.Click += new System.EventHandler(this.buttonReadTopics_Click);
            // 
            // buttonLoadList
            // 
            this.buttonLoadList.Location = new System.Drawing.Point(13, 75);
            this.buttonLoadList.Name = "buttonLoadList";
            this.buttonLoadList.Size = new System.Drawing.Size(59, 23);
            this.buttonLoadList.TabIndex = 4;
            this.buttonLoadList.Text = "Load List";
            this.buttonLoadList.UseVisualStyleBackColor = true;
            this.buttonLoadList.Click += new System.EventHandler(this.buttonLoadList_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.webBrowserGoogleGroups);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(490, 425);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Browser";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // webBrowserGoogleGroups
            // 
            this.webBrowserGoogleGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserGoogleGroups.Location = new System.Drawing.Point(3, 3);
            this.webBrowserGoogleGroups.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserGoogleGroups.Name = "webBrowserGoogleGroups";
            this.webBrowserGoogleGroups.ScriptErrorsSuppressed = true;
            this.webBrowserGoogleGroups.Size = new System.Drawing.Size(484, 419);
            this.webBrowserGoogleGroups.TabIndex = 0;
            this.webBrowserGoogleGroups.Url = new System.Uri("https://groups.google.com/forum/m/#!overview", System.UriKind.Absolute);
            this.webBrowserGoogleGroups.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserGoogleGroups_DocumentCompleted);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 429);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(498, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(71, 17);
            this.toolStripStatusLabel1.Text = "Current URL";
            // 
            // buttonEditPostsDate
            // 
            this.buttonEditPostsDate.Location = new System.Drawing.Point(18, 110);
            this.buttonEditPostsDate.Name = "buttonEditPostsDate";
            this.buttonEditPostsDate.Size = new System.Drawing.Size(113, 23);
            this.buttonEditPostsDate.TabIndex = 3;
            this.buttonEditPostsDate.Text = "Edit Posts - Date";
            this.buttonEditPostsDate.UseVisualStyleBackColor = true;
            this.buttonEditPostsDate.Click += new System.EventHandler(this.buttonEditPostsDate_Click);
            // 
            // GGC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 451);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "GGC";
            this.Text = "Google Groups Crawler";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBoxGoogleGroups.ResumeLayout(false);
            this.groupBoxGoogleGroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser webBrowserGoogleGroups;
        private System.Windows.Forms.GroupBox groupBoxGoogleGroups;
        private System.Windows.Forms.Label labelGroupID;
        private System.Windows.Forms.TextBox textBoxGroupID;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Label labelDelay;
        private System.Windows.Forms.Button buttonLoadList;
        private System.Windows.Forms.Button buttonReadTopics;
        private System.Windows.Forms.NumericUpDown numericUpDownStartAt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Button buttonEditPostsContent;
        private System.Windows.Forms.Button buttonEditPostsEmail;
        private System.Windows.Forms.Button buttonEditPostsID;
        private System.Windows.Forms.Button buttonEditPostsDate;
    }
}

