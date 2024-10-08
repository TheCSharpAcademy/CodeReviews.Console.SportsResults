using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SportsResultsNotifier.Services;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<BackgroundTask>();
builder.Services.AddTransient<HtmlScraperService>();
builder.Services.AddLogging(logger =>
{
    logger.ClearProviders();
    logger.AddConsole();
});

var app = builder.Build();

await app.RunAsync();