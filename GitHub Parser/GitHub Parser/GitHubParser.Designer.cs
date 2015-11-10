namespace GitHub_Parser
{
    partial class GitHubParser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GitHubParser));
            this.buttonGetUserIDs = new System.Windows.Forms.Button();
            this.buttonCreateQuery = new System.Windows.Forms.Button();
            this.buttonUsersJSONtoXML = new System.Windows.Forms.Button();
            this.buttonPostsJSONtoXML = new System.Windows.Forms.Button();
            this.buttonFixJSONFormat = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonStats = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGetUserIDs
            // 
            this.buttonGetUserIDs.Location = new System.Drawing.Point(13, 13);
            this.buttonGetUserIDs.Name = "buttonGetUserIDs";
            this.buttonGetUserIDs.Size = new System.Drawing.Size(100, 23);
            this.buttonGetUserIDs.TabIndex = 0;
            this.buttonGetUserIDs.Text = "Get User IDs";
            this.buttonGetUserIDs.UseVisualStyleBackColor = true;
            this.buttonGetUserIDs.Click += new System.EventHandler(this.buttonGetUserIDs_Click);
            // 
            // buttonCreateQuery
            // 
            this.buttonCreateQuery.Enabled = false;
            this.buttonCreateQuery.Location = new System.Drawing.Point(119, 13);
            this.buttonCreateQuery.Name = "buttonCreateQuery";
            this.buttonCreateQuery.Size = new System.Drawing.Size(100, 23);
            this.buttonCreateQuery.TabIndex = 1;
            this.buttonCreateQuery.Text = "Create Query";
            this.buttonCreateQuery.UseVisualStyleBackColor = true;
            this.buttonCreateQuery.Click += new System.EventHandler(this.buttonCreateQuery_Click);
            // 
            // buttonUsersJSONtoXML
            // 
            this.buttonUsersJSONtoXML.Location = new System.Drawing.Point(13, 71);
            this.buttonUsersJSONtoXML.Name = "buttonUsersJSONtoXML";
            this.buttonUsersJSONtoXML.Size = new System.Drawing.Size(206, 23);
            this.buttonUsersJSONtoXML.TabIndex = 2;
            this.buttonUsersJSONtoXML.Text = "Users - JSON to XML";
            this.buttonUsersJSONtoXML.UseVisualStyleBackColor = true;
            this.buttonUsersJSONtoXML.Click += new System.EventHandler(this.buttonUsersJSONtoXML_Click);
            // 
            // buttonPostsJSONtoXML
            // 
            this.buttonPostsJSONtoXML.Location = new System.Drawing.Point(13, 100);
            this.buttonPostsJSONtoXML.Name = "buttonPostsJSONtoXML";
            this.buttonPostsJSONtoXML.Size = new System.Drawing.Size(206, 23);
            this.buttonPostsJSONtoXML.TabIndex = 3;
            this.buttonPostsJSONtoXML.Text = "Posts - JSON to XML";
            this.buttonPostsJSONtoXML.UseVisualStyleBackColor = true;
            this.buttonPostsJSONtoXML.Click += new System.EventHandler(this.buttonPostsJSONtoXML_Click);
            // 
            // buttonFixJSONFormat
            // 
            this.buttonFixJSONFormat.Location = new System.Drawing.Point(13, 42);
            this.buttonFixJSONFormat.Name = "buttonFixJSONFormat";
            this.buttonFixJSONFormat.Size = new System.Drawing.Size(206, 23);
            this.buttonFixJSONFormat.TabIndex = 4;
            this.buttonFixJSONFormat.Text = "Fix JSON format";
            this.buttonFixJSONFormat.UseVisualStyleBackColor = true;
            this.buttonFixJSONFormat.Click += new System.EventHandler(this.buttonFixJSONFormat_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 162);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(329, 312);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // buttonStats
            // 
            this.buttonStats.Enabled = false;
            this.buttonStats.Location = new System.Drawing.Point(13, 133);
            this.buttonStats.Name = "buttonStats";
            this.buttonStats.Size = new System.Drawing.Size(206, 23);
            this.buttonStats.TabIndex = 6;
            this.buttonStats.Text = "Stats";
            this.buttonStats.UseVisualStyleBackColor = true;
            this.buttonStats.Click += new System.EventHandler(this.buttonStats_Click);
            // 
            // GitHubParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 478);
            this.Controls.Add(this.buttonStats);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonFixJSONFormat);
            this.Controls.Add(this.buttonPostsJSONtoXML);
            this.Controls.Add(this.buttonUsersJSONtoXML);
            this.Controls.Add(this.buttonCreateQuery);
            this.Controls.Add(this.buttonGetUserIDs);
            this.Name = "GitHubParser";
            this.Text = "GitHub Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGetUserIDs;
        private System.Windows.Forms.Button buttonCreateQuery;
        private System.Windows.Forms.Button buttonUsersJSONtoXML;
        private System.Windows.Forms.Button buttonPostsJSONtoXML;
        private System.Windows.Forms.Button buttonFixJSONFormat;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonStats;
    }
}

