using System;
using System.Collections.Generic;
using src.Database;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using src.Database;
using src.Api.Types;

namespace src.Api.Subscriptions
{
    [ExtendObjectType(Name = "Subscription")]

    public class DataSubscription
    {
        public GenericObject OnData(
            int sensorId,
            IEventMessage message,
            //[Service] IUploadDataRepository _repository
            [Service] ITimeSeriesRepository _repository
            )
        {
            var data = (CreatedDataValues) message.Payload;
            List<(int, string)> sensorNameIdPairs = _repository.GetSensorTablePair(new List<int>(){ data.sensorId });
            var queryString = "";
            foreach(var sensorNameIdPair in sensorNameIdPairs)
            {
                queryString = queryString + TimeSeriesQueryBuilder.CreateTimeSeriesQueryString(
                    sensorNameIdPair.Item1, 
                    sensorNameIdPair.Item2, 
                    data.fromDate, 
                    data.toDate
                    );
            }
            return _repository.GetTimeSeries("", queryString).Result;
        }
    }
}
