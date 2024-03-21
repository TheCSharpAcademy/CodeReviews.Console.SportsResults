using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.frockett;

// ATTN: Add your own email information in EmailService.cs before running!

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcContext, services) =>
    {
        services.AddHostedService<SportsResultsService>();
        services.AddSingleton<EmailService>();
        services.AddSingleton<ScraperService>();
    }).Build();

host.Run();