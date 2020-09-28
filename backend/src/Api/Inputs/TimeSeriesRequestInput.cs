using System;
using System.Collections.Generic;
using HotChocolate;

namespace src.Api.Inputs
{
    public class TimeSeriesRequestInput
    {
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