using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using parser.Config;


namespace parser
{
    public class Program
    {
        static public void Main(string[] args)
        {   
            // Get config from parser.json file
            var path = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("parser.json")
                .Build();

            var dataConfig = builder.GetSection("ParserConfig:Wavedata").Get<ParserConfig>();

            int[] id = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}; // information about how many sensors and sensorID a file consists of
            
            //ParserFilePath parser = new ParserFilePath(dataConfig);
            //(List<String>, string[]) parsedFile = parser.ParseFile("Data/Optode/20200812T082107.txt", false);
            //PrepareWritingDataToDB.PrepareQueryAndWrite(parsedFile.Item1, dataConfig, parsedFile.Item2, id);

            //DataClass dataClass = new Tension();
            //(List<String>, string[]) parsedFile = ParseFile("Data/Tension/2020-08-26 00.42.24.txt", dataClass, false);
            //PrepareWritingDataToDB.PrepareQuery(parsedFile.Item1, dataClass, parsedFile.Item2, 3);

            //DataClass dataClass = new Wavedata();
            //(List<String>, string[]) parsedFile = ParseFile("Data/ACE_Buoy_Wavedata.csv", dataClass, false);

            //DataClass dataClass = new Metocean();
            //(List<String>, string[]) parsedFile = ParseFile("Data/ACE_Buoy_Metoceandata.csv", dataClass, false);
            //PrepareWritingDataToDB.PrepareQuery(parsedFile.Item1, dataClass, parsedFile.Item2, 5);

            Byte[] bytes = File.ReadAllBytes("Data/ACE_Buoy_Wavedata.csv");
            String b = Convert.ToBase64String(bytes);

            var base64EncodedBytes = System.Convert.FromBase64String(b);
            string file = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            
            ParserString parser = new ParserString(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile(file, false);
            PrepareWritingDataToDB.PrepareQueryAndWrite(parsedFile.Item1, dataConfig, parsedFile.Item2);
        }
    }
}
