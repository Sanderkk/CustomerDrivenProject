using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using parser;
using src.Utils;

namespace src
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            DataClass dataClass = new Optode();
            Csv csv = Csv.FromTextFile("Data/Optode/20200812T082107.txt", dataClass, true);
            csv.WriteCsvFile("Data/csv/optode.csv");
            // Hello world
            // Hello world
            
            //DataClass dataClass = new Tension();
            //Csv csv = Csv.FromTextFile("Data/Tension/2020-08-25 22.42.24.txt", dataClass, false);
            //csv.WriteCsvFile("Data/csv/optode.csv");

            //DataClass dataClass = new Wavedata();
            //List<PointData> record = ParseFile("Data/ACE_Buoy_Wavedata.csv", dataClass, false);

            //DataClass dataClass = new Metocean();
            //List<PointData> record = ParseFile("Data/ACE_Buoy_Metoceandata.csv", dataClass, false);
            // CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
