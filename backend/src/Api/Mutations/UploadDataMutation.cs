using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using src.Api.Inputs;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using src.Database;
using src.Api.Subscriptions;
using src.Api.Types;

namespace src.Api.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class UploadDataMutation
    {

        public async Task<bool> UploadData(
                [GraphQLNonNullType] UploadDataInput input,
                [Service]IUploadDataRepository repo,
                [Service]IEventSender eventSender
            )
        {
            try
            {
                var dataobj = repo.UploadData(input.encodedData, input.encodedConfig);
                foreach (var sensorId in dataobj.sensorIds)
                {
                    await eventSender.SendAsync(new OnDataInsert(
                        sensorId,
                        new CreatedDataValues()
                        {
                            sensorId = sensorId,
                            fromDate = dataobj.fromDate,
                            toDate = dataobj.toDate
                        }
                        )).ConfigureAwait(true);

                }
                return true;
            } catch
            {
                throw new QueryException(ErrorBuilder.New().SetMessage("There occured an error with either with encoded file string or the parser type given.").Build());
            }
        }

    }
}
