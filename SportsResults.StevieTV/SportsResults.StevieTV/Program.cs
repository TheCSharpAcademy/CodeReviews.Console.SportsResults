using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.StevieTV.EmailService;
using SportsResults.StevieTV.Scraper;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddSingleton<IGameScraper, GameScraper>();
builder.Services.AddHostedService<EmailService>();


using IHost host = builder.Build();

await host.RunAsync();