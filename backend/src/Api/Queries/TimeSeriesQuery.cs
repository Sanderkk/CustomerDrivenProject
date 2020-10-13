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
using HotChocolate.Execution;

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
            DateTime from;
            DateTime to;
            if (input.SpecifiedTimePeriode)
            {
                if (input.From == null || input.To == null || input.From >= input.To)
                {
                    throw new QueryException(ErrorBuilder.New().SetMessage("There is an error with the given to and from times.").Build());
                }
                from = input.From ?? DateTime.Now;
                to = input.To ?? DateTime.Now;
            } else
            {
                if (input.TicksBackwards == null || input.TicksBackwards <= 0)
                {
                    throw new QueryException(ErrorBuilder.New().SetMessage("Please specify ticks backwards.").Build());
                }
                from = new DateTime(DateTime.Now.Ticks - input.TicksBackwards ?? 1 );
                to = DateTime.Now;
            }
            var variance = DateTime.UtcNow.Ticks - DateTime.Parse("2020-08-12T08:21:19.000Z").Ticks;
            var queryString = DbQueryBuilder.CreateTimeSeriesQueryString(input.SensorId, input.TableName, input.ColumnNames, from, to);
            return repo.GetTimeSeries(input.TableName, queryString).Result;
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
