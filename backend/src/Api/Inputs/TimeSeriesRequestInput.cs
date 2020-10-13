using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

namespace src.Api.Inputs
{
    public class TimeSeriesRequestInput
    {
        [GraphQLNonNullType]
        public int? SensorId { get; set; }
        [GraphQLNonNullType]
        public string TableName {get;set;}
        [GraphQLNonNullType]
        public List<string> ColumnNames {get;set;}

        [GraphQLNonNullType]
        public bool SpecifiedTimePeriode { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public long? TicksBackwards { get; set; }
    }
}