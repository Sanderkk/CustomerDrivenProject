using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public class UploadDataRepository : IUploadDataRepository
    {
        public bool UploadData(
            string encodedString,
            string parserTypeName,
            List<int> sensorIds
            )
        {
            return true;
        }
    }
}
