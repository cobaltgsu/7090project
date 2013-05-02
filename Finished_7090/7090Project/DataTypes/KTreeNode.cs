using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7090.DataTypes
{
    public class KTreeNode
    {
        public GISDataPoint dataPoint {get;set;}
        public KTreeNode childLeft { get; set;}
        public KTreeNode childRight {get;set;}
        public KTreeNode parent { get; set; }

        public KTreeNode(GISDataPoint point)
        {
            dataPoint = point;
        }
    }
}
