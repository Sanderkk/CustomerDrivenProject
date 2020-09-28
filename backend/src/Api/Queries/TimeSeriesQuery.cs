using HotChocolate;
using HotChocolate.Types;
using src.Api.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Inputs;

namespace src.Api.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class TimeSeriesQuery
    {
        public GenericObject GetTimeSeries(
            [GraphQLNonNullType] TimeSeriesRequestInput input,
            [Service] IFishFarmRepository repo
            )
        {
            var queryString = DbQueryBuilder.CreateTimeSeriesQueryString(input.TableName, input.ColumnNames, input.From, input.To);
            Console.WriteLine("Query:" + queryString);
            return repo.GetTimeSeries(queryString).Result;
        }

        public GenericObject GetTimeSeriesGiven(
            [Service] IFishFarmRepository repo
            )
        {
            var queryString = "Select * FROM tension WHERE time >= '2020-08-25T23:30:00.000Z' AND time < '2020-08-26T00:00:00.000Z'";
            return repo.GetTimeSeries(queryString).Result;
        }

        public List<string> GetTables(
            [Service] IFishFarmRepository repo
            )
        {
            // Query table names from current database
            var queryString = DbQueryBuilder.CreateTableListQueryString();
            return repo.GetDbTables(queryString).Result;
        }

        public List<string> GetTableColumns(
            [GraphQLNonNullType]string tableName,
            [Service] IFishFarmRepository repo
            )
        {
            // Query table names from current database
            var queryString = DbQueryBuilder.CreateColumnListQueryString(tableName);
            return repo.GetTableColumns(queryString).Result;
        }

        public Dictionary<string, List<string>> GetTableAndColumns(
            [Service] IFishFarmRepository repo
            )
        {
            // TODO: Query to do this?
            var results = new Dictionary<string, List<string>>();
            var queryString = DbQueryBuilder.CreateTableListQueryString();
            var tables = repo.GetDbTables(queryString).Result;
            foreach(var tableName in tables)
            {
                var columnsQueryString = DbQueryBuilder.CreateColumnListQueryString(tableName);
                var columns = repo.GetTableColumns(columnsQueryString).Result;
                results[tableName] = columns;
            };
            return results;
        }
    }
}
