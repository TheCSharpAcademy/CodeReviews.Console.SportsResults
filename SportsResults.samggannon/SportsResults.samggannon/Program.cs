using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.samggannon;

var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<ResultsService>();
    services.AddSingleton<Scraper>();
    services.AddSingleton<EmailClient>();
}).Build();
host.Run();
