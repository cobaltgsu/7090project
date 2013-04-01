namespace DataMiningTeam.WindowsForms
{
    partial class DMForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMForm));
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMinSupport = new System.Windows.Forms.Label();
            this.txtMinSupport = new System.Windows.Forms.TextBox();
            this.lblMinConfidence = new System.Windows.Forms.Label();
            this.txtMinConfidence = new System.Windows.Forms.TextBox();
            this.cmbSource = new System.Windows.Forms.ComboBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(33, 244);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 26);
            this.button2.TabIndex = 1;
            this.button2.Text = "Bonus 1";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(33, 276);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 26);
            this.button4.TabIndex = 3;
            this.button4.Text = "Bonus 2";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(492, 321);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(573, 321);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(567, 12);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 6;
            this.button7.Text = "Help";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // rtbResults
            // 
            this.rtbResults.Location = new System.Drawing.Point(156, 42);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(486, 273);
            this.rtbResults.TabIndex = 10;
            this.rtbResults.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Results";
            // 
            // lblMinSupport
            // 
            this.lblMinSupport.AutoSize = true;
            this.lblMinSupport.Location = new System.Drawing.Point(12, 326);
            this.lblMinSupport.Name = "lblMinSupport";
            this.lblMinSupport.Size = new System.Drawing.Size(108, 13);
            this.lblMinSupport.TabIndex = 12;
            this.lblMinSupport.Text = "Minimum Support (%):";
            // 
            // txtMinSupport
            // 
            this.txtMinSupport.Location = new System.Drawing.Point(127, 323);
            this.txtMinSupport.Name = "txtMinSupport";
            this.txtMinSupport.Size = new System.Drawing.Size(52, 20);
            this.txtMinSupport.TabIndex = 13;
            // 
            // lblMinConfidence
            // 
            this.lblMinConfidence.AutoSize = true;
            this.lblMinConfidence.Location = new System.Drawing.Point(185, 326);
            this.lblMinConfidence.Name = "lblMinConfidence";
            this.lblMinConfidence.Size = new System.Drawing.Size(125, 13);
            this.lblMinConfidence.TabIndex = 14;
            this.lblMinConfidence.Text = "Minimum Confidence (%):";
            // 
            // txtMinConfidence
            // 
            this.txtMinConfidence.Location = new System.Drawing.Point(316, 323);
            this.txtMinConfidence.Name = "txtMinConfidence";
            this.txtMinConfidence.Size = new System.Drawing.Size(58, 20);
            this.txtMinConfidence.TabIndex = 15;
            // 
            // cmbSource
            // 
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(15, 42);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(121, 21);
            this.cmbSource.TabIndex = 16;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(12, 20);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(70, 13);
            this.lblSource.TabIndex = 17;
            this.lblSource.Text = "Data Source:";
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Location = new System.Drawing.Point(12, 79);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(53, 13);
            this.lblAlgorithm.TabIndex = 18;
            this.lblAlgorithm.Text = "Algorithm:";
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(15, 96);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(121, 21);
            this.cmbAlgorithm.TabIndex = 19;
            // 
            // DMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(677, 378);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.lblAlgorithm);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.txtMinConfidence);
            this.Controls.Add(this.lblMinConfidence);
            this.Controls.Add(this.txtMinSupport);
            this.Controls.Add(this.lblMinSupport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DMForm";
            this.Text = "Data Mining Team Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMinSupport;
        private System.Windows.Forms.TextBox txtMinSupport;
        private System.Windows.Forms.Label lblMinConfidence;
        private System.Windows.Forms.TextBox txtMinConfidence;
        private System.Windows.Forms.ComboBox cmbSource;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblAlgorithm;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
    }
}