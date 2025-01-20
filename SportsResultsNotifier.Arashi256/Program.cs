using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResultsNotifier.Arashi256.Config;
using SportsResultsNotifier.Arashi256.Services;
using SportsResultsNotifier.Arashi256.Controllers;
using Serilog;
using SportsResultsNotifier.Arashi256.Classes;

try
{
    IHostBuilder builder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((context, services) =>
        {
            // Register services and hosted service.
            services.AddSingleton<AppManager>();
            services.AddSingleton<ContactUtils>();
            services.AddSingleton<EmailService>();
            services.AddSingleton<WebScraperService>();
            // Register the controller as background service.
            services.AddHostedService<SportsResultsController>();
        })
        .UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console().WriteTo.File("sportsnotifier.log", rollingInterval: RollingInterval.Day);
        });
    IHost host = builder.Build();
    // Start and run the background service.
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: '{ex.Message}'");
}