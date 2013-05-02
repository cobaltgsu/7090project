using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project7090;
using Project7090.DataTypes;
using System.IO;
using System.Data.SqlClient;
//using System.Drawing;
using SharpMap.Layers;
using SharpMap;


namespace TestHarness_7090Project
{
    public partial class GISForm : Form
    {
        //Properties/Variables ************************************************
        ProcessingEngine _engine = new ProcessingEngine();
        long counter = 0;
        private delegate void UpdateStepHandler(string output);
        private delegate void UpdateStatusHandler(string output);

        //Constructors ********************************************************
        public GISForm()
        {
            InitializeComponent();
           
        }

        //Methods *************************************************************
        private void GISForm_Load(object sender, EventArgs e)
        {
            btnMapData.Enabled = false;
            cmbStates.Enabled = false;

            bgWorker = new BackgroundWorker();

            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.ProgressChanged += bgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
        }

        private void InitializeMapAndEnableControls()
        {
            btnMapData.Enabled = true;
            cmbStates.Enabled = true;


            SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("State_Name");
            vlay.DataSource = new SharpMap.Data.Providers.ShapeFile(GetFullPathForShapeFile("states.shp"), true);

            SharpMap.Layers.VectorLayer vdatlay = new SharpMap.Layers.VectorLayer("dat1");

            //String datFile = "C:\\test\\st99_d00a.dat";
            //String datFile = @"C:\Users\Steven\Downloads\Shape_Files\test\st99_d00a.dat";

            mapBox1.Map.Layers.Add(vlay);
            //mapBox1.ForeColor = Color.SeaGreen;
            mapBox1.Map.BackColor = Color.Wheat;

            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
        }

        //Events **************************************************************
        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (cboTimeInterval.SelectedItem == null)
            {
                MessageBox.Show("Please choose a time domain.", "Missing Parameter");
                return;
            }

            if (string.IsNullOrEmpty(numNeighbors.Text))
            {
                MessageBox.Show("Please input Nearest Neighbors value.", "Missing Parameter");
                return;
            }

            if (string.IsNullOrEmpty(txtExponent.Text))
            {
                MessageBox.Show("Please input Exponent value.", "Missing Parameter");
                return;
            }

            if (string.IsNullOrEmpty(DatasetFile.Text))
            {
                MessageBox.Show("Please choose an input dataset file", "Missing Parameter");
                return;
            }

            if (string.IsNullOrEmpty(LocationFile.Text))
            {
                MessageBox.Show("Please choose an input location file", "Missing Parameter");
                return;
            }

            if (string.IsNullOrEmpty(this.txtInterpolationOutputFile.Text))
            {
                MessageBox.Show("Please choose a location to output the interpolation results.", "Missing Parameter");
                return;
            }

            if (bgWorker.IsBusy != true)
            {
                bgWorker.RunWorkerAsync();
            }

            /*
            _engine.GISInputFilePath = DatasetFile.Text;
            _engine.LocationInputFilePath = LocationFile.Text;
          
            _engine.NumberOfNeighbors = Convert.ToInt32(numNeighbors.Text);
            _engine.InverseDistanceWeightedExponent = Convert.ToDouble(txtExponent.Text);
            _engine.TimeEncodingFactor = Convert.ToDouble(txtTimeEncodingFactor.Text);            
            _engine.InterpolationOutputFile = GetInterpolationOutputFile();
            _engine.BaseOutputDirectory = System.IO.Path.GetDirectoryName(_engine.InterpolationOutputFile);

            _engine.Process();
             */
        }//btnExecute_Click

        private string GetInterpolationOutputFile()
        {
            string outputFile = string.Empty;

            if (!string.IsNullOrEmpty(System.IO.Path.GetFileName(txtInterpolationOutputFile.Text)))
            {
                outputFile = System.IO.Path.Combine(txtInterpolationOutputFile.Text, "county_id_t_w.txt");
            }

            return outputFile;
        }

        private string GetFullPathForShapeFile(string fileName)
        {
            string shapeFilePath = string.Empty;

            if (!string.IsNullOrEmpty(textShapeFilesDirectory.Text))
            {
                shapeFilePath = System.IO.Path.Combine(textShapeFilesDirectory.Text, fileName);
            }

            return shapeFilePath;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();//
            dlg.Multiselect = false;
            dlg.InitialDirectory = "c:\\";
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                DatasetFile.Text = dlg.FileName;
                _engine.GISInputFilePath = DatasetFile.Text;
            }//if
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgLocation = new OpenFileDialog();//
            dlgLocation.Multiselect = false;
            dlgLocation.InitialDirectory = "c:\\";
            DialogResult result = dlgLocation.ShowDialog();
            if (result == DialogResult.OK)
            {
                LocationFile.Text = dlgLocation.FileName;
                _engine.LocationInputFilePath = LocationFile.Text;
            }//if
        }


       
        private void btnMapData_Click_1(object sender, EventArgs e)
        {           

            int dataSource = this.cmbStates.SelectedIndex;

            mapBox1.ShowProgressUpdate = true;

            switch (dataSource)
            {

                case 0:
                    btnMapData.BackColor = Color.Green;
                    SharpMap.Layers.VectorLayer vdat1 = new SharpMap.Layers.VectorLayer("StatesData1");
                    
                    vdat1.DataSource = new SharpMap.Data.Providers.ShapeFile(GetFullPathForShapeFile("50states.shp"),true);

                    mapBox1.Map.Layers.Add(vdat1);                    
                  
                    mapBox1.Refresh();
                    
                    btnMapData.BackColor = Color.Red;

                    
                    break;
                case 1:
                 
                    btnMapData.BackColor = Color.Red;
                                       

                    SharpMap.Layers.VectorLayer vdat2 = new SharpMap.Layers.VectorLayer("StatesData2");
                    vdat2.DataSource = new SharpMap.Data.Providers.ShapeFile(GetFullPathForShapeFile("Query_Output.shp"), true);                    
                    
                    mapBox1.Map.Layers.Add(vdat2);                    
                    
                    mapBox1.Refresh();

                    break;

                default:
                    btnMapData.BackColor = Color.Yellow;
                  //  SharpMap.Layers.VectorLayer vdat2 = new SharpMap.Layers.VectorLayer("StatesData2");
                   // vdat2.DataSource = new SharpMap.Data.Providers.
                    break;

            }//end case
            
        }


        private void btnBrowseShapeFile_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select folder that contains shape files";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;

                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textShapeFilesDirectory.Text = dlg.SelectedPath;
                }
            }

            if (!File.Exists(System.IO.Path.Combine(textShapeFilesDirectory.Text, "states.shp")))
            {
                MessageBox.Show("Could not find [states.shp] file in specified shaped directory.\nPlease check directory.", "File not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!File.Exists(System.IO.Path.Combine(textShapeFilesDirectory.Text, "50states.shp")))
            {
                MessageBox.Show("Could not find [50states.shp] file in specified shaped directory.\nPlease check directory.", "File not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!File.Exists(System.IO.Path.Combine(textShapeFilesDirectory.Text, "Query_Output.shp")))
            {
                MessageBox.Show("Could not find [Query_Output.shp] file in specified shaped directory.\nPlease check directory.", "File not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            InitializeMapAndEnableControls();
        }

        private void btnBrowseInterpolationOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select folder to store the output";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;

                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtInterpolationOutputFile.Text = dlg.SelectedPath;
                }
            }
        }

        /// <summary>
        /// Sets the engine's time domain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboTimeInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboTimeInterval.Text)
            {
                case "Year":
                    _engine.DataSetTimeDomain = Common.TimeDomain.Year;
                    break;
                case "Month":
                    _engine.DataSetTimeDomain = Common.TimeDomain.YearMonth;
                    break;
                case "Quarter":
                    _engine.DataSetTimeDomain = Common.TimeDomain.YearQuarter;
                    break;
                case "Day":
                    _engine.DataSetTimeDomain = Common.TimeDomain.YearMonthDay;
                    break;
            }
        }



        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStatus.Text = e.UserState.ToString();  
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _engine.GISLocationProcessed +=_engine_GISLocationProcessed;
            _engine.MileStoneEvent +=_engine_MileStoneEvent;
            _engine.GISInputFilePath = DatasetFile.Text;
            _engine.LocationInputFilePath = LocationFile.Text;

            _engine.NumberOfNeighbors = Convert.ToInt32(numNeighbors.Text);
            _engine.InverseDistanceWeightedExponent = Convert.ToDouble(txtExponent.Text);
            _engine.TimeEncodingFactor = Convert.ToDouble(txtTimeEncodingFactor.Text);
            _engine.InterpolationOutputFile = GetInterpolationOutputFile();
            _engine.BaseOutputDirectory = System.IO.Path.GetDirectoryName(_engine.InterpolationOutputFile);

            _engine.Process();
        }

        void _engine_MileStoneEvent(string output)
        {
            lblStep.Invoke(new UpdateStepHandler(UpdateStep), new object[]{output});            
        }

        private void UpdateStep(string output)
        {
            lblStep.Text = output;
        }

        private void UpdateStatus(string output)
        {
            lblStatus.Text = output;
        }

        void _engine_GISLocationProcessed(string output)
        {
            lblStatus.Invoke(new UpdateStatusHandler(UpdateStatus), new object[] { output });
        }

  

    
   
    }//class
}//namespace
