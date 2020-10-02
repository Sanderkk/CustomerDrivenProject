using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Inputs
{
    public class TimeSeriesPeriodeInput
    {
        [GraphQLNonNullType]
        public string TableName { get; set; }
        [GraphQLNonNullType]
        public int SensorId { get; set; }
    }
}
