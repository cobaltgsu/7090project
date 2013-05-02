using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090.Interpolation
{
 
    public class NeighborComparer : IComparer<Neighbor>
    {
        int IComparer<Neighbor>.Compare(Neighbor x, Neighbor y)
        {
            return (x.Distance.CompareTo(y.Distance));
        }
    }
}
