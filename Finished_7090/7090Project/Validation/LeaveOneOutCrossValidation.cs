using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project7090.DataTypes;
using Project7090.Interpolation;

namespace Project7090.Validation
{
    class LeaveOneOutCrossValidation
    {
        private int[] numNeighbors = new int[] { 3, 4, 5, 6, 7 };
        private double[] exponent = new double[] { 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0 };
        private double[][][] looData;

        private KDTree tree;
        private InverseDistanceWeightedExtension idwFunction;
        private List<GISDataPoint> listGISDataPoints;

        private int progress = 1;
        private double percentComplete;
        private int numThreads;
        private string _outputDirectory = string.Empty;

        public delegate void ItemProcessedHandler(string output);
        public event ItemProcessedHandler ItemProcessed;

        public delegate void MileStoneEventHandler(string output);
        public event MileStoneEventHandler MileStoneEvent;   


        public LeaveOneOutCrossValidation(List<GISDataPoint> listGISDataPoints, KDTree tree, string outputDirectory)
        {
            this.listGISDataPoints = listGISDataPoints;
            this.tree = tree;
            this._outputDirectory = outputDirectory;
        }

        public void Validate()
        {
            int size = listGISDataPoints.Count;
            int numOutputs = numNeighbors.Length * exponent.Length * size;

            idwFunction = new InverseDistanceWeightedExtension();

            //Get numThreads.
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

            using (FileStream fs = new FileStream(System.IO.Path.Combine(_outputDirectory,"loocv_idw.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(string.Format("{0,-10}", "ORIGINAL"));
                    for (int n = 0; n < numNeighbors.Length; n++)
                    {
                        for (int e = 0; e < exponent.Length; e++)
                        {
                            sw.Write(string.Format("{0,-1}{1,-1:D}{2,-1}{3,-1:F1}    ", "N", numNeighbors[n], "E", exponent[e]));
                        }
                    }
                    sw.WriteLine("");

                    List<GISDataPoint>[] partitions = ProcessingEngine.PartitionDataSet(listGISDataPoints, numThreads); // Partition our data.

                    looData = new double[partitions.Length][][];
                    for (int indx = 0; indx < partitions.Length; indx++)
                    {
                        looData[indx] = RectangularArrays.ReturnRectangularDoubleArray(partitions[indx].Count, numNeighbors.Length * exponent.Length);
                    }

                    List<Task> tasks = new List<Task>(); // Put thread tasks in a container to iterate over for Task.Wait() ... makeshift WaitAll() threads.

                    // Create Threads
                    for (int indx = 0; indx < partitions.Length; indx++)
                    {
                        // Must methodize the task creation. Otherwise above for loop with get out of range while threads are created and give exception.
                        tasks.Add(CreateTask(partitions, indx, numOutputs));
                    }

                    // Make Main thread wait for background/worker task threads.
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        tasks[i].Wait();
                    }

                    // Output Results Outside of Threads (StreamWriter is not thread safe.)
                    //Console.WriteLine("\n\nOutputting Validation Results...\n");
                    MileStoneEvent("Writing Validation Results");

                    progress = 1;

                    for (int indx = 0; indx < partitions.Length; indx++)
                    {
                        int partSize = partitions[indx].Count;
                        for (int i = 0; i < partSize; i++)
                        {
                            sw.Write(string.Format("{0,-10:F1}", partitions[indx][i].measurement));
                            for (int n = 0; n < numNeighbors.Length; n++)
                            {
                                for (int e = 0; e < exponent.Length; e++)
                                {
                                    sw.Write(string.Format("{0,-10:F1}", looData[indx][i][n * exponent.Length + e]));

                                    // Increment Progress Bar here.
                                    percentComplete = ((double)progress / (double)numOutputs) * 100;
                                    //Console.Write("\r{0}" + " outputs written. [{1:#.##}% Complete]", progress, percentComplete);
                                    ItemProcessed(string.Format("\r{0}" + " outputs written. [{1:#.##}% Complete]", progress, percentComplete));
                                    progress++;
                                }
                            }
                            sw.WriteLine("");
                            //sw.Flush();
                        }
                    }
                    sw.Flush();
                }
            }
        }

        private Task CreateTask(List<GISDataPoint>[] partitions, int indx, int numOutputs)
        {
            var task = Task.Factory.StartNew(() =>
            {
                int partSize = partitions[indx].Count;
                for (int i = 0; i < partSize; i++)
                {
                    GISDataPoint dp = partitions[indx][i];

                    for (int n = 0; n < numNeighbors.Length; n++)
                    {
                        List<Neighbor> nearNeighbors = tree.GetNearestNeighbors(dp, numNeighbors[n] + 1); // Get neighbors.
                        List<Neighbor> looNeighbors = new List<Neighbor>();        // Copy a range of neighbors into new array.
                        looNeighbors = nearNeighbors.GetRange(1, numNeighbors[n]);    // Transpose neighbors leaving one out.

                        for (int e = 0; e < exponent.Length; e++)
                        {
                            double result = idwFunction.Interpolate(looNeighbors, dp, exponent[e]);
                            looData[indx][i][n * exponent.Length + e] = result;

                            // Increment Progress Bar here.
                            percentComplete = ((double)progress / (double)numOutputs) * 100;
                            //Console.Write("\r{0}" + " validations completed. [{1:#.##}% Complete]", progress, percentComplete);
                            ItemProcessed(string.Format("\r{0}" + " validations completed. [{1:#.##}% Complete]", progress, percentComplete));
                            progress++;
                        }
                    }                    
                }
            });
            return task;
        }


        public void CalculateError()
        {
            List<GISDataPoint>[] partitions = ProcessingEngine.PartitionDataSet(listGISDataPoints, numThreads); // Partition our data.

            int numColumns = numNeighbors.Length * exponent.Length;

            double[] MAE = new double[numColumns];
            double[] MSE = new double[numColumns];
            double[] MARE = new double[numColumns];
            double[] MSRE = new double[numColumns];
            double[] RMSE = new double[numColumns];
            double[] RMSRE = new double[numColumns];

            for (int j = 0; j < numColumns; j++)
            {
                MAE[j] = 0;
                MSE[j] = 0;
                MARE[j] = 0;
                MSRE[j] = 0;
                RMSE[j] = 0;
                RMSRE[j] = 0;
            }

            //Console.WriteLine("\n\nCalculating Error Results...\n");
            MileStoneEvent("Calculate Error Results");

            progress = 0;

            using (FileStream fs = new FileStream(System.IO.Path.Combine(_outputDirectory,"error_statistics_idw.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write("{0,-10}", " ");
                    for (int n = 0; n < numNeighbors.Length; n++)
                    {
                        for (int e = 0; e < exponent.Length; e++)
                        {
                            sw.Write(string.Format("{0,-1}{1,-1:D}{2,-1}{3,-1:F1}    ", "N", numNeighbors[n], "E", exponent[e]));
                        }
                    }
                    sw.WriteLine("");

                    for (int indx = 0; indx < partitions.Length; indx++)
                    {
                        int partSize = partitions[indx].Count;
                        for (int i = 0; i < partSize; i++)
                        {
                            GISDataPoint dp = partitions[indx][i];
                            for (int j = 0; j < numColumns; j++)
                            {
                                MAE[j] += Math.Abs(looData[indx][i][j] - dp.measurement);
                                MSE[j] += Math.Pow((looData[indx][i][j] - dp.measurement), 2);
                                MARE[j] += (Math.Abs(looData[indx][i][j] - dp.measurement)) / dp.measurement;
                                MSRE[j] += (Math.Pow((looData[indx][i][j] - dp.measurement), 2)) / dp.measurement;
                            }

                            // Increment Progress Bar here.
                            progress++;
                            percentComplete = ((double)progress / (double)listGISDataPoints.Count) * 100;
                            //Console.Write("\r{0}" + " error calculations completed. [{1:#.##}% Complete]", progress, percentComplete);
                            ItemProcessed(string.Format("\r{0}" + " error calculations completed. [{1:#.##}% Complete]", progress, percentComplete));
                        }
                    }

                    for (int j = 0; j < numColumns; j++)
                    {
                        MAE[j] /= listGISDataPoints.Count;
                        MSE[j] /= listGISDataPoints.Count;
                        MARE[j] /= listGISDataPoints.Count;
                        MSRE[j] /= listGISDataPoints.Count;
                        RMSE[j] = Math.Sqrt(MSE[j]);
                        RMSRE[j] = Math.Sqrt(MSRE[j]);
                    }

                    // Output all columns.
                    sw.Write("{0,-10}", "MAE");
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write("{0,-10:F1}", MAE[j]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "MSE");
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write("{0,-10:F1}", MSE[j]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "MARE");
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write("{0,-10:F1}", MARE[j]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "MSRE");
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write("{0,-10:F1}", MSRE[j]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "RMSE");
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write("{0,-10:F1}", RMSE[j]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "RMSRE");
                    for (int j = 0; j < numColumns; j++)
                    {
                        sw.Write("{0,-10:F1}", RMSRE[j]);
                    }
                    sw.WriteLine("");
                    sw.Flush();
                }
            }
        }

        // Helper for C# jagged/rectangular arrays creation.
        internal static partial class RectangularArrays
        {
            internal static double[][] ReturnRectangularDoubleArray(int size1, int size2)
            {
                double[][] array;
                if (size1 > -1)
                {
                    array = new double[size1][];
                    if (size2 > -1)
                    {
                        for (int array1 = 0; array1 < size1; array1++)
                        {
                            array[array1] = new double[size2];
                        }
                    }
                }
                else
                    array = null;

                return array;
            }
        }

    }
}
