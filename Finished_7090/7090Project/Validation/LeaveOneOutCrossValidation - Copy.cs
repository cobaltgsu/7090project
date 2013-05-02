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
        private double[][] loocvArrays;

        private KDTree tree;
        private InverseDistanceWeightedExtension idwFunction;
        private List<GISDataPoint> listGISDataPoints;

        private int progress = 0;
        private double percentComplete;
        private int numThreads;

        public LeaveOneOutCrossValidation(List<GISDataPoint> listGISDataPoints, KDTree tree)
        {
            this.listGISDataPoints = listGISDataPoints;
            this.tree = tree;
        }

        public void Validate()
        {
            int size = listGISDataPoints.Count;
            int numOutputs = numNeighbors.Length * exponent.Length * size;
            if (Environment.ProcessorCount >= 4)
            {
                numThreads = Environment.ProcessorCount - 2;
            }
            else if (Environment.ProcessorCount > 1 && Environment.ProcessorCount < 4)
            {
                numThreads = Environment.ProcessorCount - 1;
            }
            else
            {
                numThreads = 1;
            }

            loocvArrays = RectangularArrays.ReturnRectangularDoubleArray(size, numNeighbors.Length * exponent.Length);
            idwFunction = new InverseDistanceWeightedExtension();

            using (FileStream fs = new FileStream("C:\\Lectures\\Spring2013\\GIS\\M8\\loocv_idw.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
                    Console.WriteLine("\n\nOutputting Validation Results...\n");
                    progress = 0;

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
                                    sw.Write(string.Format("{0,-10:F1}", loocvArrays[(indx * partSize) + i][n * exponent.Length + e]));

                                    // Increment Progress Bar here.
                                    percentComplete = ((double)progress / (double)numOutputs) * 100;
                                    Console.Write("\r{0}" + " outputs written. [{1:#.##}% Complete]", progress, percentComplete);
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
                            loocvArrays[(indx * partSize) + i][n * exponent.Length + e] = result;

                            // Increment Progress Bar here.
                            percentComplete = ((double)progress / (double)numOutputs) * 100;
                            Console.Write("\r{0}" + " validations completed. [{1:#.##}% Complete]", progress, percentComplete);
                            progress++;
                        }
                    }                    
                }
            });
            return task;
        }


        public void CalculateError()
        {
            double[] MAE = new double[loocvArrays[0].Length];
            double[] MSE = new double[loocvArrays[0].Length];
            double[] RMSE = new double[loocvArrays[0].Length];
            double[] MARE = new double[loocvArrays[0].Length];
            double[] MSRE = new double[loocvArrays[0].Length];
            double[] RMSRE = new double[loocvArrays[0].Length];

            for (int i = 0; i < MAE.Length; i++)
            {
                MAE[i] = 0;
            }
            for (int i = 0; i < MSE.Length; i++)
            {
                MSE[i] = 0;
            }
            for (int i = 0; i < RMSE.Length; i++)
            {
                RMSE[i] = 0;
            }
            for (int i = 0; i < MARE.Length; i++)
            {
                MARE[i] = 0;
            }
            for (int i = 0; i < MSRE.Length; i++)
            {
                MSRE[i] = 0;
            }
            for (int i = 0; i < RMSRE.Length; i++)
            {
                RMSRE[i] = 0;
            }

            Console.WriteLine("\n\nCalculating Error Results...\n");

            progress = 0;

            using (FileStream fs = new FileStream("C:\\Lectures\\Spring2013\\GIS\\M8\\error_statistics_idw.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
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

                    for (int indx = 0; indx < listGISDataPoints.Count; indx++)
                    {
                        GISDataPoint dp = listGISDataPoints[indx];
                        for (int i = 0; i < loocvArrays[0].Length; i++)
                        {
                            MAE[i] += Math.Abs(loocvArrays[indx][i] - dp.measurement);
                            MSE[i] += Math.Pow((loocvArrays[indx][i] - dp.measurement), 2);
                            MARE[i] += (Math.Abs(loocvArrays[indx][i] - dp.measurement)) / dp.measurement;
                            MSRE[i] += (Math.Pow((loocvArrays[indx][i] - dp.measurement), 2)) / dp.measurement;
                        }

                        // Increment Progress Bar here.
                        percentComplete = ((double)progress / (double)listGISDataPoints.Count) * 100;
                        Console.Write("\r{0}" + " error calculations completed. [{1:#.##}% Complete]", progress, percentComplete);
                        progress++;
                    }
                    sw.Write("{0,-10}", "MAE");
                    for (int i = 0; i < MAE.Length; i++)
                    {
                        MAE[i] /= listGISDataPoints.Count;
                        sw.Write("{0,-10:F1}", MAE[i]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "MSE");
                    for (int i = 0; i < MSE.Length; i++)
                    {
                        MSE[i] /= listGISDataPoints.Count;
                        sw.Write("{0,-10:F1}", MSE[i]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "RMSE");
                    for (int i = 0; i < RMSE.Length; i++)
                    {
                        RMSE[i] = Math.Sqrt(MSE[i]);
                        sw.Write("{0,-10:F1}", RMSE[i]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "MARE");
                    for (int i = 0; i < MARE.Length; i++)
                    {
                        MARE[i] /= listGISDataPoints.Count;
                        sw.Write("{0,-10:F1}", MARE[i]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "MSRE");
                    for (int i = 0; i < MSRE.Length; i++)
                    {
                        MSRE[i] /= listGISDataPoints.Count;
                        sw.Write("{0,-10:F1}", MSRE[i]);
                    }
                    sw.WriteLine("");
                    sw.Write("{0,-10}", "RMSRE");
                    for (int i = 0; i < RMSRE.Length; i++)
                    {
                        RMSRE[i] = Math.Sqrt(MSRE[i]);
                        sw.Write("{0,-10:F1}", RMSRE[i]);
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
