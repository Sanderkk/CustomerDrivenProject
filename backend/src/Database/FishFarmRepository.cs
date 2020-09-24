using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Npgsql;
using src.Config;
using Newtonsoft.Json;

namespace src.Database
{
    public class FishFarmRepository : IFishFarmRepository
    {
        
        private NpgsqlConnection _npgsqlConnection;
        public FishFarmRepository(IDatabaseConfig databaseSettings)
        {
            _npgsqlConnection = new NpgsqlConnection(databaseSettings.DatabaseConnectionString);
        }

        public async Task<GenericObject> GetTimeSeries()
        {
            var command = "SELECT * from wavedata_from_bjorn";
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(command);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var timeData = new List<DateTime>();
            var data = new Dictionary<string, List<string>>();

            var fieldsCount = dataReader.GetColumnSchema().Count();
            List<string> fieldNames = new List<string>();
            for(var i = 0; i < fieldsCount; i++) {
                fieldNames.Add(dataReader.GetName(i));
                data[dataReader.GetName(i)] = new List<string>();
            }

            while(dataReader.Read())
            {
                for (var i = 0; i < fieldNames.Count(); i++)
                {
                    if (fieldNames[i].Equals("time"))
                    {
                        timeData.Add(dataReader.GetFieldValue<DateTime>(i));
                    } else
                    {
                        data[fieldNames[i]].Add(dataReader.GetFieldValue<dynamic>(i).ToString());
                    }
                }
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();

            var result = new GenericObject()
            {
                Table = "tension",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Time = timeData,
                Data = data
            };
            return result;
        }
    }
}
