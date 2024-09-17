using SportsResults.Controllers;
using SportsResults.Services;
using SportsResults.Utilities;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddTransient<ScraperService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddSingleton<HtmlParser>();
builder.Services.AddSingleton<ConfigReader>();
builder.Services.AddSingleton<ScraperController>();

builder.Services.AddHostedService<WorkerService>();

await builder.Build().RunAsync();