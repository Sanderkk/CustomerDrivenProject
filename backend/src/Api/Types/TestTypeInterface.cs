using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Types
{
    public interface ITestType
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
