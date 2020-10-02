using src.Api.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public interface IFishFarmRepository
    {
        Task<GenericObject> GetTimeSeries(string queryString);
        Task<List<SensorType>> GetSensorsData(string queryString);
        Task<GenericTimeType> GetTimeSeriesPeriode(string queryString);
    }
}
