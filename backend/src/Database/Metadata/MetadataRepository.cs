using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Npgsql;
using src.Config;
using Newtonsoft.Json;
using src.Api.Inputs;
using src.Api.Types;

namespace src.Database
{
    public class MetadataRepository : IMetadataRepository
    {

        private IDatabaseConfig _databaseSettings;
        public MetadataRepository(IDatabaseConfig databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        //Helping methods
        public MetadataType BuildMetadataObject(NpgsqlDataReader dataReader)
        {
            var tmpResult = new MetadataType()
            {
                //Never null fields in Database
                MetadataID = dataReader.GetFieldValue<int>(dataReader.GetOrdinal("id")),
                SensorID = dataReader.GetFieldValue<int>(dataReader.GetOrdinal("sensor_id")),
                CreatedAt = dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("created_at")),
                UpdatedAt = dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("updated_at")),
                Name = dataReader.GetFieldValue<string>(dataReader.GetOrdinal("name")),
                Number = dataReader.GetFieldValue<string>(dataReader.GetOrdinal("number")),
                //Possible null fields in Database: If that column contain DBNull, return null casted in the right format
                Coordinate = dataReader["coordinate"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("coordinate")),
                Altitude = dataReader["altitude"] == DBNull.Value ? (int?)null : dataReader.GetFieldValue<int>(dataReader.GetOrdinal("altitude")),
                LocationID = dataReader["location_id"] == DBNull.Value ? (int?)null : dataReader.GetFieldValue<int>(dataReader.GetOrdinal("location_id")),
                LocationDescription = dataReader["description"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("description")),
                Company = dataReader["company"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("company")),
                ServicePartner = dataReader["service_partner"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("service_partner")),
                Department = dataReader["department"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("department")),
                OwnerID = dataReader["owner_id"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("owner_id")),
                SerialNumber = dataReader["serial_number"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("serial_number")),
                Tag1 = dataReader["tag_1"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("tag_1")),
                Tag2 = dataReader["tag_2"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("tag_2")),
                Tag3 = dataReader["tag_3"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("tag_3")),
                Identificator = dataReader["identificator"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("identificator")),
                PurchaseDate = dataReader["purchase_date"] == DBNull.Value ? (DateTime?)null : dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("purchase_date")),
                WarrantyDate = dataReader["warranty_date"] == DBNull.Value ? (DateTime?)null : dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("warranty_date")),
                NextService = dataReader["next_service"] == DBNull.Value ? (DateTime?)null : dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("next_service")),
                PlannedDisposal = dataReader["planned_disposal"] == DBNull.Value ? (DateTime?)null : dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("planned_disposal")),
                ActualDisposal = dataReader["actual_disposal"] == DBNull.Value ? (DateTime?)null : dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("actual_disposal")),
                ModelNumber = dataReader["model_number"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("model_number")),
                Picture = dataReader["picture"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("picture")),
                Signal = dataReader["signal"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("signal")),
                MeasureArea = dataReader["measure_area"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("measure_area")),
                Website = dataReader["website"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("website")),
                InspectionRound = dataReader["inspection_round"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("inspection_round")),
                Lending = dataReader["lending"] == DBNull.Value ? (bool?)null : dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("lending")),
                Timeless = dataReader["timeless"] == DBNull.Value ? (bool?)null : dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("timeless")),
                CheckOnInspectionRound = dataReader["check_on_inspectionround"] == DBNull.Value ? (bool?)null : dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("check_on_inspectionround")),
                Tollerance = dataReader["tollerance"] == DBNull.Value ? (bool?)null : dataReader.GetFieldValue<bool>(dataReader.GetOrdinal("tollerance")),
                LendingPrice = dataReader["lending_price"] == DBNull.Value ? (float?)null : dataReader.GetFieldValue<float>(dataReader.GetOrdinal("lending_price")),
                CableLength = dataReader["cable_length"] == DBNull.Value ? (float?)null : dataReader.GetFieldValue<float>(dataReader.GetOrdinal("cable_length")),
                Voltage = dataReader["voltage"] == DBNull.Value ? (string?)null : dataReader.GetFieldValue<string>(dataReader.GetOrdinal("voltage")),
                OutdatedFrom = dataReader["outdated_from"] == DBNull.Value ? (DateTime?)null : dataReader.GetFieldValue<DateTime>(dataReader.GetOrdinal("outdated_from"))
            };
            return tmpResult;
        }
        public bool isLocationNew(MetadataInput newMetadata, MetadataType lastMetadata){
        if(newMetadata.Coordinate == lastMetadata.Coordinate && 
                newMetadata.Altitude == lastMetadata.Altitude &&
                newMetadata.LocationDescription == lastMetadata.LocationDescription){
                return false;
            }
         return true;
        }
        public bool isLocationNotEmpty(MetadataInput newMetadata){
        return newMetadata.Coordinate != null || newMetadata.Altitude != null || newMetadata.LocationDescription != null; 
        }
        public async Task<int?> saveLocationToDB(MetadataInput newMetadata, NpgsqlConnection connection){
            string newLocationQuery = MetadataQueryBuilder.InsertLocationString(newMetadata);
            int? locationID = null;
            using var cmdLocation = new NpgsqlCommand(newLocationQuery);
            cmdLocation.Connection = connection;
            var dataReaderLocation = await cmdLocation.ExecuteReaderAsync();

            var fieldsCountLocation = dataReaderLocation.GetColumnSchema().Count();
            while (dataReaderLocation.Read())
            {
                locationID = dataReaderLocation["id"] == DBNull.Value ? (int?)null : dataReaderLocation.GetFieldValue<int>(dataReaderLocation.GetOrdinal("id"));
            }
            await dataReaderLocation.CloseAsync();
            return locationID;
        }
        public async Task<MetadataType> saveMetadataToDB(int? locationID, MetadataInput newMetadata, NpgsqlConnection connection){
                String newMetadataQuery = MetadataQueryBuilder.CreateInsertMetadataString(newMetadata, locationID);
                using var cmd = new NpgsqlCommand(newMetadataQuery);
                cmd.Connection = connection;
                var dataReader = await cmd.ExecuteReaderAsync();

                MetadataType addedMetadata = null;
                while (dataReader.Read())
                {
                    addedMetadata = BuildMetadataObject(dataReader);
                };
                await dataReader.CloseAsync();
            return addedMetadata;
        }

        //Queries
        public async Task<List<MetadataType>> GetMetadata(string queryString)
        {

            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            var fieldsCount = dataReader.GetColumnSchema().Count();
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
        public async Task<MetadataType> addMetadataToDatabase(MetadataInput newMetadata)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            
            await _npgsqlConnection.OpenAsync();
            //Retrieve the last metadata from database if exists
            String lastMetadataQuery = MetadataQueryBuilder.CreateMetadataString(newMetadata.SensorID,null,true);
            int? locationID = null;
            MetadataType lastMetadata =null;
            var queryResult = GetMetadata(lastMetadataQuery).Result;
            bool addNewLocation = true;
            // If the sensor has previous metadata
            if(queryResult.Count == 1){
                lastMetadata = queryResult.ElementAt(0);
                locationID = lastMetadata.LocationID;
                addNewLocation = isLocationNew(newMetadata, lastMetadata);

                //If location is new for that metadata, registrer it.
                if (isLocationNew(newMetadata, lastMetadata))
                {
                    if(isLocationNotEmpty(newMetadata)){
                        //Add the new location to the database and update the locationID with the returned ID from the Database
                        locationID = await saveLocationToDB(newMetadata, _npgsqlConnection);
                    } else {
                        // if all of location fields are empty then set locationID to null
                        locationID = null;
                        }
                }

                //save the new metadata to DB
                MetadataType addedMetadata = await saveMetadataToDB(locationID, newMetadata, _npgsqlConnection);

                //Update outdated timestamp on last metadata
                String updateQuery = MetadataQueryBuilder.UpdateOldMetadataString(addedMetadata.CreatedAt, lastMetadata.MetadataID);
                using var cmdUpdate = new NpgsqlCommand(updateQuery);
                cmdUpdate.Connection = _npgsqlConnection;
                await cmdUpdate.ExecuteNonQueryAsync();
                await _npgsqlConnection.CloseAsync();
            
                return addedMetadata;
            } else {
                //save location to DB if not empty
                if(isLocationNotEmpty(newMetadata)){
                    //Add the new location to the database and update the locationID with the returned ID from the Database
                    locationID = await saveLocationToDB(newMetadata, _npgsqlConnection);
                }
            
                //save the new metadata to DB
                return await saveMetadataToDB(locationID, newMetadata, _npgsqlConnection);
            }
        }
    }
}