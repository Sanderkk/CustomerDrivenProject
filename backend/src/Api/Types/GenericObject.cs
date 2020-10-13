using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public class GenericObject
    {
        public string Table { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DateTime> Time { get; set; }
        public List<DataObject<Decimal>> Data { get; set; }
    }

    public class DataObject<T>
    {
        public string Name { get; set; }
        public List<T> Data { get; set; }
        public long StartTime { get; set; }
        public long Interval { get; set; }
    }
}
