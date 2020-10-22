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
        public List<MetadataType> GetMetadata(
            string sensorNumber,
            int? sensorID,
            bool? onlyLast,
            [Service] IMetadataRepository repo
            )
        { 
            var queryString = MetadataQueryBuilder.CreateMetadataString(sensorID, sensorNumber, onlyLast);
            return repo.GetMetadata(queryString).Result;
        }
     }

}

