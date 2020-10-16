using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Database
{
    public interface IUploadDataRepository
    {
        bool UploadData(string encodedString, string parserTypeName, List<int> sensorIds);
    }
}
