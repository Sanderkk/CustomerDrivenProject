using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace src.Database
{
    public interface IUploadDataRepository
    {
        SubscriptionObject UploadData(string encodedData, string encodedConfig);
    }
}
