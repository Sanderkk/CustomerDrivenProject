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

           public async Task<List<MetadataType>> GetMetadataBySensorID(string queryString)
        {
            //Delete this two lines when updated database
            string tmpDatabaseString ="Host=sanderkk.com;Username=sintef;Password=123456;Database=fishfarm2";
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(tmpDatabaseString);
            
            //NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var fieldsCount = dataReader.GetColumnSchema().Count();
            //MetadataType result = null;
            List<MetadataType> allMetadata = new List<MetadataType>();
            while (dataReader.Read())
            {
                var result = BuildMetadataObject(dataReader);
                allMetadata.Add(result);
            };

            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return allMetadata;
        }
        public MetadataType BuildMetadataObject(NpgsqlDataReader dataReader ){
            var tmpResult = new MetadataType()
                {   
                    //Never null fields in Database
                    MetadataID = dataReader.GetFieldValue<int>(dataReader.GetOrdinal("id")),
                    SensorID = dataReader.GetFieldValue<int>(dataReader.GetOrdinal("sensor_id")),
                    CreatedAt = dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("created_at")),
                    UpdatedAt = dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("updated_at")),
                    Name = dataReader.GetFieldValue<string>(dataReader.GetOrdinal("name")),
                    Number =  dataReader.GetFieldValue<string>(dataReader.GetOrdinal("number")),
                    //Possible null fields in Database: If that column contain DBNull, return null casted in the right format
                    Coordinate = dataReader["coordinate"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("coordinate")),
                    Altitude = dataReader["altitude"]==DBNull.Value ? (int?)null :dataReader.GetFieldValue<int>(dataReader.GetOrdinal("altitude")),
                    LocationDescription = dataReader["description"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("description")),
                    Company = dataReader["company"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("company")),
                    ServicePartner = dataReader["service_partner"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("service_partner")),
                    Department = dataReader["department"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("department")),
                    OwnerID = dataReader["owner_id"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("owner_id")),
                    SerialNumber = dataReader["serial_number"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("serial_number")),
                    Tag1 = dataReader["tag_1"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("tag_1")),
                    Tag2 = dataReader["tag_2"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("tag_2")),
                    Tag3 = dataReader["tag_3"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("tag_3")),
                    Identificator = dataReader["identificator"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("identificator")),
                    PurchaseDate = dataReader["purchase_date"]==DBNull.Value ? (DateTime?)null :dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("purchase_date")),
                    WarrantyDate = dataReader["warranty_date"]==DBNull.Value ? (DateTime?)null :dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("warranty_date")),
                    NextService = dataReader["next_service"]==DBNull.Value ? (DateTime?)null :dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("next_service")),
                    PlannedDisposal = dataReader["planned_disposal"]==DBNull.Value ? (DateTime?)null :dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("planned_disposal")),
                    ActualDisposal = dataReader["actual_disposal"]==DBNull.Value ? (DateTime?)null :dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("actual_disposal")),
                    ModelNumber = dataReader["model_number"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("model_number")),
                    Picture = dataReader["picture"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("picture")),
                    Signal = dataReader["signal"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("signal")),
                    MeasureArea = dataReader["measure_area"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("measure_area")),
                    Website = dataReader["website"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("website")),
                    InspectionRound = dataReader["inspection_round"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("inspection_round")),
                    Lending = dataReader["lending"]==DBNull.Value ? (bool?)null :dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("lending")),
                    Timeless = dataReader["timeless"]==DBNull.Value ? (bool?)null :dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("timeless")),
                    CheckOnInspectionRound = dataReader["check_on_inspectionround"]==DBNull.Value ? (bool?)null :dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("check_on_inspectionround")),
                    Tollerance = dataReader["tollerance"]==DBNull.Value ? (bool?)null :dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("tollerance")),
                    LendingPrice = dataReader["lending_price"]==DBNull.Value ? (float?)null :dataReader.GetFieldValue<float>(dataReader.GetOrdinal("lending_price")),
                    CableLength = dataReader["cable_length"]==DBNull.Value ? (float?)null :dataReader.GetFieldValue<float>(dataReader.GetOrdinal("cable_length")),
                    Voltage = dataReader["voltage"]==DBNull.Value ? (string?)null :dataReader.GetFieldValue<string>(dataReader.GetOrdinal("voltage")),
                    OutdatedFrom=dataReader["outdated_from"]==DBNull.Value ? (DateTime?)null :dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("outdated_from"))
                };
            return tmpResult;
        } 
    }
}
