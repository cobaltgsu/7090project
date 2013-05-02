using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class GISDataPointComparerByT : IComparer<GISDataPoint>
    {
        int IComparer<GISDataPoint>.Compare(GISDataPoint obj1, GISDataPoint obj2)
        {
            return (obj1.time.CompareTo(obj2.time));
        }

    }
}
