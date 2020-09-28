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

        public async Task<GenericObject> GetTimeSeries(string queryString)
        {
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var timeData = new List<DateTime>();
            var data = new Dictionary<string, List<string>>();
            var numberData = new Dictionary<string, List<Decimal>>();

            var fieldsCount = dataReader.GetColumnSchema().Count();
            List<string> fieldNames = new List<string>();
            for(var i = 0; i < fieldsCount; i++) {
                var fieldName = dataReader.GetName(i);
                fieldNames.Add(fieldName);
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
                        var dataValue = dataReader.GetFieldValue<dynamic>(i);
                        Type dataType = dataValue.GetType();
                        if (dataType.Equals(typeof(Decimal)))
                        {
                            if (!numberData.ContainsKey(fieldNames[i]))
                            {
                                numberData[dataReader.GetName(i)] = new List<Decimal>();
                            }
                            numberData[fieldNames[i]].Add(dataValue);
                        } else
                        {
                            if (!data.ContainsKey(fieldNames[i]))
                            {
                                data[dataReader.GetName(i)] = new List<string>();
                            }
                            data[fieldNames[i]].Add(dataReader.GetFieldValue<dynamic>(i).ToString());
                        }
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
                Data = data,
                NumberData = numberData
            };
            return result;
        }

        public async Task<List<string>> GetDbTables(string queryString)
        {
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            List<string> tableNames = new List<string>();
            while (dataReader.Read())
            {
                var data = dataReader.GetFieldValue<dynamic>(0);
                tableNames.Add(data);
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return tableNames;
        }

        public async Task<List<string>> GetTableColumns(string queryString)
        {
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            List<string> result = new List<string>();
            while (dataReader.Read())
            {
                var value = dataReader.GetFieldValue<dynamic>(0);
                result.Add(value);
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }
    }
}
