using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7090.DataTypes
{
    public class GISDataPoint : DataPoint
    {
        public double time { get; set; }
        public double measurement { get; set; }
        public TimeDomainContainer timeContainer { get; set; }

        public GISDataPoint(Int64 idParameter, double timeParameter, double xParameter, double yParameter, double measurementParameter, TimeDomainContainer timeContainerParameter)
            : base(idParameter, xParameter, yParameter)
        {
            time = timeParameter;
            measurement = measurementParameter;
            timeContainer = timeContainerParameter;
        }

        public double GetDimension(int dimension)
        {
            switch (dimension)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return time;
                default:
                    return -1;
            }
        }

        public string ToStringForOutputFile()
        {
            return string.Format("{0}\t{1}\t{2}", base.id, timeContainer.ToString(), measurement);
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}", base.id, timeContainer.ToString(), base.x, base.y, measurement);
        }

    }
}
