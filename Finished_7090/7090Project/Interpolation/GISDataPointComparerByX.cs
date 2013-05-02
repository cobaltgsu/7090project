using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class GISDataPointComparerByX : IComparer<GISDataPoint>
    {
        int IComparer<GISDataPoint>.Compare(GISDataPoint obj1, GISDataPoint obj2)
        {
            return (obj1.x.CompareTo(obj2.x));
        }

    }
}
