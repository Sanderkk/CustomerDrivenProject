using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Types
{
    public class TestType : ITestType
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
