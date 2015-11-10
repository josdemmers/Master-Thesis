namespace TopicAnalyser
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSurvey = new System.Windows.Forms.TextBox();
            this.buttonSurvey = new System.Windows.Forms.Button();
            this.labelOR = new System.Windows.Forms.Label();
            this.buttonCustom = new System.Windows.Forms.Button();
            this.buttonTFIDF = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCustom_SO = new System.Windows.Forms.Button();
            this.buttonTFIDF_SO = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxSurveyGH = new System.Windows.Forms.TextBox();
            this.buttonSurveyGH = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTrainingSet = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSurvey);
            this.groupBox1.Controls.Add(this.buttonSurvey);
            this.groupBox1.Controls.Add(this.labelOR);
            this.groupBox1.Controls.Add(this.buttonCustom);
            this.groupBox1.Controls.Add(this.buttonTFIDF);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Google Groups";
            // 
            // textBoxSurvey
            // 
            this.textBoxSurvey.Location = new System.Drawing.Point(7, 118);
            this.textBoxSurvey.Name = "textBoxSurvey";
            this.textBoxSurvey.Size = new System.Drawing.Size(117, 20);
            this.textBoxSurvey.TabIndex = 14;
            this.textBoxSurvey.Text = "User Id";
            this.textBoxSurvey.TextChanged += new System.EventHandler(this.textBoxSurvey_TextChanged);
            // 
            // buttonSurvey
            // 
            this.buttonSurvey.Location = new System.Drawing.Point(6, 89);
            this.buttonSurvey.Name = "buttonSurvey";
            this.buttonSurvey.Size = new System.Drawing.Size(75, 23);
            this.buttonSurvey.TabIndex = 13;
            this.buttonSurvey.Text = "Survey";
            this.buttonSurvey.UseVisualStyleBackColor = true;
            this.buttonSurvey.Click += new System.EventHandler(this.buttonSurvey_Click);
            // 
            // labelOR
            // 
            this.labelOR.AutoSize = true;
            this.labelOR.Enabled = false;
            this.labelOR.Location = new System.Drawing.Point(32, 44);
            this.labelOR.Name = "labelOR";
            this.labelOR.Size = new System.Drawing.Size(16, 13);
            this.labelOR.TabIndex = 10;
            this.labelOR.Text = "or";
            // 
            // buttonCustom
            // 
            this.buttonCustom.Location = new System.Drawing.Point(6, 60);
            this.buttonCustom.Name = "buttonCustom";
            this.buttonCustom.Size = new System.Drawing.Size(75, 23);
            this.buttonCustom.TabIndex = 9;
            this.buttonCustom.Text = "Custom";
            this.buttonCustom.UseVisualStyleBackColor = true;
            this.buttonCustom.Click += new System.EventHandler(this.buttonCustom_Click);
            // 
            // buttonTFIDF
            // 
            this.buttonTFIDF.Location = new System.Drawing.Point(6, 19);
            this.buttonTFIDF.Name = "buttonTFIDF";
            this.buttonTFIDF.Size = new System.Drawing.Size(75, 23);
            this.buttonTFIDF.TabIndex = 8;
            this.buttonTFIDF.Text = "TFIDF";
            this.buttonTFIDF.UseVisualStyleBackColor = true;
            this.buttonTFIDF.Click += new System.EventHandler(this.buttonTFIDF_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonCustom_SO);
            this.groupBox2.Controls.Add(this.buttonTFIDF_SO);
            this.groupBox2.Location = new System.Drawing.Point(144, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(130, 143);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stack Overflow";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(32, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "or";
            // 
            // buttonCustom_SO
            // 
            this.buttonCustom_SO.Location = new System.Drawing.Point(6, 60);
            this.buttonCustom_SO.Name = "buttonCustom_SO";
            this.buttonCustom_SO.Size = new System.Drawing.Size(75, 23);
            this.buttonCustom_SO.TabIndex = 16;
            this.buttonCustom_SO.Text = "Custom";
            this.buttonCustom_SO.UseVisualStyleBackColor = true;
            this.buttonCustom_SO.Click += new System.EventHandler(this.buttonCustom_SO_Click);
            // 
            // buttonTFIDF_SO
            // 
            this.buttonTFIDF_SO.Location = new System.Drawing.Point(6, 19);
            this.buttonTFIDF_SO.Name = "buttonTFIDF_SO";
            this.buttonTFIDF_SO.Size = new System.Drawing.Size(75, 23);
            this.buttonTFIDF_SO.TabIndex = 15;
            this.buttonTFIDF_SO.Text = "TFIDF";
            this.buttonTFIDF_SO.UseVisualStyleBackColor = true;
            this.buttonTFIDF_SO.Click += new System.EventHandler(this.buttonTFIDF_SO_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 192);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(417, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(26, 17);
            this.toolStripStatusLabel1.Text = "Idle";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxSurveyGH);
            this.groupBox3.Controls.Add(this.buttonSurveyGH);
            this.groupBox3.Location = new System.Drawing.Point(280, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(130, 143);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GitHub";
            // 
            // textBoxSurveyGH
            // 
            this.textBoxSurveyGH.Location = new System.Drawing.Point(6, 118);
            this.textBoxSurveyGH.Name = "textBoxSurveyGH";
            this.textBoxSurveyGH.Size = new System.Drawing.Size(117, 20);
            this.textBoxSurveyGH.TabIndex = 15;
            this.textBoxSurveyGH.Text = "User Id";
            this.textBoxSurveyGH.TextChanged += new System.EventHandler(this.textBoxSurveyGH_TextChanged);
            // 
            // buttonSurveyGH
            // 
            this.buttonSurveyGH.Enabled = false;
            this.buttonSurveyGH.Location = new System.Drawing.Point(6, 89);
            this.buttonSurveyGH.Name = "buttonSurveyGH";
            this.buttonSurveyGH.Size = new System.Drawing.Size(75, 23);
            this.buttonSurveyGH.TabIndex = 14;
            this.buttonSurveyGH.Text = "Survey";
            this.buttonSurveyGH.UseVisualStyleBackColor = true;
            this.buttonSurveyGH.Click += new System.EventHandler(this.buttonSurveyGH_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select training set:";
            // 
            // comboBoxTrainingSet
            // 
            this.comboBoxTrainingSet.FormattingEnabled = true;
            this.comboBoxTrainingSet.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboBoxTrainingSet.Location = new System.Drawing.Point(144, 166);
            this.comboBoxTrainingSet.Name = "comboBoxTrainingSet";
            this.comboBoxTrainingSet.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTrainingSet.TabIndex = 4;
            this.comboBoxTrainingSet.Text = "1";
            this.comboBoxTrainingSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxTrainingSet_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 214);
            this.Controls.Add(this.comboBoxTrainingSet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Topic Analyser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelOR;
        private System.Windows.Forms.Button buttonCustom;
        private System.Windows.Forms.Button buttonTFIDF;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox textBoxSurvey;
        private System.Windows.Forms.Button buttonSurvey;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxSurveyGH;
        private System.Windows.Forms.Button buttonSurveyGH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCustom_SO;
        private System.Windows.Forms.Button buttonTFIDF_SO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxTrainingSet;
    }
}

