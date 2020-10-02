using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

namespace src.Api.Inputs
{
    public class TimeSeriesRequestInput
    {
        public int? SensorId { get; set; }
        [GraphQLNonNullType]
        public string TableName {get;set;}
        [GraphQLNonNullType]
        public List<string> ColumnNames {get;set;}
        [GraphQLNonNullType]
        public DateTime From { get; set; }
        [GraphQLNonNullType]
        public DateTime To { get; set; }
    }
}