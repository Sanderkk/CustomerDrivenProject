using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Npgsql;
using src.Config;
using Newtonsoft.Json;
using src.Api.Types;

namespace src.Database
{
    public class FishFarmRepository : IFishFarmRepository
    {

        // NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(databaseSettings.DatabaseConnectionString);

        private IDatabaseConfig _databaseSettings;
        public FishFarmRepository(IDatabaseConfig databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<GenericObject> GetTimeSeries(string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
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
                        timeData.Add(dataReader.GetFieldValue<DateTime>(i).ToUniversalTime());
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
                StartDate = timeData.Count > 0 ? timeData[0] : DateTime.UtcNow,
                EndDate = timeData.Count > 0 ? timeData[timeData.Count - 1] : DateTime.UtcNow,
                Time = timeData,
                Data = data,
                NumberData = numberData
            };
            return result;
        }


        public async Task<List<SensorType>> GetSensorsData(string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var fieldsCount = dataReader.GetColumnSchema().Count();
            var sensorData = new List<SensorType>();
            while (dataReader.Read())
            {
                var result = new SensorType()
                {
                    SensorId = dataReader.GetFieldValue<int>(0),
                    SensorTabel = dataReader.GetFieldValue<string>(1),
                    SensorColumns = dataReader.GetFieldValue<string>(2)?.Split(".").ToList()
                };
                sensorData.Add(result);
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return sensorData;
        }

        public async Task<GenericTimeType> GetTimeSeriesPeriode(string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var fieldsCount = dataReader.GetColumnSchema().Count();
            GenericTimeType result = null;
            while (dataReader.Read())
            {
                result = new GenericTimeType()
                {
                    From = dataReader.GetFieldValue<DateTime>(0),
                    To = dataReader.GetFieldValue<DateTime>(1),
                };
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }
    }
}
