using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

host.Run();

static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    services.AddHostedService<SportsResultsBackgroundService>();
    services.AddSingleton<Scraper>();
    services.AddSingleton<Mailer>();
    services.AddSingleton<BodyBuilder>();
}