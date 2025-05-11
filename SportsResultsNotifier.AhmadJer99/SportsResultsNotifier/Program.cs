using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SportsResultsNotifier.Services;
using SportsResultsNotifier.Interfaces;
using SportsResultsNotifier;

var builder = new ConfigurationBuilder();
BuildConfig(builder);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application Starting");

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IConfiguration>(builder.Build());
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IScraperService, ScraperService>();
        //services.AddScoped<SportsResultsWorker>();
        services.AddHostedService<SportsResultsWorker>();
    })
    .Build();
//var svc = ActivatorUtilities.CreateInstance<SportsResultsWorker>(host.Services);
host.Run();

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
           .AddEnvironmentVariables();
}