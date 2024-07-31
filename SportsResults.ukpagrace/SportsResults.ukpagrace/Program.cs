using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.ukpagrace.Interfaces;
using SportsResults.ukpagrace.Mail;
using SportsResults.ukpagrace.Scheduler;
using SportsResults.ukpagrace.Scraper;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddScoped<IGameScraper, GameScraper>();
builder.Services.AddScoped<IEmailInterface, SendEmail>();
builder.Services.AddHostedService<WorkerService>();


IHost host = builder.Build();
host.Run();
