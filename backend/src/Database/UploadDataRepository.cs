using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using parser;
using parser.Config;

namespace src.Database
{
    public class UploadDataRepository : IUploadDataRepository
    {
        public void UploadData(
            string encodedString,
            string parserTypeName,
            int[] sensorIds
            )
        {
            // Get config from parser.json file
            var path = Directory.GetCurrentDirectory();
            var newPath = Path.GetFullPath(Path.Combine(path, @"../services/parser/"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("parser.json")
                .Build();

            var base64EncodedBytes = System.Convert.FromBase64String(encodedString);
            string file = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            var dataConfig = builder.GetSection("ParserConfig:" + parserTypeName).Get<ParserConfig>();
            ParserString parser = new ParserString(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile(file, false);
            PrepareWritingDataToDB.PrepareQueryAndWrite(parsedFile.Item1, dataConfig, parsedFile.Item2, sensorIds);
        }
    }
}
