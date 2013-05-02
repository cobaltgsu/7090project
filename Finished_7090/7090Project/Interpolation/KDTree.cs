using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class KDTree
    {
        private int k;
        private int size;
        private KTreeNode root;

        public KDTree(List<GISDataPoint> dataSet)
        {
            k = 3;
            root = buildTree(dataSet, 0);
        }

        private KTreeNode buildTree(List<GISDataPoint> ds, int depth)
        {
            int dimension = depth % k;
            int median = (ds.Count / 2);
            KTreeNode currentNode = null;

            switch(dimension)
            {
                case 1:
                    ds.Sort(new GISDataPointComparerByX());
                    break;
                case 2:
                    ds.Sort(new GISDataPointComparerByY());
                    break;
                case 3:
                    ds.Sort(new GISDataPointComparerByT());
                    break;
            }

            while (median > 0 && ds.ElementAt(median - 1).GetDimension(dimension) == ds.ElementAt(median).GetDimension(dimension))
            {
                median = median - 1;
            }

            currentNode = new KTreeNode(ds.ElementAt(median));

            if (median > 0)
            {
                KTreeNode childLeft = buildTree(ds.GetRange(0, median), depth + 1);
                childLeft.parent = currentNode;
                currentNode.childLeft = childLeft;
            }

            if (ds.Count > median + 1)
            {
                int medianPlusOne = median + 1;

                //beginning index to ending index
                KTreeNode childRight = buildTree(ds.GetRange(medianPlusOne, (ds.Count - medianPlusOne)), depth + 1); //shouldn't it be median instead of median + 1?
                childRight.parent = currentNode;
                currentNode.childRight = childRight;
            }

            return currentNode;
        }

        public List<Neighbor> GetNearestNeighbors(GISDataPoint dataPoint, int numberOfNeighbors)
        {
            List<Neighbor> nearestNeighbors = new List<Neighbor>();
            List<Neighbor> neighbors = GetNearestNeighbors(dataPoint, numberOfNeighbors, root, 0);

            for (int indx = 0; indx < numberOfNeighbors; indx++)
            {
                nearestNeighbors.Add(neighbors.ElementAt(indx));
            }

            return nearestNeighbors;
        }

        public List<Neighbor> GetNearestNeighbors(GISDataPoint dataPoint, int numberOfNeighbors, KTreeNode startNode, int depth)
        {
            KTreeNode currentNode = startNode;
            KTreeNode skippedSubRoot;

            List<Neighbor> nearestNeighbors = new List<Neighbor>();
            List<Neighbor> nearestNeighborsAlternate = new List<Neighbor>();

            nearestNeighbors.Add(new Neighbor(currentNode.dataPoint, CalculateDistance(currentNode.dataPoint, dataPoint)));

            double targetDistance = CalculateDistance(dataPoint, nearestNeighbors.ElementAt(0).Point);
            double candidateDistance;
            int dimension;
            int k = this.k;

            while (currentNode.childLeft != null || currentNode.childRight != null)
            {
                dimension = depth % k;

                if (dataPoint.GetDimension(dimension) < currentNode.dataPoint.GetDimension(dimension))
                {
                    if (currentNode.childLeft != null)
                    {
                        currentNode = currentNode.childLeft;
                    }
                    else
                    {
                        currentNode = currentNode.childRight;
                    }
                }
                else
                {
                    if (currentNode.childRight != null)
                    {
                        currentNode = currentNode.childRight;
                    }
                    else
                    {
                        currentNode = currentNode.childLeft;
                    }
                }

                candidateDistance = CalculateDistance(dataPoint, currentNode.dataPoint);

                if (candidateDistance < targetDistance || nearestNeighbors.Count < numberOfNeighbors)
                {
                    nearestNeighbors.Add(new Neighbor(currentNode.dataPoint, candidateDistance));
                    Neighbor.Sort(nearestNeighbors, numberOfNeighbors);
                    targetDistance = nearestNeighbors.ElementAt(nearestNeighbors.Count - 1).Distance;
                }

                depth++;
            }

            while (currentNode != startNode)
            {
                depth--;
                currentNode = currentNode.parent;

                dimension = depth % k;

                candidateDistance = Math.Sqrt(Math.Pow((currentNode.dataPoint.GetDimension(dimension) - dataPoint.GetDimension(dimension)), 2));

                if (candidateDistance < targetDistance || nearestNeighbors.Count < numberOfNeighbors)
                {
                    if (dataPoint.GetDimension(dimension) < currentNode.dataPoint.GetDimension(dimension))
                    {
                        if (currentNode.childLeft != null && currentNode.childRight != null)
                        {
                            skippedSubRoot = currentNode.childRight;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (currentNode.childLeft != null && currentNode.childRight != null)
                        {
                            skippedSubRoot = currentNode.childLeft;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    nearestNeighborsAlternate = GetNearestNeighbors(dataPoint, numberOfNeighbors, skippedSubRoot, depth + 1);
                    nearestNeighbors.AddRange(nearestNeighborsAlternate);
                    Neighbor.Sort(nearestNeighbors, numberOfNeighbors);
                    targetDistance = nearestNeighbors.ElementAt(nearestNeighbors.Count - 1).Distance;
                }
            }
            return nearestNeighbors;
        }


        private double CalculateDistance(GISDataPoint dataPoint1, GISDataPoint dataPoint2)
        {
            double distanceResult = 0.0;
                        
            distanceResult = Math.Sqrt(Math.Pow((dataPoint1.x - dataPoint2.x), 2.0) +
                             Math.Pow((dataPoint1.y - dataPoint2.y), 2.0) +
                             Math.Pow((dataPoint1.time - dataPoint2.time), 2.0));
            
            return distanceResult;
        }
    }
}
