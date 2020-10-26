using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using src.Utils;
using System.IO;
using parser;
using parser.Config;
using src.Database;

namespace src
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Byte[] bytesData = File.ReadAllBytes("Data/ACE_Buoy_Wavedata.csv");
            //String bData = Convert.ToBase64String(bytesData);
            
            //Byte[] bytesConfig = File.ReadAllBytes("wavedata.json");
            //String bConfig = Convert.ToBase64String(bytesConfig);

            //UploadDataRepository u = new UploadDataRepository();
            //u.UploadData(bData, bConfig);
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
