using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Npgsql;
using src.Config;
using Newtonsoft.Json;
using src.Api.Types;
using HotChocolate.Types;
using HotChocolate.Execution;
using HotChocolate;

namespace src.Database
{
    public class TimeSeriesRepository : ITimeSeriesRepository
    {

        // NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(databaseSettings.DatabaseConnectionString);

        private IDatabaseConfig _databaseSettings;
        public TimeSeriesRepository(IDatabaseConfig databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public void ReadTimeSeriesData<T>(NpgsqlDataReader dataReader, DataObject<T> dataObject, int pos)
        {
            var value = dataReader.GetFieldValue<T>(pos);
            dataObject.Data.Add(value);
        }

        public List<(string, int)> GetColumnNames(NpgsqlDataReader dataReader)
        {
            var fieldsCount = dataReader.GetColumnSchema().Count();
            List<(string, int)> fieldNames = new List<(string, int)>();
            for (var i = 0; i < fieldsCount; i++)
            {
                var fieldName = dataReader.GetName(i);
                if (fieldName.Equals("time") || fieldName.Equals("sensorid"))
                {
                    continue;
                }
                fieldNames.Add((fieldName, i));
            }
            return fieldNames;
        }

        public async Task<GenericObject> GetTimeSeries(string tableName, string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var completeTimeData = new List<List<DateTime>>();
            var completeData = new List<DataObject<Decimal>>();

            do
            {
                var timeData = new List<DateTime>();
                var data = new List<DataObject<Decimal>>();
                List<(string, int)> tableColumns = this.GetColumnNames(dataReader);
                foreach(var column in tableColumns)
                {
                    data.Add(
                        new DataObject<Decimal>() {
                            Data = new List<Decimal>()
                        }
                    );
                }
                while(dataReader.Read())
                {
                    DateTime date = dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("time")).ToUniversalTime();
                    timeData.Add(date);
                    for (var i = 0; i < tableColumns.Count(); i++)
                    {
                        var dataValue = dataReader.GetFieldValue<dynamic>(tableColumns[i].Item2);
                        Type dataType = dataValue.GetType();
                        ReadTimeSeriesData<Decimal>(dataReader, data[i], tableColumns[i].Item2);
                    }
                };
                if (timeData.Count() == 0)
                {
                    throw new QueryException(ErrorBuilder.New().SetMessage("No data.").Build());
                }

                var startTime = timeData[0].Ticks;
                var interval = timeData[1].Ticks - startTime;
                for (var i = 0; i < tableColumns.Count(); i++)
                {
                    data[i].Name = tableColumns[0].Item1;
                    data[i].StartTime = startTime;
                    data[i].Interval = interval;
                }
                completeTimeData.Add(timeData);
                completeData.AddRange(data);
            } while (dataReader.NextResult());

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();

            var selectedTimeData = completeTimeData.FirstOrDefault();
            var result = new GenericObject()
            {
                Table = tableName,
                StartDate = selectedTimeData.Count > 0 ? selectedTimeData[0].ToUniversalTime() : DateTime.UtcNow,
                EndDate = selectedTimeData.Count > 0 ? selectedTimeData[selectedTimeData.Count - 1].ToUniversalTime() : DateTime.UtcNow,
                Time = selectedTimeData,
                Data = completeData
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
                var sensorTypeNameNull = dataReader.IsDBNull(1);
                var sensorTypeName = !sensorTypeNameNull ? dataReader.GetFieldValue<string>(1) : "";
                var sensor = sensorData.Where(x => x.SensorTypeName.Equals(sensorTypeName ?? "")).FirstOrDefault();
                if (sensor == null)
                {
                    var result = new SensorType()
                    {
                        SensorTypeName = sensorTypeName,
                        SensorIds = new List<int>() { dataReader.GetFieldValue<int>(0) },
                        SensorColumns = !dataReader.IsDBNull(2) ? dataReader.GetFieldValue<string>(2)?.Split(".").ToList() : null
                    };
                    sensorData.Add(result);
                } else
                {
                    sensor.SensorIds.Add(dataReader.GetFieldValue<int>(0));
                }
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
                    From = dataReader.GetFieldValue<DateTime>(0).ToUniversalTime(),
                    To = dataReader.GetFieldValue<DateTime>(1).ToUniversalTime(),
                };
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }


    }
}
