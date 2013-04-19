using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;
using Project7090.Interpolation;

namespace Project7090.Validation
{
    class LOOCV
    {
        public GISDataSet DS { get; set; }

        private int size;
        private int progress = 0;
        private double percentComplete;
        private int[] numNeighbors = new int[] { 3, 4, 5, 6, 7 };
        private double[] exponent = new double[] { 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0 };
        private double[][] LOO;


        public LOOCV(GISDataSet DS)
        {
            this.DS = DS;
        }

        public void Validate()
        {
            size = DS.Count;
            GISDataPoint dp;
            LOO = RectangularArrays.ReturnRectangularDoubleArray(size, numNeighbors.Length * exponent.Length);

            using(FileStream fs = new FileStream("loocv_idw.txt", FileMode.Open))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(string.Format("{0,-10}", "Original"));
                    for (int j = 0; j < numNeighbors.Length; j++)
                    {
                        for (int k = 0; k < exponent.Length; k++)
                        {
                            sw.Write("{0,-1}", "n");
                            sw.Write("{0,-1:D}", numNeighbors[j]);
                            sw.Write("{0,-1}", "e");
                            sw.Write("{0,-1:F1}", exponent[k]);
                            sw.Write(" ");
                        }
                    }
                    sw.Write("\n");

                    for (int i = 0; i < size; i++)
                    {
                        dp = DS[i];
                        sw.Write("{0,-10:F}", dp.measurement);
                        for (int j = 0; j < numNeighbors.Length; j++)
                        {
                            GISDataPointDistance[] nearNeighbors = NearestNeighbors(dp, numNeighbors[j] + 1); // Get neighbors.
                            GISDataPointDistance[] looNeighbors = new GISDataPointDistance[numNeighbors[j]];        // Copy a range of neighbors into new array.
                                for (int index = 0; index < numNeighbors[j]; index++)   // Transpose neighbors.
                                {
                                    looNeighbors[index] = nearNeighbors[index + 1];
                                }

                            for (int k = 0; k < exponent.Length; k++)
                            {
                                double result = IDW.Interpolate(looNeighbors, exponent[k], numNeighbors[j]);
                                sw.Write("{0,-10:F}", result);
                                LOO[i][j * exponent.Length + k] = result;

                                // Increment Progress Bar here.
                                percentComplete = ((double)progress / (double)DS.Count) * 100;
                                Console.Write("\r{0}" + " calculations completed. [{1:#.#}% Complete]", progress, percentComplete);
                                progress++;
                            }
                        }
                        sw.Flush();
                    }
                }
            }
        }

        public void CalculateError()
        {
            double[] mae = new double[LOO[0].Length];
            double[] mse = new double[LOO[0].Length];
            double[] rmse = new double[LOO[0].Length];
            double[] mare = new double[LOO[0].Length];
            double[] msre = new double[LOO[0].Length];
            double[] rmsre = new double[LOO[0].Length];

            for (int i = 0; i < mae.Length; i++)
            {
                mae[i] = 0;
            }
            for (int i = 0; i < mse.Length; i++)
            {
                mse[i] = 0;
            }
            for (int i = 0; i < rmse.Length; i++)
            {
                rmse[i] = 0;
            }
            for (int i = 0; i < mare.Length; i++)
            {
                mare[i] = 0;
            }
            for (int i = 0; i < msre.Length; i++)
            {
                msre[i] = 0;
            }
            for (int i = 0; i < rmsre.Length; i++)
            {
                rmsre[i] = 0;
            }

            StreamWriter sw = new StreamWriter("error_statistics_idw.txt");

            sw.Write("{0,-7}", "");
            for (int j = 0; j < numNeighbors.Length; j++)
            {
                for (int k = 0; k < exponent.Length; k++)
                {
                    sw.Write("{0,-1}", "n");
                    sw.Write("{0,-1:D}", numNeighbors[j]);
                    sw.Write("{0,-1}", "e");
                    sw.Write("{0,-1:F1}", exponent[k]);
                    sw.Write(" ");
                }
            }
            sw.Write("\n");

            for (int i = 0; i < DS.Count; i++)
            {
                // Progress Bar update here.
                GISDataPoint dp = DS[i];
                for (int j = 0; j < LOO[0].Length; j++)
                {
                    mae[j] += Math.Abs(LOO[i][j] - dp.measurement);
                    mse[j] += Math.Pow((LOO[i][j] - dp.measurement), 2);
                    mare[j] += (Math.Abs(LOO[i][j] - dp.measurement)) / dp.measurement;
                    msre[j] += (Math.Pow((LOO[i][j] - dp.measurement), 2)) / dp.measurement;
                }
            }
            sw.Write("{0,-7}", "MAE");
            for (int i = 0; i < mae.Length; i++)
            {
                mae[i] /= DS.Count;
                sw.Write("{0,-7:F1}", mae[i]);
            }
            sw.Write("\n");
            sw.Write("{0,-7}", "MSE");
            for (int i = 0; i < mse.Length; i++)
            {
                mse[i] /= DS.Count;
                sw.Write("{0,-7:F1}", mse[i]);
            }
            sw.Write("\n");
            sw.Write("{0,-7}", "RMSE");
            for (int i = 0; i < rmse.Length; i++)
            {
                rmse[i] = Math.Sqrt(mse[i]);
                sw.Write("{0,-7:F1}", rmse[i]);
            }
            sw.Write("\n");
            sw.Write("{0,-7}", "MARE");
            for (int i = 0; i < mare.Length; i++)
            {
                mare[i] /= DS.Count;
                sw.Write("{0,-7:F1}", mare[i]);
            }
            sw.Write("\n");
            sw.Write("{0,-7}", "MSRE");
            for (int i = 0; i < msre.Length; i++)
            {
                msre[i] /= DS.Count;
                sw.Write("{0,-7:F1}", msre[i]);
            }
            sw.Write("\n");
            sw.Write("{0,-7}", "RMSRE");
            for (int i = 0; i < rmsre.Length; i++)
            {
                rmsre[i] = Math.Sqrt(msre[i]);
                sw.Write("{0,-7:F1}", rmsre[i]);
            }
            sw.Write("\n");
            sw.Flush();
        }

        public GISDataPointDistance[] NearestNeighbors(GISDataPoint dp, int numNeighbors)
        {
            GISDataPointDistance[] results = new GISDataPointDistance[numNeighbors];
            foreach (GISDataPoint dpd in DS)
            {
                double distance = IDW.Distance(dpd, dp);

                bool shift = false;
                GISDataPointDistance shifted = null;

                for (int i = 0; i < numNeighbors; i++)
                {
                    if (shift)
                    {
                        GISDataPointDistance temp = results[i];
                        results[i] = shifted;
                        shifted = temp;
                    }
                    else if (results[i] == null)
                    {
                        GISDataPointDistance newdpd = new GISDataPointDistance();
                        newdpd.GSPoint = dpd;
                        newdpd.Distance = distance;

                        results[i] = newdpd;
                    }
                    else if (distance < results[i].Distance)
                    {
                        GISDataPointDistance newdpd = new GISDataPointDistance();
                        newdpd.GSPoint = dpd;
                        newdpd.Distance = distance;

                        shifted = results[i];
                        results[i] = newdpd;

                        shift = true;
                    }
                }
            }
            return results;
        }

        // Helper to allow C# jagged/rectangular arrays for ideal data representation.
        internal static partial class RectangularArrays
        {
            internal static double[][] ReturnRectangularDoubleArray(int Size1, int Size2)
            {
                double[][] Array;
                if (Size1 > -1)
                {
                    Array = new double[Size1][];
                    if (Size2 > -1)
                    {
                        for (int Array1 = 0; Array1 < Size1; Array1++)
                        {
                            Array[Array1] = new double[Size2];
                        }
                    }
                }
                else
                    Array = null;

                return Array;
            }
        }
    }
}
