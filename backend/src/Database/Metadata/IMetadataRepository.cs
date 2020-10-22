using src.Api.Inputs;
using src.Api.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public interface IMetadataRepository
    {
        Task<List<MetadataType>> GetMetadata(string queryString);
        Task<MetadataType> addMetadataToDatabase(MetadataInput metadata);
    }
}
