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
            if (input.sensorIds == null || input.sensorIds.Length <= 0)
            {
                throw new QueryException(ErrorBuilder.New().SetMessage("There were no sensorIds as input").Build());
            }
            try
            {
                repo.UploadData(input.encodedFileData, input.parserType, input.sensorIds);
                return true;
            } catch
            {
                throw new QueryException(ErrorBuilder.New().SetMessage("There occured an error with either with encoded file string or the parser type given.").Build());
            }
        }

    }
}
