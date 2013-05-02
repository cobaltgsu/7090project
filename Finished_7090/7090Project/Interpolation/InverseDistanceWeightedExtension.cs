using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class InverseDistanceWeightedExtension
    {
        public GISDataSet GISDataToProcess {get;set;}
        public int NumberOfNeighbors { get; set; }
        public Double Exponent { get; set; }

        public InverseDistanceWeightedExtension()
        {

        }

        public double Interpolate(KDTree tree, GISDataPoint candidateDataPoint, List<GISDataPoint> gisData)
        {

            double interpolationResult = 0.0;

            List<Neighbor> neighbors = tree.GetNearestNeighbors(candidateDataPoint, this.NumberOfNeighbors);

            for (int indx = 0; indx < neighbors.Count; indx++)
            {
                interpolationResult += (CalculateWeight(CalculateDistance(candidateDataPoint, neighbors.ElementAt(indx).Point), neighbors) * neighbors.ElementAt(indx).Point.measurement);
            }

            return interpolationResult;
        }

        private double CalculateDistance(GISDataPoint pointCandidate, GISDataPoint pointToProcess)
        {
            double distanceResult = 0.0;

            distanceResult = Math.Sqrt(Math.Pow((pointToProcess.x - pointCandidate.x), 2.0) +
                             Math.Pow((pointToProcess.y - pointCandidate.y), 2.0) +
                             Math.Pow((pointToProcess.time - pointCandidate.time), 2.0));

            return distanceResult;
        }

        private double CalculateWeight(double distanceFromSelectedPoint, List<Neighbor> neighbors)
        {
            double weight = 0.0;

            double numerator = 0.0;
            double denominator = 0.0;

            foreach (Neighbor neighbor in neighbors)
            {
                denominator += Math.Pow((1 / neighbor.Distance), Exponent);
            }

            numerator = Math.Pow((1 / distanceFromSelectedPoint), Exponent);

            weight = (numerator / denominator);

            return weight;
        }

        public double Interpolate(List<Neighbor> nearNeighbors, GISDataPoint candidateDataPoint, double exponent)
        {
            double weightDenominator = 0;
            double interpolationResult = 0;

            for (int indx = 0; indx < nearNeighbors.Count; indx++)
            {
                weightDenominator += WeightNumerator(nearNeighbors[indx], exponent);
            }
            for (int indx = 0; indx < nearNeighbors.Count; indx++)
            {
                interpolationResult += ((nearNeighbors[indx].Weight * nearNeighbors[indx].Point.measurement) / weightDenominator);
            }

            return interpolationResult;
        }

        public double WeightNumerator(Neighbor neighbor, double exponent)
        {
            neighbor.Weight = Math.Pow(1 / neighbor.Distance, exponent);
            return neighbor.Weight;
        }

        /// <summary>
        /// --- OLD IMPLEMENTATIONS (For Reference and Debugging) --- ///
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="candidateDataPoint"></param>
        /// <param name="gisData"></param>
        /// <returns></returns>

        public double Interpolate(double candidateLocationX, double candidateLocationY, double encodedCandidateTime)
        {
            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            double interpolationResult = 0.0;

            //contains the GISDataPoint and it's calculated distance based on the candidateLocationPoint
            List<GISDataPointDistance> gisPointDistances = new List<GISDataPointDistance>();

            //for each entry in GISDatapoint list, calculate the distance to the candidateLocation
            foreach (GISDataPoint gisPoint in GISDataToProcess) 
            {
                gisPointDistances.Add(new GISDataPointDistance(gisPoint,CalculateDistance(gisPoint.x, gisPoint.y, candidateLocationX, candidateLocationY, gisPoint.time, encodedCandidateTime)));                
            }

            
            //List of GISDataPointDistance objecs from shortest to longest distance from the candidateLocationPoint
            //return a list with specified number of neighbors from the sorted list
            //var neighbors = from points in gisPointDistances.OrderBy(p => p.Distance).Take(NumberOfNeighbors)
            //                    select points;

            var neighbors = from points in gisPointDistances.AsParallel().OrderBy(p => p.Distance).Take(NumberOfNeighbors)
                            select points;

            gisPointDistances.Clear();
            gisPointDistances = null;
            
            
            foreach(GISDataPointDistance gsd in neighbors)
            {
                interpolationResult  += (CalculateWeight(gsd.Distance, neighbors) * gsd.GSPoint.measurement);
            }
            
            //watch.Stop();
            //Console.WriteLine("Interpolation Time: {0}", watch.Elapsed.TotalMilliseconds);

            return interpolationResult;
        }

        /// <summary>
        /// old implementation
        /// </summary>
        /// <param name="candidateLocationPoint"></param>
        /// <param name="encodedCandidateTime"></param>
        /// <returns></returns>
        public double Interpolate(DataPoint candidateLocationPoint, double encodedCandidateTime)
        {
            
            double interpolationResult = 0.0;
            
            //contains the GISDataPoint and it's calculated distance based on the candidateLocationPoint
            List<GISDataPointDistance> gisPointDistances = new List<GISDataPointDistance>();
            
            //for each entry in GISDatapoint list, calculate the distance       
            
            
            foreach (GISDataPoint gisPoint in GISDataToProcess)
            {              
               
               //gisPointDistances.Add(new GISDataPointDistance(gisPoint, CalculateDistance(candidateLocationPoint, gisPoint, encodedCandidateTime)));             
                gisPointDistances.Add(new GISDataPointDistance(gisPoint,CalculateDistance(gisPoint.x, gisPoint.y, candidateLocationPoint.x, candidateLocationPoint.y, gisPoint.time, encodedCandidateTime)));
            }          
            
            
            //List of GISDataPointDistance objecs from shortest to longest distance from the candidateLocationPoint
            //return a list with specified number of neighbors from the sorted list
            var neighbors = from points in gisPointDistances.OrderBy(p => p.Distance).Take(NumberOfNeighbors)
                                select points;
            
            foreach(GISDataPointDistance gsd in neighbors)
            {
                interpolationResult  += (CalculateWeight(gsd.Distance, neighbors) * gsd.GSPoint.measurement);
            }

            return interpolationResult ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointCandidate">Data point to be interpolated</param>
        /// <param name="pointToProcess">GIS data point to reference</param>
        /// <param name="encodedTimeTarget">The encoded time period to be used in distance calculation</param>
        /// <returns></returns>
        private double CalculateDistance(DataPoint pointCandidate, GISDataPoint pointToProcess, double encodedTimeTarget)
        {
            double distanceResult = 0.0;

            distanceResult = Math.Sqrt(Math.Pow((pointToProcess.x - pointCandidate.x),2.0) +
                             Math.Pow((pointToProcess.y - pointCandidate.y), 2.0) +
                             Math.Pow((pointToProcess.time - encodedTimeTarget), 2.0));

            return distanceResult;
        }

        /// <summary>
        /// Optimized version.  Runs faster than passing in objects
        /// </summary>
        /// <param name="xp">GISDataPoint.x</param>
        /// <param name="yp">GISDataPoint.y</param>
        /// <param name="xc">DataPoint.x</param>
        /// <param name="yc">DataPoint.y</param>
        /// <param name="timeP">GISDataPoint.time</param>
        /// <param name="encodedTimeTarget"></param>
        /// <returns></returns>
        public double CalculateDistance(double xp, double yp, double xc, double yc,double timeP,double encodedTimeTarget)
        {            
            return (Math.Sqrt(((xp - xc) * (xp - xc)) +
                             ((yp - yc) * (yp - yc)) +
                             ((timeP - encodedTimeTarget) * (timeP - encodedTimeTarget))));
        }

        /***********************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gisSelectedPointDistance">distance of selected gis point from the point to be interpolated</param>
        /// <param name="neighbors">Keyvalue pair list that contains GISDataPoint and its distance</param>
        /// <returns></returns>
        private double CalculateWeight(double gisSelectedPointDistance, IEnumerable<GISDataPointDistance> neighbors)
        {
            double weight = 0.0;

            double numerator = 0.0;
            double denominator = 0.0;


            foreach (GISDataPointDistance gsDistancePoint in neighbors)
            {
                denominator += Math.Pow((1 / gsDistancePoint.Distance), Exponent);
            }

            numerator = Math.Pow((1 / gisSelectedPointDistance), Exponent);

            weight = (numerator / denominator);            

            return weight;
        }

        
        private double CalculateWeight(double gisSelectedPointDistance, IEnumerable<double> neighbors)
        {
            double weight = 0.0;

            double numerator = 0.0;
            double denominator = 0.0;


            foreach (double distance in neighbors)
            {
                denominator += Math.Pow((1 / distance), Exponent);
            }

            numerator = Math.Pow((1 / gisSelectedPointDistance), Exponent);

            weight = (numerator / denominator);

            return weight;
        }
/*
         void Each<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
*/ 
    }
}
