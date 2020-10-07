using HotChocolate;
using HotChocolate.Types;
using src.Api.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Inputs;

namespace src.Api.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class MetadataQuery
    {
        public MetadataType GetMetadata(
            //[GraphQLNonNullType] MetadataInput input,
            [GraphQLNonNullType] int sensorID,
            [Service] IFishFarmRepository repo
            )
        {
            var queryString = DbQueryBuilder.CreateMetadataBySensorIDString(sensorID);
            return repo.GetMetadataBySensorID(queryString).Result;
        }
    }
}

