using System;
using System.Collections.Generic;

namespace src.Database
{
    public class DbQueryBuilder
    {
        
        public static string CreateTimeSeriesQueryString(int? sensorId, string tableName, List<string> columnNames, DateTime startDate, DateTime endDate)
        {
            var selectString = "SELECT " + (columnNames.Count > 0 ? string.Join(",", columnNames) : "*");
            var tableString = "FROM " + tableName;
            var timeFilterString = $"WHERE time >= '{startDate.ToString("s")}.000Z' AND time < '{endDate.ToString("s")}.000Z'";
            var sensor = sensorId != null ? " AND sensorId = " + sensorId.ToString() : "";
            var order = "Order BY time ASC";
            var queryString = selectString + " " + tableString + " " + timeFilterString + sensor + " " + order;
            return queryString;
        }

        public static string CreateSensorsQueryString()
        {
            return "SELECT * FROM sensor LIMIT 1000;";
        }

        public static string CreateTimeSeriesPeriodeQueryString(int sensorId, string tableName)
        {
            return $@"SELECT 
                    first(time, time), last(time, time) 
                    FROM {tableName} where sensorId={sensorId};
                ";
        }

        public static string CreateMetadataBySensorIDString(int? sensorID)
        {
            return $"SELECT * from METADATA where sensor_id={sensorID}";
        }
    }
}