using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

namespace src.Api.Inputs
{
    public class TimeSeriesRequestInput
    {
        [GraphQLNonNullType]
        public List<int> Sensors { get; set; }
        [GraphQLNonNullType]
        public bool SpecifiedTimePeriode { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public long? TicksBackwards { get; set; }
    }
}