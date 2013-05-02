using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7090.DataTypes
{
    public class GISDataPointDistance
    {
        public GISDataPoint GSPoint { get; set; }
        public double Distance { get; set; }

        public GISDataPointDistance()
        {

        }

        public GISDataPointDistance(GISDataPoint gsPoint, double distance)
        {
            GSPoint = gsPoint;
            Distance = distance;
        }
    }
}
