namespace TestHarness_7090Project
{
    partial class GISForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GISForm));
            this.btnExecute = new System.Windows.Forms.Button();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.cboTimeInterval = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.DatasetFile = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numNeighbors = new System.Windows.Forms.TextBox();
            this.txtExponent = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTimeEncodingFactor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInterpolationOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseInterpolationOutput = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textShapeFilesDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowseShapeFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LocationFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.cmbStates = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMapData = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.lblStep = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(927, 750);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(100, 28);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Location = new System.Drawing.Point(39, 565);
            this.lblAlgorithm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(272, 17);
            this.lblAlgorithm.TabIndex = 18;
            this.lblAlgorithm.Text = "Time Domain (Day, Month, Quarter, Year)";
            // 
            // cboTimeInterval
            // 
            this.cboTimeInterval.FormattingEnabled = true;
            this.cboTimeInterval.Items.AddRange(new object[] {
            "Year",
            "Month",
            "Quarter",
            "Day"});
            this.cboTimeInterval.Location = new System.Drawing.Point(43, 591);
            this.cboTimeInterval.Margin = new System.Windows.Forms.Padding(4);
            this.cboTimeInterval.Name = "cboTimeInterval";
            this.cboTimeInterval.Size = new System.Drawing.Size(160, 24);
            this.cboTimeInterval.TabIndex = 19;
            this.cboTimeInterval.SelectedIndexChanged += new System.EventHandler(this.cboTimeInterval_SelectedIndexChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(379, 101);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnBrowse.TabIndex = 22;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // DatasetFile
            // 
            this.DatasetFile.Location = new System.Drawing.Point(9, 105);
            this.DatasetFile.Margin = new System.Windows.Forms.Padding(4);
            this.DatasetFile.Name = "DatasetFile";
            this.DatasetFile.Size = new System.Drawing.Size(360, 22);
            this.DatasetFile.TabIndex = 21;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(8, 85);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(178, 17);
            this.lblFile.TabIndex = 20;
            this.lblFile.Text = "Choose Input Dataset File: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "Nearest Neighbors (int)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 74);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "Exponent (float)";
            // 
            // numNeighbors
            // 
            this.numNeighbors.Location = new System.Drawing.Point(53, 40);
            this.numNeighbors.Margin = new System.Windows.Forms.Padding(4);
            this.numNeighbors.Name = "numNeighbors";
            this.numNeighbors.Size = new System.Drawing.Size(132, 22);
            this.numNeighbors.TabIndex = 27;
            this.numNeighbors.Text = "2";
            // 
            // txtExponent
            // 
            this.txtExponent.Location = new System.Drawing.Point(53, 93);
            this.txtExponent.Margin = new System.Windows.Forms.Padding(4);
            this.txtExponent.Name = "txtExponent";
            this.txtExponent.Size = new System.Drawing.Size(132, 22);
            this.txtExponent.TabIndex = 28;
            this.txtExponent.Text = "1.08";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTimeEncodingFactor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtExponent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numNeighbors);
            this.groupBox1.Location = new System.Drawing.Point(43, 624);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(283, 179);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IDW";
            // 
            // txtTimeEncodingFactor
            // 
            this.txtTimeEncodingFactor.Location = new System.Drawing.Point(56, 145);
            this.txtTimeEncodingFactor.Margin = new System.Windows.Forms.Padding(4);
            this.txtTimeEncodingFactor.Name = "txtTimeEncodingFactor";
            this.txtTimeEncodingFactor.Size = new System.Drawing.Size(132, 22);
            this.txtTimeEncodingFactor.TabIndex = 30;
            this.txtTimeEncodingFactor.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 126);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 17);
            this.label6.TabIndex = 29;
            this.label6.Text = "Time Encoding (int)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtInterpolationOutputFile);
            this.groupBox2.Controls.Add(this.btnBrowseInterpolationOutput);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textShapeFilesDirectory);
            this.groupBox2.Controls.Add(this.btnBrowseShapeFile);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.LocationFile);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.lblFile);
            this.groupBox2.Controls.Add(this.DatasetFile);
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Location = new System.Drawing.Point(419, 565);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(495, 238);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "FilePicker";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 184);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 17);
            this.label7.TabIndex = 31;
            this.label7.Text = "Choose Output File: ";
            // 
            // txtInterpolationOutputFile
            // 
            this.txtInterpolationOutputFile.Location = new System.Drawing.Point(8, 204);
            this.txtInterpolationOutputFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtInterpolationOutputFile.Name = "txtInterpolationOutputFile";
            this.txtInterpolationOutputFile.Size = new System.Drawing.Size(360, 22);
            this.txtInterpolationOutputFile.TabIndex = 32;
            // 
            // btnBrowseInterpolationOutput
            // 
            this.btnBrowseInterpolationOutput.Location = new System.Drawing.Point(378, 200);
            this.btnBrowseInterpolationOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseInterpolationOutput.Name = "btnBrowseInterpolationOutput";
            this.btnBrowseInterpolationOutput.Size = new System.Drawing.Size(100, 28);
            this.btnBrowseInterpolationOutput.TabIndex = 33;
            this.btnBrowseInterpolationOutput.Text = "Browse";
            this.btnBrowseInterpolationOutput.UseVisualStyleBackColor = true;
            this.btnBrowseInterpolationOutput.Click += new System.EventHandler(this.btnBrowseInterpolationOutput_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 29);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(402, 17);
            this.label5.TabIndex = 28;
            this.label5.Text = "Choose Shape Directory (directory that contains all .shp files): ";
            // 
            // textShapeFilesDirectory
            // 
            this.textShapeFilesDirectory.Location = new System.Drawing.Point(9, 49);
            this.textShapeFilesDirectory.Margin = new System.Windows.Forms.Padding(4);
            this.textShapeFilesDirectory.Name = "textShapeFilesDirectory";
            this.textShapeFilesDirectory.Size = new System.Drawing.Size(360, 22);
            this.textShapeFilesDirectory.TabIndex = 29;
            // 
            // btnBrowseShapeFile
            // 
            this.btnBrowseShapeFile.Location = new System.Drawing.Point(379, 45);
            this.btnBrowseShapeFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseShapeFile.Name = "btnBrowseShapeFile";
            this.btnBrowseShapeFile.Size = new System.Drawing.Size(100, 28);
            this.btnBrowseShapeFile.TabIndex = 30;
            this.btnBrowseShapeFile.Text = "Browse";
            this.btnBrowseShapeFile.UseVisualStyleBackColor = true;
            this.btnBrowseShapeFile.Click += new System.EventHandler(this.btnBrowseShapeFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 133);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "Choose Interpolation Locations File: ";
            // 
            // LocationFile
            // 
            this.LocationFile.Location = new System.Drawing.Point(9, 153);
            this.LocationFile.Margin = new System.Windows.Forms.Padding(4);
            this.LocationFile.Name = "LocationFile";
            this.LocationFile.Size = new System.Drawing.Size(360, 22);
            this.LocationFile.TabIndex = 26;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(379, 149);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 27;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mapBox1
            // 
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mapBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(43, 70);
            this.mapBox1.Margin = new System.Windows.Forms.Padding(4);
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.Silver;
            this.mapBox1.ShowProgressUpdate = false;
            this.mapBox1.Size = new System.Drawing.Size(984, 437);
            this.mapBox1.TabIndex = 30;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = 2D;
            // 
            // cmbStates
            // 
            this.cmbStates.FormattingEnabled = true;
            this.cmbStates.Items.AddRange(new object[] {
            "50 States Input Shp File",
            "Query Output Shp File"});
            this.cmbStates.Location = new System.Drawing.Point(43, 37);
            this.cmbStates.Margin = new System.Windows.Forms.Padding(4);
            this.cmbStates.Name = "cmbStates";
            this.cmbStates.Size = new System.Drawing.Size(240, 24);
            this.cmbStates.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 17);
            this.label4.TabIndex = 33;
            this.label4.Text = "Data to Represent on the Map";
            // 
            // btnMapData
            // 
            this.btnMapData.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMapData.Location = new System.Drawing.Point(292, 34);
            this.btnMapData.Margin = new System.Windows.Forms.Padding(4);
            this.btnMapData.Name = "btnMapData";
            this.btnMapData.Size = new System.Drawing.Size(100, 28);
            this.btnMapData.TabIndex = 34;
            this.btnMapData.Text = "Map Data";
            this.btnMapData.UseVisualStyleBackColor = false;
            this.btnMapData.Click += new System.EventHandler(this.btnMapData_Click_1);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(419, 511);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(608, 43);
            this.lblStatus.TabIndex = 35;
            this.lblStatus.Text = "Progress";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            // 
            // lblStep
            // 
            this.lblStep.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep.ForeColor = System.Drawing.Color.White;
            this.lblStep.Location = new System.Drawing.Point(40, 518);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(373, 43);
            this.lblStep.TabIndex = 36;
            this.lblStep.Text = "Step";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GISForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1059, 807);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnMapData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbStates);
            this.Controls.Add(this.mapBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboTimeInterval);
            this.Controls.Add(this.lblAlgorithm);
            this.Controls.Add(this.btnExecute);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GISForm";
            this.Text = "GIS Team Project";
            this.Load += new System.EventHandler(this.GISForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label lblAlgorithm;
        private System.Windows.Forms.ComboBox cboTimeInterval;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox DatasetFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox numNeighbors;
        private System.Windows.Forms.TextBox txtExponent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LocationFile;
        private System.Windows.Forms.Button button1;
        private SharpMap.Forms.MapBox mapBox1;
        private System.Windows.Forms.ComboBox cmbStates;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMapData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textShapeFilesDirectory;
        private System.Windows.Forms.Button btnBrowseShapeFile;
        private System.Windows.Forms.TextBox txtTimeEncodingFactor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtInterpolationOutputFile;
        private System.Windows.Forms.Button btnBrowseInterpolationOutput;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.Label lblStep;
    }
}