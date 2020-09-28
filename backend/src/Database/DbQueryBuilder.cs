using System;
using System.Collections.Generic;

namespace src.Database
{
    public class DbQueryBuilder
    {
        
        public static string CreateTimeSeriesQueryString(string tableName, List<string> columnNames, DateTime startDate, DateTime endDate)
        {
            var selectString = "SELECT " + (columnNames.Count > 0 ? string.Join(",", columnNames) : "*");
            var tableString = "FROM " + tableName;
            var timeFilterString = $"WHERE time >= '{startDate.ToString("s")}.000Z' AND time < '{endDate.ToString("s")}.000Z'";
            var queryString = selectString + " " + tableString + " " + timeFilterString;
            return queryString;
        }

        public static string CreateTableListQueryString()
        {
            return "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
        }

        public static string CreateColumnListQueryString(string tableName)
        {
            return $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}'";
        }

        public static string CreateTableAndColumnQueryString()
        {
            return "select t.table_name, array_agg(c.column_name::text) as columns from information_schema.tables t inner join information_schema.columns c on t.table_name = c.table_name where t.table_schema = 'public' and t.table_type= 'BASE TABLE' and c.table_schema = 'public' group by t.table_name;";
        }

    }
}