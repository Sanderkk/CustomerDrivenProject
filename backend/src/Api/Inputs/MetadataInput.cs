using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Inputs
{
    public class MetadataInput
    {
        [GraphQLNonNullType]
        public int SensorID { get; set; }
    }
}
