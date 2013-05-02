using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class GISDataPointComparerByY : IComparer<GISDataPoint>
    {
        int IComparer<GISDataPoint>.Compare(GISDataPoint obj1, GISDataPoint obj2)
        {
            return (obj1.y.CompareTo(obj2.y));
        }

    }
}
