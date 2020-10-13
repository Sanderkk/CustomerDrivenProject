using System;
using System.Collections.Generic;
using System.Linq;

namespace src.Database
{
    public class DbQueryBuilder
    {
        
        public static string CreateTimeSeriesQueryString(int? sensorId, string tableName, List<string> columnNames, DateTime startDate, DateTime endDate)
        {
            if (columnNames.Count > 0)
            {
                columnNames = columnNames.Select(x => x.ToLower()).ToList();
                columnNames.Add("time");
                columnNames.Distinct();
            }
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

        public static string CreateMetadataBySensorIDString(int sensorID, bool onlyLast)
        {
            string query = $"SELECT * FROM metadata left join location on metadata.location_id=location.id where sensor_id={sensorID} ORDER By created_at desc ";
            if(onlyLast)
                query=query + " limit 1";
            return query;
        }
    }
}