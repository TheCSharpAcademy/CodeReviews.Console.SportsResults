using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Phonebook.Services;
using SportsResult;
using SportsResults;

internal class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddUserSecrets<Program>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<EmailSettings>(hostContext.Configuration.GetSection("EmailSettings"));
                    services.AddHostedService<SportsResultsBackgroundService>();
                    services.AddSingleton<Scrapper>();
                    services.AddSingleton<EmailService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });

}