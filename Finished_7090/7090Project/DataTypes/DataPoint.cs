using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7090.DataTypes
{
    public class DataPoint
    {
        public double x { get; set; }
        public double y { get; set; }
        public Int64  id { get; set; }

        public DataPoint(Int64 idParameter, double xParameter, double yParameter)
        {
            id = idParameter;
            x = xParameter;
            y = yParameter;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}",id,x,y);
        }
    }
}
