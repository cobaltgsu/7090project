using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Project7090.DataTypes;
using System.Collections.Concurrent;
using System.Threading;
using Project7090.Validation;
using Project7090.Interpolation;

namespace Project7090
{
    public partial class ProcessingEngine
    {
        private  GISDataSet _gisDataSet = null; //Holds GIS information from file
        
        private static List<GISDataPoint> _listGISDataPoints = new List<GISDataPoint>();
        private static List<GISDataPoint> _listLocationDataPoints = new List<GISDataPoint>();

        private DataPointSet _locationDataSet = null; // holds the locations to be interpolated from file
        private Dictionary<double, TimeDomainContainer> _dictionaryTimeDecoder = null;

        private List<double> rangeOfTime = null;
        private TimeDomainContainer domainContainer = null;

        private Dictionary<double, TimeDomainContainer> rangeOfTimeDictionary = null;

        private int _dataYear = 0; //The year of the input data;
        private int _dataYearEnd = 0; //The last year of data. This is for the timeRange of year.

        private int progress = 1;
        private double percentComplete;
        string[] outputList;

        // Container For IDW Output
        List<string>[] _outputLists;


        #region Public Properties
        public Common.TimeDomain DataSetTimeDomain { get; set; }
        public double TimeEncodingFactor {get;set;}
        public string InterpolationOutputFile { get; set; }
        public string GISInputFilePath { get; set; }
        public string LocationInputFilePath { get; set; }
        public double InverseDistanceWeightedExponent { get; set; }
        public int NumberOfNeighbors { get; set; }
        public string BaseOutputDirectory { get; set; }
        
        #endregion

        public delegate void GISLocationProcessedHandler(string output);
        public event GISLocationProcessedHandler GISLocationProcessed;

        public delegate void MileStoneEventHandler(string output);
        public event MileStoneEventHandler MileStoneEvent;        
        

        private const int COLUMN_ID = 0;
        private const int COLUMN_X = 1;
        private const int COLUMN_Y = 2;


        public ProcessingEngine()
        {

        }

        public void Process()
        {
            LoadDataFromFile(this.GISInputFilePath);

            SetTimeRange(this.DataSetTimeDomain);

            LoadLocationDataFile(this.LocationInputFilePath);

            // --- INTERPOLATION --- //
            //Prepare the IDW functionality.
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            MileStoneEvent("INTERPOLATING");
            
            watch.Start();

            Interpolation.InverseDistanceWeightedExtension idwFunction = new Interpolation.InverseDistanceWeightedExtension();

            idwFunction.GISDataToProcess = _gisDataSet;
            idwFunction.NumberOfNeighbors = this.NumberOfNeighbors;
            idwFunction.Exponent = this.InverseDistanceWeightedExponent;

            Project7090.Interpolation.KDTree tree = new Interpolation.KDTree(_listGISDataPoints);

            int numOutputs = _listLocationDataPoints.Count;
            int numThreads;            //Get numThreads.
            if (Environment.ProcessorCount >= 4)
            {
                numThreads = Environment.ProcessorCount - 2;
            }
            else if (Environment.ProcessorCount >= 2 && Environment.ProcessorCount < 4)
            {
                numThreads = Environment.ProcessorCount - 1;
            }
            else
            {
                numThreads = 1;
            }

            List<GISDataPoint>[] partitions = ProcessingEngine.PartitionDataSet(_listLocationDataPoints, numThreads); // Partition our data.

            List<Task> tasks = new List<Task>(); // Put thread tasks in a container to iterate over for Task.Wait() ... makeshift WaitAll() threads.
            outputList = new string[numOutputs];

            _outputLists = new List<string>[partitions.Length];

            // Create Threads
            for (int indx = 0; indx < partitions.Length; indx++)
            {
                // Must methodize the task creation. Otherwise above for loop with get out of range while threads are created and give exception.
                tasks.Add(CreateTask(partitions, idwFunction, tree, indx, numOutputs));
            }

            // Make Main thread wait for background/worker task threads.
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Wait();
            }


            watch.Stop();
            //Console.WriteLine("\nInterpolation of data took " + watch.Elapsed.TotalMinutes + " minutes.\n");
            watch.Reset();

            // --- OUTPUT OF INTERPOLATION --- //
            MileStoneEvent("OUTPUT TO FILE");
            watch.Start();

            progress = 1;
            using (FileStream fs = new FileStream(InterpolationOutputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (List<string> list in _outputLists)
                    {
                        foreach (string pointOutput in list)
                        {
                            sw.WriteLine(pointOutput);

                            // Increment Progress Bar here.
                            percentComplete = ((double)progress / (double)numOutputs) * 100;
                            GISLocationProcessed(string.Format("\r{0}" + " outputs written. [{1:#.##}% Complete]", progress, percentComplete));
                            
                            //Console.Write("\r{0}" + " outputs written. [{1:#.##}% Complete]", progress, percentComplete);
                            progress++;
                        }
                    }
                    sw.Flush();
                }
            }

            watch.Stop();
            //Console.WriteLine("\nOutput of data took " + watch.Elapsed.TotalSeconds + " seconds.\n\n");
            watch.Reset();

             
            // --- VALIDATION, ERROR CALC, AND OUTPUT OF BOTH --- //
            
            MileStoneEvent("VALIDATING");
            watch.Start();

            // LOOCV Class is internally threaded.
            LeaveOneOutCrossValidation loocv = new LeaveOneOutCrossValidation(_listGISDataPoints, tree,this.BaseOutputDirectory);

            loocv.ItemProcessed += loocv_ItemProcessed;
            loocv.MileStoneEvent += loocv_MileStoneEvent;
            loocv.Validate();
            loocv.CalculateError();

            watch.Stop();
            //Console.WriteLine("\nValidation of data took " + watch.Elapsed.TotalMinutes + " minutes.\n");
            watch.Reset();

            MileStoneEvent("COMPLETED");
            //Console.ReadLine();
        }

        void loocv_MileStoneEvent(string output)
        {
            MileStoneEvent(output);
        }

        void loocv_ItemProcessed(string output)
        {
            GISLocationProcessed(output);
        }

        
        private Task CreateTask(List<GISDataPoint>[] partitions, InverseDistanceWeightedExtension idwFunction, KDTree tree, int indx, int numOutputs)
        {
            var task = Task.Factory.StartNew(() =>
            {
                _outputLists[indx] = new List<string>();

                int partSize = partitions[indx].Count;
                for (int i = 0; i < partSize; i++)
                {
                    GISDataPoint dp = partitions[indx][i];
                    dp.measurement = idwFunction.Interpolate(tree, dp, _listGISDataPoints);
                    
                    //_outputLists[indx].Add(dp.ToString());    
                    _outputLists[indx].Add(dp.ToStringForOutputFile());    

                    // Increment Progress Bar here.
                    percentComplete = ((double)progress / (double)numOutputs) * 100;
                    //Console.Write("\r{0}" + " calculations done. [{1:#.##}% Complete]", progress, percentComplete);
                    GISLocationProcessed(string.Format("\r{0}" + " calculations done. [{1:#.##}% Complete]", progress, percentComplete));
                    progress++;
                }
            });
            return task;
        }       
    }
}
