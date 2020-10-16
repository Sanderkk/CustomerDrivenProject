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
        public string encodedFileData { get; set; } // base64String
        [GraphQLNonNullType]
        public string parserType { get; set; }
        [GraphQLNonNullType]
        public List<int> sensorIds { get; set; }
    }
}
