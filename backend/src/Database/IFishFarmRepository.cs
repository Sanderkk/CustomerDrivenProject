using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public interface IFishFarmRepository
    {
        Task<GenericObject> GetTimeSeries(string queryString);
        Task<List<string>> GetDbTables(string queryString);
        Task<List<string>> GetTableColumns(string queryString);
    }
}
