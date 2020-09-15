using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using src.Pipeline;

namespace src
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            Optode optode = new Optode();
            var result = optode.ParseTextFile(
                "C:\\Users\\sebas\\dev\\CustomerDrivenProject\\backend\\src\\Data\\Optode\\20200812T082107.txt");
            var i = 0;
            foreach (var line in result)
            {
                Console.WriteLine("Line: ",line);
            }
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
