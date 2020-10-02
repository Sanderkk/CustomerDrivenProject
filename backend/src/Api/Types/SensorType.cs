using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Types
{
    public class SensorType
    {
        public int SensorId { get; set; }
        public string SensorTabel { get; set; }
        public List<string> SensorColumns { get; set; }
    }
}
