using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;
using Project7090.Interpolation;

namespace Project7090
{
    public class Neighbor : IComparable<Neighbor>
    {
        public GISDataPoint Point { get; set;}
        public double Distance {get;set;}
        public double Weight { get; set; }

        public Neighbor(GISDataPoint dataPoint, double distance)
        {
            Point = dataPoint;
            Distance = distance;
        }

        public Neighbor()
        {
            
        }
        
        public static void Sort(List<Neighbor> list, int numberOfNeighbors)
        {
            list.Sort(new NeighborComparer());

            while (list.Count > numberOfNeighbors)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public int CompareTo(Neighbor n)
        {
            return (int)Math.Sign(this.Distance - n.Distance);
        }

        class NeighborComparer : IComparer<Neighbor>
        {
            public int Compare(Neighbor n1, Neighbor n2)
            {
                return (int)Math.Sign(n1.Distance - n2.Distance);
            }
        }
    }
}
