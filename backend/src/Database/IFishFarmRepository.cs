using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public interface IFishFarmRepository
    {
        Task<GenericObject> GetTimeSeries();
    }
}
