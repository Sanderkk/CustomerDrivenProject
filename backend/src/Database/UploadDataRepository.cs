using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using parser;
using parser.Config;

namespace src.Database
{
    public class UploadDataRepository : IUploadDataRepository
    {
        public SubscriptionObject UploadData(
            string encodedData,
            string encodedConfig
            )
        {
            // Get config from json file
            byte[] byteArray = Convert.FromBase64String(encodedConfig);
            string jsonBack = System.Text.Encoding.UTF8.GetString(byteArray);
            var dataConfig = JsonConvert.DeserializeObject<ParserConfig>(jsonBack);
            
            var base64EncodedBytes = Convert.FromBase64String(encodedData);
            string file = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            
            ParserString parser = new ParserString(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile(file, false);
            PrepareWritingDataToDB.PrepareQueryAndWrite(parsedFile.Item1, dataConfig, parsedFile.Item2);

            int[] sensorIds = dataConfig.sensorIDs;
            List<String> record = parsedFile.Item1;
            int numColumns = dataConfig.fieldIndexes[1] - dataConfig.fieldIndexes[0];
            DateTime fromDate = DateTime.ParseExact(record[0], dataConfig.timeFormatTimescaleDB, System.Globalization.CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(record[record.Count - numColumns - 1], dataConfig.timeFormatTimescaleDB, System.Globalization.CultureInfo.InvariantCulture);
            SubscriptionObject so = new SubscriptionObject(sensorIds, fromDate, toDate);
            return so;
        }
    }
}
