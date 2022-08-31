using Tehotasapaino.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using static Tehotasapaino.Models.DayAHeadPriceAPIClient;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Tehotasapaino
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

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
