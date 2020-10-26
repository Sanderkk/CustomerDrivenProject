using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using src.Api.Inputs;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class UploadDataMutation
    {

        public bool UploadData(
                [GraphQLNonNullType] UploadDataInput input,
                [Service] IUploadDataRepository repo
            )
        {
            try
            {
                repo.UploadData(input.encodedData, input.encodedConfig);
                return true;
            } catch
            {
                throw new QueryException(ErrorBuilder.New().SetMessage("There occured an error with either with encoded file string or the parser type given.").Build());
            }
        }

    }
}
