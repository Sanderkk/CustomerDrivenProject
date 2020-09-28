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
        public Dictionary<string, List<string>> Data { get; set; }
        public Dictionary<string, List<Decimal>> NumberData { get; set; }
    }
}
