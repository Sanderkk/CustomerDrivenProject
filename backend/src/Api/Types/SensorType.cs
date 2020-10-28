using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Types
{
    public class SensorType
    {
        public string SensorTypeName { get; set; }
        public List<int> SensorIds { get; set; }
        public List<string> SensorNumbers {get;set;}
        public List<string> SensorColumns { get; set; }
    }
}
