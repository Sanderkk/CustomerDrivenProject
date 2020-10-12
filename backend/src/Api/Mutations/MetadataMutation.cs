using HotChocolate;
using HotChocolate.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Api.Inputs;
using src.Api.Types;


namespace src.Api.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class MetadataMutation
    {
        public MetadataType addMetadata(
           [GraphQLNonNullType] MetadataInput newMetadata,
           [Service] IMetadataRepository repo
           )
        {
            return repo.addMetadataToDatabase(newMetadata).Result;
        }
    }
}