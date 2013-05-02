using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7090.DataTypes
{
    public class GISDataSet : List<GISDataPoint>
    {
        public void Remove(Int64 key)
        {
            this.Remove(key);
        }

        public GISDataPoint Find(Int64 key)
        {
            return this.Find(obj => obj.id == key);   
        }
    }
}
