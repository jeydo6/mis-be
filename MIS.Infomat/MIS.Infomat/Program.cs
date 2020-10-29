using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace MIS.Infomat
{
    public static class Program
    {
        public static IServiceProvider Run()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting Program.");

                return new Startup(configuration)
                    .ConfigureServices()
                    .BuildServiceProvider();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly.");

                return null;
            }
        }
    }
}
