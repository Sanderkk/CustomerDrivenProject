using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Inputs
{
    public class UploadDataInput
    {
        [GraphQLNonNullType]
        public string encodedData { get; set; } // base64String
        [GraphQLNonNullType]
        public string encodedConfig { get; set; } // base64String
    }
}
