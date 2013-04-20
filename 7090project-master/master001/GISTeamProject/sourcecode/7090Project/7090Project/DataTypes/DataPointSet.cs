using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7090.DataTypes
{
    public class DataPointSet : List<DataPoint>
    {
        public void Remove(Int64 key)
        {
            this.Remove(key);
        }

        public DataPoint Find(Int64 key)
        {
            return this.Find(obj => obj.id == key);
        }
    }
}
