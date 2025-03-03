using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<SportsController>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<ScrappingService>();
builder.Services.AddSingleton<ConfigurationService>();

var host = builder.Build();
host.Run();


