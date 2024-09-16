using SportsResults.Controllers;
using SportsResults.Services;
using SportsResults.Utilities;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddTransient<ScraperService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<HtmlParser>();
builder.Services.AddTransient<ConfigReader>();
builder.Services.AddSingleton<ScraperController>();

builder.Services.AddHostedService<WorkerService>();