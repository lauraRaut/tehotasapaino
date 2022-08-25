using active_directory_aspnetcore_webapp_openidconnect_v2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using static active_directory_aspnetcore_webapp_openidconnect_v2.Models.ApiHelper;
using static active_directory_aspnetcore_webapp_openidconnect_v2.Models.TestModel;

namespace active_directory_aspnetcore_webapp_openidconnect_v2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //  CreateHostBuilder(args).Build().Run();
            PriceProcessor.GetPricesPerSearch();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



    }
}
