namespace Google_Groups_Parser
{
    partial class GGP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GGP));
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSortName = new System.Windows.Forms.RadioButton();
            this.radioButtonSortZScore = new System.Windows.Forms.RadioButton();
            this.radioButtonSortActivity = new System.Windows.Forms.RadioButton();
            this.radioButtonSortDefault = new System.Windows.Forms.RadioButton();
            this.checkBoxBase64 = new System.Windows.Forms.CheckBox();
            this.buttonImportData = new System.Windows.Forms.Button();
            this.buttonExportData = new System.Windows.Forms.Button();
            this.buttonSplit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 155);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(479, 89);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSplit);
            this.groupBox1.Controls.Add(this.radioButtonSortName);
            this.groupBox1.Controls.Add(this.radioButtonSortZScore);
            this.groupBox1.Controls.Add(this.radioButtonSortActivity);
            this.groupBox1.Controls.Add(this.radioButtonSortDefault);
            this.groupBox1.Controls.Add(this.checkBoxBase64);
            this.groupBox1.Controls.Add(this.buttonImportData);
            this.groupBox1.Controls.Add(this.buttonExportData);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 137);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Google Groups - Parser";
            // 
            // radioButtonSortName
            // 
            this.radioButtonSortName.AutoSize = true;
            this.radioButtonSortName.Checked = true;
            this.radioButtonSortName.Location = new System.Drawing.Point(107, 105);
            this.radioButtonSortName.Name = "radioButtonSortName";
            this.radioButtonSortName.Size = new System.Drawing.Size(53, 17);
            this.radioButtonSortName.TabIndex = 11;
            this.radioButtonSortName.TabStop = true;
            this.radioButtonSortName.Text = "Name";
            this.radioButtonSortName.UseVisualStyleBackColor = true;
            // 
            // radioButtonSortZScore
            // 
            this.radioButtonSortZScore.AutoSize = true;
            this.radioButtonSortZScore.Location = new System.Drawing.Point(107, 85);
            this.radioButtonSortZScore.Name = "radioButtonSortZScore";
            this.radioButtonSortZScore.Size = new System.Drawing.Size(63, 17);
            this.radioButtonSortZScore.TabIndex = 10;
            this.radioButtonSortZScore.Text = "Z-Score";
            this.radioButtonSortZScore.UseVisualStyleBackColor = true;
            // 
            // radioButtonSortActivity
            // 
            this.radioButtonSortActivity.AutoSize = true;
            this.radioButtonSortActivity.Location = new System.Drawing.Point(107, 65);
            this.radioButtonSortActivity.Name = "radioButtonSortActivity";
            this.radioButtonSortActivity.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSortActivity.TabIndex = 9;
            this.radioButtonSortActivity.Text = "Activity";
            this.radioButtonSortActivity.UseVisualStyleBackColor = true;
            // 
            // radioButtonSortDefault
            // 
            this.radioButtonSortDefault.AutoSize = true;
            this.radioButtonSortDefault.Location = new System.Drawing.Point(107, 45);
            this.radioButtonSortDefault.Name = "radioButtonSortDefault";
            this.radioButtonSortDefault.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSortDefault.TabIndex = 8;
            this.radioButtonSortDefault.Text = "Default";
            this.radioButtonSortDefault.UseVisualStyleBackColor = true;
            // 
            // checkBoxBase64
            // 
            this.checkBoxBase64.AutoSize = true;
            this.checkBoxBase64.Location = new System.Drawing.Point(107, 25);
            this.checkBoxBase64.Name = "checkBoxBase64";
            this.checkBoxBase64.Size = new System.Drawing.Size(62, 17);
            this.checkBoxBase64.TabIndex = 6;
            this.checkBoxBase64.Text = "Base64";
            this.checkBoxBase64.UseVisualStyleBackColor = true;
            // 
            // buttonImportData
            // 
            this.buttonImportData.Location = new System.Drawing.Point(10, 25);
            this.buttonImportData.Name = "buttonImportData";
            this.buttonImportData.Size = new System.Drawing.Size(91, 23);
            this.buttonImportData.TabIndex = 5;
            this.buttonImportData.Text = "Import Data";
            this.buttonImportData.UseVisualStyleBackColor = true;
            this.buttonImportData.Click += new System.EventHandler(this.buttonImportData_Click);
            // 
            // buttonExportData
            // 
            this.buttonExportData.Location = new System.Drawing.Point(10, 54);
            this.buttonExportData.Name = "buttonExportData";
            this.buttonExportData.Size = new System.Drawing.Size(91, 23);
            this.buttonExportData.TabIndex = 4;
            this.buttonExportData.Text = "Export Data";
            this.buttonExportData.UseVisualStyleBackColor = true;
            this.buttonExportData.Click += new System.EventHandler(this.buttonExportData_Click);
            // 
            // buttonSplit
            // 
            this.buttonSplit.Location = new System.Drawing.Point(10, 83);
            this.buttonSplit.Name = "buttonSplit";
            this.buttonSplit.Size = new System.Drawing.Size(91, 23);
            this.buttonSplit.TabIndex = 12;
            this.buttonSplit.Text = "Split";
            this.buttonSplit.UseVisualStyleBackColor = true;
            this.buttonSplit.Click += new System.EventHandler(this.buttonSplit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 257);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(503, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // GGP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 279);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GGP";
            this.Text = "Google Groups Parser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonImportData;
        private System.Windows.Forms.Button buttonExportData;
        private System.Windows.Forms.CheckBox checkBoxBase64;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RadioButton radioButtonSortActivity;
        private System.Windows.Forms.RadioButton radioButtonSortDefault;
        private System.Windows.Forms.RadioButton radioButtonSortZScore;
        private System.Windows.Forms.RadioButton radioButtonSortName;
        private System.Windows.Forms.Button buttonSplit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

