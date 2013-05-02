using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
    public class GISPointDistanceComparer : IComparer<GISDataPointDistance>
    {
        int IComparer<GISDataPointDistance>.Compare(GISDataPointDistance x, GISDataPointDistance y)
        {
            return (x.Distance.CompareTo(y.Distance));  
        }      
    }
}
