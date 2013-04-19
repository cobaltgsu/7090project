using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Project7090.DataTypes;
using Project7090.Interpolation;
using Project7090.Validation;

namespace Project7090
{
    public partial class ProcessingEngine
    {
        private GISDataSet _gisDataSet = null; // Holds GIS information from file.
        private GISDataSet _locationDataSet = null; // Holds the locations to be interpolated from file.
        private Dictionary<double, TimeDomainContainer> _dictionaryTimeDecoder = null;
        private List<double> rangeOfTime;
        private int progress = 0;
        private double percentComplete;
        
        #region Public Properties
        public Common.TimeDomain DataSetTimeDomain { get; set; }
        public double TimeEncodingFactor { get; set; } // Open Research Q: This factor allows us control over the timeRange and has implications when calc distances,etc.
        public string InterpolationOutputFile { get; set; }
        public string GISInputFilePath { get; set; }
        public string LocationInputFilePath { get; set; }
        public double InverseDistanceWeightedExponent { get; set; }
        public int NumberOfNeighbors { get; set; }
        #endregion

        public delegate void GISLocationProcessedHandler();
        public event GISLocationProcessedHandler GISLocationProcessed;        
        

        private const int COLUMN_ID = 0;
        private const int COLUMN_X = 1;
        private const int COLUMN_Y = 2;
 
        public ProcessingEngine()
        {

        }

        public void Process()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();       
            
            LoadDataFromFile(this.GISInputFilePath);
            LoadLocationDataFile(this.LocationInputFilePath);

            SetTimeRange(this.DataSetTimeDomain);

            watch.Stop();
            Console.WriteLine("\nImport of data took " + watch.Elapsed.TotalSeconds + " seconds.\n\n");
            watch.Reset();
            watch.Start();
            
            // --- Interpolation --- //
            //IDW idw = new IDW(_gisDataSet, NumberOfNeighbors, InverseDistanceWeightedExponent); // Went static with the class.
            Console.WriteLine("\nINTERPOLATING...\n");
            watch.Start();
            foreach (GISDataPoint dp in _locationDataSet)
            {
                /// TO DO: ENCAPSULATE THIS & IMPLEMENT THREADPOOL HERE
                foreach (double currentTime in rangeOfTime)
                {
                    dp.time = currentTime; // Assign time values to location data set's data points.
                    GISDataPointDistance[] nearNeighbors = NearestNeighbors(dp, NumberOfNeighbors); // Get neighbors.
                    dp.measurement = IDW.Interpolate(nearNeighbors, InverseDistanceWeightedExponent, NumberOfNeighbors); // Interpolate.

                    // Increment Progress Bar here.
                    percentComplete = ((double)progress / (double)_gisDataSet.Count) * 100;
                    Console.Write("\r{0}" + " calculations done. [{1:#.#}% Complete] [Elapsed: {2} Minutes {3} Seconds]", progress, percentComplete, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
                    progress++;
                }
                /// END ENCAPSULATION
            }
            watch.Stop();
            Console.WriteLine("\nInterpolation of data took " + watch.Elapsed.TotalMinutes + " minutes.\n");
            watch.Reset();


            // --- Output Interp --- //
            Console.WriteLine("\nWriting results to output file...\n");
            watch.Start();
            OutputDataToFile(this._locationDataSet);
            watch.Stop();
            Console.WriteLine("\nOutput of data took " + watch.Elapsed.TotalSeconds + " seconds.\n\n");
            watch.Reset();

            // --- Validation --- //
            LOOCV loo = new LOOCV(_gisDataSet);
            Console.WriteLine("\nVALIDATING...\n");
            watch.Start();
            /// TO DO: ENCAPSULATE THIS & IMPLEMENT THREADPOOL HERE
            loo.Validate();
            loo.CalculateError();
            /// END ENCAPSULATION
            watch.Stop();
            Console.WriteLine("\nValidation of data took " + watch.Elapsed.TotalMinutes + " minutes.\n");
            watch.Reset();
        }
    }
}
