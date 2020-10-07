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
        public List<MetadataType> GetLastMetadata(
            //[GraphQLNonNullType] MetadataInput input,
            [GraphQLNonNullType] int sensorID,
            [Service] IMetadataRepository repo
            )
        {
            var queryString = DbQueryBuilder.CreateMetadataBySensorIDString(sensorID,true);
            return repo.GetMetadataBySensorID(queryString).Result;
        }

        public List<MetadataType> GetAllMetadata(
            [GraphQLNonNullType] int sensorID,
            [Service] IMetadataRepository repo
            )
        {
            var queryString = DbQueryBuilder.CreateMetadataBySensorIDString(sensorID,false);
            return repo.GetMetadataBySensorID(queryString).Result;
        }
            }
}

