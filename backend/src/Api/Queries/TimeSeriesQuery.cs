using HotChocolate;
using HotChocolate.Types;
using src.Api.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Inputs;
using HotChocolate.AspNetCore.Authorization;

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
            var queryString = DbQueryBuilder.CreateTimeSeriesQueryString(input.SensorId,input.TableName, input.ColumnNames, input.From, input.To);
            return repo.GetTimeSeries(queryString).Result;
        }

        public List<SensorType> GetSensors(
            [Service] IFishFarmRepository repo
            )
        {
            var queryString = DbQueryBuilder.CreateSensorsQueryString();
            return repo.GetSensorsData(queryString).Result;
        }

        public GenericTimeType GetTimeSeriesPeriode(
            TimeSeriesPeriodeInput input,
            [Service] IFishFarmRepository repo
            )
        {
            var queryString = DbQueryBuilder.CreateTimeSeriesPeriodeQueryString(input.SensorId, input.TableName);
            return repo.GetTimeSeriesPeriode(queryString).Result;
        }
    }
}
