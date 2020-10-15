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
            var sensorsQueryString = DbQueryBuilder.CreateSensorsQueryString();
            var sensors = repo.GetSensorsData(sensorsQueryString).Result;
            var sensorNameIdPairs = input.Sensors.Select(
                x => (
                    x,
                    sensors.Where(y => y.SensorIds.Contains(x)).Select(y => y.SensorTypeName).FirstOrDefault()
                    ??
                    throw new QueryException(ErrorBuilder.New().SetMessage("There exists no sensor with id "+x.ToString()+".").Build())
                      )
            ).ToList();

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
            var queryString = "";
            foreach(var sensorNameIdPair in sensorNameIdPairs)
            {
                queryString = queryString + DbQueryBuilder.CreateTimeSeriesQueryString(sensorNameIdPair.Item1, sensorNameIdPair.Item2, from, to);
            }
            return repo.GetTimeSeries("", queryString).Result;
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
