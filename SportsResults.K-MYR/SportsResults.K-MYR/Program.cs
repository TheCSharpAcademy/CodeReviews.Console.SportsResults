using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.K_MYR;


HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<IWebScrapper, WebScrapper>();
builder.Services.AddHostedService<MailingService>();

using IHost host = builder.Build();

await host.RunAsync();
