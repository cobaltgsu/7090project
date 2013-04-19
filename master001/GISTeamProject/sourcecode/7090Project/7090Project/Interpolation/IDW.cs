using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class IDW	// Inverse Distance Weighted Extension
    {
        //public GISDataSet DS { get; set; }
        //public int NumNeighbors { get; set; }
        //public double Exponent { get; set; }

        //public IDW(GISDataSet ds, int numNeighbors, double exponent)
        //{
        //    this.DS = ds;
        //    this.NumNeighbors = numNeighbors;
        //    this.Exponent = exponent;
        //}

        public static double Interpolate(GISDataPointDistance[] neighborList, double exponent, int numNeighbors)
        {
            int i;
            double sum = 0.0;

            for (i = 0; i < numNeighbors; i++)
            {
                sum += Weight(neighborList, neighborList[i].Distance, exponent, numNeighbors) * neighborList[i].GSPoint.measurement;
            }
            return sum;
        }

        public static double Distance(GISDataPoint dp1, GISDataPoint dp2)
        {
            double distance;
            double x1, y1, x2, y2, z1, z2, x, y, z;

            x1 = dp1.x;
            y1 = dp1.y;
            z1 = (double)dp1.time;

            x2 = dp2.x;
            y2 = dp2.y;
            z2 = (double)dp2.time;

            x = x1 - x2;
            y = y1 - y2;
            z = z1 - z2;

            distance = Math.Sqrt(x * x + y * y + z * z);
            return distance;
        }

        public static double Weight(double distance, double exponent)
        {
            double result;
            result = Math.Pow((1 / distance), exponent);
            return result;
        }

        public static double Weight(GISDataPointDistance[] neighborList, double exponent, int numNeighbors)
        {
            double sum = 0.0;
            double result;
            int i;
            for (i = 0; i < numNeighbors; i++)
            {
                result = Math.Pow((1 / neighborList[i].Distance), exponent);
                sum += result;
            }
            return sum;
        }

        public static double Weight(GISDataPointDistance[] neighborList, double distance, double exponent, int numNeighbors)
        {
            double result;
            result = Weight(distance, exponent) / Weight(neighborList, exponent, numNeighbors);
            return result;
        }
    }
}