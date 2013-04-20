using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Project7090;
using Project7090.DataTypes;
using System.Diagnostics;



namespace TestHarness_7090Project
{
    public partial class GISForm : Form
    {
        //Properties/Variables ************************************************
        ProcessingEngine _engine = new ProcessingEngine();
        long counter = 0;
       

        //Constructors ********************************************************
        public GISForm()
        {
            InitializeComponent();
            cmbSource.Items.Add("pm25_2009_measured_small");
            cmbSource.Items.Add("pm25_2009_measured");
           // this.timer1.Stop();
     
        }//gisForm

        //Methods *************************************************************
        //Events **************************************************************
        private void btnExecute_Click(object sender, EventArgs e)
        {


            progressBar1.Visible = true;

            this.backgroundWorker1.WorkerReportsProgress = true;

            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);

            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);

            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);

            this.backgroundWorker1.RunWorkerAsync();

            _engine.Process();

           // progressBar1.Visible = false;
           
        }//btnExecute_Click

      

        private void DMForm_Load(object sender, EventArgs e)
        {
            _engine.GISLocationProcessed += engine_GISLocationProcessed;
            _engine.TimeEncodingFactor = 1.0;
            _engine.InverseDistanceWeightedExponent = 1.08;
            _engine.NumberOfNeighbors = 2;
            _engine.DataSetTimeDomain = Common.TimeDomain.YearMonthDay;
            _engine.GISInputFilePath = "C:\\Lectures\\Spring2013\\GIS\\M8\\pm25_2009_measured.txt";
            _engine.LocationInputFilePath = "C:\\Lectures\\Spring2013\\GIS\\M8\\country_xy.txt";
            _engine.InterpolationOutputFile = "c:\\trash\\proj_output.txt";
        }//button1_Click

        void engine_GISLocationProcessed()
        {
            counter++;
            Console.WriteLine("Processed: {0}", counter);
            rtbResults.Text = "Processed: " + counter + "\n";
            
        }
        int i = 0;
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            timerLabel.Text = i.ToString();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _engine.GISInputFilePath = txtFileName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            
        }

        private void rtbResults_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
       

        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) //call back method
        {

            progressBar1.Value = e.ProgressPercentage;

        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) //call back method
        {

            progressBar1.Value = progressBar1.Maximum;

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {

               // Thread.Sleep(100);

                backgroundWorker1.ReportProgress(i); //run in back thread


            }
        }



    }//class
}//namespace
