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

namespace TestHarness_7090Project
{
    public partial class Form1 : Form
    {
        ProcessingEngine _engine = new ProcessingEngine();
        long counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            _engine.GISLocationProcessed += _engine_GISLocationProcessed;
            _engine.TimeEncodingFactor = 1.0;
            _engine.InverseDistanceWeightedExponent = 1.08;
            _engine.NumberOfNeighbors = 2;
            _engine.DataSetTimeDomain = Common.TimeDomain.YearMonthDay;           
            _engine.GISInputFilePath = "C:\\Lectures\\Spring2013\\GIS\\M8\\pm25_2009_measured.txt";
            _engine.LocationInputFilePath = "C:\\Lectures\\Spring2013\\GIS\\M8\\county_xy.txt";
            _engine.InterpolationOutputFile = "c:\\trash\\proj_output.txt";

        }

        void _engine_GISLocationProcessed(int specialEvent, string output)
        {
            throw new NotImplementedException();
        }

        void _engine_GISLocationProcessed(string output)
        {
            throw new NotImplementedException();
        }

        void engine_GISLocationProcessed()
        {
            counter++;
            Console.WriteLine("Processed: {0}", counter);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _engine.Process();
        }
    }
}
