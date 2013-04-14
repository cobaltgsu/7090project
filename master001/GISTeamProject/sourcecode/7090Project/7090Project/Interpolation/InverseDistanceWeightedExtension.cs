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



        public double Interpolate(double candidateLocationX, double candidateLocationY, double encodedCandidateTime)
        {
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
            var neighbors = from points in gisPointDistances.OrderBy(p => p.Distance).Take(NumberOfNeighbors)
                                select points;
            
            foreach(GISDataPointDistance gsd in neighbors)
            {
                interpolationResult  += (CalculateWeight(gsd.Distance, neighbors) * gsd.GSPoint.measurement);
            }
            
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

/*
         void Each<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
*/ 
    }
}
